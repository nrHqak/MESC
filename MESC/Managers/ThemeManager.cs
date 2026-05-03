using System.Drawing;
using System.Windows.Forms;

namespace MESC.Managers
{
    public static class ThemeManager
    {
        public static bool IsDark { get; private set; }
        public static void Toggle(Form form)
        {
            IsDark = !IsDark;
            Apply(form);
        }
        public static void Apply(Control root)
        {
            var bg = IsDark ? Color.FromArgb(30, 41, 59) : Color.FromArgb(248, 250, 252);
            var fg = IsDark ? Color.WhiteSmoke : Color.FromArgb(30, 41, 59);
            root.BackColor = bg;
            root.ForeColor = fg;
            foreach (Control c in root.Controls) Apply(c);
        }
    }
}
