# Documentation

This folder contains instructions as well as design notes and the like.
Most of these documents have not been substantially edited since the early
days of the project and may contain some information that is out of date.
Unless noted otherwise, these documents are for the benefit of the developer
and database administrator, not the end user of the system.

## `Build Instructions`

These instructions document how to compile the source code to create the files
necessary for the installation instructions
([link](./Build%20Instructions.rst)).

## `Data Cleanup Issues`

Issues with the integrity of some of the historical collar data that was
loaded into the database in the early days 
([link](./Data%20Cleanup%20Issues.md)).

## `Data Dependencies`

This document describes in details most of the tables in the database, as
well as how they relate to each other ([link](./Data%20Dependencies.md)).
It also describes the various stored procedures, triggers, functions and
external applications and how they impact the state of the database.  This
document is quite long, but it is neither complete not up to date.
Nevertheless, it has a lot of very useful information on how the database works.
.

## `Database Export Instructions`

How to use SQL Server Management Studio to create the database definition files
([link](./Database%20Export%20Instructions.md)).  The files
[../Database/CreateDatabaseObjects.sql](../Database/CreateDatabaseObjects.sql)
and [../Database/LookupTableData.sql](../Database/LookupTableData.sql) should
be committed to the repository updated whenever the database schema changes.

## `Extending Animal Movements`

Notes on the steps required to add support for a new collar to the database
([link](./Extending%20Animal%20Movements.md)).
Created in 2020 when support for the Vectronic collars was added. Adding
support for another manufacture will undoubtedly have other unexpected
challenges, but this document should help a developer get started.

## `Images`

A folder of image files that are referenced in these documents

## `Installation Instructions`

Instruction on creating a new database from scratch and deploying the various
parts of the system to make it all functional
([link](./Installation%20Instructions.rst)).

## `Notes on Database Design and Testing`

A record of some of the developers design questions and decisions
([link](./Notes%20on%20Database%20Design%20and%20Testing.md)).

## `Readme`

This document

## `Reference Material`

A folder of information on manufacturer file formats and other details
useful for building the system.

## `UserGuide`

An guide for end users on using the Animal Movements windows application
([link](./UserGuide.rst)).

## `Vectronic Collar Cheatsheet`

Simple instructions to help end users add Vectronic collars to the database
([link](./Vectronic%20Collar%20Cheatsheet.md)).

**TODO:** Add this to the User Guide.


# Editing

All documentation is in either
[Github flavored markdown](https://github.github.com/gfm/) 
([getting started](https://github.com/adam-p/markdown-here/wiki/Markdown-Cheatsheet)) or
[reStructuredText](https://docutils.sourceforge.io/docs/ref/rst/restructuredtext.html)
([getting started](http://docutils.sourceforge.net/docs/user/rst/quickref.html)).
Markdown is used for smaller files, and restructured Text for the longer more
complicated documents. Both formats are easy to read and edit in a simple text
editor but can be rendered as a formatted document in Github and converted to
HTML, PDF and other formats as needed.


# Building

## reStructuredText

To convert reStructuredText to HTML, PDF and many other formats, you will need
to install various free tools. Available options and installation details will
likely change by the time you read this. Hopefully the this StackOverflow
question on
[reStructuredText Tool Support](http://stackoverflow.com/questions/2746692/restructuredtext-tool-support)
will still be helpful, if not use your internet search engine.  

Instructions similar to the following in a batch file were used to create
HTML and PDF versions.

```bat
REM HTML, repeat for other *.rst documents
C:\> \path\to\python.exe C:\path\to\rst2html.py UserGuide.rst UserGuide.html
REM PDF, repeat for other *.rst documents
C:\> \path\to\rst2pdf.exe --default-dpi=100 UserGuide.rst UserGuide.pdf
```

## Markdown

These documents, usually for developer reference, do not need to be convert
to PDF or HTMl.  Visual Studio code (and other editors) have a preview mode
and the Github website displays the formatted documents to make them easier
to read.

If you do need to convert them then search for an online conversion tool
(there are several).  If you want more control, then I suggest installing
the free [pandoc](https://pandoc.org) toolkit.
