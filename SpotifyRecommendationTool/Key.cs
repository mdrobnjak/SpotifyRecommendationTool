using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyRecommendationTool
{
    public static class Key
    {
        static readonly Dictionary<int, string> keyDict = new Dictionary<int, string>
        {
            { 0, "C" },
            { 1, "C#" },
            { 2, "D" },
            { 3, "D#" },
            { 4, "E" },
            { 5, "F" },
            { 6, "F#" },
            { 7, "G" },
            { 8, "G#" },
            { 9, "A" },
            { 10, "A#" },
            { 11, "B" }
        };

        public static string ConvertToString(int key, int mode)
        {
            if (key == -1)
            {
                return "-1";
            }
            else
            {
                return keyDict[key] + (mode == 0 ? " min" : " Maj");
            }
        }

        public static Tuple<int, int> ConvertToInt(string key)
        {
            return new Tuple<int, int>(
                keyDict.FirstOrDefault(x => x.Value == key.Split(' ')[0]).Key,
                key.Split(' ')[1] == "min" ? 0 : 1);
        }
    }
}
