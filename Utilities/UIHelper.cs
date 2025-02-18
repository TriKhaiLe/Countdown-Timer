using System.Collections.Generic;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ChroniTask
{
    internal class UIHelper
    {
        private static readonly Random _random = new Random();

        private static readonly (Color Start, Color End)[] GradientColors = new (Color Start, Color End)[]
        {
        (Color.FromArgb(255, 153, 102), Color.FromArgb(200, 94, 58)),   // Sunrise
        (Color.FromArgb(0, 204, 255), Color.FromArgb(0, 102, 204)),     // Ocean Breeze
        (Color.FromArgb(102, 255, 178), Color.FromArgb(34, 139, 34)),   // Forest Green
        (Color.FromArgb(204, 153, 255), Color.FromArgb(102, 0, 150)),   // Lavender Field
        (Color.FromArgb(255, 140, 0), Color.FromArgb(200, 69, 10)),      // Sunset
        (Color.FromArgb(25, 25, 112), Color.FromArgb(200, 100, 128)),       // Midnight
        (Color.FromArgb(255, 182, 193), Color.FromArgb(255, 105, 180)), // Cotton Candy
        (Color.FromArgb(255, 255, 102), Color.FromArgb(255, 204, 51)),  // Lemonade
        (Color.FromArgb(255, 192, 203), Color.FromArgb(255, 105, 97)),  // Cherry Blossom
        (Color.FromArgb(173, 216, 230), Color.FromArgb(0, 255, 127))    // Cool Mint
        };

        public static void InitializeUI(MainWindow form)
        {
            var gradient = GradientColors[_random.Next(GradientColors.Length)];

            form.Paint += (sender, e) =>
            {
                if (form.ClientRectangle.Width > 0 && form.ClientRectangle.Height > 0)
                {
                    using (LinearGradientBrush brush = new LinearGradientBrush(
                        form.ClientRectangle,
                        gradient.Start,
                        gradient.End,
                        45F))
                    {
                        e.Graphics.FillRectangle(brush, form.ClientRectangle);
                    }
                }
            };

            form.Invalidate(); // Force the form to repaint
            form.period_box.Text = "45";
        }
    }
}