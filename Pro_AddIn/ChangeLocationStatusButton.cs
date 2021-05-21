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
using System.Data;
using System.Data.SqlClient;
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
        async protected override void OnClick()
        {
            if (MapView.Active == null)
            {
                MessageBox.Show("This command requires an active map.", "No Map");
                return;
            }
            try
            {
                // valid layers are query layers that are derived from the location table in SQL Server
                // the value in the dictionary is the number of features selected in that layer.
                Dictionary<FeatureLayer, int> validLayers = await GetValidLayersAsync();

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
                    MessageBox.Show("You must select the animal locations that you wish to change.",
                        "Unable to change status of animal locations");
                    return;
                }

                //too many valid layers with selected features
                if (validLayersWithSelectedFeatures > 1)
                {
                    MessageBox.Show("There are multiple animal location query layers with selected features.\n" +
                    "It is not clear which set of features you want to change." +
                    "Please clear the selections on all but one layer.",
                    "Unable to change status of animal locations");
                    return;
                }

                //The actionLayer is the one that is just right
                FeatureLayer actionLayer = validLayers.First(validLayer => validLayer.Value > 0).Key;

                // get user's prefered action
                System.Windows.MessageBoxResult action;
                action = MessageBox.Show("Click 'Yes' to hide the selected features.\n" +
                    "Click 'No' to unhide the selected features.\n" +
                    "Click 'Cancel' to leave everything the way it is.\n\n" +
                    "Hidden locations are ignored when determining animal movement."
                    , "What Do you want to do?", System.Windows.MessageBoxButton.YesNoCancel,
                    System.Windows.MessageBoxImage.Question);
                if (action == System.Windows.MessageBoxResult.Yes)
                {
                    try
                    {
                        await HideSelectedFeaturesAsync(actionLayer);
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show(ex.Message, "Unable to hide features", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                    }
                }

                if (action == System.Windows.MessageBoxResult.No)
                {
                    try
                    {
                        await UnHideSelectedFeaturesAsync(actionLayer);
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show(ex.Message, "Unable to unhide features", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                    }
                }

                if (action != System.Windows.MessageBoxResult.Cancel)
                {
                    await MapView.Active.RedrawAsync(false);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unhandled Exception in OnClick Method\n\n" + ex, "Updating animal location status");
            }

        }

        private async Task<Dictionary<FeatureLayer, int>> GetValidLayersAsync()
        {
            var results = new Dictionary<FeatureLayer, int>();
            // A valid layer is a Query Layer connection to SQL Server database
            // We do not check the server name or the database name, as the layer could be pointing to a replica
            // A valid layer is also a point layer with 3 fields we need values from to write the update query
            var layers = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>();
            await QueuedTask.Run(() => {
                foreach (FeatureLayer layer in layers)
                {
                    if (!(layer.GetDataConnection() is CIMSqlQueryDataConnection connection))
                    {
                        continue;
                    }
                    var isSqlServer = connection.WorkspaceConnectionString.Contains("sqlserver", StringComparison.OrdinalIgnoreCase);
                    var isPoint = connection.GeometryType == esriGeometryType.esriGeometryPoint;
                    // Note that CIMSqlQueryDataConnection has a queryFields property that has the field info we need, but it is not exposed in the API
                    if (isSqlServer && isPoint && HasCorrectColumns(layer.GetFeatureClass().GetDefinition()))
                    {
                        results[layer] = layer.SelectionCount;
                    }
                }
            });
            return results;
        }

        private static bool HasCorrectColumns(TableDefinition tableDef)
        {
            Field projectIdField = tableDef.GetFields().FirstOrDefault(x => x.Name.Equals("ProjectId"));
            if (projectIdField == null || projectIdField.Length != 16 || projectIdField.FieldType != FieldType.String)
            {
                return false;
            }

            Field animalIdField = tableDef.GetFields().FirstOrDefault(x => x.Name.Equals("AnimalId"));
            if (animalIdField == null || animalIdField.Length != 16 || animalIdField.FieldType != FieldType.String)
            {
                return false;
            }

            Field fixDateField = tableDef.GetFields().FirstOrDefault(x => x.Name.Equals("FixDate"));
            if (fixDateField == null || fixDateField.FieldType != FieldType.Date)
            {
                return false;
            }
            // We do not check for status, because it may not be in the query layer even though it is in the database
            return true;
        }

        private static async Task HideSelectedFeaturesAsync(FeatureLayer actionLayer)
        {
            await UpdateRowsAsync(actionLayer, "H");
        }

        private static async Task UnHideSelectedFeaturesAsync(FeatureLayer actionLayer)
        {
            await UpdateRowsAsync(actionLayer, null);
        }

        private static async Task UpdateRowsAsync(FeatureLayer actionLayer, string status)
        {
            const string sql = "EXEC [dbo].[Location_UpdateStatus] @ProjectId, @AnimalId, @FixDate, @Status;";

            await QueuedTask.Run(() => {
                var connectionString = BuildSqlConnectionString(actionLayer);
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
                        using (var features = actionLayer.GetSelection())
                        {
                            var cursor = features.Search(null, true);
                            int projectIndex = cursor.FindField("ProjectId");
                            int animalIndex = cursor.FindField("AnimalId");
                            int dateIndex = cursor.FindField("FixDate");
                            while (cursor.MoveNext())
                            {
                                var row = cursor.Current;
                                cmd.Parameters["@ProjectId"].Value = row.GetOriginalValue(projectIndex);
                                cmd.Parameters["@AnimalId"].Value = row.GetOriginalValue(animalIndex);
                                cmd.Parameters["@FixDate"].Value = row.GetOriginalValue(dateIndex);
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                }
            });
        }

        /// <summary>
        /// Use the layer's database connection information to get a SQL Server Connection string.
        /// 
        /// The layer may be connected to a replica.  We will ask the replica for the name of the
        /// master server and database.  See https://www.connectionstrings.com/sql-server/.
        /// </summary>
        /// <remarks>
        /// I could not find official documentation on the CIMSqlQueryDataConnection.WorkspaceConnectionString
        /// Samples from a Pro Layer file (consistent across several ways of creating a Query layer):
        /// Keys: [SERVER, INSTANCE, DBCLIENT, DB_CONNECTION_PROPERTIES, DATABASE, AUTHENTICATION_MODE]
        /// From other samples online had VERSION, USER and ENCRYPTED_PASSWORD when using AUTHENTICATION_MODE=DBMS (vs. OSA) 
        /// </remarks>
        /// <param name="layer">A feature layer with a data connection to location points in SQL database</param>
        /// <returns>A SQL Server Connection string</returns>
        private static string BuildSqlConnectionString(FeatureLayer layer)
        {
            var dataConnection = (CIMSqlQueryDataConnection)layer.GetDataConnection();
            var connectionProperties = ConnectionProperties(dataConnection.WorkspaceConnectionString);
            string localServer = connectionProperties["SERVER"];
            string localDatabase = connectionProperties["DATABASE"];
            string connectionString = string.Format("server={0};Database={1};",
                                                    localServer,
                                                    localDatabase);
            if (connectionProperties["AUTHENTICATION_MODE"] == "OSA")
            {
                connectionString += "Trusted_Connection=True;";
            }
            else
            {
                connectionString += "Trusted_Connection=False;" +
                                    string.Format("User Id={0};Password={1};",
                                    connectionProperties["USER"],
                                    connectionProperties["PASSWORD"]);
            }

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

        /// <summary>
        /// Converts an ArcGIS Connection string to a key/value dictionary.
        ///
        /// Problems with connection string are silently ignored.
        /// </summary>
        /// <param name="arcgisConnection">A string in the form 'key1=value1;...;keyN=valueN'</param>
        /// <returns>The dictionary of keys and values</returns>
        private static Dictionary<string,string> ConnectionProperties(string arcgisConnection)
        {
            var properties = new Dictionary<string, string>();
            foreach (var pair in arcgisConnection.Split(';'))
            {
                var keyValue = pair.Split('=');
                if (keyValue.Length < 2) { continue; }
                properties[keyValue[0]] = keyValue[1];
            }
            return properties;
        }
    }

    public static class StringExtensions
    {
        public static bool Contains(this string source, string toCheck, StringComparison comp)
        {
            return source?.IndexOf(toCheck, comp) >= 0;
        }
    }
}
