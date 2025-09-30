namespace AICQD.Config
{
    public static class AppSettings
    {
        // Point this at wherever your Flask app is running.
        // For Android emulator talking to host machine, use http://10.0.2.2:5000
        public const string ApiBaseUrl =
#if ANDROID
            "http://10.0.2.2:5000";
#else
            "http://localhost:5000";
#endif
    }
}
