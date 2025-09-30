using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Plugin.BLE;
using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using AICQD.Models;

namespace AICQD
{
    public partial class MainPage : ContentPage
    {
        // XIAO nRF52840 Sense specific UUIDs
        private static readonly Guid VapeServiceUuid = Guid.Parse("19B10000-E8F2-537E-4F6C-D104768A1214");
        private static readonly Guid PuffCountCharacteristicUuid = Guid.Parse("19B10001-E8F2-537E-4F6C-D104768A1214");
        private static readonly Guid DeviceControlCharacteristicUuid = Guid.Parse("19B10002-E8F2-537E-4F6C-D104768A1214");

        private readonly IAdapter _adapter;
        private readonly IBluetoothLE _ble;

        private IDevice? _connectedDevice;
        private ICharacteristic? _puffCountCharacteristic;
        private ICharacteristic? _controlCharacteristic;

        public ObservableCollection<BluetoothDevice> DiscoveredDevices { get; } = new();
        
        private int _todayPuffCount = 0;
        private DateTime _lastPuffTime = DateTime.MinValue;
        
        public MainPage()
        {
            InitializeComponent();

            _ble = CrossBluetoothLE.Current;
            _adapter = CrossBluetoothLE.Current.Adapter;

            DevicesList.ItemsSource = DiscoveredDevices;

            _adapter.DeviceDiscovered += OnDeviceDiscovered;
            _adapter.DeviceConnectionLost += OnDeviceConnectionLost;
            
            // Initialize UI state
            UpdateBleStatus(_ble.State);
            _ble.StateChanged += (s, e) => MainThread.BeginInvokeOnMainThread(() => UpdateBleStatus(e.NewState));
            
            Debug.WriteLine("[MainPage] Initialized - Ready for Bluetooth operations");
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            CheckBluetoothPermissions();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            
            // Clean up BLE resources
            if (_puffCountCharacteristic != null)
            {
                _puffCountCharacteristic.ValueUpdated -= OnPuffCountUpdated;
            }
            
            _adapter.DeviceDiscovered -= OnDeviceDiscovered;
            _adapter.DeviceConnectionLost -= OnDeviceConnectionLost;
        }

        private void UpdateBleStatus(BluetoothState state)
        {
            BleStatusLabel.Text = $"Bluetooth: {state}";
            ScanButton.IsEnabled = state == BluetoothState.On;
            
            if (state == BluetoothState.On)
            {
                BleStatusLabel.TextColor = Color.FromArgb("#27AE60");
            }
            else
            {
                BleStatusLabel.TextColor = Color.FromArgb("#E74C3C");
            }
        }

