using System.Reflection;
using System.Runtime.InteropServices;

//TODO - DATABASE - Add business logic to ensure that all Deployments.Collar.DeploymentDate < Collar.DisposalDate
//TODO - DATABASE - Add logic to limit collar fixes to those before the Collar.DisposalDate
//TODO - DATABASE - Move collar deployment date checking from Stored procedure to trigger to protect against SA and to allow bulk uploading.
//TODO - DATABASE - Add more unit testing.
//TODO - DATABASE - Write local time to the Location and movements layers - make the views simpler/faster
//TODO - DATABASE - Writing local time to the Location and movements layers, will simplify replication - do not replicate localtime function

//TODO - Provide some global QAQC tools - I.e. show all collars with conflict in the last x days
//TODO - Provide some global QAQC tools - I.e. show all collars with multiple parameter files
//TODO - Provide some global QAQC tools - I.e. show files with overlapping fix dates
//TODO - Provide some global QAQC tools - I.e. show Telonics collars that do not have a TPF (Gen4) or FixPeriod (Gen3) or do have active PPF file (Gen3)
//TODO - Provide some global QAQC tools - I.e. show Telonics Gen4 collars that have multiple active TPF files
//TODO - Provide some global QAQC tools - I.e. show Telonics collars that have duplicate Alternative Id and identical Disposal dates
//TODO - build tool to visualize deployments (i.e. show a graphical time line of animals & collars)
//TODO - Create a Mortality Layer
//TODO - Create a layer of last location (or locations)
//TODO - Create a simple location layer, create a table of animal data, and join in ArcMap
//TODO - Replace the wait cursors with a message box and progress bar
//TODO - Add a warning (consent to monitoring) message at start up.
//TODO - How do I add this warning to a layer file??
//TODO - Support other file formats
//TODO - 	a) support data as it is/was delivered to the PI
//TODO - 	b) support direct download from Argos web services
//TODO - Replication to remote locations.
//TODO - 	a) Application/input  will all happen on the central server, which will push all data out to remote locations for fast readonly access.
//TODO - help documents/tutorials
//TODO - bulk upload (data grid view copy/paste) of multiple animals/collars/deploymnents (optional - sa can do this for PI using SSMS)
//TODO - Add MS Access readonly interface
//TODO - Add R statistics interface and adehabitat example

//Deployment Notes:
//  The connection string for the DataModel.dll (app.config) must be copied in and replace the connection string in the
//     exe.config file.  Servers with a default instance name, only need the server name


// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Animal Movement")]
[assembly: AssemblyDescription("Manage GPS collar data for animals")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("National Park Service, Alaska")]
[assembly: AssemblyProduct("Animal Movement")]
[assembly: AssemblyCopyright("Copyright ©  2012")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("4192f16f-12af-40b2-9d50-6b44ae69d3f6")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
// Do not use the auto update of assembly versions, it cuases a reset of settings each time
// and it isn't really visible.  Update the file version each time a new exe is published. 
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.9")]
