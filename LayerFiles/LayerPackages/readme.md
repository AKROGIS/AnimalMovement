# Layer Packages

Layer packages contain the data and symbology in a convenient package.
Layer packages are created from the layer file and the data in the Animal
Movements database by the python scripts which are executed on a regular
schedule.

The master copy of the scripts and layer files in this folder is
<https://github.com/AKROGIS/AnimalMovement/tree/master/LayerFiles/LayerPackages>.
The working copy is at `\\inpakrovmais\LayerPackages`.

The scripts in this folder are run by a scheduled task to create layer packages
from the layer files herein. This is provided as a convenience for users at
remote parks with slow database connections.

Since X Drive data is replicated to the parks nightly, this is typically only
used for data in our enterprise database like SDE or Animal Movements.
