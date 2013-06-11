using System.Reflection;
using System.Runtime.InteropServices;

//FIXME - DATABASE - Hide the ArgosPlatforms.Password from all but the Manager, and the Download API
//TODO - DATABASE - Must the CollarDeployments update trigger preclude changes to collar and animal (provided the change maintains RI)? - Changing a collar id in collars table cascades the change to deployments where it fails.
//TODO - DATABASE - If a collar is updated (HasGps, Model, DispDate), then the collar may gain (or lose) fixes in files already processed - provide tool to rescan files
//TODO - DATABASE - Track the preserve the status of locations when files are reloaded/reactivated. (this is hard - all inserts to locations needs to first scan the preservation table)
//TODO - DATABASE - Make viewing the Settings table off limits, provide a Store Procedure to see only your settings -- Need special exception for sql_proxy
//TODO - DATABASE - Review/Document all the business rules, and then verify they are implemented correctly 
//TODO - DATABASE - Add more unit testing.

//TODO - Document the optimal "getting started" process, and make sure the code supports it
//TODO - bulk upload (data grid view copy/paste) of multiple animals/collars/deploymnents (optional - sa can do this for PI using SSMS)
//TODO - Add ability to add Argos Platforms when creating an Argos Program (now we have to go to the details tab after creation)
//TODO - Add ability to create an Argos deployment when creating a collar (now we have to go to the details tab after creation)
//TODO - Add Icon or collar coding to Collar List to signify (VHF only, PTT only, Storeonboard GPS, GPS+Argos)
//TODO - build tool to visualize deployments (i.e. show a graphical time line of animals & collars)
//TODO - Create a simple location layer, create a table of animal data, and join in ArcMap
//TODO - Replace the wait cursors with a message box and progress bar
//TODO - Add a warning (consent to monitoring) message at start up.
//TODO - How do I add this warning to a layer file??
//TODO - Replication to remote locations.
//TODO - help documents/tutorials
//TODO - Add MS Access readonly interface
//TODO - Add R statistics interface and adehabitat example
//TODO - Build a tool to hide locations outside a reasonable (user provided) range
//TODO - Pass the TPF filename to the Gen4 processor, since the filename (currently temp) is written to the output file
//TODO - Add an ArcGIS tool for creating a mortality date
//TODO - New QC Report - Show files with overlapping fix dates
//TODO - New QC Report - Show Argos collars that might be missing data since downloading began.
//TODO - New QC Report - Identify collars with data gaps (analyze download dates, transmissions in email files).
//TODO - New QC Report - Data sheet view of all animals, collar, deployments, files, fixes, etc.

//To NOT do or fix:
//  do not require that CollarDeployments.RetrievalDate < Collar.DisposalDate; just limit locations to before disposal date
//  (collar may be disposed but never be retreived)
//  do not limit collar fixes to those before the Collar.DisposalDate; just limit locations
//  Do not remove UTC fixtime from Locations.  this is used to create movement vectors, and due to daylight savings time, local time will not work
//    also, removing the functions from the spatial view did not make them any faster.

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
[assembly: AssemblyFileVersion("1.0.0.12")]
