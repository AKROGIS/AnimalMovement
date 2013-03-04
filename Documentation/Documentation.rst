Editing Location Data
=====================

The title of this section is mis-named.
The source location data in the database is never deleted or altered.


Resolving Conflicting Fixes
---------------------------

Coming soon.


Hidding Bogus Fixes
-------------------

There is no way to change the time or location of a fix.  Nor should you.  You cannot aad-hoc location.
All fix data must come from the raw collar data.  This ensures a defensible dataset.
You can however decide that some location data is *bad*, and eliminate this from dispaly and analysis.
These hidden locations are available for review, and can be *un-hidden* if your assessment of the data
changes in the future.


Removing Pre/Post Deployment data
---------------------------------

If you notice locations on the map that are before or after the collar was deployed on the animal,
Then you need to edit the deployment dates in the Database application.

Similarly if you think some locations may be missing from the map, you can increase the deployment range
to show locations that may be hidden.

Fixes that are outside the deployment dates are not shown in the invalid locations layer, they can only
be displayed/hidden by editing the deployment dates.

If an animal slips a collar, or a collar releases prematurely, this should be treated as a retrieved collar
to remove the locations of the stationary collar no longer on the animal.  Since you are not 'retrieval date'
in the deployment is not the date you actually retrieved the collar, you may want to make a note in the collar
table remarks section for future reference.


Dead Animals
------------

Identifying mortalities, and hiding the locations of the stationary collar is covered in the Mortalities section.
  

Using a bounding box to hide extreme locations
----------------------------------------------

There may be a tool to put in geographic coordinates that define the extreme boundaries of your project area.
Any locations that occur outside those bounds will automatically be hidden.  This is easy to do in the database,
and can be problematic in ArcMap.  There is a current limitation in ArcMap when you zoom out too far (so that more than one hemisphere is displayed
- easy to do in alaska, where anything above 90 degrees north is in the other hemisphere).  In this case, the
database returns no locations, so you cannot see the data to hide the bogus locations.

This database feature will be coming soon.


Using ArcMap to Identify Bad Locations
--------------------------------------

There are primarily two was to identify bad locations in ArcMap.

1. Visual Review.  Some bad fixes are visually obvious when reviewing the movement vectors.
   These will show up as a spike from and immediatly returning to a cluster of locations.

.. image:: images/ArcMapTools_HideLocations.png

2. By reviewing the attributes (speed, duration, and distance) in the movement vectors table to identify
   suspect locations.  this is easiest to do if you first define a definition query on the locations and movement
   layers to limit the data to just one animal.

	a. In ArcMap, right click on the Movement Vectors layer and select Open Attributes

	b. Right click on either the speed, duration, or distance column and sort.  High speeds, short durations,
	   or large distances are all the result of suspect locations.

	c. When you identify a movement vector that is suspect, note the start and ending time.

	d. Open the attribute table for the location data.

	e. Scroll to the locations at the start and end time.

	f. Select each location, starting two locations before the suspect vector,
	   and proceeding to two locations after the suspect vector, and watch the animals progress
	   on the map.  In this way, it is usually quite easy to identify which end of the vector is the bad location.


Hidding Location
----------------

1. Use the ArcMap Selection tool to select the unwanted location(s).

2. Click the paw print icon (Edit Location Status tool).

.. image:: images/ArcMapTools_HideLocations.png

3. Click the Yes to hide the selected locations.


If you turn on the invalid locations layer, you will see these points have been removed from the valid locations/vectors layers
and added to the invalid locations layer.  If you want to re-activate them, select the points in the invalid locations layer, click the paw print,
and then select No to unhide these locations.


Un-Hidding Locations
--------------------

coming soon.



Mortalities
===========

Dead animals should be identified so that the locations after the animal died are removed
from the data set used for analysis, particularly home range analysis.

Finding Mortalities
-------------------
This is done in ArcMap.  More to come.

Correcting for Mortalities
--------------------------

1. Click on Project List

.. image:: images/Mortalities_MainMenu.png

2. Double click your project

.. image:: images/Mortalities_ProjectList.png

3. In the middle list, double click the animal that died

.. image:: images/Mortalities_AnimalDetails1.png

4. In the Animal Details window, click edit, then check the box next to Date of Death

.. image:: images/Mortalities_AnimalDetails1.png

5. Correct the date, then click Save.

.. image:: images/Mortalities_AnimalDetails2.png

If you refresh the view in ArcMap, it should draw with the corrected data.

Close the windows (If you close the first window, it will close all the others), and your done.



ArcMap Tools
============


1. Open ArcMap, and make sure that the Animal Movements Addin is loaded.
   If you select *Customize->Add-In Manager...* from the ArcMap menu, you should see the following:

.. image:: images/ArcMapTools_Add-In-Manager.png

If not, make sure that you have *X:\GIS\Addins\10.1* specified as the Add-Ins folder in the Options tab.

2. Select *Customize->Customize Mode...* from the ArcMap menu.

3. Click the Commands tab

4. Scroll down to the *NPS Alaska Category*.

5. Find the *Edit Location Status* Command (paw print icon)

.. image:: images/ArcMapTools_Customize.png

6. Click on the paw icon, and drag it over an existing toolbar (I like to use the Alaska Pak toolbar), and drop it.
   The icon should now be on the toolbar.

7. Close the Customize window.
