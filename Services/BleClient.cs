using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using Plugin.BLE.Abstractions.Exceptions;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;

namespace AICQD.Services
{
    public class BleClient : INotifyPropertyChanged
    {
        private readonly IBluetoothLE _bluetoothLe;
        private readonly IAdapter _adapter;

        private IDevice? _connectedDevice;
        private IService? _vapeService;
        private ICharacteristic? _puffCharacteristic;

        private bool _isConnected;
        private string _statusMessage = string.Empty;

        public event EventHandler<int>? PuffReceived;
        public event PropertyChangedEventHandler? PropertyChanged;

        public bool IsConnected
        {
            get => _isConnected;
            set => SetProperty(ref _isConnected, value);
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set => SetProperty(ref _statusMessage, value);
        }

        public BleClient(IBluetoothLE bluetoothLe, IAdapter adapter)
        {
            _bluetoothLe = bluetoothLe;
            _adapter = adapter;

            _adapter.DeviceDiscovered += OnDeviceDiscovered;
            _adapter.DeviceConnected += OnDeviceConnected;
            _adapter.DeviceDisconnected += OnDeviceDisconnected;
            _adapter.DeviceConnectionLost += OnDeviceConnectionLost;
        }

        public async Task<bool> ScanAndConnectAsync(string? deviceName = null)
        {
            try
            {
                StatusMessage = "Scanning for devices...";

                if (!_bluetoothLe.IsAvailable)
                {
                    StatusMessage = "Bluetooth is not available on this device";
                    return false;
                }

                if (_bluetoothLe.State != BluetoothState.On)
                {
                    StatusMessage = "Bluetooth is not on";
                    return false;
                }

                var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(10));
                await _adapter.StartScanningForDevicesAsync(cancellationToken: cancellationTokenSource.Token);

                StatusMessage = "Scan completed";
                return IsConnected;
            }
            catch (Exception ex)
            {
                StatusMessage = $"Scan error: {ex.Message}";
                return false;
            }
        }

        private void OnDeviceDiscovered(object? sender, DeviceEventArgs args)
        {
            var device = args.Device;

            if (device.Name?.Contains("XiaoVapeSensor") == true ||
                device.Name?.Contains("AI-CQD") == true)
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    try
                    {
                        await _adapter.StopScanningForDevicesAsync();
                        await ConnectToDeviceAsync(device);
                    }
                    catch (Exception ex)
                    {
                        StatusMessage = $"Connection error: {ex.Message}";
                    }
                });
            }
        }

        private async Task ConnectToDeviceAsync(IDevice device)
        {
            StatusMessage = $"Connecting to {device.Name}...";
            await _adapter.ConnectToDeviceAsync(device);
        }

        private async void OnDeviceConnected(object? sender, DeviceEventArgs args)
        {
            _connectedDevice = args.Device;
            IsConnected = true;
            StatusMessage = $"Connected to {_connectedDevice.Name}";

            var services = await _connectedDevice.GetServicesAsync();
            _vapeService = services.FirstOrDefault(s =>
                s.Id.ToString().ToUpper().Contains("19B10000-E8F2-537E-4F6C-D104768A1214"));

            if (_vapeService == null)
            {
                StatusMessage = "Vape service not found";
                return;
            }

            var characteristics = await _vapeService.GetCharacteristicsAsync();
            _puffCharacteristic = characteristics.FirstOrDefault(c =>
                c.Id.ToString().ToUpper().Contains("19B10001-E8F2-537E-4F6C-D104768A1214"));

            if (_puffCharacteristic == null)
            {
                StatusMessage = "Puff characteristic not found";
                return;
            }

            StatusMessage = "Device setup complete";
        }

        private void OnDeviceDisconnected(object? sender, DeviceEventArgs args)
        {
            IsConnected = false;
            StatusMessage = "Device disconnected";
            _connectedDevice = null;
            _vapeService = null;
            _puffCharacteristic = null;
        }

        private void OnDeviceConnectionLost(object? sender, DeviceErrorEventArgs args)
        {
            IsConnected = false;
            StatusMessage = $"Connection lost: {args.ErrorMessage}";
            _connectedDevice = null;
            _vapeService = null;
            _puffCharacteristic = null;
        }

        public async Task<bool> SubscribeAsync()
        {
            if (_puffCharacteristic == null)
            {
                StatusMessage = "Not connected to a device";
                return false;
            }

            if (!_puffCharacteristic.CanUpdate)
            {
                // CORRECTED TYPO HERE
                StatusMessage = "Characteristic doesn't support notifications";
                return false;
            }

            try
            {
                _puffCharacteristic.ValueUpdated -= OnCharacteristicValueUpdated;
                _puffCharacteristic.ValueUpdated += OnCharacteristicValueUpdated;

                await _puffCharacteristic.StartUpdatesAsync();
                StatusMessage = "Subscribed to puff notifications";
                return true;
            }
            catch (Exception ex)
            {
                StatusMessage = $"Subscription error: {ex.Message}";
                return false;
            }
        }

        private void OnCharacteristicValueUpdated(object? sender, CharacteristicUpdatedEventArgs e)
        {
            try
            {
                var value = e.Characteristic.Value;
                if (value != null && value.Length >= 4)
                {
                    var puffCount = BitConverter.ToUInt32(value, 0);
                    PuffReceived?.Invoke(this, (int)puffCount);
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error reading puff value: {ex.Message}";
            }
        }

        public async Task DisconnectAsync()
        {
            if (_connectedDevice != null)
            {
                await _adapter.DisconnectDeviceAsync(_connectedDevice);
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(storage, value))
            {
                return false;
            }

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}