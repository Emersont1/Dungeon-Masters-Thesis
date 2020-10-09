using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Rationals;

using fp_t = Rationals.Rational;
using roll_t = System.Int32;

namespace Dice {
    public static class Extensions {
        public static fp_t Sum (this IEnumerable<fp_t> data) {
            fp_t sum = 0;
            foreach (fp_t t in data) {
                sum += t;
            }
            return sum;
        }
    }
}