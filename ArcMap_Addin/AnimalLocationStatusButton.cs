using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
//using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace ArcMap_Addin
{
    public class AnimalLocationStatusButton : ESRI.ArcGIS.Desktop.AddIns.Button
    {
        protected override void OnClick()
        {
            try
            {
                // valid layers are query layers that are derived from the location table in SQL Server
                // the value in the dictionary is the number of features selected in that layer.
                Dictionary<IGeoFeatureLayer, int> validLayers = GetValidLayers();

                //No valid layers
                if (validLayers.Count == 0)
                {
                    MessageBox.Show("There are no query layers in your map that are derived from the animal location data in SQL Server.",
                        "Unable to change status of animal locations");
                    return;
                }

                int validLayersWithSelectedFeatures = validLayers.Count(validLayer => validLayer.Value > 0);

                //not enough valid layers with selected features
                if (validLayersWithSelectedFeatures == 0)
                {
                    MessageBox.Show("You must select the animal locations that you wish to change.", "Unable to change status of animal locations");
                    return;
                }

                //too many valid layers with selected features
                if (validLayersWithSelectedFeatures > 1)
                {
                    MessageBox.Show("There are multiple animal location query layers with selected features.\n" +
                    "It is not clear which set of features you want to change." +
                    "Please clear the selections on all but one layer.", "Unable to change status of animal locations");
                    return;
                }

                //The actionLayer is the one that is just right
                IGeoFeatureLayer actionLayer = validLayers.First(validLayer => validLayer.Value > 0).Key;

                // get user's prefered action
                DialogResult action = MessageBox.Show("Click 'Yes' to hide the selected features.\n" +
                    "Click 'No' to unhide the selected features.\n" +
                    "Click 'Cancel' to leave everything the way it is.\n\n" +
                    "Hidden locations are effectively deleted.\n" +
                    "They are ignored when determining animal movement."
                    , "What Do you want to do?", MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question);
                if (action == DialogResult.Yes)
                    try
                    {
                        HideSelectedFeatures(actionLayer);
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show(ex.Message, "Unable to hide features", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                if (action == DialogResult.No)
                    try
                    {
                        UnHideSelectedFeatures(actionLayer);
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show(ex.Message, "Unable to unhide features", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                if (action != DialogResult.Cancel)
                    ArcMap.Document.ActiveView.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unhandled Exception in OnClick Method\n\n" + ex, "Updating animal location status");
            }
        }

        private static Dictionary<IGeoFeatureLayer, int> GetValidLayers()
        {
            var results = new Dictionary<IGeoFeatureLayer, int>();
            // A valid layer is a remote database connection to SQL Server
            // wherein the resulting query class (ITable) has a text(8) column named ProjectId,
            // a text(8) column named AnimalId, and a datetime column named 'FixDate'
            // The layer does NOT need the status field, as it is not read by this add-in,
            // However, the Location table must have a status field.
            // There may be more other layers that meet this description, so it is up to the user
            // to ensure that the correct layer has selected features before being modified.
            const string geoFeatureLayerTypeId = "{E156D7E5-22AF-11D3-9F99-00C04F6BC78E}";
            IEnumerable<ILayer> layers = LayerUtils.GetAllLayers(ArcMap.Document, geoFeatureLayerTypeId);
            // ReSharper disable once PossibleInvalidCastExceptionInForeachLoop
            foreach (IGeoFeatureLayer layer in layers)
            {
                if (!(((IDataset)layer).Workspace is ISqlWorkspace w))
                    continue;
                //Debug.Print("query = {0}", w.GetQueryDescription("").Query);
                var connectionProperties = GetProperties(((IWorkspace)w).ConnectionProperties);
                if (connectionProperties["DBCLIENT"].Equals("sqlserver", StringComparison.InvariantCultureIgnoreCase) &&
                    HasCorrectColumns(layer.FeatureClass))
                    results[layer] = ((IFeatureSelection)layer).SelectionSet.Count;
            }
            return results;
        }

        private static bool HasCorrectColumns(IFeatureClass featureClass)
        {
            int i1 = featureClass.Fields.FindField("ProjectId");
            int i2 = featureClass.Fields.FindField("AnimalId");
            int i3 = featureClass.Fields.FindField("FixDate");
            // We do not check for status, because it may not be in the query layer even though it is in the database
            if (i1 < 0 || i2 < 0 || i3 < 0)
                return false;
            if (featureClass.Fields.Field[i1].Length != 16 ||
                featureClass.Fields.Field[i1].Type != esriFieldType.esriFieldTypeString)
                return false;
            if (featureClass.Fields.Field[i2].Length != 16 ||
                featureClass.Fields.Field[i2].Type != esriFieldType.esriFieldTypeString)
                return false;
            if (featureClass.Fields.Field[i3].Type != esriFieldType.esriFieldTypeDate)
                return false;
            return true;
        }

        private static void HideSelectedFeatures(IGeoFeatureLayer actionLayer)
        {
            UpdateRows(actionLayer, "H");
        }

        private static void UnHideSelectedFeatures(IGeoFeatureLayer actionLayer)
        {
            UpdateRows(actionLayer, null);
        }

        static void UpdateRows(IGeoFeatureLayer actionLayer, string status)
        {
            const string sql = "EXEC [dbo].[Location_UpdateStatus] @ProjectId, @AnimalId, @FixDate, @Status;";

            string connectionString = BuildConnectionString(actionLayer);

            using (var connection = new SqlConnection(connectionString))
            {
                using (var cmd = new SqlCommand(sql, connection))
                {
                    cmd.Parameters.Add("@ProjectId", SqlDbType.NVarChar, 255);
                    cmd.Parameters.Add("@AnimalId", SqlDbType.NVarChar, 255);
                    cmd.Parameters.Add("@FixDate", SqlDbType.DateTime2);
                    cmd.Parameters.Add("@Status", SqlDbType.Char, 1);
                    cmd.Parameters["@Status"].Value = (object)status ?? DBNull.Value;

                    connection.Open();

                    var features = ((IFeatureSelection)actionLayer).SelectionSet;
                    features.Search(null, true, out ICursor cursor);
                    int projectIndex = cursor.FindField("ProjectId");
                    int animalIndex = cursor.FindField("AnimalId");
                    int dateIndex = cursor.FindField("FixDate");
                    IRow row = cursor.NextRow();
                    while (row != null)
                    {
                        cmd.Parameters["@ProjectId"].Value = row.Value[projectIndex];
                        cmd.Parameters["@AnimalId"].Value = row.Value[animalIndex];
                        cmd.Parameters["@FixDate"].Value = row.Value[dateIndex];
                        cmd.ExecuteNonQuery();
                        row = cursor.NextRow();
                    }
                }
            }
        }

        private static string BuildConnectionString(IGeoFeatureLayer actionLayer)
        {
            // See http://help.arcgis.com/en/sdk/10.0/arcobjects_net/conceptualhelp/index.html#/Working_with_SQL_workspaces/0001000003z8000000/SQL 
            // for a description the connection properties: "DBCLIENT", "SERVERINSTANCE", "DATABASE", "AUTHENTICATION_MODE", "USER", "PASSWORD".

            var connectionProperties = GetProperties(((IDataset)actionLayer).Workspace.ConnectionProperties);

#if ARCGIS_10_0            
            string localServer = connectionProperties["SERVERINSTANCE"];
#else
            string localServer = connectionProperties["DB_CONNECTION_PROPERTIES"];
#endif
            string localDatabase = connectionProperties["DATABASE"];
            string connectionString = string.Format("server={0};Database={1};",
                                                    localServer,
                                                    localDatabase);
            if (connectionProperties["AUTHENTICATION_MODE"] == "OSA")
                connectionString += "Trusted_Connection=True;";
            else
                connectionString += "Trusted_Connection=False;" +
                                    string.Format("User Id={0};Password={1};",
                                    connectionProperties["USER"],
                                    connectionProperties["PASSWORD"]);

            //Connect to the local server to find the master server, and change the connection string appropriately
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var cmd = new SqlCommand("SELECT [Connection],[Database] FROM [dbo].[LookupQueryLayerServers] WHERE [Location] = 'AKRO'", connection))
                {
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        var masterServer = (string)reader["Connection"];
                        var masterDatabase = (string)reader["Database"];
                        connectionString = connectionString.Replace(localServer, masterServer).Replace(localDatabase, masterDatabase);
                    }
                }
            }

            return connectionString;
        }

        protected override void OnUpdate()
        {
            Enabled = ArcMap.Application != null;
        }

        private static Dictionary<string, string> GetProperties(IPropertySet propertySet)
        {
            var results = new Dictionary<string, string>();
            propertySet.GetAllProperties(out object n, out object v);
            var names = (object[])n;
            var values = (object[])v;
            for (int i = 0; i < names.Length; i++)
                results[names[i].ToString()] = values[i].ToString();
            return results;
        }
    }

}
