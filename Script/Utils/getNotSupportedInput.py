import json
import os
import time
start = time.time()

def getInput(fname):
    fo = open(r".\count\\" + fname.split("\\")[-1], "w",encoding = "UTF-8")
    with open(fname, "r", encoding = "utf-8-SIG") as load_f:
        load_dict = json.load(load_f)
        for i in load_dict:
            if "NotSupported" in i and "dotnet" in i["NotSupported"]:
                fo.write(i["Input"]+"\n")

def getAllDir(path):
    filess = os.listdir(path)
    print(filess)
    for files in filess:
        if files != "README.md":
            if os.path.isdir(path+files):
                print("dir:"+files)
                getAllDir(path+files+'\\')
            else:
                getInput(path+"\\"+files)
path = r".\Specs\DateTime\Spanish\\"
fo.close
getAllDir(path)
end = time.time()
print(end-start)