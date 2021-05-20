using ArcGIS.Core.CIM;
using ArcGIS.Core.Data;
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Catalog;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Editing;
using ArcGIS.Desktop.Extensions;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;
using ArcGIS.Desktop.Framework.Dialogs;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Layouts;
using ArcGIS.Desktop.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimalMovement
{
    /// <summary>
    /// A button to change the status of an animal location.
    /// 
    /// Locations with a non-null status are not "valid" and are not included in
    /// the movement vectors.  The status (a single character code) indicates why
    /// the location is not valid.  The code 'H' is general used for hiding bad GPS data.
    /// 
    /// Query layers are not editable, so this button makes use of the connection information
    /// in the selected layer to edit the SQL server database on the user's behalf.
    /// </summary>
    internal class ChangeLocationStatusButton : Button
    {
        protected override void OnClick()
        {
            var title = "Configuration Error";
            var msg = "The code for this button is not finished.";
            MessageBox.Show(msg, title,
                System.Windows.MessageBoxButton.OK,
                System.Windows.MessageBoxImage.Error);

        }
    }
}
