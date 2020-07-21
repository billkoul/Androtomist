import os
import glob
import sys

decompiled_folder = sys.argv[1]
#package_name = sys.argv[2]
path = decompiled_folder+"\\smali"

files = glob.glob(path + "/**/*.smali", recursive = True)

log = ""
apiCalls = []

for file in files:
	with open(file) as f:
		Lines = f.readlines()
		for line in Lines:
			if "invoke-virtual" in line or "invoke-direct" in line or "invoke-static" in line:
				apiCall = line.rsplit("L", 1)[1]
				if apiCall not in apiCalls and len(apiCall) > 10 and ";->" in apiCall and "$" not in apiCall and "<" not in apiCall:
					#apiCalls.append("L"+apiCall[:-2]+"\n")
					apiCalls.append("L"+apiCall[:-2])

for apiCall in apiCalls:
	print(apiCall)

#f = open("log\\" + package_name + "_log.txt", "a")
#for apiCall in apiCalls:
#	f.write(apiCall)
#f.close()