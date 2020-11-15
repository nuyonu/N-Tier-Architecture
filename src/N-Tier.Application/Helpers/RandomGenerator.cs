using System;

namespace N_Tier.Application.Helpers
{
    public static class RandomGenerator
    {
        public static int GenerateInteger(int min, int max)
        {
            return new Random().Next(min, max);
        }
    }
}
