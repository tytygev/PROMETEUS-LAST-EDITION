using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;

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


    // модель данных для DataBase
    public class DatabaseData
    {
        public Person Person { get; set; }
        public City City { get; set; }

        public DatabaseData() { } // конструкторы без параметров нужны для сериализуемых классов, если в них есть конструкторы с параметрами

        public DatabaseData(Person person, City city)
		{
            this.Person = person;
            this.City = city;
		}

        public static DatabaseData Default => new DatabaseData(Person.Default, City.Default);
    }

    public class City
    {
        public string Name { get; set; }
        public bool Concern { get; set; }

        public City() { }

        public City(string name, bool concern)
		{
            this.Name = name;
            this.Concern = concern;
		}

        public static City Default => new City("moscow", true);

    }

    public class Person
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public bool Concern { get; set; }

        public Person() { }

        public Person(string name, string phone, bool concern)
		{
            this.Name = name;
            this.Phone = phone;
            this.Concern = concern;
		}

        public static Person Default => new Person("tytygev", "+78005553535", true);
    }

    // модель данных для Settings
    public class SettingsData
    {
        public float SomeValue { get; set; }

        public static SettingsData Default => new SettingsData { SomeValue = 37.0f };
    }
}
