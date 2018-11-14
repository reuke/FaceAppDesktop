namespace FaceApp
{
    public static class SettingManager
    {
        public static int ApiVersion
        {
            get => Properties.Settings.Default.ApiVersion;
            set
            {
                Properties.Settings.Default.ApiVersion = value;
                Properties.Settings.Default.Save();
            }
        }

        public static bool PreLoad
        {
            get => Properties.Settings.Default.PreLoad;
            set
            {
                Properties.Settings.Default.PreLoad = value;
                Properties.Settings.Default.Save();
            }
        }
    }
}
