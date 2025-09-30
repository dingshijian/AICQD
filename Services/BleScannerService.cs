using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AICQD.Models;
using Plugin.BLE.Abstractions.Contracts; // Required for IDevice

namespace AICQD.Services
{
    public class BleScannerService
    {
        private readonly IAdapter _adapter;
        public event Action<BluetoothDevice>? DeviceDiscovered;
        public event Action<BluetoothDevice>? DeviceConnected;
        public event Action<BluetoothDevice>? DevicePaired;
        public event Action<IReadOnlyList<BluetoothDevice>>? ScanCompleted;

        public bool IsScanning => _adapter.IsScanning;

        private readonly Dictionary<Guid, BluetoothDevice> _discoveredDevices = new();

        public BleScannerService(IAdapter adapter)
        {
            _adapter = adapter;
            _adapter.DeviceDiscovered += (s, e) =>
            {
                if (!_discoveredDevices.ContainsKey(e.Device.Id) && !string.IsNullOrEmpty(e.Device.Name))
                {
                    var newDevice = new BluetoothDevice(e.Device);
                    _discoveredDevices[newDevice.Uuid] = newDevice;
                    DeviceDiscovered?.Invoke(newDevice);
                }
            };
        }

        public IReadOnlyList<BluetoothDevice> GetDiscoveredDevices() => _discoveredDevices.Values.ToList();

        public async Task StartScanAsync(int durationSec = 10)
        {
            if (IsScanning) return;
            
            _discoveredDevices.Clear();
            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(durationSec));
            
            try
            {
                await _adapter.StartScanningForDevicesAsync(cancellationToken: cts.Token);
                ScanCompleted?.Invoke(GetDiscoveredDevices());
            }
            catch(TaskCanceledException)
            {
                // This is expected when the scan times out
                ScanCompleted?.Invoke(GetDiscoveredDevices());
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[SCANNER] Scan failed: {ex.Message}");
            }
        }

        public void StopScan()
        {
            if (IsScanning)
            {
                _adapter.StopScanningForDevicesAsync();
            }
        }

        public async Task<bool> ConnectAsync(BluetoothDevice device)
        {
            try
            {
                if (device.Status == DeviceStatus.Connected) return true;

                device.ConnectionAttempts++;
                device.Status = DeviceStatus.Connecting;

                await _adapter.ConnectToDeviceAsync(device.NativeDevice);

                device.Status = DeviceStatus.Connected;
                DeviceConnected?.Invoke(device);

                // Simulate a pairing step after connection for this app's logic
                await Task.Delay(500);
                device.Status = DeviceStatus.Paired;
                device.PairedAt = DateTime.UtcNow;
                DevicePaired?.Invoke(device);
                
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[SCANNER] Connect failed for {device.Uuid}: {ex.Message}");
                device.Status = DeviceStatus.Failed;
                return false;
            }
        }

        public async Task DisconnectAsync(BluetoothDevice device)
        {
            try
            {
                if (device.Status == DeviceStatus.Connected || device.Status == DeviceStatus.Paired)
                {
                    await _adapter.DisconnectDeviceAsync(device.NativeDevice);
                    device.Status = DeviceStatus.Disconnected;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[SCANNER] Disconnect failed for {device.Uuid}: {ex.Message}");
            }
        }
    }
}