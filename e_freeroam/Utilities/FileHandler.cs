using System;
using System.Collections.Generic;
using System.IO;
using e_freeroam.Utilities.ServerUtils;

namespace e_freeroam.Utilities
{
    public class FileHandler
    {
		const ushort maxFileLen = 500;

        string file = null, dir = null;
        bool empty = true, fileLoaded = false;
        int fileLen = 0;

        Dictionary<string, string> fileContent = null;
        List<string> keys = null;

        FileTypes fileType;

		public static int getMaxFileLen() {return maxFileLen;}

        public FileHandler(FileTypes type, string directory, string fileName)
        {
            if(directory == null) directory = "";

            this.fileType = type;

            this.dir = directory;
            this.file = directory + '/' + fileName + ".ini";

            fileContent = new Dictionary<string, string>(ServerData.maxKeys);
            keys = new List<string>(ServerData.maxKeys);

            for(int i = 0; i < ServerData.maxKeys; i++) keys.Insert(i, "NULL");
        }

		public void updateFileType(FileTypes value) {this.fileType = value;}
		public FileTypes getFileType() {return this.fileType;}

		private void updateEmpty(bool value) {this.empty = value;}
        private bool IsEmpty() {return this.empty;}

        public Dictionary<string, string> getInfo() {return this.fileContent;}
		public void setInfo(Dictionary<string, string> content) {this.fileContent = content;}

        public bool containsKey(string key) {return this.fileContent.ContainsKey(key);}
		public bool containsValue(string value) {return this.fileContent.ContainsValue(value);}

		public string getKey(string value)
		{
			if(!this.containsValue(value)) return null;
			for(int i = 0; i < this.keys.Count; i++) if(this.getValue(keys[i]) == value) return keys[i];
			return null;
		}

        public string getValue(string key, bool addIfUnfound=false, string defaultVal="") 
        {
			if(!this.containsKey(key)) 
			{
				ServerData.logToConsole($"{this.file} did not contain key {key}.");
				if(!addIfUnfound) return null;
				this.addValue(key, defaultVal);
			}

            string result;
            this.fileContent.TryGetValue(key, out result);

            return result;
        }

        private void editValue(string key, string value) 
        {
            if(this.containsKey(key))
            {
                this.fileContent.Remove(key);
                this.fileContent.Add(key, value);
            }
        }
        public void addValue(string key, string value)
        {
            if(!this.containsKey(key) && (this.fileLen < maxFileLen))
            {
                this.fileContent.Add(key, value);
                this.keys.Insert(this.fileLen++, key);

                if(this.IsEmpty()) updateEmpty(false);
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
                if(i != this.fileLen)
                {
                    string newVal = this.keys[i + 1];
                    this.keys.Insert(i, newVal);
                }
                else
                {
                    this.keys.Remove(this.keys[i]);
                    break;
                }
				this.fileLen--;
            }
        }

		public bool hasFileBeenLoaded() {return this.fileLoaded;}

        private void processLine(string line)
        {
            string key, value;

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
            Console.WriteLine($"Loading {this.file}");

            string line = null;
            StreamReader reader = null;

            try {reader = new StreamReader(this.file);}
            catch(FileNotFoundException e) {System.Console.WriteLine(e.StackTrace);}
            catch(IOException e) {System.Console.WriteLine(e.StackTrace);}

            int index;
            for(index = 0; index < FileHandler.getMaxFileLen(); index++)
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

            for(int i = 0; i < ServerData.maxKeys; i++)
            {
                string key = this.keys[i];
                if(key == "NULL") continue;
            }

			this.fileLoaded = true;
            return true;
        }

        public bool saveFile(string[] skip = null)
        {
            if(!File.Exists(this.file))
            {
                System.IO.Directory.CreateDirectory(this.dir);
                FileStream temp = File.Create(this.file);

                temp.Close();
            }

            StreamWriter writer = new StreamWriter(this.file);

            for (int i = 0; i < ServerData.maxKeys; i++)
            {
                string key = this.keys[i];

				if(skip != null && skip.Length != 0)
				{
					bool cont = false;

					for(int ii = 0; ii < skip.Length; ii++) 
					{
						if(skip[ii] == key)
						{
							cont = true;
							break;
						}
					} if(cont) continue;
				}

                if (key == "NULL")
                {
                    writer.Flush();
                    writer.Close();
                    break;
                }

                /*if (this.fileType == FileTypes.PLAYER)
                {
                    try {Enum.Parse(typeof(PlayerInfo), key);}
                    catch(ArgumentException) { continue;}
                }

                if(this.fileType == FileTypes.SERVER)
                {
                    try {Enum.Parse(typeof(Utilities.ServerUtils.ServerDataInfo), key);}
                    catch(ArgumentException) {continue;}
                }*/

                string result = null;
                this.fileContent.TryGetValue(key, out result);

                if (i > 0) writer.Write('\n');
                writer.Write(key + "=" + result);
            }

            return true;
        }

		public bool deleteFile()
		{
			if(!File.Exists(this.file)) return false;
			try {File.Delete(this.file);}
			catch (Exception) { return false;}

			this.fileContent = null;
			this.keys = null;

			this.fileLen = 0;
			this.empty = true;

			return true;
		}
    }
}
