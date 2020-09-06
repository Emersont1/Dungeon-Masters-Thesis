using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

using fp_t = System.Decimal;
using roll_t = System.UInt16;
namespace Dice {

    public class Graph {

        PlotModel Model;

        public Graph (string title, string subtitle, bool use_XKCD = true) {

            Model = new PlotModel {

            LegendPosition = LegendPosition.LeftTop,
            LegendBorderThickness = 1, LegendFontSize = 18, LegendItemSpacing = 5,
            Title = title,
            Subtitle = subtitle,

            };
            if (use_XKCD) {
                Model.RenderingDecorator = rc => new XkcdRenderingDecorator (rc);
            }
        }

        public void AddRolls (DiceRoll roll, string title /*, OxyColor c*/ ) {
            LineSeries l = new LineSeries ();
            for (int i = roll.Min (); i <= roll.Max (); i++) {
                if (roll.rolls.ContainsKey (i)) {
                    l.Points.Add (new DataPoint (i, (double) roll.rolls[i]));
                } else {
                    l.Points.Add (new DataPoint (i, 0));
                }
            }
            foreach (var p in roll.rolls) {

            }
            // l.Smooth = true;
            l.MarkerType = MarkerType.Circle;
            l.MarkerSize = 7;
            l.StrokeThickness = 2;
            //l.Color =c;
            l.Title = title;
            Model.Series.Add (l);
        }

        public void export (string path, int w = 1280, int h = 720) {
            using (var stream = File.Create (path)) {
                // Export to svg, store in memory stream
                var exporter = new SvgExporter { Width = w, Height = h };
                exporter.Export (Model, stream);

            }
        }

    }
}