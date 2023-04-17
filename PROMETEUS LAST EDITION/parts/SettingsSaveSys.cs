using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using System.IO;
using System.Collections;

namespace PROMETEUS_LAST_EDITION
{
    public class SettingsFX
    {

        public static List<string> LoadUserSettings(string userName)//static
        {
            bool flag = true;
            int length = Properties.Settings.Default.UserSettings.Count;//узнаем количество трок в параметре            
            List<string> linesUserSet = new List<string>();//Список всех настроек построчно
            for (int i = 0; i < length; i++) linesUserSet.Add(Properties.Settings.Default.UserSettings[i]);//получаем все строки из настроек
            MainWindow.LOG("Параметр UserSettings из Properties прочитан. Количество строк {length}",true,true);
            List<string> userSettings = new List<string>();//Список настроек искомой строки
            //Теперь поиск юзера
            foreach (var line in linesUserSet) //проходим по строкам
            {
                string[] subs = line.Split(char.Parse(",")); //разбиваем строку на массив
                if (userName == subs[0])//проходим по элементам и добавляем в очищенный список только если нашли юзера
                {//userSettings.Clear(); //очистка требовалась раньше, когда параметры по умолчанию хранились в параметрах и добавлялись из самой первой строки
                    subs = subs.Skip(1).ToArray(); //удаляем из массива первый элемент (имя юзера)
                    foreach (var sub in subs) userSettings.Add(sub);//накидываем в список
                    MainWindow.LOG("Пользователь {userName} найден. Все элементы добавлены в список", true, true);
                    flag = false;
                }
            }
            if (flag) MainWindow.LOG("Пользователь {userName} не найден. Функция вернёт пустой список", true, true);
            return userSettings;
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
                    for (int j = 0; j < userSettings.Count; j++) line = line + "," + userSettings[j];//формируем строку
                    Properties.Settings.Default.UserSettings[i] = line;
                    flag = false;
                }
                
            }
            if (flag) //если юзера не нашлось, то добавляем новую хапись в сеттингс
            {
                string line = userName;
                for (int j = 0; j < userSettings.Count; j++) line = line + "," + userSettings[j];//формируем строку
                Properties.Settings.Default.UserSettings.Add(line);
            }


            //удаление пустых строк
            for (int l = 0; l < Properties.Settings.Default.UserSettings.Count; l++) { if (Properties.Settings.Default.UserSettings[l] == "") Properties.Settings.Default.UserSettings.RemoveAt(l); }

            Properties.Settings.Default.Save();


            List<string> linesUserSetlog = new List<string>();//Список всех настроек построчно
            for (int i = 0; i < Properties.Settings.Default.UserSettings.Count; i++) linesUserSetlog.Add(Properties.Settings.Default.UserSettings[i]);//получаем все строки из настроек
            File.WriteAllLines("UserSetlog.txt", linesUserSetlog);

            return true;
        }





    }

    public class DefUserSettings
    {
        enum ElementType
        {
            ComboBox,
            CheckBox
            //TextBox,
            //RadioButton,
        }
        

        public object this[string propertyName]
        {
            get { return this.GetType().GetProperty(propertyName).GetValue(this, null); }
            set { this.GetType().GetProperty(propertyName).SetValue(this, value, null); }
        }
        public static bool NoShowStartPageCheckBox { get; set; } = false;
        public static int ThemeComboBox { get; set; } = 2;
        public static bool SaveWinSizeCheckBox { get; set; } = false;
    }

    public class DefGlobalSettings
    {
    }
}
