using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Axes;
using OxyPlot.Series;

using fp_t = System.Decimal;
using roll_t = System.UInt16;
namespace Dice {

    public class Graph {

        private PlotModel Model;
        private int palettestatus;
        List<OxyColor> Pallette = new List<OxyColor>{Colours.Black,Colours.Red,Colours.Green, Colours.Blue, Colours.Cyan, Colours.Magenta, Colours.Yellow, Colours.Gray };

        public Graph (string title, string subtitle, bool use_XKCD = true) {

            Model = new PlotModel {

            LegendPosition = LegendPosition.LeftTop,
            LegendBorderThickness = 1, LegendFontSize = 18, LegendItemSpacing = 5,
            Title = title,
            Subtitle = subtitle,

            };
            Model.Axes.Add (new LinearAxis { Position = AxisPosition.Bottom });
            Model.Axes.Add (new LinearAxis { Position = AxisPosition.Left });
            if (use_XKCD) {
                Model.RenderingDecorator = rc => new XkcdRenderingDecorator (rc);
            }
            palettestatus = 0;
        }

        public void AddRolls (DiceRoll roll, string title, OxyColor? c = null, bool show_expected = false, bool stem = true, MarkerType marker_type = MarkerType.Cross) {
            OxyColor colour;
            if(c==null){
                colour = NextColour();
            } else {
                colour = c.Value;
            }

            var points = new List<DataPoint> ();
            for (int i = roll.Min (); i <= roll.Max (); i++) {
                if (roll.rolls.ContainsKey (i)) {
                    points.Add (new DataPoint (i, (double) roll.rolls[i]));
                } else if (!stem) {
                    points.Add (new DataPoint (i, 0));
                }
            }

            if (show_expected) {
                LineAnnotation Line = new LineAnnotation () {
                    StrokeThickness = 2,
                    Color = colour,
                    Type = LineAnnotationType.Vertical,
                    Text = "Expected",
                    TextColor = colour,
                    X = (double) roll.Expected (),
                    //Y = 0.0f
                };
                Model.Annotations.Add (Line);
            }

            if (stem) {
                var series = new StemSeries {
                    MarkerStroke = colour,
                    MarkerSize = 3,
                    MarkerFill = colour,
                    MarkerType = marker_type,
                    StrokeThickness = 0,
                    //Smooth = true,
                    Color = colour,
                    Title = title
                };
                series.MarkerStroke = series.Color;
                series.Points.AddRange (points);
                Model.Series.Add (series);
            } else {
                var series = new LineSeries {
                    MarkerType = marker_type,
                    MarkerSize = 7,
                    StrokeThickness = 0,
                    //Smooth = true,
                    Color = colour,
                    Title = title
                };
                series.Points.AddRange (points);
                Model.Series.Add (series);
            }

        }

        public void export (string path, int w = 1280, int h = 720) {
            using (var stream = File.Create (path)) {
                // Export to svg, store in memory stream
                var exporter = new SvgExporter { Width = w, Height = h };
                exporter.Export (Model, stream);

            }
        }

        private OxyColor NextColour(){
            if(palettestatus == Pallette.Count)palettestatus=0;
            return Pallette[palettestatus++];
        }

    }
}