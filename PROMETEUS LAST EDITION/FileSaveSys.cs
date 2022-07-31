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
    abstract class FileSaveSys
    {
        void Load(){}
        void Save(){}


    }
    
    class DataBase
    {
        void Save()
        {
            //string data = JsonSerializer.Serialize();
            //StreamWriter file = File.CreateText("DataBase.json");
            //file.WriteLine(data);
            //file.Close();
        }
        void Load()
        {
            //string data = File.ReadAllText("DataBase.json");
            //City city = JsonSerializer.Deserialize();
          
        }
    }
    class City
    {
        public string name;
        public bool concern;

    }
    class Person
    {
        public string name;
        public string phone;
        public bool concern;
    }
}
