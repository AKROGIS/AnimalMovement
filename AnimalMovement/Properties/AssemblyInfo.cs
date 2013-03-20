using System.Reflection;
using System.Runtime.InteropServices;

//FIXME - DATABASE - CollarUpdate Trigger - check for Disposal Date/ArgosId conflict; check for disposalDate/parameter startdate conflict
//FIXME - DATABASE - If there are conflicting collar parameters, the older one should have the end date set to equal the newer one's start date
//TODO - DATABASE - Save warnings from Argos processing into a table for review by the user 
//TODO - DATABASE - Query for Analyzable collars should be same as C# code (i.e. check for ambiguous collars)
//TODO - Capture issues from argos processing in database for review/correection.
//TODO - When a new collar is added to the database, we need to create Argos deployments, and update old Argos deployments
//TODO - When a new TPF file for an existing Collar is added, the old parameters should be ended.
//TODO - When Updating the Disposal Date of a collar consider adding an end date to the collar parameter (this is not a database requirement)
//TODO - DATABASE - Remove all dependencies on CollarDisposal Date, this is for information/display only.
//TODO - DATABASE - Remove ArgosId, Gen3period from the Collars table.
//TODO - Create tab on collar details to show Argos platfroms used on this collar
//TODO - Create tab on collar details to show Collar Parametes used on this process this collar

//FIXME - DATABASE - Review/Document all the business rules, and then verify they are implemented correctly 
//FIXME - DATABASE - The ArgosProcesser called from the database only works if the SQL_Proxy account is logged in.
//FIXME - DATABASE - Add stored procedures to add/del/update the ArgosPlatforms and Programs Tables
//TODO - DATABASE - Replace the hardcoded sql_proxy name with a system setting
//TODO - DATABASE - Must the CollarDeployments update trigger preclude changes to collar and animal (provided the change maintains RI)? - Changing a collar id in collars table cascades the change to deployments where it fails.
//TODO - DATABASE - If a new collar is added, or properties (ArgosId, HasGps, Gen3period, Model, DispDate) are changed, then the collar may gain (or lose) fixes in files already processed - provide tool to rescan files
//TODO - DATABASE - Modify CollarData_Insert to add Argos PTT locations from Emails to new DB table
//TODO - DATABASE - Modify CollarFixes_Insert to add Fixes from PTT locations for non-GPS Argos Collars (formats E & F)
//TODO - DATABASE - Add more unit testing.
//TODO - DATABASE - Write local time to the Location and movements layers - make the views simpler/faster
//TODO - DATABASE - Writing local time to the Location and movements layers, will simplify replication - do not replicate localtime function
//TODO - DATABASE - Add a Hidden attribute to the CollarFixes table which caches Location.Hidden, for when locations are deleted/restored.
//TODO - DATABASE - Make viewing the Settings table off limits, provide a Store Procedure to see only your settings -- Need special exception for sql_proxy
//TODO - DATABASE - Fix permissions for processing files, check collar owner, program owner
//TODO - DATABASE - Not all files will belong to a project - i.e Argos emails and program downloads may belong to several projects
//TODO - DATABASE - Add a Release Date to the collardeployment - this will make a lot of logic way more complicated.
//TODO - DATABASE - Cleanup LookupCollarFileFormats remove Tables column, add 'RequiresArgosProcessing' bit column
//TODO - DATABASE - Fix ArgosFile_ClearProcessingResults, ArgosFilePlatformDates_Insert, ArgosFileProcessingIssues_Insert, and ?? to use LookupCollarFileFormats.RequiresArgosProcessing

//FIXME - Adding a AWS file manually will not get it processed, needs to be a special case in the file upload
//TODO - Add option in UI for requesting no emails be send from the Telonics downloader
//TODO - provide user interface for checking on status of downloads
//TODO - Document the optimal "getting started" process, and make sure the code supports it
//TODO - Build UI to add/edit/delete Argos Projects and Platforms
//TODO - Add Icon or collar coding to Collar List to signify (VHF only, PTT only, Storeonboard GPS, GPS+Argos)
//TODO - Simplify display of files.  honor heirachy, add dummy parents for email and AWS files
//TODO - Provide UI to upload multiple emails (folder?) in one operation
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
//TODO - Pass the TPF filename to the Gen4 processor, since the filename (currently temp) is written to the output file

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
