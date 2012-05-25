using System.Drawing;
using System.Windows.Forms;

namespace AnimalMovement
{
    internal class BaseForm : Form
    {

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);

            Properties.Settings.Default[Name + "Location"] = Location;
            Properties.Settings.Default[Name + "Size"] = Size;
            //Properties.Settings.Default[Name + "WindowState"] = Convert.ToString(this.WindowState, CultureInfo.CurrentCulture);
            Properties.Settings.Default.Save();
        }

        protected void RestoreWindow()
        {
            //var windowState = (string)Properties.Settings.Default[Name + "WindowState"];
            //switch (windowState)
            //{
            //    case "Normal":
            //        WindowState = FormWindowState.Normal;
            //        break;
            //    case "Maximized":
            //        WindowState = FormWindowState.Maximized;
            //        break;
            //    case "Minimized":
            //        WindowState = FormWindowState.Minimized;
            //        break;
            //}

            var location = (Point)Properties.Settings.Default[Name + "Location"];
            if (location.X == 0 && location.Y == 0)
                StartPosition = FormStartPosition.WindowsDefaultLocation;
            else
            {
                StartPosition = FormStartPosition.Manual;
                Location = location;
            }

            var size = (Size)Properties.Settings.Default[Name + "Size"];
            if (Size.Height == 0 || Size.Width == 0)
                return;
            Size = size;
        }
    }
}
