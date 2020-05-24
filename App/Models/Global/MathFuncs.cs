using System;

namespace Androtomist.Models.Global
{
    public class MathFuncs
    {
        public double GetRandomDouble(double min, double max)
        {
            Random random = new Random();
            return random.NextDouble() * (max - min) + min;
        }

        public long GetRandomLong(long min, long max)
        {
            Random random = new Random();
            byte[] buf = new byte[8];
            random.NextBytes(buf);
            long longRand = BitConverter.ToInt64(buf, 0);

            return (Math.Abs(longRand % (max - min)) + min);
        }
    }
}
