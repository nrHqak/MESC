using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace MESC.Controls
{
    public class RoundedPanel : Panel
    {
        public int Radius { get; set; } = 14;
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
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
