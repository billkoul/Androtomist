CREATE TABLE A_INFO
(	
	PROJECT_PATH	 		TEXT,
	TOOLS_PATH	 			TEXT,
	FRIDA_PATH				TEXT,
	SCRIPT		 			TEXT,
	EVENTS					INTEGER,
	REMOTE_ADDR				VARCHAR(200),
	MALWARE_FILE_LABEL		VARCHAR(200),
	GOODWARE_FILE_LABEL 	VARCHAR(200)
);

--TOOLS_PATH is the path of the Android platform-tools
--PROJECT_PATH is the path of the project solution
--FRIDA_PATH is the path of frida scripts
--SCRIPT is the name of the script to run during dynamic instrumentation
--EVENTS is the number of events to inject with Monkey
--REMOTE_ADDR is the IP addr of the VM