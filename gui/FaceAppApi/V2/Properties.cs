namespace FaceAppApi.V2
{
    internal static class Properties
    {
        public static string[] Filters =
        {
            "smile",
            "smile_2",
            "hot",
            "old",
            "young",
            "female",
            "female_2",
            "makeup",
            "impression",
            "bangs",
            "glasses",
            "wave",
            "male",
            "hipster",
            "pan",
            "lion",
            "hitman",
            "heisenberg"
        };

        public static string[] NonCropFilters =
        {
            "smile",
            "smile_2",
            "hot",
            "old",
            "young"
        };

        public static int RetryCount = 5;
        public static int RetryWait = 25;
    }
}