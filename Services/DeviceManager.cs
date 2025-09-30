using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SQLite;
using AICQD.Models;
using AICQD.Data; // Add this to reference the correct AppDatabase
using System.Text.Json;

namespace AICQD.Services
{
    [Table("devices")]
    public class DeviceRecord
    {
        [PrimaryKey] public string Uuid { get; set; } = "";
        public string Name { get; set; } = "";
        public int Rssi { get; set; }
        public string ManufacturerDataJson { get; set; } = "[]";
        public string ServiceUuidsJson { get; set; } = "[]";
        public DateTime DiscoveredAt { get; set; } = DateTime.UtcNow;
        public DateTime LastSeen { get; set; } = DateTime.UtcNow;
        public string Status { get; set; } = DeviceStatus.Discovered.ToString();
        public int ConnectionAttempts { get; set; }
        public DateTime? PairedAt { get; set; }
    }

    // The duplicate AppDatabase class has been removed from this file.

    public class DeviceManager
    {
        private readonly AppDatabase _database;

        public DeviceManager(AppDatabase database)
        {
            _database = database;
        }

        public async Task RegisterOrUpdateDeviceAsync(BluetoothDevice device)
        {
            var record = await _database.GetDeviceAsync(device.Uuid.ToString()) ?? new DeviceRecord
            {
                Uuid = device.Uuid.ToString(),
                DiscoveredAt = device.DiscoveredAt
            };

            record.Name = device.Name;
            record.Rssi = device.Rssi;
            record.LastSeen = DateTime.UtcNow;
            record.Status = device.Status.ToString();
            record.ConnectionAttempts = device.ConnectionAttempts;
            record.PairedAt = device.PairedAt;
            record.ManufacturerDataJson = JsonSerializer.Serialize(device.ManufacturerData);
            record.ServiceUuidsJson = JsonSerializer.Serialize(device.ServiceUuids);

            await _database.UpsertDeviceAsync(record);
        }

        public async Task<bool> UpdateStatusAsync(string uuid, DeviceStatus status)
        {
            var record = await _database.GetDeviceAsync(uuid);
            if (record == null) return false;
            
            record.Status = status.ToString();
            record.LastSeen = DateTime.UtcNow;
            await _database.UpsertDeviceAsync(record);
            return true;
        }

        public Task<List<DeviceRecord>> GetAllDevicesAsync()
        {
            return _database.GetAllDevicesAsync();
        }
    }
}