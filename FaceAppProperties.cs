using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceApp
{
    static class FaceAppProperties
    {
        public static string TempFolderPath = Path.Combine(Path.GetTempPath(), "FaceAppTemp");

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
