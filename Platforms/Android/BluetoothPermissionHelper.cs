#if ANDROID
using System.Threading.Tasks;
using Microsoft.Maui.ApplicationModel;

namespace AICQD.Platforms.Android
{
    // Custom permission definitions for Android 12+
    public class PermissionsBluetoothScan : Permissions.BasePlatformPermission
    {
        public override (string androidPermission, bool isRuntime)[] RequiredPermissions =>
            new (string, bool)[]
            {
                (global::Android.Manifest.Permission.BluetoothScan, true)
            };
    }

    public class PermissionsBluetoothConnect : Permissions.BasePlatformPermission
    {
        public override (string androidPermission, bool isRuntime)[] RequiredPermissions =>
            new (string, bool)[]
            {
                (global::Android.Manifest.Permission.BluetoothConnect, true)
            };
    }

    public static class BluetoothPermissionHelper
    {
        public static async Task EnsureAsync()
        {
            if (global::Android.OS.Build.VERSION.SdkInt >= global::Android.OS.BuildVersionCodes.S)
            {
                // Android 12+: Request using our custom permission classes
                var scanStatus = await Permissions.CheckStatusAsync<PermissionsBluetoothScan>();
                if (scanStatus != PermissionStatus.Granted)
                    await Permissions.RequestAsync<PermissionsBluetoothScan>();

                var connectStatus = await Permissions.CheckStatusAsync<PermissionsBluetoothConnect>();
                if (connectStatus != PermissionStatus.Granted)
                    await Permissions.RequestAsync<PermissionsBluetoothConnect>();
            }
            else
            {
                // Pre-Android 12: Scanning requires location
                var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
                if (status != PermissionStatus.Granted)
                    await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            }
        }
    }
}
#endif