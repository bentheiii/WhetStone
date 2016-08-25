using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WhetStone.Enviroment
{
    public static class capture
    {
        public static Image Capture(Rectangle r, bool showcursor = true)
        {
            Bitmap bitmap = new Bitmap(r.Width, r.Height);
            Graphics g = Graphics.FromImage(bitmap);
            g.CopyFromScreen(r.Location, Point.Empty, r.Size);
            if (showcursor)
            {
                Point p = new Point(Cursor.Position.X - r.Location.X, Cursor.Position.Y - r.Location.Y);
                Rectangle cursorBounds = new Rectangle(p, Cursor.Current.Size);
                Cursors.Default.Draw(g, cursorBounds);
            }
            return bitmap;
        }
        public static Image Capture(bool showcursor = true)
        {
            return Capture(Screen.GetBounds(Screen.GetBounds(Point.Empty)), showcursor);
        }
        public static Image Capture(Form f, bool showcursor = true)
        {
            Rectangle r = new Rectangle(f.Location, f.Size);
            return Capture(r, showcursor);
        }
    }
}
