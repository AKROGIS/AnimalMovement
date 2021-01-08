Adding New Collar Manufacturers
===============================

* Add a record to the `LookupCollarManufacturers` table
* Consider the various files that may be added to the database for these collars.
  See [Adding new Collars](#adding-new-collar-file)
* If the collar comes in different models, and those models are retrieved/processed differently, then add the
  models to the LookupCollarModals table.  This is not required when there is only one collar model being used,
  and can be added when there is a need to differentiate models.
* Consider if there is additional metadata associated with these collars required to automatically
  retrieve or process collar data files. If so you will need to build the following:
  - Tables to hold the metadata
  - Stored procedures to Insert/Update/Delete from the metadata tables
  - Queries that produce a list of collars to retrieve and/or process
  - Forms for the user to be able to add the metadata
  - For an example, See the [Git commits on Sept 14, 2020](https://github.com/AKROGIS/AnimalMovement/commits/master)
    for the additional tables/views/store procedures for the Vectronic collars


Adding New Collar File
======================

Collar files may be uploaded manually, or automatically, and the system must be enhanced to support both
modes.  Collar files may be derived from another collar file.
Collar files should eventually be processed into tabular data that will add to the CollarFixes table.
There may be multiple file types that need to be supported for a given collar manufacturer.
The format of a collar file (and hence the necessary processing) is determined by the contents, not an
extension, or the opinion of the operator.  Therefore the database needs information necessary to
uniquely identify each supported file format based on the contents.

* Identify the relevant collar file formats, and document in the `LookupCollarFileFormats` table.
  Assign a unique alpha code to the file format.
* For each file format, add a record to `LookupCollarHeaderFormat` to uniquely identify the format
  of the file based on the contents of the beginning of the file.



