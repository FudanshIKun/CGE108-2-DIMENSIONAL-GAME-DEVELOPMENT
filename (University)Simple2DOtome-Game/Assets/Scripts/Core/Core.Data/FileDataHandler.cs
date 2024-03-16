using System;
using System.IO;
using UnityEngine;

namespace Otome.Core
{
    public class FileDataHandler : MonoBehaviour
    {
        private string dataDirPath = "";
        private string dataFileName = "";

        public FileDataHandler(string dataDirPath, string dataFileName)
        {
            this.dataDirPath = dataDirPath;
            this.dataFileName = dataFileName;
        }

        public GameData Load()
        {
            string fullPath = Path.Combine(dataDirPath, dataFileName);
            GameData loadedData = null;
            if (File.Exists(fullPath))
            {
                try
                {
                    string dataToLoad = "";
                    using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            dataToLoad = reader.ReadToEnd();
                        }
                    }

                    // deserialize data from Json back into the c# Object
                    loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
                }
                catch (Exception e)
                {
                    Debug.Log("Error occured when trying to load data from file: " + fullPath + "\n" + e);
                }
            }

            return loadedData;
        }

        public void Save(GameData data)
        {
            string fullPath = Path.Combine(dataDirPath, dataFileName);
            try
            {
                // create directory where the file will be written to
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

                // serialize the c# game data obejct to Json
                string dataToScore = JsonUtility.ToJson(data, true);

                // write the serialized data to file
                using (FileStream stream = new FileStream(fullPath, FileMode.Create))
                {
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        writer.Write(dataToScore);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Log("Error occured when trying to save data to file: " + fullPath + "\n" + e);
            }
        }
    }
}
