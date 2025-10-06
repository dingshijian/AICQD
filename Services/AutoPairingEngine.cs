using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AICQD.Models;

namespace AICQD.Services
{
    public class AutoPairingEngine
    {
        public event Action<BluetoothDevice>? OnPairingSuccess;
        public event Action<BluetoothDevice, string>? OnPairingFailure;

        public async Task TryAutoPairAsync(BleScannerService scanner, BluetoothDevice device)
        {
            if (!IsXiaoVapeSensor(device))
            {
                System.Diagnostics.Debug.WriteLine($"[AUTOPAIR] Skipping non-target device: {device.Name}");
                return;
            }

            System.Diagnostics.Debug.WriteLine($"[AUTOPAIR] Attempting to auto-pair with: {device.Name}");
            bool success = await scanner.ConnectAsync(device);

            if (success)
            {
                System.Diagnostics.Debug.WriteLine($"[AUTOPAIR] Success for: {device.Name}");
                OnPairingSuccess?.Invoke(device);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"[AUTOPAIR] Failure for: {device.Name}");
                OnPairingFailure?.Invoke(device, "Connection process failed during auto-pair.");
            }
        }

        private bool IsXiaoVapeSensor(BluetoothDevice device)
        {
            // Simple security check: must be the target device and have a strong signal.
            bool isTargetDevice = device.Name?.Contains("XiaoVapeSensor", StringComparison.OrdinalIgnoreCase) ?? false;
            bool hasGoodSignal = device.Rssi > -70;

            return isTargetDevice && hasGoodSignal;
        }
    }
}