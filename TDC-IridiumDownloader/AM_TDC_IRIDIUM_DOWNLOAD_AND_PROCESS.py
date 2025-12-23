#-------------------------------------------------------------------------------
# Name:        AM_TDC_IRIDIUM_DOWNLOAD_AND_PROCESS.py v2.0
# Purpose:     Download Iridium Collar data from Telonics Web and process files in Animal Movement
#
#              This file should be called daily by the Task Scheduler
#
# Parameters:  YAMLFILE = Configuration file for Telonics Web; if used, ignore
#
# Author:      alsouthwould
#
# Created:     07/06/2022
#-------------------------------------------------------------------------------

import argparse
import os
import shutil
import subprocess
import sys
import yaml
from datetime import datetime

#Setup arguments
parser = argparse.ArgumentParser(description="Download Iridium Collar data from Telonics Web and process files in Animal Movement", epilog="The YAML File will contain all information necessary to run this script. If provided, the arguments following the YAML File are ignored. Otherwise, those arguments must be provided.")
parser.add_argument("-SKIPTDCDOWNLOAD", required=False, action="store_true", help="Flag indicating that the TDC Web download should be skipped")
parser.add_argument("-SKIPAMUPLOAD", required=False, action="store_true", help="Flag indicating that the Animal Movement upload should be skipped")
parser.add_argument("-yamlfile", required=False, type=str, help="Configuration file for Telonics Web download")
parser.add_argument("-tdcexe", required=False, type=str, help="Path including file for Telonics Data Converter (TDC.exe)", default="C:\Program Files (x86)\Telonics\Data Converter\TDC.exe")
parser.add_argument("-cflexe", required=False, type=str, help="Path including file for AM Collar File Loader (CollarFileLoader.exe)")
parser.add_argument("-datapath", required=False, type=str, help="Root path for storing the downloaded data and processing files")
parser.add_argument("-loadpath", required=False, type=str, help="Data path for storing the downloaded data and processing files")
parser.add_argument("-logpath", required=False, type=str, help="Path for storing the log files")
parser.add_argument("-npsuser", required=False, type=str, help="NPS username")
parser.add_argument("-tdcuser", required=False, type=str, help="Username for Telonics Web account")
parser.add_argument("-tdcpass", required=False, type=str, help="Password for Telonics Web account")
parser.add_argument("-startdate", required=False, type=str, help="Download range start date in the format YYYY-MM-DD")
parser.add_argument("-enddate", required=False, type=str, help="Download range end date in the format YYYY-MM-DD")
parser.add_argument("-TESTRUN", required=False, action="store_true", help="Flag indicating this is a test run for documentation purposes and actual updates will not be made")
args = parser.parse_args()


def initialize_log():
    logpath = None
    if args.yamlfile != None:

        yamlfile = args.yamlfile
        #Confirm yaml file exists
        if not os.path.isfile(yamlfile):
            logpath = None
        else:
            #Load yaml configuration file
            tdcconfig = read_yaml(yamlfile)
            try:
                #Check path for Telonics Data Converter
                logpath = tdcconfig["PATHS"]["LOGPATH"]
                if not os.path.isdir(logpath):
                    logpath = None
            except:
                logpath = None
    if logpath == None and args.logpath != None:
        logpath = args.logpath
        if not os.path.isdir(logpath):
            logpath = None

    if logpath == None:
        return None
    else:
        now = datetime.now().strftime("%Y%m%d") + "_" + datetime.now().strftime("%H%M")
        logfile = logpath + "\\" + "AM_TDC_IRIDIUM_DOWNLOAD_AND_PROCESS_LOG_" + now + ".txt"
        newfile = open(logfile, "w")
        newfile.close()
        write_log(logfile, "================================================================================")
        write_log(logfile, "Executing AM_TDC_IRIDIUM_DOWNLOAD_AND_PROCESS.py")
        write_log(logfile, "================================================================================")
        return logfile


def write_log(logfile, message, error = False):
    if logfile != None:
        appendfile = open(logfile, "a")
        if error:
            appendfile.write("ERROR: ")
        appendfile.write(str(message))
        appendfile.write("\n")
        appendfile.close()
    if error:
        sys.exit(message)


def read_yaml(filename):

    #Read yaml file
    with open(filename) as file:
        yamlfile = yaml.safe_load(file)

    file.close()

    return yamlfile


