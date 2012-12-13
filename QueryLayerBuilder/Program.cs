using System;
using System.Collections.Generic;
using System.Globalization;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
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

#if ARCGIS_10_1            
            const esriLicenseProductCode productCode = esriLicenseProductCode.esriLicenseProductCodeAdvanced;
#else
            const esriLicenseProductCode productCode = esriLicenseProductCode.esriLicenseProductCodeArcInfo;
#endif
            if (!LicenseInitializer.InitializeApplication(new [] { productCode },
                                                          new esriLicenseExtensionCode[] { }))
            {
                Console.WriteLine(LicenseInitializer.LicenseMessage());
                Console.WriteLine("This application could not initialize with the correct ArcGIS license and will shutdown.");
                LicenseInitializer.ShutdownApplication();
                return 2;
            }

            //args = new[] { "Single", @"C:\tmp\Locations.lyr", @"dbclient=SQLServer;serverinstance=INPAKRO39088\SQL2008R2;database=Animal_Movement;authentication_mode=OSA", "SELECT * FROM Locations" };
            //args = new[] { "Multiple", @"C:\tmp\LACL_wolf_1120.lyr", @"dbclient=SQLServer;serverinstance=INPAKROMS53AIS;database=Animal_Movement;authentication_mode=OSA", "ProjectId = 'LACL_Wolf' AND AnimalId = 'LC1120'" };
            //args = new[] { "Prebuilt", @"C:\tmp\Template.lyr", @"C:\tmp\Locations.lyr", @"dbclient=SQLServer;serverinstance=INPAKRO39088\SQL2008R2;database=Animal_Movement;authentication_mode=OSA", "ProjectId = 'LACL_Sheep' AND AnimalId = '0602'" };

            if (args.Length < 2)
            {
                Console.WriteLine("Insufficient arguments.  Usage: QueryLayerBuilder Style Style_parameters");
                LicenseInitializer.ShutdownApplication();
                return 3;
            }

            var style = args[0].ToLower()[0];
            switch (style)
            {
                case 's':
                    if (args.Length < 4)
                    {
                        Console.WriteLine("Insufficient arguments.  Usage: QueryLayerBuilder Single \"Layer Name\" \"path\\to\\new\\file.lyr\" \"Connection string\" \"SQL SELECT Query\"");
                        LicenseInitializer.ShutdownApplication();
                        return 3;
                    }
                    SingleQueryLayer(args[1], args[2], args[3]);
                    break;
                case 'm':
                    if (args.Length < 3)
                    {
                        Console.WriteLine("Insufficient arguments.  Usage: QueryLayerBuilder Multiple \"path\\to\\new\\file.lyr\" [\"Connection string\"] [\"SQL predicate\"]");
                        LicenseInitializer.ShutdownApplication();
                        return 3;
                    }
                    MultipleQueryLayer(args[1], args[2], args.Length > 3 ?  args[3] : "");
                    break;
                case 'p': //Prebuilt
                    if (args.Length < 3)
                    {
                        Console.WriteLine("Insufficient arguments.  Usage: QueryLayerBuilder Prebuilt \"path\\to\\template.lyr\" \"path\\to\\new\\file.lyr\" [\"Connection string\"] [\"SQL predicate\"]");
                        LicenseInitializer.ShutdownApplication();
                        return 3;
                    }
                    //PrebuiltQueryLayer(args[1], args[2], (args.Length > 3 ? args[3] : ""), (args.Length > 4 ? args[4] : ""));
                    //break;
                    Console.WriteLine("QueryLayerBuilder Prebuilt Not implemented yet");
                    LicenseInitializer.ShutdownApplication();
                    return 3;
                default:
                    //throw new ArgumentOutOfRangeException("args", args[0], "Style is not recognized.");
                    Console.WriteLine("Style parameter `" + args[0] + "' is not recognized.");
                    LicenseInitializer.ShutdownApplication();
                    return 3;
            }

            LicenseInitializer.ShutdownApplication();
            return 0;
        }

        private static void SingleQueryLayer(string layerFilePath, string connection, string query)
        {
            string name = System.IO.Path.GetFileNameWithoutExtension(layerFilePath);
            var layer = BuildQueryLayer(connection, query, name, null, null, null);
            SaveLayerFile(layerFilePath, layer);
        }

        //private static void PrebuiltQueryLayer(string templatePath, string layerFilePath, string connection, string predicate)
        //{
        //    throw new NotImplementedException();
        //}

        private static void MultipleQueryLayer(string layerFilePath, string connection, string predicate)
        {
            var layers = new[]
                             {
                                 BuildSpecialQueryLayer("Velocity Vectors", connection, predicate),
                                 BuildSpecialQueryLayer("Valid Locations", connection, predicate),
                                 BuildSpecialQueryLayer("Invalid Locations", connection, predicate),
                                 BuildSpecialQueryLayer("No Movement Points", connection, predicate),
                             };
            var layer = BuildLayerGroup(layers);
            layer.Name = System.IO.Path.GetFileNameWithoutExtension(layerFilePath) ;
            SaveLayerFile(layerFilePath, layer);
        }

        // ESRI.ArcGIS.Carto.FeatureLayerClass has version specific dependencies on ESRI.ArcGIS.Display
        // which means that this code must be compiled specifically for each version of ArcGIS that it
        // will run on.

        private static ILayer BuildSpecialQueryLayer(string name, string connection, string predicate)
        {
            var geometryType = esriGeometryType.esriGeometryPoint;
            string oidFields = "ProjectId,AnimalId,FixDate";
            const int srid = 4326;
            string query;

            switch (name)
            {
                case "Invalid Locations":
                    query = "SELECT * FROM InvalidLocations " +
                            (String.IsNullOrEmpty(predicate) ? "" : "WHERE " + predicate.Replace("EndLocalDateTime", "LocalDateTime"));
                    break;
                case "Valid Locations":
                    query = "SELECT * FROM ValidLocations " +
                            (String.IsNullOrEmpty(predicate) ? "" : "WHERE " + predicate.Replace("EndLocalDateTime", "LocalDateTime"));
                    break;
                case "Velocity Vectors":
                    query = "SELECT * FROM VelocityVectors " +
                            (String.IsNullOrEmpty(predicate) ? "" : "WHERE " + predicate);
                    geometryType = esriGeometryType.esriGeometryPolyline;
                    oidFields = "ProjectId,AnimalId,LocalDateTime";
                    break;
                case "No Movement Points":
                    query = "SELECT * FROM NoMovement " +
                            (String.IsNullOrEmpty(predicate) ? "" : "WHERE " + predicate);
                    oidFields = "ProjectId,AnimalId,LocalDateTime";
                    break;
                default:
                    throw new ArgumentOutOfRangeException("name", name, "Unknown layer name");
            }

            var featureClass = BuildQueryFeatureClass(connection, query, name, oidFields, srid, geometryType);
            var featureLayer = new FeatureLayerClass { FeatureClass = featureClass, Name = name };
            return featureLayer;
        }

        private static ILayer BuildQueryLayer(string connection, string query, string name, string oidFields, int? srid, esriGeometryType? geometryType)
        {
            var featureClass = BuildQueryFeatureClass(connection, query, name, oidFields, srid, geometryType);
            var featureLayer = new FeatureLayerClass { FeatureClass = featureClass, Name = name };
            return featureLayer;
        }

        private static IFeatureClass BuildQueryFeatureClass(string connection, string query, string name, string oidFields, int? srid, esriGeometryType? geometryType)
        {
            var factory = new SqlWorkspaceFactoryClass();
            var workspace = factory.OpenFromString(connection, 0);
            //var workspace = factory.OpenFromFile(@"c:\tmp\AnimalMovement.sde", 0);
            var sqlWorkspace = (ISqlWorkspace)workspace;
            var queryDescription = sqlWorkspace.GetQueryDescription(query);
            if (!String.IsNullOrEmpty(oidFields))
                queryDescription.OIDFields = oidFields;
            if (srid.HasValue)
            {
                queryDescription.Srid = srid.Value.ToString(CultureInfo.InvariantCulture);
                var srEnv = new SpatialReferenceEnvironmentClass();
                queryDescription.SpatialReference = srEnv.CreateSpatialReference(srid.Value);
            }
            if (geometryType.HasValue)
                queryDescription.GeometryType = geometryType.Value;
            sqlWorkspace.CheckDatasetName(name, queryDescription, out name);
            var queryClass = sqlWorkspace.OpenQueryClass(name, queryDescription);
            var featureClass = (IFeatureClass)queryClass;
            return featureClass;
        }

        private static ILayer BuildLayerGroup(IEnumerable<ILayer> layers)
        {
            var layerGroup = new GroupLayerClass();
            foreach (var layer in layers)
            {
                layerGroup.Add(layer);
            }
            return layerGroup;
        }

        private static void SaveLayerFile(string layerFilePath, ILayer layer)
        {
            ILayerFile layerFile = new LayerFileClass();
            if (layerFile.IsPresent[layerFilePath])
            {
                //The user gave permission in the file save dialog to replace the existing file.
                try
                {
                    System.IO.File.Delete(layerFilePath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Unable to overwrite " + layerFilePath + Environment.NewLine + ex.Message);
                    Environment.Exit(4);
                }
            }
            layerFile.New(layerFilePath);
            layerFile.ReplaceContents(layer);
            layerFile.Save();
        }
    }
}
