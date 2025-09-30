using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using SQLite;
using AICQD.Services;

namespace AICQD.Data
{
    public class AppDatabase
    {
        private SQLiteAsyncConnection? _database;
        private bool _isInitialized;

        public async Task InitializeAsync()
        {
            if (_isInitialized)
                return;

            try
            {
                var databasePath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "aicqd.db3");

                _database = new SQLiteAsyncConnection(databasePath);

                await _database.CreateTableAsync<DeviceRecord>();

                _isInitialized = true;
                System.Diagnostics.Debug.WriteLine($"[Database] Initialized at: {databasePath}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[Database] Initialization error: {ex.Message}");
                throw;
            }
        }

        private async Task EnsureInitializedAsync()
        {
            if (!_isInitialized)
            {
                await InitializeAsync();
            }
        }

        public async Task<DeviceRecord?> GetDeviceAsync(string uuid)
        {
            await EnsureInitializedAsync();
            return await _database!.Table<DeviceRecord>()
                .Where(d => d.Uuid == uuid)
                .FirstOrDefaultAsync();
        }

        public async Task<List<DeviceRecord>> GetAllDevicesAsync()
        {
            await EnsureInitializedAsync();
            return await _database!.Table<DeviceRecord>().ToListAsync();
        }

        public async Task<int> UpsertDeviceAsync(DeviceRecord device)
        {
            await EnsureInitializedAsync();
            
            var existing = await GetDeviceAsync(device.Uuid);
            if (existing != null)
            {
                return await _database!.UpdateAsync(device);
            }
            else
            {
                return await _database!.InsertAsync(device);
            }
        }

        public async Task<int> DeleteDeviceAsync(string uuid)
        {
            await EnsureInitializedAsync();
            var device = await GetDeviceAsync(uuid);
            if (device != null)
            {
                return await _database!.DeleteAsync(device);
            }
            return 0;
        }
    }

    public class DeviceRepository
    {
        private readonly AppDatabase _database;

        public DeviceRepository(AppDatabase database)
        {
            _database = database;
        }

        public Task<DeviceRecord?> GetDeviceAsync(string uuid)
            => _database.GetDeviceAsync(uuid);

        public Task<List<DeviceRecord>> GetAllDevicesAsync()
            => _database.GetAllDevicesAsync();

        public Task<int> SaveDeviceAsync(DeviceRecord device)
            => _database.UpsertDeviceAsync(device);

        public Task<int> DeleteDeviceAsync(string uuid)
            => _database.DeleteDeviceAsync(uuid);
    }
}