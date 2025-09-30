using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Plugin.BLE.Abstractions.Contracts;

namespace AICQD.Models
{
    public enum DeviceStatus
    {
        Discovered,
        Connecting,
        Connected,
        Paired,
        Disconnected,
        Failed
    }

    public class BluetoothDevice : INotifyPropertyChanged
    {
        private int _rssi;
        private DateTime _lastSeen;
        private DeviceStatus _status;

        public BluetoothDevice(IDevice device)
        {
            NativeDevice = device;
            Name = device.Name ?? "Unknown Device";
            Uuid = device.Id;
            Rssi = device.Rssi;
            LastSeen = DateTime.UtcNow;
            DiscoveredAt = DateTime.UtcNow;
            Status = DeviceStatus.Discovered;
            ConnectionAttempts = 0;
            ManufacturerData = new List<byte[]>();
            ServiceUuids = new List<Guid>();
        }

        public IDevice NativeDevice { get; }
        
        public string Name { get; }
        
        public Guid Uuid { get; }

        public int Rssi
        {
            get => _rssi;
            set
            {
                if (_rssi != value)
                {
                    _rssi = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime LastSeen
        {
            get => _lastSeen;
            set
            {
                if (_lastSeen != value)
                {
                    _lastSeen = value;
                    OnPropertyChanged();
                }
            }
        }

        public DeviceStatus Status
        {
            get => _status;
            set
            {
                if (_status != value)
                {
                    _status = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime DiscoveredAt { get; set; }
        
        public int ConnectionAttempts { get; set; }
        
        public DateTime? PairedAt { get; set; }
        
        public List<byte[]> ManufacturerData { get; set; }
        
        public List<Guid> ServiceUuids { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}