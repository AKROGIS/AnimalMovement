using System.Reflection;
using System.Runtime.InteropServices;

//FIXME - DATABASE - Add Sql_Proxy to main database
//FIXME - DATABASE - When project is created add SQL_proxy as an editor
//FIXME - DATABASE - Implement EXEC xp_cmdshell 'ArgosProcessor.exe' in CollarData_Insert
//FIXME - DATABASE - Start scheduled process for ArgosDownload on server
//FIXME - DATABASE - File status update should trigger status update to all child files
//FIXME - DATABASE - If there are conflicting collar parameters, the older one should have the end date set to equal the newer one's start date
//FIXME - DATABASE - If a new collar is added to the database, then it must disable the active older version (same argos id).
//TODO - DATABASE - Add GPS (Y/N) column to the Collars Table, fix insert/update stored procedures, and simplify QA/QC queries
//TODO - DATABASE - Rename the column AlternativeId to ArgosId in the Collars Table, fix all queries appropriately
//TODO - DATABASE - Remove the TelonicsGen3_xxx variants from the LookupCollarModels table, fix all queries appropriately
//TODO - DATABASE - Create Table for Argos PTT locations from Emails
//TODO - DATABASE - Modify CollarData_Insert to add Argos PTT locations from Emails to new DB table
//TODO - DATABASE - Modify CollarFixes_Insert to add Fixes from PTT locations for non-GPS Argos Collars (formats E & F)
//TODO - DATABASE - Change FixId to 32bit int, so it can be an OID in ArcMap (we are at 1.4million, with all data loaded)
//                - At 2^31 positive ints, we can have 1 fix every hour for 20 years for 12,000+ animals or 4,000 animals with existing ratio of 3 fix ids to 1 location
//TODO - DATABASE - Query for Analyzable collars should be same as C# code (i.e. check for ambiguous collars)
//TODO - DATABASE - Add business logic to ensure that all Deployments.Collar.DeploymentDate < Collar.DisposalDate
//TODO - DATABASE - Add logic to limit collar fixes to those before the Collar.DisposalDate
//TODO - DATABASE - Move collar deployment date checking from Stored procedure to trigger to protect against SA and to allow bulk uploading.
//TODO - DATABASE - Add more unit testing.
//TODO - DATABASE - Write local time to the Location and movements layers - make the views simpler/faster
//TODO - DATABASE - Writing local time to the Location and movements layers, will simplify replication - do not replicate localtime function
//TODO - DATABASE - Remove unused views from database, put them in an external file.

//FIXME - Do not show, or allow edit/delete of the SQL_Proxy editor on each project
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
