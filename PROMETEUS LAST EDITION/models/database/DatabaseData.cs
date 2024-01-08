using System;
using System.Collections.Generic;
using System.Linq;

namespace PROMETEUS_LAST_EDITION.models.database
{

    // модель данных для DataBase
    public class DatabaseData
    {
        public List<Person> Drivers { get; set; }
        public List<City> Cities { get; set; }

        public DatabaseData() { } // конструкторы без параметров нужны для сериализуемых классов, если в них есть конструкторы с параметрами

        public DatabaseData(List<Person> drivers, List<City> cities)
        {
            this.Drivers = drivers;
            this.Cities = cities;
        }

        public static DatabaseData Default => new DatabaseData();
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

        public List<Car> Cars { get; set; }

        public Person() { }

        public Person(string name, string phone, bool concern, params Car[] cars)
        {
            this.Name = name;
            this.Phone = phone;
            this.Concern = concern;
            Cars = cars.ToList();    
        }

        public static Person Default => new Person("tytygev", "+78005553535", true, Car.Default);
    }

    public class Car
    {
        public enum BrandType
        {
            VAZ = 0,
            Kia = 1,
            Huynahuydai = 2,
        }

        public enum CarBodyType
        {
            Buggy,
            Convertible,
            Coupe,
            Hatchback,
            Hearse,
            Limousine,
            Microvan,
            Minivan,
            Pickup,
            Roadster,
            Sedan,
            Ute
        }

        public BrandType Brand { get; set; }

        public string Model { get; set; }

        public string RegistrationNumber { get; set; }

        public Car() { }


        public Car(BrandType brand, string model, string registrationNumber)
        {
            Brand = brand;
            Model = model;
            RegistrationNumber = registrationNumber;
        }

        public static Car Default => new Car(BrandType.VAZ, "2106", "е666кх99");
    }
}
