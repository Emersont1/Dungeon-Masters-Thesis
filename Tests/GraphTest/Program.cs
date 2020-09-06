    using System.IO;
    using System;
    using OxyPlot.Axes;
    using OxyPlot.Series;
    using OxyPlot;

    namespace GraphTest {
        class Program {
            static void Main (string[] args) {
                var model = new PlotModel {
                    Title = "XKCD style plot",
                    Subtitle = "Install the 'Humor Sans' font for the best experience",
                    RenderingDecorator = rc => new XkcdRenderingDecorator (rc)
                };
                model.Series.Add (new FunctionSeries (Math.Sin, 0, 10, 50, "sin(x)"));

                using (var stream = File.Create ("image.svg")) {
                    // Export to svg, store in memory stream
                    var exporter = new SvgExporter { Width = 1280, Height = 720 };
                    exporter.Export (model, stream);

                }
            }

        }
    }