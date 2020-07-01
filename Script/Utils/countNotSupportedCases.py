import json
import os
import time
start = time.time()


def get_input(fname):
    count = 0
    with open(fname, "r", encoding="UTF-8") as load_f:
        load_dict = json.load(load_f)
        for i in load_dict:
            if "NotSupported" in i and "python" in i["NotSupported"]:
                count += 1
    print(fname, count)


def get_all_dir(path):
    filess = os.listdir(path)
    print(filess)
    for files in filess:
        if files != "README.md":
            if os.path.isdir(path+files):
                print("dir:"+files)
                get_all_dir(path+files+'\\')
            else:
                get_input(path+"\\"+files)


path = r".\Specs\DateTime\Spanish\\"
get_all_dir(path)
end = time.time()
print(end-start)