def write_yaml(data, filename):

    #Write yaml file
    with open(filename, 'w') as file:
        yamlfile = yaml.dump(data, stream=file, default_flow_style=False, sort_keys=False)

    file.close()

def initialize_yaml(yamlfile, logfile):

    write_log(logfile, "----------------------------------------")
    write_log(logfile, "Initializing YAML File")
    write_log(logfile, "----------------------------------------")
    write_log(logfile, "YAML File: " + yamlfile)

    #Confirm yaml file exists
    if not os.path.isfile(yamlfile):
        write_log(logfile, "Path " + yamlfile + " for YAML File does not exist.", True)

    #Load yaml configuration file
    tdcconfig = read_yaml(yamlfile)

    try:
        #Check path for Telonics Data Converter
        tdcexe = tdcconfig["PATHS"]["TDCEXE"]
    except:
        write_log(logfile, "[PATHS][TDCEXE] key was not provided.", True)

    try:
        #Check path for AM Collar File Loader
        cflexe = tdcconfig["PATHS"]["CFLEXE"]
    except:
        write_log(logfile, "[PATHS][CFLEXE] key was not provided.", True)

    try:
        #Check path for scripts
        toolpath = tdcconfig["PATHS"]["TOOLPATH"]
    except:
        write_log(logfile, "[PATHS][TOOLPATH] key was not provided.", True)

    try:
        #Check path for data
        datapath = tdcconfig["PATHS"]["DATAPATH"]
    except:
        write_log(logfile, "[PATHS][DATAPATH] key was not provided.", True)

    try:
        #Check start date
        startdate = tdcconfig["STARTDATE"]
    except:
        write_log(logfile, "[STARTDATE] key was not provided.", True)

    try:
        #Check end date
        enddate = tdcconfig["ENDDATE"]
    except:
        write_log(logfile, "[ENDDATE] key was not provided.", True)

    try:
        #Check end date
        tdcusers = tdcconfig["USERS"]
    except:
        write_log(logfile, "[USERS] key was not provided.", True)

    try:
        #Check the flag to skip the TDC Download
        skiptdcdownload = tdcconfig["TOOLS"]["SKIPTDCDOWNLOAD"]
    except:
        write_log(logfile, "[TOOLS][SKIPTDCDOWNLOAD] key was not provided.", True)

    try:
        #Get the flag to skip the AM Upload
        skipamupload = tdcconfig["TOOLS"]["SKIPAMUPLOAD"]
    except:
        write_log(logfile, "[TOOLS][SKIPAMUPLOAD] key was not provided.", True)

    return tdcconfig


def download_tdcweb_iridium_data(tdcexe, datapath, tdcuser, tdcpassword, startdate, enddate, logfile):

