using System.Reflection;
using System.Runtime.InteropServices;

//FIXME - DATABASE - Implement EXEC xp_cmdshell 'ArgosProcessor.exe' in CollarData_Insert
//FIXME - DATABASE - Start scheduled process for ArgosDownload on server
//FIXME - DATABASE - If there are conflicting collar parameters, the older one should have the end date set to equal the newer one's start date
//FIXME - DATABASE - CollarUpdate Trigger - check for Disposal Date/ArgosId conflict
//FIXME - DATABASE - Add stored procedures to add/del/update the ArgosPlatforms and Programs Tables
//TODO - DATABASE - Updating the Disposal Date of a collar should add a end date to a collar parameter
//TODO - DATABASE - Must the CollarDeployments update trigger preclude changes to collar and animal (provided the change maintains RI)? - Changing a collar id in collars table cascades the change to deployments where it fails.
//TODO - DATABASE - If a new collar is added, or properties (ArgosId, HasGps, Gen3period, Model, DispDate) are changed, then the collar may gain (or lose) fixes in files already processed - provide tool to rescan files
//TODO - DATABASE - Modify CollarData_Insert to add Argos PTT locations from Emails to new DB table
//TODO - DATABASE - Modify CollarFixes_Insert to add Fixes from PTT locations for non-GPS Argos Collars (formats E & F)
//TODO - DATABASE - Query for Analyzable collars should be same as C# code (i.e. check for ambiguous collars)
//TODO - DATABASE - Add more unit testing.
//TODO - DATABASE - Write local time to the Location and movements layers - make the views simpler/faster
//TODO - DATABASE - Writing local time to the Location and movements layers, will simplify replication - do not replicate localtime function
//TODO - DATABASE - Add a Hidden attribute to the CollarFixes table which caches Location.Hidden, for when locations are deleted/restored.


//FIXME - Deleting multiple files and subfile in one operation fails, and corrupts datamodel. - Fix with heirarchy
//TODO - Add setting for getting emails from Telonics downloader
//TODO - provide user interface for checking on status of downloads
//TODO - When a new collar is added to the database, we should offer to disable the active older version (same argos id).
//TODO - Document the optimal "getting started" process, and make sure the code supports it
//TODO - Build UI to add/edit/delete Argos Projects and Platforms
//TODO - Add Icon or collar coding to Collar List to signify (VHF only, PTT only, Storeonboard GPS, GPS+Argos)
//TODO - Simplify display of files.  honor heirachy, add dummy parents for email and AWS files
//TODO - Provide UI to upload multiple emails (folder?) in one operation
//TODO - Provide practice, or test upload, of emails to check for errors before doing the read deal.
//TODO - Provide tool to upload and process Gen3 (*.tfb) and Gen4 (*.tdf) store on board source data
//TODO - Provide some global QAQC tools - I.e. show all collars with conflict in the last x days
//TODO - Provide some global QAQC tools - I.e. show all collars with multiple parameter files
//TODO - Provide some global QAQC tools - I.e. show files with overlapping fix dates
//TODO - Provide some global QAQC tools - I.e. show Telonics collars that do not have a TPF (Gen4) or FixPeriod (Gen3) or do have active PPF file (Gen3)
//TODO - Provide some global QAQC tools - I.e. show Telonics Gen4 collars that have multiple active TPF files
//TODO - Provide some global QAQC tools - I.e. show Telonics collars that have duplicate Alternative Id and identical Disposal dates
//TODO - Provide some global QAQC tools - I.e. show Argos collars that do not have an active platform, or cannot be downloaded for any reason
//TODO - Provide some global QAQC tools - I.e. show Argos collars that have not downloded any data, or might be missing data since downloading began.
//TODO - Provide some global QAQC tools - I.e. Identify collars with data gaps (analyze download dates, transmissions in email files).
//TODO - Allow a collar to have multiple Active non-overlapping TPF files (for now, must deactive all but one)
//TODO - If a collar has multiple TPF files, then sort messages by transmission date per TPF, and process separately. (for now, create multiple collars)
//TODO - build tool to visualize deployments (i.e. show a graphical time line of animals & collars)
//TODO - Create a simple location layer, create a table of animal data, and join in ArcMap
//TODO - Replace the wait cursors with a message box and progress bar
//TODO - Add a warning (consent to monitoring) message at start up.
//TODO - How do I add this warning to a layer file??
//TODO - Replication to remote locations.
//TODO - 	a) Application/input  will all happen on the central server, which will push all data out to remote locations for fast readonly access.
//TODO - help documents/tutorials
//TODO - bulk upload (data grid view copy/paste) of multiple animals/collars/deploymnents (optional - sa can do this for PI using SSMS)
//TODO - Add MS Access readonly interface
//TODO - Add R statistics interface and adehabitat example
//TODO - Build a tool to hide locations outside a reasonable (user provided) range
//TODO - provide datasheet views of collars, animals, deployments, and maybe files and fixes

//To NOT do or fix:
//  do not require that CollarDeployments.RetrievalDate < Collar.DisposalDate; just limit locations to before disposal date
//  (collar may be disposed but never be retreived)
//  do not limit collar fixes to those before the Collar.DisposalDate; just limit locations

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
