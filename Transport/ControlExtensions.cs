using System.Drawing.Drawing2D;

namespace Transport
{
    public static class ControlExtensions
    {
        /// <summary>
        /// Applies a scaling transformation to a control (for hover effects)
        /// </summary>
        /// <param name="control">The control to transform</param>
        /// <param name="scale">The scale factor (1.0 = normal size, 1.1 = 10% larger)</param>
        public static void Transform(this Control control, float scale)
        {
            if (control == null) return;

            // Calculate the new size
            int newWidth = (int)(control.Width * scale);
            int newHeight = (int)(control.Height * scale);

            // Calculate the position adjustment to keep the control centered
            int deltaX = (newWidth - control.Width) / 2;
            int deltaY = (newHeight - control.Height) / 2;

            // Apply the transformation
            control.Size = new Size(newWidth, newHeight);
            control.Location = new Point(control.Location.X - deltaX, control.Location.Y - deltaY);
        }

        /// <summary>
        /// Adds rounded corners to a control using a graphics path
        /// </summary>
        /// <param name="control">The control to apply rounded corners to</param>
        /// <param name="radius">The corner radius</param>
        public static void AddRoundedCorners(this Control control, int radius)
        {
            if (control == null) return;

            GraphicsPath path = new GraphicsPath();
            path.AddArc(0, 0, radius, radius, 180, 90);
            path.AddArc(control.Width - radius, 0, radius, radius, 270, 90);
            path.AddArc(control.Width - radius, control.Height - radius, radius, radius, 0, 90);
            path.AddArc(0, control.Height - radius, radius, radius, 90, 90);
            path.CloseFigure();

            control.Region = new Region(path);
        }

        /// <summary>
        /// Applies a gradient background for a control
        /// </summary>
        /// <param name="control">The control to apply gradient to</param>
        /// <param name="startColor">Starting color of the gradient</param>
        /// <param name="endColor">Ending color of the gradient</param>
        /// <param name="mode">Gradient direction</param>
        public static void ApplyGradientBackground(this Control control, Color startColor, Color endColor, LinearGradientMode mode = LinearGradientMode.Vertical)
        {
            if (control == null) return;

            control.Paint += (sender, e) =>
            {
                using (LinearGradientBrush brush = new LinearGradientBrush(
                    control.ClientRectangle, startColor, endColor, mode))
                {
                    e.Graphics.FillRectangle(brush, control.ClientRectangle);
                }
            };
        }

        /// <summary>
        /// Adds a drop shadow effect to a control
        /// </summary>
        /// <param name="control">The control to add shadow to</param>
        /// <param name="shadowColor">Color of the shadow</param>
        /// <param name="offset">Shadow offset</param>
        public static void AddDropShadow(this Control control, Color shadowColor, Point offset)
        {
            if (control == null) return;

            control.Paint += (sender, e) =>
            {
                // Draw shadow
                Rectangle shadowRect = new Rectangle(
                    control.ClientRectangle.X + offset.X,
                    control.ClientRectangle.Y + offset.Y,
                    control.ClientRectangle.Width,
                    control.ClientRectangle.Height);

                using (SolidBrush shadowBrush = new SolidBrush(shadowColor))
                {
                    e.Graphics.FillRectangle(shadowBrush, shadowRect);
                }
            };
        }

        /// <summary>
        /// Adds a glow effect to a control
        /// </summary>
        /// <param name="control">The control to add glow to</param>
        /// <param name="glowColor">Color of the glow</param>
        /// <param name="glowSize">Size of the glow effect</param>
        public static void AddGlowEffect(this Control control, Color glowColor, int glowSize = 5)
        {
            if (control == null) return;

            control.Paint += (sender, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                // Create glow effect with multiple overlapping rectangles
                for (int i = 0; i < glowSize; i++)
                {
                    Color currentGlowColor = Color.FromArgb(
                        Math.Max(0, glowColor.A - (i * 30)),
                        glowColor.R, glowColor.G, glowColor.B);

                    using (Pen glowPen = new Pen(currentGlowColor, i + 1))
                    {
                        Rectangle glowRect = new Rectangle(
                            -i, -i,
                            control.Width + (i * 2) - 1,
                            control.Height + (i * 2) - 1);

                        e.Graphics.DrawRectangle(glowPen, glowRect);
                    }
                }
            };
        }
    }
}