#TESTRUN: Files are downloaded into a folder prefixed with "TEST"

    write_log(logfile, "----------------------------------------")
    write_log(logfile, "Downloading Iridium Data from Telonics Web")
    write_log(logfile, "----------------------------------------")

    #Check Telonics Data Converter
    if not os.path.isfile(tdcexe):
        write_log(logfile, "Path " + tdcexe + " for Telonics Data Converter does not exist.", True)

    #Check data path
    if not os.path.isdir(datapath):
        write_log(logfile, "Path " + datapath + " for data folder does not exist.", True)

    #Create new folder for this batch process
    if not args.TESTRUN:
        newpath = datapath + "\\" + tdcuser + "_" + datetime.now().strftime("%Y%m%d") + "_" + datetime.now().strftime("%H%M")
    else:
        newpath = datapath + "\\" + "TEST_" + tdcuser + "_" + datetime.now().strftime("%Y%m%d") + "_" + datetime.now().strftime("%H%M")
    if not os.path.isdir(newpath):
        os.mkdir(newpath)

    write_log(logfile, "Download Folder: " + newpath)

    #Create new subfolder for this batch process
    otherfolder = newpath + "\\" + "OTHER_FILES"
    if not os.path.isdir(otherfolder):
        os.mkdir(otherfolder)

    #Create XML file containing TDC batch download settings
    spacer = "    "
    xmlfile = newpath + "\\" + "TDC_IRIDIUM_DOWNLOAD_BATCH.XML"
    newfile = open(xmlfile, "w")
    newfile.write("<BatchSettingsV2>" + "\n")
    newfile.write(spacer + "<Iridium>" + "\n")
    newfile.write(spacer + spacer + "<Username>" + tdcuser + "</Username>" + "\n")
    newfile.write(spacer + spacer + "<Password>" + tdcpassword + "</Password>" + "\n")
    newfile.write(spacer + "</Iridium>" + "\n")
    newfile.write(spacer + "<DownloadData>true</DownloadData>" + "\n")
    newfile.write(spacer + "<ConvertNewDataOnly>" + "\n")
    newfile.write(spacer + "<BatchLog>" + newpath + "\\" + "TDC_IRIDIUM_DOWNLOAD_BATCH_LOG.TXT" + "</BatchLog>" + "\n")
    newfile.write(spacer + "<OutputFolder>" + newpath + "</OutputFolder>" + "\n")
    newfile.write(spacer + "<ReportFormat>both</ReportFormat>" + "\n")
    newfile.write(spacer + "<ReportFileMode>overwrite</ReportFileMode>" + "\n")
    newfile.write("</BatchSettingsV2>" + "\n")
    newfile.close()

    write_log(logfile, "Download Settings: " + xmlfile)

    #Create BAT file
    batfile = newpath + "\\" + "TDC_IRIDIUM_DOWNLOAD_BATCH.BAT"
    newfile = open(batfile, "w")
    newfile.write("start /wait \"TDC Batch\" \""+ tdcexe + "\" /batch:\"" + xmlfile + "\"")
    newfile.close()

    write_log(logfile, "Download Batch File: " + batfile)

    #Download data from TDC
    write_log(logfile, ">>Running batch file to download data")
    subprocess.call(batfile)
    write_log(logfile, ">>Downloading data from Telonics Web")

    #Move non-essential files to subfolder
    write_log(logfile, ">>Moving non-essential files to " + otherfolder)
    trashfiles = [checkfile for checkfile in os.listdir(newpath) if "complete" in checkfile.lower()]
    for trashfile in trashfiles:
        shutil.move(newpath + "\\" + trashfile, otherfolder + "\\" + trashfile)

    trashfiles = [checkfile for checkfile in os.listdir(newpath) if "statistics" in checkfile.lower()]
    for trashfile in trashfiles:
        shutil.move(newpath + "\\" + trashfile, otherfolder + "\\" + trashfile)

    trashfiles = [checkfile for checkfile in os.listdir(newpath) if "kml" in checkfile.lower()]
    for trashfile in trashfiles:
        shutil.move(newpath + "\\" + trashfile, otherfolder + "\\" + trashfile)

    #Rename essential files
    daterange = datetime.now().strftime("%Y%m%d")

    write_log(logfile, ">>Renaming data files to {COLLARID}_Condensed_TDCWEB_" + daterange + ".csv")
    keepfiles = [checkfile for checkfile in os.listdir(newpath) if "condensed" in checkfile.lower()]
    for keepfile in keepfiles:
        newkeepfile = keepfile
        newkeepfile = newkeepfile.replace(" ", "_")
        newkeepfile = newkeepfile.replace("Condensed", "Condensed_TDCWEB_" + daterange)
        os.rename(newpath + "\\" + keepfile, newpath + "\\" + newkeepfile)

    write_log(logfile, ">>Download complete for " + tdcuser.upper() + " for " + startdate + " to " + enddate)

    return newpath

def load_tdcweb_iridium_data(cflexe, loadpath, npsuser, logfile):

#TESTRUN: Files are not uploaded

    write_log(logfile, "----------------------------------------")
    write_log(logfile, "Loading Iridium Data to Animal Movement")
    write_log(logfile, "----------------------------------------")
    write_log(logfile, "Upload Folder: " + loadpath)

    #Check Animal Movement Collar File Loader
    if not os.path.isfile(cflexe):
        write_log(logfile, "Path " + cflexe + " for AM Collar File Loader does not exist.", True)

    #Confirm load path exists
    if not os.path.isdir(loadpath):
        write_log(logfile, "Path " + loadpath + " for load folder does not exist.", True)

    #Create BAT file
    batfile = loadpath + "\\" + "TDC_IRIDIUM_LOAD_BATCH_AM.BAT"
    newfile = open(batfile, "w")
    newfile.write("\"" + cflexe + "\" /owner:\"" + npsuser + "\" \"" + loadpath + "\"")
    newfile.close()

    write_log(logfile, "Upload Batch File: " + batfile)

    #Upload data to AM
    write_log(logfile, ">>Running batch file to upload data")
    if not args.TESTRUN:
        subprocess.call(batfile)
    else:
        print("")
        print("IGNORE TEST CODE: " + "\"" + cflexe + "\" /owner:\"" + npsuser + "\" \"" + loadpath + "\"")
        write_log(logfile, ">>TEST CODE: " + "\"" + cflexe + "\" /owner:\"" + npsuser + "\" \"" + loadpath + "\"")
    write_log(logfile, ">>Uploading data to Animal Movement")

    write_log(logfile, ">>Upload complete for " + npsuser.upper())


