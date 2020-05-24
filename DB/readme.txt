These files contain the DB structure.

The A_INFO file contains important paths and other details. Fill in before using:
--TOOLS_PATH is the path of the Android platform-tools
--PROJECT_PATH is the path of the project solution
--FRIDA_PATH is the path of frida scripts
--SCRIPT is the name of the script to run during dynamic instrumentation
--EVENT is the number of events to inject with Monkey
--REMOTE_ADDR is the IP addr of the VM

B_USER is the user table, create a demo user before using:
note passwords are sha128 hashes, for example test123 is the following:
DAEF4953B9783365CAD6615223720506CC46C5167CD16AB500FA597AA08FF964EB24FB19687F34D7665F778FCB6C5358FC0A5B81E1662CF90F73A2671C53F991