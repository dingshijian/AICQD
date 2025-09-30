using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AICQD.Models;

namespace AICQD.Services
{
    public class AutoPairingEngine
    {
        private readonly HashSet<int> _trustedMfgIds = new() { 0x0059 }; // Nordic Semiconductor
        public event Action<BluetoothDevice>? OnPairingSuccess;
        public event Action<BluetoothDevice, string>? OnPairingFailure;

        public async Task TryAutoPairAsync(BleScannerService scanner, BluetoothDevice device)
        {
            if (!IsSecureDevice(device))
            {
                System.Diagnostics.Debug.WriteLine($"[AUTOPAIR] Skipping insecure device: {device.Name}");
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

        private bool IsSecureDevice(BluetoothDevice device)
        {
            // Simple security check: must be a known manufacturer and have a strong signal.
            bool isTrusted = device.ManufacturerData.Any(data => data.Length > 1 && (data[1] << 8 | data[0]) == 0x0059);
            bool hasGoodSignal = device.Rssi > -70;

            return isTrusted && hasGoodSignal;
        }
    }
}