if __name__ == "__main__":

#TESTRUN: Notice is posted to log and yaml file is not updated

    logfile = initialize_log()

    if args.TESTRUN:
        write_log(logfile, "NOTE: THIS IS A TEST RUN.")
        write_log(logfile, "NOTE: Data may be downloaded, but the configuration settings will not be updated and no data will be loaded into Animal Movement!")
        write_log(logfile, "WARNING: If run with the <ConvertNewDataOnly> batchfile option, Downloaded data will no longer be flagged as new on the Telonics Server.")
    if args.yamlfile != None:
        #Load yaml file
        yamlfile = args.yamlfile
        tdcconfig = initialize_yaml(yamlfile, logfile)

        skiptdcdownload = tdcconfig["TOOLS"]["SKIPTDCDOWNLOAD"] or args.SKIPTDCDOWNLOAD
        skipamupload = tdcconfig["TOOLS"]["SKIPAMUPLOAD"] or args.SKIPAMUPLOAD

        tdcexe = tdcconfig["PATHS"]["TDCEXE"]
        cflexe = tdcconfig["PATHS"]["CFLEXE"]
        toolpath = tdcconfig["PATHS"]["TOOLPATH"]
        datapath = tdcconfig["PATHS"]["DATAPATH"]
        loadpath = None

        #Update the start and end dates in the yaml file
        if not skiptdcdownload:
            if tdcconfig["ENDDATE"] != None:
                try:
                    trydate = datetime.strptime(tdcconfig["ENDDATE"], "%Y-%m-%d")
                except:
                    write_log(logfile, "[ENDDATE] key value " + tdcconfig["ENDDATE"] + " is not in the correct date format YYYY-MM-DD.", True)
                else:
                    tdcconfig["STARTDATE"] = tdcconfig["ENDDATE"]
            else:
                tdcconfig["STARTDATE"] = datetime.now().strftime("%Y-%m-%d")
            tdcconfig["ENDDATE"] = datetime.now().strftime("%Y-%m-%d")

            if not args.TESTRUN:
                write_yaml(tdcconfig, yamlfile)

        startdate = tdcconfig["STARTDATE"]
        enddate = tdcconfig["ENDDATE"]

        tdcusers = tdcconfig["USERS"]
        npsuser = None
        tdcuser = None
        tdcpass = None

    else:
       #Load arguments
        skiptdcdownload = args.SKIPTDCDOWNLOAD
        skipamupload = args.SKIPAMUPLOAD

        tdcexe = args.tdcexe
        cflexe = args.cflexe
        toolpath = None
        datapath = args.datapath
        loadpath = args.loadpath

        startdate = args.startdate
        if not skiptdcdownload:
            if startdate != None:
                try:
                    trydate = datetime.strptime(startdate, "%Y-%m-%d")
                except:
                    write_log(logfile, "-startdate argument value " + startdate + " is not in the correct date format YYYY-MM-DD.", True)
            else:
                startdate = datetime.now().strftime("%Y-%m-%d")

        enddate = args.enddate
        if not skiptdcdownload:
            if enddate != None:
                try:
                    trydate = datetime.strptime(enddate, "%Y-%m-%d")
                except:
                    write_log(logfile, "-enddate argument value " + enddate + " is not in the correct date format YYYY-MM-DD.", True)
            else:
                enddate = datetime.now().strftime("%Y-%m-%d")

        tdcusers = None
        npsuser = args.npsuser
        tdcuser = args.tdcuser
        tdcpass = args.tdcpass

    #Check arguments

    #Check Telonics Data Converter
    if not skiptdcdownload:
        if tdcexe == None:
            if args.yamlfile != None:
                write_log(logfile, "[PATHS][TDCEXE] key value was not provided.", True)
            else:
                write_log(logfile, "-TDCEXE argument was not provided.", True)
        else:
            if not os.path.isfile(tdcexe):
                write_log(logfile, "Path " + tdcexe + " for Telonics Data Converter does not exist.", True)

    #Check Animal Movement Collar File Loader
    if not skipamupload:
        if cflexe == None:
            if args.yamlfile != None:
                write_log(logfile, "[PATHS][CFLEXE] key value was not provided.", True)
            else:
                write_log(logfile, "-CFLEXE argument was not provided.", True)
        else:
            if not os.path.isfile(cflexe):
                write_log(logfile, "Path " + cflexe + " for AM Collar File Loader does not exist.", True)

    #Check data path
    if not skiptdcdownload:
        if datapath == None:
            if args.yamlfile != None:
                write_log(logfile, "[PATHS][DATAPATH] key value was not provided.", True)
            else:
                write_log(logfile, "-DATAPATH argument was not provided.", True)
        else:
            if not os.path.isdir(datapath):
                write_log(logfile, "Path " + datapath + " for data folder does not exist.", True)

    #Check load path
    if skiptdcdownload and not skipamupload:
        if loadpath == None:
            if args.yamlfile != None:
                #write_log(logfile, "[PATHS][LOADPATH] key value was not provided.", True)
                pass
            else:
                write_log(logfile, "-LOADPATH argument was not provided.", True)
        else:
            if not os.path.isdir(loadpath):
                write_log(logfile, "Path " + loadpath + " for load folder does not exist.", True)

    #Check users
    if args.yamlfile != None:
        if tdcusers == None:
            write_log(logfile, "[USERS] key value was not provided.", True)
    else:
        if not skipamupload:
            if npsuser == None:
                write_log(logfile, "-NPSUSER argument was not provided.", True)

        if not skiptdcdownload:
            if tdcuser == None:
                write_log(logfile, "-TDCUSER argument was not provided.", True)

        if not skiptdcdownload:
            if tdcpass == None:
                write_log(logfile, "-TDCPASS argument was not provided.", True)

    write_log(logfile, "----------------------------------------")
    if args.yamlfile != None:
        write_log(logfile, "Loading YAML File")
        write_log(logfile, "----------------------------------------")
        write_log(logfile, "YAML File: " + yamlfile)
    else:
        write_log(logfile, "Loading Arguments")
        write_log(logfile, "----------------------------------------")
    write_log(logfile, "----------------------------------------")
    write_log(logfile, "Confirmed Telonics Data Converter " + str(tdcexe))
    write_log(logfile, "Confirmed AM Collar File Loader " + str(cflexe))
    write_log(logfile, "Confirmed Data Path " + str(datapath))
    write_log(logfile, "Confirmed Load Path " + str(loadpath))
    write_log(logfile, "Confirmed Start Date " + str(startdate))
    write_log(logfile, "Confirmed End Date " + str(enddate))
    write_log(logfile, "Confirmed Users: ")
    if args.yamlfile != None:
        write_log(logfile, tdcusers)
    else:
        write_log(logfile, "NPSUSER=" + str(npsuser))
        write_log(logfile, "TDCUSER=" + str(tdcuser))
        write_log(logfile, "TDCPASS=" + str(tdcpass))
    if skiptdcdownload == True:
        write_log(logfile, "Confirmed TDC Dowload is being skipped")
    else:
        write_log(logfile, "Confirmed TDC Download is being executed")
    if skipamupload == True:
        write_log(logfile, "Confirmed AM Upload is being skipped")
    else:
        write_log(logfile, "Confirmed AM Upload is being executed")

    #Run TDC Download
    if not skiptdcdownload:

        if tdcusers == None:
            loadpath = download_tdcweb_iridium_data(tdcexe, datapath, tdcuser, tdcpass, startdate, enddate, logfile)
            if not skipamupload:
                load_tdcweb_iridium_data(cflexe, loadpath, npsuser, logfile)
        else:
            for tdcuser in tdcusers:
                loadpath = download_tdcweb_iridium_data(tdcexe, datapath, tdcuser["TDCUSER"], tdcuser["TDCPASS"], startdate, enddate, logfile)
                if not skipamupload:
                    load_tdcweb_iridium_data(cflexe, loadpath, tdcuser["NPSUSER"], logfile)

    else:
        if not skipamupload:
            if tdcusers == None:
                load_tdcweb_iridium_data(cflexe, loadpath, npsuser, logfile)
            else:
                for tdcuser in tdcusers:
                    load_tdcweb_iridium_data(cflexe, tdcuser["LOADPATH"], tdcuser["NPSUSER"], logfile)


