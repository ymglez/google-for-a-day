using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GoogleForADay.Core.Utils
{
    public static class Parallel
    {
        private static IEnumerable<bool> IterateUntilFalse(Func<bool> condition)
        {
            while (condition()) yield return true;
        }


        public static void While(
            ParallelOptions parallelOptions, Func<bool> condition, Action body)
        {
            System.Threading.Tasks.Parallel.ForEach(IterateUntilFalse(condition), parallelOptions,
                ignored => body());
        }



    }
}
