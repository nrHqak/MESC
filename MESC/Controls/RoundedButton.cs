using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace MESC.Controls
{
    public class RoundedButton : Button
    {
        public int Radius { get; set; } = 18;
        private Color _baseColor = Color.FromArgb(37, 99, 235);
        public Color BaseColor { get => _baseColor; set { _baseColor = value; BackColor = value; } }
        public Color HoverColor { get; set; } = Color.FromArgb(59, 130, 246);

        public RoundedButton()
        {
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            ForeColor = Color.White;
            BackColor = BaseColor;
            Font = new Font("Segoe UI", 10, FontStyle.Bold);
            Cursor = Cursors.Hand;
            MouseEnter += (s, e) => BackColor = HoverColor;
            MouseLeave += (s, e) => BackColor = BaseColor;
            Resize += (s, e) => UpdateRegion();
            UpdateRegion();
        }

        private void UpdateRegion()
        {
            using (var path = new GraphicsPath())
            {
                path.AddArc(0, 0, Radius, Radius, 180, 90);
                path.AddArc(Width - Radius, 0, Radius, Radius, 270, 90);
                path.AddArc(Width - Radius, Height - Radius, Radius, Radius, 0, 90);
                path.AddArc(0, Height - Radius, Radius, Radius, 90, 90);
                path.CloseFigure();
                Region = new Region(path);
            }
        }
    }
}
