using System;
using System.Linq;
using Dice;

namespace DiceTest {
    class Program {
        static void Main (string[] args) {
            // 3D6
            var a = 3 * DiceRoll.d6 ();
            /*
            Console.WriteLine (a.Max ());
            Console.WriteLine (a.Min ());
            a.output ();
            */

            // 4D6 Best 3
            var b = DiceRoll.CustomOperation ((
                rolls) => {
                return (ushort) rolls.OrderByDescending (x => x).Take (3).Sum (x => x);
            }, DiceRoll.d6 (), DiceRoll.d6 (), DiceRoll.d6 (), DiceRoll.d6 ());

            //5d6 middle 3
            var c = DiceRoll.CustomOperation ((
                rolls) => {
                return (ushort) rolls.OrderByDescending (x => x).Skip (1).Take (3).Sum (x => x);
            }, DiceRoll.d6 (), DiceRoll.d6 (), DiceRoll.d6 (), DiceRoll.d6 (), DiceRoll.d6 ());
            /*
            Console.WriteLine (b.Max ());
            Console.WriteLine (b.Min ());
            b.output ();
            */

         
            Graph g = new Graph ("Figure 2", "Damage Dealt against a Bugbear by a typical level 1 fighter (5e)", false);
            //g.AddRolls (a, "", Colours.Blue, true);
            //g.export ("rolls.svg");
            g.AddRolls (a, "3d6", show_expected: true, marker_type:OxyPlot.MarkerType.Plus);
            g.AddRolls (b, "4d6, best 3", show_expected: true,  marker_type:OxyPlot.MarkerType.Circle);
            g.AddRolls (c, "5d6, middle 3", show_expected: true);
            g.export ("rolls.svg");

        }
    }
}