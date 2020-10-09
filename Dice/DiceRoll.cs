using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Rationals;

using fp_t = Rationals.Rational;
using roll_t = System.Int32;

namespace Dice {

    public class DiceRoll {
        public roll_t Max () {
            return rolls.Where (l => l.Value > 0).Select (l => l.Key).Max ();
        }

        public roll_t Min () {
            return rolls.Where (l => l.Value > 0).Select (l => l.Key).Min ();
        }

        public Dictionary<roll_t, fp_t> rolls { get; protected set; }

        public DiceRoll (roll_t sides) {
            rolls = new Dictionary<roll_t, fp_t> ();
            for (roll_t i = 1; i <= sides; i++) {
                rolls[i] = ((fp_t) 1) / sides;
            }
        }

        protected DiceRoll () {
            rolls = new Dictionary<roll_t, fp_t> ();
        }

        public void output () {
            foreach (var entry in rolls) {
                Console.WriteLine ("{0}:{1}", entry.Key, entry.Value);
            }
        }

        public static DiceRoll operator + (DiceRoll a, DiceRoll b) {
            DiceRoll r = new DiceRoll ();
            DiceRoll s = new DiceRoll ();
            foreach (var m in a.rolls) {
                foreach (var n in b.rolls) {
                    if (!r.rolls.ContainsKey ((roll_t) (m.Key + n.Key))) {
                        r.rolls[(roll_t) (m.Key + n.Key)] = 0;
                    }
                    r.rolls[(roll_t) (m.Key + n.Key)] += m.Value * n.Value;
                }
            }
            foreach (var m in r.rolls) {
                s.rolls[m.Key] = m.Value.CanonicalForm;
            }
            return s;
        }

        public static DiceRoll operator * (roll_t a, DiceRoll b) {
            DiceRoll r = b;
            for (int i = 1; i < a; i++)
                r += b;

            return r;
        }

        public static DiceRoll CustomOperation (Func<roll_t[], roll_t> f, params DiceRoll[] dice) {
            // embrace your inner Haskell, Peter and use recursion
            DiceRoll d = new DiceRoll ();
            DiceRoll s = new DiceRoll ();

            InnerCustomOperation (f, new List<roll_t> (), dice, 1, d);
            foreach (var m in d.rolls) {
                s.rolls[m.Key] = m.Value.CanonicalForm;
            }
            return s;

        }
        private static void InnerCustomOperation (Func<roll_t[], roll_t> f, List<roll_t> before, DiceRoll[] dice, fp_t existing_p, DiceRoll d) {
            DiceRoll r = dice[before.Count];

            foreach (var m in r.rolls) {
                var a = before.ToList ();
                a.Add (m.Key);

                if (before.Count + 1 == dice.Length) {
                    // innermost dice, modify d
                    var n = f (a.ToArray ());
                    if (!d.rolls.ContainsKey (n))
                        d.rolls[n] = 0;
                    d.rolls[n] += existing_p * m.Value;

                } else {
                    // we need to go deeper!
                    InnerCustomOperation (f, a, dice, existing_p * m.Value, d);
                }
            }
        }

        public fp_t Expected () {
            return rolls.Select (x => x.Key * x.Value).Sum ();
        }

        public static DiceRoll d20 () { return new DiceRoll (20); }
        public static DiceRoll d12 () { return new DiceRoll (12); }
        public static DiceRoll d10 () { return new DiceRoll (10); }
        public static DiceRoll d8 () { return new DiceRoll (8); }
        public static DiceRoll d6 () { return new DiceRoll (6); }
        public static DiceRoll d4 () { return new DiceRoll (4); }
    }
}