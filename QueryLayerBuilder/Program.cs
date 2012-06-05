using System;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.DataSourcesGDB;

namespace QueryLayerBuilder
{
    class Program
    {
        private static readonly LicenseInitializer LicenseInitializer = new LicenseInitializer();
    
        [STAThread]
        static int Main(string[] args)
        {
            //In ArcGIS 10.1, this is called esriLicenseProductCode.esriLicenseProductCodeAdvanced
            
            if (!LicenseInitializer.InitializeApplication(new [] { esriLicenseProductCode.esriLicenseProductCodeArcInfo },
                                                          new esriLicenseExtensionCode[] { }))
            {
                Console.WriteLine(LicenseInitializer.LicenseMessage());
                Console.WriteLine("This application could not initialize with the correct ArcGIS license and will shutdown.");
                LicenseInitializer.ShutdownApplication();
                return 2;
            }
            if (args.Length < 4)
            {
                Console.WriteLine("Insufficient arguments.  Usage: QueryLayerBuilder \"Layer Name\" \"path\\to\\new\\file.lyr\" \"Connection string\" \"SQL SELECT Query\"");
                return 3;
            }

            //args = new[] { "Locations", @"C:\tmp\Locations.lyr", @"dbclient=SQLServer;serverinstance=INPAKRO39088\SQL2008R2;database=Animal_Movement;authentication_mode=OSA", "SELECT * FROM Locations" };
            var name = args[0];
            var layerFilePath = args[1];
            var connection = args[2];
            var query = args[3];
            var layer = BuildQueryLayer(connection, query, name);
            SaveLayerFile(layerFilePath, layer);

            LicenseInitializer.ShutdownApplication();
            return 0;
        }

        // ESRI.ArcGIS.Carto.FeatureLayerClass has version specific dependencies on ESRI.ArcGIS.Display
        // which means that this code must be compiled specifically for each version of ArcGIS that it
        // will run on.

        private static ILayer BuildQueryLayer(string connection, string query, string name)
        {
            var factory = new SqlWorkspaceFactoryClass();
            var workspace = factory.OpenFromString(connection, 0);
            var sqlWorkspace = (ISqlWorkspace)workspace;
            var queryDescription = sqlWorkspace.GetQueryDescription(query);
            sqlWorkspace.CheckDatasetName(name, queryDescription, out name);
            var queryClass = sqlWorkspace.OpenQueryClass(name, queryDescription);
            var featureClass = (IFeatureClass)queryClass;
            var featureLayer = new FeatureLayerClass {FeatureClass = featureClass, Name = name};
            return featureLayer;
        }

        private static void SaveLayerFile(string layerFilePath, ILayer layer)
        {
            ILayerFile layerFile = new LayerFileClass();
            if (layerFile.IsPresent[layerFilePath])
            {
                Console.WriteLine("Layer file exists.");
                Environment.Exit(4);
            }
            layerFile.New(layerFilePath);
            layerFile.ReplaceContents(layer);
            layerFile.Save();
        }
    }
}
