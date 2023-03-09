using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROMETEUS_LAST_EDITION
{
   public class SettingsFX
    {

        public static List<string> LoadUserSettings(string userName)
        {           
            int length = Properties.Settings.Default.UserSettings.Count;
            List<string> userSettings = new List<string>();//Список настроек искомой строки
            List<string> linesUserSet = new List<string>();//Список всех настроек построчно
            for (int i = 0; i < length; i++) linesUserSet.Add(Properties.Settings.Default.UserSettings[i]);//получаем все строки из настроек

            //Предварительное заполнение списка настроек из первой строки, т.к. если пользователь не найдётся, то используются настройки по дефаулту
            string[] subsero = linesUserSet[0].Split(char.Parse(",")); //разбиваем 1ю строку на массив
            foreach (var sub in subsero) userSettings.Add(sub); //проходим по элементам и добавляем в список

            //Теперь поиск юзера
            foreach (var line in linesUserSet) //проходим по строкам
            {
                string[] subs = line.Split(char.Parse(",")); //разбиваем строку на массив
                if (userName == subs[0]){userSettings.Clear(); foreach (var sub in subs) userSettings.Add(sub);}//проходим по элементам и добавляем в очищенный список только если нашли юзера
            }          
            return userSettings; //возвращаем 
        }

        public static bool SaveUserSettings(string userName, List<string> userSettings)
        {
            bool flag = true;
            int length = Properties.Settings.Default.UserSettings.Count;            
            List<string> linesUserSet = new List<string>();//Список всех настроек построчно

            for (int i = 0; i < length; i++)
            {
                linesUserSet.Add(Properties.Settings.Default.UserSettings[i]);//получаем строку из настроек
                string[] subs = linesUserSet[i].Split(char.Parse(",")); //разбиваем строку на массив
                if (userName == subs[0])//если находим сохранённого ранее пользователя
                {
                    string line = userName;
                    for (int j =1; j<userSettings.Count; j++) line = line + "," + userSettings[j];//формируем строку
                    Properties.Settings.Default.UserSettings[i] = line;
                    flag = false;
                }
              
            }
            if (flag) //если юзера не нашлось, то добавляем новую хапись в сеттингс
            {
                string line = userName;
                for (int j = 1; j < userSettings.Count; j++) line = line + "," + userSettings[j];//формируем строку
                Properties.Settings.Default.UserSettings.Add(line);
            }
            Properties.Settings.Default.Save();
            return true;
        }





    }
}
