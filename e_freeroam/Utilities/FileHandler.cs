using System;
using System.Collections.Generic;
using System.IO;
using e_freeroam.Utilities.ServerUtils;

namespace e_freeroam.Utilities
{
    public class FileHandler
    {
        string file, dir;
        Dictionary<string, string> fileContent = null;
        List<string> keys = null;
        bool empty = true;
        int fileLen = 0;
        const int maxFileLen = 500;
        FileTypes fileType;

        public FileHandler(FileTypes type, string directory, string fileName)
        {
            if(directory == null) directory = "";

            this.fileType = type;

            this.dir = directory;
            this.file = directory + '\\' + fileName + ".ini";

            fileContent = new Dictionary<string, string>(ServerData.maxKeys);
            keys = new List<string>(ServerData.maxKeys);

            for(int i = 0; i < ServerData.maxKeys; i++) keys.Insert(i, "NULL");
        }

        public static int getMaxFileLen() { return maxFileLen; }

        bool IsEmpty() {return this.empty;}

        public Dictionary<string, string> getInfo() {return this.fileContent;}

        public bool containsKey(string key) {return this.fileContent.ContainsKey(key);}

        public string getValue(string key) 
        {
            string result;
            this.fileContent.TryGetValue(key, out result);

            return result;
        }

        private void editValue(string key, string value) {if(this.containsKey(key)) this.fileContent.Add(key, value);}
        public void addValue(string key, string value)
        {
            if(!this.containsKey(key) && (this.fileLen < maxFileLen))
            {
                this.fileContent.Add(key, value);
                this.keys.Insert(this.fileLen++, key);

                if(this.IsEmpty()) this.empty = false;
            }
            else this.editValue(key, value);
        }

        public void removeValue(string key)
        {
            this.fileLen--;
            int index = this.keys.IndexOf(key);

            if (this.containsKey(key)) this.fileContent.Remove(key);
            for (int i = index; i < this.fileLen; i++)
            {
                if (i != this.fileLen)
                {
                    string newVal = this.keys[i + 1];
                    this.keys.Insert(i, newVal);
                }
                else
                {
                    this.keys.Remove(this.keys[i]);
                    break;
                }
            }
        }

        void processLine(string line)
        {
            string key, value;
            int index = 0;
            for(int i = 0; i < line.Length; i++)
            {
                if(line[i] == '=')
                {
                    key = line.Substring(0, i);

                    if(i != (line.Length - 1)) value = line.Substring((i + 1));
                    else value = "null";

                    this.addValue(key, value);
                    return;
                }
            }
        }

        public bool loadFile()
        {
            if(!File.Exists(this.file)) return false;

            string line = null;
            StreamReader reader = null;

            try {reader = new StreamReader(this.file);}
            catch(FileNotFoundException e) {System.Console.WriteLine(e.StackTrace);}
            catch(IOException e) {System.Console.WriteLine(e.StackTrace);}

            int index;
            for (index = 0; index < FileHandler.getMaxFileLen(); index++)
            {
                line = reader.ReadLine();

                if(line == null)
                {
                    this.empty = (index == 0);
                    break;
                }
                else this.processLine(line);

                line = null;
            }

            for (int i = 0; i < ServerData.maxKeys; i++)
            {
                string key = this.keys[i];
                if(key == "NULL") continue;
            }

            return true;
        }

        public bool saveFile()
        {
            if(!File.Exists(this.file))
            {
                System.IO.Directory.CreateDirectory(this.dir);
                FileStream temp = File.Create(this.file);

                temp.Close();
            }

            StreamWriter writer = null;
            writer = new StreamWriter(this.file);

            for (int i = 0; i < ServerData.maxKeys; i++)
            {
                string key = this.keys[i];

                if (key == "NULL")
                {
                    writer.Flush();
                    writer.Close();
                    break;
                }

                if (this.fileType == FileTypes.PLAYER)
                {
                    try {Enum.Parse(typeof(PlayerDataInfo), key);}
                    catch(ArgumentException e) {continue;}
                }

                if(this.fileType == FileTypes.SERVER)
                {
                    try {Enum.Parse(typeof(Utilities.ServerUtils.ServerDataInfo), key);}
                    catch (ArgumentException e) {continue;}
                }

                string result = null;
                this.fileContent.TryGetValue(key, out result);

                if (i > 0) writer.Write('\n');
                writer.Write(key + "=" + result);
            }

            return true;
        }
    }
}