        private void OnDeviceDiscovered(object? sender, DeviceEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Device.Name))
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    var existingDevice = DiscoveredDevices.FirstOrDefault(d => d.Uuid == e.Device.Id);
                    if (existingDevice == null)
                    {
                        DiscoveredDevices.Add(new BluetoothDevice(e.Device));
                        DevicesHeaderLabel.IsVisible = true;
                        Debug.WriteLine($"[BLE] Discovered: {e.Device.Name} ({e.Device.Id})");
                    }
                    else
                    {
                        existingDevice.Rssi = e.Device.Rssi;
                        existingDevice.LastSeen = DateTime.UtcNow;
                    }
                });
            }
        }

        private async void OnDeviceConnectionLost(object? sender, DeviceErrorEventArgs e)
        {
            Debug.WriteLine($"[BLE] Connection lost: {e.Device.Name}");
            
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                StatusLabel.Text = "Connection lost. Attempting to reconnect...";
                
                // Attempt to reconnect
                try
                {
                    await Task.Delay(2000);
                    await _adapter.ConnectToDeviceAsync(e.Device);
                    StatusLabel.Text = $"Reconnected to {e.Device.Name}";
                    await SubscribeToPuffCountAsync();
                }
                catch (Exception ex)
                {
                    StatusLabel.Text = "Reconnection failed";
                    Debug.WriteLine($"[BLE] Reconnection error: {ex.Message}");
                }
            });
        }

        private async void OnScanClicked(object sender, EventArgs e)
        {
            if (_ble.State != BluetoothState.On)
            {
                await DisplayAlert("Bluetooth Off", 
                    "Please enable Bluetooth to scan for your AICQD device.", "OK");
                return;
            }

            DiscoveredDevices.Clear();
            DevicesHeaderLabel.IsVisible = false;
            StatusLabel.Text = "🔍 Scanning for AICQD devices...";
            ScanButton.IsEnabled = false;

            try
            {
                _adapter.ScanMode = ScanMode.LowLatency;
                _adapter.ScanTimeout = 10000; // 10 seconds
                
                await _adapter.StartScanningForDevicesAsync(
                    cancellationToken: new System.Threading.CancellationTokenSource(TimeSpan.FromSeconds(10)).Token);
                
                Debug.WriteLine($"[BLE] Scan complete. Found {DiscoveredDevices.Count} devices");
                
                if (DiscoveredDevices.Count == 0)
                {
                    StatusLabel.Text = "No devices found. Make sure your AICQD device is powered on.";
                }
                else
                {
                    StatusLabel.Text = $"Found {DiscoveredDevices.Count} device(s). Tap to connect.";
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[BLE] Scan Error: {ex.Message}");
                await DisplayAlert("Scan Error", 
                    $"Failed to scan for devices: {ex.Message}", "OK");
                StatusLabel.Text = "Scan failed";
            }
            finally
            {
                ScanButton.IsEnabled = true;
            }
        }
        
        private async void OnConnectClicked(object sender, EventArgs e)
        {
            if (DevicesList.SelectedItem is not BluetoothDevice selectedDevice)
            {
                await DisplayAlert("No Device Selected", 
                    "Please select an AICQD device from the list first.", "OK");
                return;
            }

            await ConnectToDevice(selectedDevice);
        }

        private async void OnDisconnectClicked(object sender, EventArgs e)
        {
            if (_connectedDevice == null)
            {
                await DisplayAlert("Not Connected", 
                    "No device is currently connected.", "OK");
                return;
            }

            try
            {
                if (_puffCountCharacteristic != null)
                {
                    await _puffCountCharacteristic.StopUpdatesAsync();
                    _puffCountCharacteristic.ValueUpdated -= OnPuffCountUpdated;
                    _puffCountCharacteristic = null;
                }

                string deviceName = _connectedDevice.Name;
                await _adapter.DisconnectDeviceAsync(_connectedDevice);
                _connectedDevice = null;
                
                StatusLabel.Text = $"Disconnected from {deviceName}";
                Debug.WriteLine($"[BLE] Successfully disconnected from {deviceName}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[BLE] Disconnection error: {ex.Message}");
                await DisplayAlert("Disconnection Error", ex.Message, "OK");
            }
        }

        private async void OnDevicesListTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is not BluetoothDevice selectedDevice) return;
            await ConnectToDevice(selectedDevice);
        }
        
        private async Task ConnectToDevice(BluetoothDevice selectedDevice)
        {
            try
            {
                StatusLabel.Text = $"Connecting to {selectedDevice.Name}...";
                ScanButton.IsEnabled = false;
                
                // Stop scanning before connecting
                if (_adapter.IsScanning)
                {
                    await _adapter.StopScanningForDevicesAsync();
                }
                
                // Set connection parameters for better stability
                var connectParameters = new ConnectParameters(
                    autoConnect: false,
                    forceBleTransport: true);
                
                await _adapter.ConnectToDeviceAsync(
                    selectedDevice.NativeDevice, 
                    connectParameters);

                _connectedDevice = selectedDevice.NativeDevice;
                StatusLabel.Text = $"✅ Connected to {_connectedDevice.Name}";
                
                Debug.WriteLine($"[BLE] Connected to {_connectedDevice.Name}");
                Debug.WriteLine($"[BLE] Device ID: {_connectedDevice.Id}");
                Debug.WriteLine($"[BLE] Connection State: {_connectedDevice.State}");
                
                // Subscribe to puff count notifications
                await SubscribeToPuffCountAsync();
                
                // Send initial handshake to device
                await SendDeviceCommand(0x01); // Command: Connected
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[BLE] Connection error: {ex.Message}");
                await DisplayAlert("Connection Error", 
                    $"Could not connect to device: {ex.Message}\n\nMake sure the device is in range and not connected to another phone.", 
                    "OK");
                StatusLabel.Text = "Connection failed";
            }
            finally
            {
                ScanButton.IsEnabled = true;
            }
        }

        private async Task SubscribeToPuffCountAsync()
        {
            if (_connectedDevice == null || _connectedDevice.State != DeviceState.Connected)
            {
                Debug.WriteLine("[BLE] Cannot subscribe: Device not connected");
                return;
            }

            try
            {
                StatusLabel.Text = "Discovering services...";
                
                var services = await _connectedDevice.GetServicesAsync();
                Debug.WriteLine($"[BLE] Found {services.Count} services");
                
                var service = services.FirstOrDefault(s => s.Id == VapeServiceUuid);
                
                if (service == null)
                {
                    Debug.WriteLine("[BLE] AICQD service not found");
                    await DisplayAlert("Service Not Found", 
                        "The AICQD service was not found on this device. Make sure you have the correct firmware installed.", 
                        "OK");
                    return;
                }
                
                Debug.WriteLine($"[BLE] Found AICQD service: {service.Id}");
                
                var characteristics = await service.GetCharacteristicsAsync();
                Debug.WriteLine($"[BLE] Found {characteristics.Count} characteristics");
                
                var puffChar = characteristics.FirstOrDefault(c => c.Id == PuffCountCharacteristicUuid);
                var controlChar = characteristics.FirstOrDefault(c => c.Id == DeviceControlCharacteristicUuid);

                if (puffChar != null)
                {
                    _puffCountCharacteristic = puffChar;
                    _puffCountCharacteristic.ValueUpdated -= OnPuffCountUpdated;
                    _puffCountCharacteristic.ValueUpdated += OnPuffCountUpdated;
                    
                    await _puffCountCharacteristic.StartUpdatesAsync();
                    Debug.WriteLine("[BLE] Subscribed to puff count notifications");
                    StatusLabel.Text = "✅ Monitoring puff count";
                }
                
                if (controlChar != null)
                {
                    _controlCharacteristic = controlChar;
                    Debug.WriteLine("[BLE] Control characteristic ready");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[BLE] Subscription error: {ex.Message}");
                await DisplayAlert("Subscription Error", 
                    $"Could not subscribe to device notifications: {ex.Message}", 
                    "OK");
                StatusLabel.Text = "Subscription failed";
            }
        }

        private async Task SendDeviceCommand(byte command)
        {
            if (_controlCharacteristic == null)
            {
                Debug.WriteLine("[BLE] Control characteristic not available");
                return;
            }

            try
            {
                await _controlCharacteristic.WriteAsync(new byte[] { command });
                Debug.WriteLine($"[BLE] Sent command: 0x{command:X2}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[BLE] Command send error: {ex.Message}");
            }
        }

        private void OnPuffCountUpdated(object? sender, CharacteristicUpdatedEventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                try
                {
                    var data = e.Characteristic.Value;
                    if (data != null && data.Length >= 4)
                    {
                        uint puffCount = BitConverter.ToUInt32(data, 0);
                        _todayPuffCount = (int)puffCount;
                        _lastPuffTime = DateTime.Now;
                        
                        PuffCountLabel.Text = $"{puffCount} Puffs Today";
                        
                        Debug.WriteLine($"[BLE] Puff count updated: {puffCount}");
                        
                        // Trigger AI response if craving level is high
                        if (CravingSlider.Value > 7)
                        {
                            CoachMessageLabel.Text = $"I see you're having a tough moment. You've had {puffCount} puffs today. Let's try a breathing exercise or play a game!";
                        }
                        
                        // Save to database (implement later)
                        // await _deviceRepository.SavePuffCountAsync(puffCount, DateTime.UtcNow);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"[BLE] Error updating puff count: {ex}");
                }
            });
        }

        public async Task ReconnectAndResubscribe(IDevice device)
        {
            _connectedDevice = device;
            StatusLabel.Text = $"Reconnected to {device.Name}";
            await SubscribeToPuffCountAsync();
        }

        // ===== UI EVENT HANDLERS =====
        
        private void OnLogPuffClicked(object sender, EventArgs e)
        {
            _todayPuffCount++;
            _lastPuffTime = DateTime.Now;
            PuffCountLabel.Text = $"{_todayPuffCount} Puffs Today";
            
            // Send to device if connected
            if (_connectedDevice != null && _controlCharacteristic != null)
            {
                _ = SendDeviceCommand(0x10); // Command: Manual puff logged
            }
            
            CoachMessageLabel.Text = $"Puff logged. Remember, you're working towards your goal. Stay strong!";
        }

        private async void OnResetTodayClicked(object sender, EventArgs e)
        {
            bool confirm = await DisplayAlert("Reset Count", 
                "Are you sure you want to reset today's puff count?", 
                "Yes", "No");
            
            if (confirm)
            {
                _todayPuffCount = 0;
                PuffCountLabel.Text = "0 Puffs Today";
                
                if (_connectedDevice != null && _controlCharacteristic != null)
                {
                    await SendDeviceCommand(0x20); // Command: Reset count
                }
                
                CoachMessageLabel.Text = "Count reset. Starting fresh!";
            }
        }

        private void OnCravingChanged(object sender, ValueChangedEventArgs e)
        {
            CravingValueLabel.Text = ((int)e.NewValue).ToString();
            
            int cravingLevel = (int)e.NewValue;
            
            if (cravingLevel >= 8)
            {
                CoachMessageLabel.Text = "🚨 High craving detected! Let's play a game or do breathing exercises. You've got this!";
            }
            else if (cravingLevel >= 5)
            {
                CoachMessageLabel.Text = "💪 Craving rising. Stay strong! Try a distraction technique.";
            }
            else if (cravingLevel >= 3)
            {
                CoachMessageLabel.Text = "👍 Manageable craving. You're doing great!";
            }
            else
            {
                CoachMessageLabel.Text = "✨ Feeling good! Keep up the great work!";
            }
        }

        private async void OnPlayCravingGameClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Games", 
                "Opening mini-games to help with cravings...\n\n" +
                "• Craving Crusher\n" +
                "• Breath Quest\n" +
                "• Vape Dodge\n" +
                "• Zen Garden", 
                "OK");
            // Navigate to games page (implement later)
        }

        private void OnStartSessionClicked(object sender, EventArgs e)
        {
            CoachMessageLabel.Text = "🎯 Session started! I'm here to support you. How are you feeling right now?";
            
            if (_connectedDevice != null)
            {
                _ = SendDeviceCommand(0x30); // Command: Session started
            }
        }

        private void OnEndSessionClicked(object sender, EventArgs e)
        {
            CoachMessageLabel.Text = $"✅ Session complete! You handled {_todayPuffCount} moments today. Proud of your awareness!";
            
            if (_connectedDevice != null)
            {
                _ = SendDeviceCommand(0x31); // Command: Session ended
            }
        }

        private async void OnOpenAiChatClicked(object sender, EventArgs e)
        {
            await DisplayAlert("AI Chat", 
                "Opening AI Coach chat interface...", 
                "OK");
            // Navigate to chat page (implement later)
        }

        private async void OnViewProgressClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Progress", 
                $"Today: {_todayPuffCount} puffs\n" +
                $"Last puff: {(_lastPuffTime != DateTime.MinValue ? _lastPuffTime.ToString("h:mm tt") : "N/A")}\n\n" +
                "Full progress tracking coming soon!", 
                "OK");
        }

        private async void OnViewAchievementsClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Achievements", 
                "🏆 Your achievements:\n\n" +
                "• First Day ✓\n" +
                "• Device Connected ✓\n" +
                "• Coming soon: More achievements!", 
                "OK");
        }

        private async void OnViewCommunityClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Community", 
                "Connect with others on their quit journey!\n\n" +
                "• Weekly check-ins\n" +
                "• Share progress\n" +
                "• Support others\n\n" +
                "Community features coming soon!", 
                "OK");
        }

        private async void OnOpenSettingsClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Settings", 
                "⚙️ Settings:\n\n" +
                "• Notification preferences\n" +
                "• Device settings\n" +
                "• AI coach personality\n" +
                "• Privacy options\n\n" +
                "Full settings coming soon!", 
                "OK");
        }

        private async void OnAgeGateToggled(object sender, ToggledEventArgs e)
        {
            Debug.WriteLine($"[Settings] Age Gate: {e.Value}");
            
            if (e.Value && _connectedDevice != null)
            {
                await SendDeviceCommand(0x40); // Command: Enable age gate
                await DisplayAlert("Age Gate", 
                    "Age gate protection enabled on device", 
                    "OK");
            }
            else if (!e.Value && _connectedDevice != null)
            {
                await SendDeviceCommand(0x41); // Command: Disable age gate
            }
        }

        private async void OnLossFindToggled(object sender, ToggledEventArgs e)
        {
            Debug.WriteLine($"[Settings] Loss & Find: {e.Value}");
            
            if (e.Value && _connectedDevice != null)
            {
                await SendDeviceCommand(0x50); // Command: Enable find my device
                await DisplayAlert("Find Device", 
                    "Find my device enabled. Your device will beep if disconnected.", 
                    "OK");
            }
            else if (!e.Value && _connectedDevice != null)
            {
                await SendDeviceCommand(0x51); // Command: Disable find my device
            }
        }

        private async void OnRefreshClicked(object sender, EventArgs e)
        {
            StatusLabel.Text = "Refreshing...";
            
            // Refresh data from device
            if (_connectedDevice != null && _connectedDevice.State == DeviceState.Connected)
            {
                await SendDeviceCommand(0x60); // Command: Request data refresh
                await Task.Delay(1000);
                StatusLabel.Text = "Data refreshed";
            }
            else
            {
                StatusLabel.Text = "No device connected";
            }
        }

        private async void OnExportClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Export Data", 
                "Exporting your quit journey data...\n\n" +
                $"Total puffs today: {_todayPuffCount}\n" +
                "Export functionality coming soon!", 
                "OK");
        }
        private void CheckBluetoothPermissions()
        {
        #if ANDROID || IOS
            Task.Run(async () =>
            {
                var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
                if (status != PermissionStatus.Granted)
                {
                    status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                    if (status != PermissionStatus.Granted)
                    {
                        await MainThread.InvokeOnMainThreadAsync(async () =>
                        {
                            await DisplayAlert("Permission Required", 
                                "Location permission is required for Bluetooth scanning on your device.", "OK");
                        });
                    }
                }

        #if ANDROID
                var bluetoothStatus = await Permissions.CheckStatusAsync<Permissions.Bluetooth>();
                if (bluetoothStatus != PermissionStatus.Granted)
                {
                    bluetoothStatus = await Permissions.RequestAsync<Permissions.Bluetooth>();
                }
#endif
            });
#endif
        }
    }
}