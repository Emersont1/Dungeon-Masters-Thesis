using System;
using System.Linq;
using Dice;

namespace DiceTest {
    class Program {
        static void Main (string[] args) {
            //Fighter 1 (from 5e pregens ) Vs bugbear

            DiceRoll a = DiceRoll.CustomOperation ((
                rolls) => {
                if (rolls[0] == 20) { return rolls[1] + rolls[2] + 3; }
                if (rolls[0] + 3 >= 16) {
                    return rolls[1] + 3;
                } else { return 0; }
            }, DiceRoll.d20 (), DiceRoll.d12 (), DiceRoll.d12 ());

            Graph g = new Graph ("Figure 2", "Damage Dealt against a Bugbear by a typical level 1 fighter (5e)");
            g.AddRolls (a, "");
            g.export ("damage.svg");
            a.rolls.Remove (0);
            g = new Graph ("Figure 3", "Damage Dealt against a Bugbear by a typical level 1 fighter (5e) (miss removed)");
            g.AddRolls (a, "");
            g.export ("damage2.svg");

        }
    }
}