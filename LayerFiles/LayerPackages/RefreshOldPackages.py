# Can create a large layer package and take a while to create
# The data is unlikely to change; only refresh once a month
import arcpy
arcpy.env.overwriteOutput = True
arcpy.PackageLayer_management(r"\\INPAKROVMAIS\LayerPackages\LACL Brown Bears (prior years).lyr", r"\\INPAKROVMAIS\LayerPackages\LACL Brown Bears (prior years).lpk")
arcpy.PackageLayer_management(r"\\INPAKROVMAIS\LayerPackages\KATM Brown Bears (prior years).lyr", r"\\INPAKROVMAIS\LayerPackages\KATM Brown Bears (prior years).lpk")
arcpy.PackageLayer_management(r"\\INPAKROVMAIS\LayerPackages\GAAR Bears (prior years).lyr", r"\\INPAKROVMAIS\LayerPackages\GAAR Bears (prior years).lpk")

