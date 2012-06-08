using System.Reflection;
using System.Runtime.InteropServices;

//FIXME - DATABASE - Collar deployment date checking is not working (it is possible to deploy a collar simultaneously)
//FIXME - DATABASE - Deploy the database changes to check mortality date when adding locations
//FIXME - DATABASE - Deleting a project with editors does not properly delete the editors (user accounts remains)
//FIXME - DATABASE - Project editors table is not properly related to the projects table.
//TODO - DATABASE - Add unit testing.
//TODO - DATABASE - Add table for Telonics tfp files, each records owned by a PI.
//TODO - DATABASE - Add column in Collars table to reference the tfp file to convert this collars data
//TODO - DATABASE - Fix database to use Male/Female/Unknown in LookupGender instead of M/F/U
//TODO - DATABASE - Conflicting Fixes needs better support (don't show multiple fixes with same location)
//TODO - DATABASE - Conflicting Fixes needs better support (don't show fixes hidden by store on board)

//FIXME - provide better information if the exe cannot find the database
//FIXME - Cannot overwrite an existing layer file (despite giving approval) - generates an exception.

//TODO - Add lost/disposed date to collar, to filter out 'ignorable' collars
//TODO - Rename the telonics condensed/complete format
//TODO - Add an ArcGIS tool for creating a mortality date
//TODO - Consider viewer permission issues - All NPS is too permissive for some PIs
//TODO - support and testing of various file formats
//TODO - 	a) would like to support all data as it is/was delivered to the PI
//TODO - 	b) would like to support direct download from Argos web services
//TODO - replication to remote locations.
//TODO - 	a) Application/input  will all happen on the central server, which will push all data out to remote locations for fast readonly access.
//TODO - help documents/tutorials
//TODO - bulk upload (data grid view copy/paste) of multiple animals/collars/deploymnents (optional - sa can do this for PI using SSMS)
//TODO - Add MS Access readonly interface
//TODO - Add R statistics interface and adehabitat example
//TODO - Replace the wait cursors with a message box and progress bar
//TODO - uses a regular expression matcher to check the Debevek Header

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
[assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyFileVersion("1.0.0.0")]
