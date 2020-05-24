using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Androtomist.Models.Global
{
    public class ColorFuncs
    {
        private Random _random = new Random(2);

        public Color RandomColor()
        {
            Color randomColor;


            //KnownColor[] names = (KnownColor[])Enum.GetValues(typeof(KnownColor));
            //KnownColor randomColorName = names[_random.Next(names.Length)];
            //randomColor = Color.FromKnownColor(randomColorName);

            randomColor = Color.FromArgb(255, _random.Next(128) + 127, _random.Next(128) + 127, _random.Next(128) + 127);

            return randomColor;
        }

        public Color RGBstringToColor(string RGB)
        {
            Regex digitsOnly = new Regex(@"[^.\d]");
            List<int> RGB_COLORS = Regex
                .Split(RGB, @",", RegexOptions.IgnoreCase)
                .Select(x => digitsOnly.Replace(x, ""))
                .Select((x, i) => i >= 3 ? Convert.ToInt32(Convert.ToDouble(x) * 255) :
                Convert.ToInt32(x)).ToList();
            if (RGB_COLORS.Count == 3) RGB_COLORS.Add(255);

            return Color.FromArgb(RGB_COLORS[3], RGB_COLORS[0], RGB_COLORS[1], RGB_COLORS[2]);
        }
    }
}