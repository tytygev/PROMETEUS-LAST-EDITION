using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;
using PROMETEUS_LAST_EDITION.models.database;
using PROMETEUS_LAST_EDITION.models.settings;

namespace PROMETEUS_LAST_EDITION
{
    // базовый класс для всех систем сохранения файлов
    public abstract class FileSaveSystem<T>
    {
        private readonly string fileName;

        public T Data { get; set; }

        public FileSaveSystem(string fileName)
		{
            this.fileName = fileName;
		}

        public void Load()
        {
			string data = File.ReadAllText(fileName);
			Data = JsonSerializer.Deserialize<T>(data);
		}

        public void Save()
        {
            var options = new JsonSerializerOptions();
            options.WriteIndented = true;
            //options.IncludeFields = true; //только для .Net version >= 5

            string data = JsonSerializer.Serialize(Data, options);
			StreamWriter file = File.CreateText(fileName);
			file.WriteLine(data);
			file.Close();
		}
    }
    
    public class DataBase : FileSaveSystem<DatabaseData>
    {
        public DataBase() : base("DataBase.json")
		{
            Data = DatabaseData.Default;
		}
    }

    public class Settings : FileSaveSystem<SettingsData>
	{
        public Settings() : base("Settings.json")
		{
            Data = SettingsData.Default;
        }
	}
}
