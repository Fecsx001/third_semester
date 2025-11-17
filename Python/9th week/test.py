import os

print(os.getcwd())

print(os.path.join(os.getcwd(), "temp_dir"))

print([x for x in os.listdir() if x.endswith(".py")])

f = open("file1.txt", "w")
with open("file1.txt", "w") as f2:
    print(f == f2)
f.close()
