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
        /// <summary>
        /// Полностью очищает переменную UserSettings!
        /// </summary>
        public void SettingsClear() { Properties.Settings.Default.UserSettings.Clear(); Properties.Settings.Default.Save(); MainWindow.LOG("\nProperties.Settings.Default.UserSettings очищен!\n",true); }

        /// <summary>
        /// Считывает список параметров из PropertiesSettings
        /// </summary>
        /// <param name = "PropertiesSettings" >Параметр Properties.Settings.Default</param >
        /// <param name = "userName" >Имя пользователя (не обязательно)</param >
        /// <returns>Возвращает строковый список параметров</returns>
        public List<string> LoadSettingsStringCollect(System.Collections.Specialized.StringCollection PropertiesSettings, string userName=null)//static 
        {
            bool flag = true;
            int length = PropertiesSettings.Count;//узнаем количество трок в параметре            
            List<string> PropertiesSettingsList = new List<string>();//Список всех настроек построчно
            for (int i = 0; i < length; i++) PropertiesSettingsList.Add(PropertiesSettings[i]);//получаем все строки из настроек
            MainWindow.LOG("Параметр "+ PropertiesSettings.ToString() + " из Properties прочитан. Количество строк "+length.ToString ());                     
            if (userName!=null) //Теперь поиск юзера, если пришел параметр
            {
            List<string> SettingsList = new List<string>();//Список настроек искомой строки   
            foreach (var line in PropertiesSettingsList) //проходим по строкам
            {
                string[] subs = line.Split(new string[] { "," }, StringSplitOptions.None); //разбиваем строку на массив
                if (userName == subs[0])//проходим по элементам и добавляем в очищенный список только если нашли юзера
                {
                    subs = subs.Skip(1).ToArray(); //удаляем из массива первый элемент (имя юзера)
                    foreach (var sub in subs) SettingsList.Add(sub);//накидываем в список
                    MainWindow.LOG("Пользователь "+userName+ " найден в "+ PropertiesSettings.ToString() +". Все значения параметров добавлены в список");
                    flag = false;
                }
            }
            if (flag) MainWindow.LOG("Пользователь " + userName + " не найден в " + PropertiesSettings.ToString() + ". Функция вернёт пустой список");
            return SettingsList;
            }
            return PropertiesSettingsList;
        }

        /// <summary>
        /// Парсит список с настройками пользователя и обновляет значения свойств объекта UserSettings
        /// </summary>
        /// <param name = "SettingsList" >список настроек</param >
        /// <returns>Возвращает булево значение</returns>
        public bool ParceUserSettings(List<string> SettingsList)
        {

            int enumCount = Enum.GetNames(typeof(DefUserSettings.SettingsElem)).Length;//длинна перечеслителя
            int listCount = SettingsList.Count;//длинна массива с сохраненными параметрами
            MainWindow.LOG("Длинна Enum и listSettingsOfUser совпадают? - " + (enumCount == listCount).ToString());
            if (enumCount == listCount)
            {
                for (int i = 0; i < enumCount; i++)
                {
                    string nameElem = Enum.GetName(typeof(DefUserSettings.SettingsElem), i);//имя поля перечеслителя

                    //string typeElem = FindName(nameElem).GetType().ToString().Substring(FindName(nameElem).GetType().ToString().LastIndexOf('.') + 1);
                    string typeElem = MainWindow.UserSettings[nameElem].GetType().ToString().Substring(MainWindow.UserSettings[nameElem].GetType().ToString().LastIndexOf('.') + 1);

                    switch (typeElem)
                    {
                        case "Boolean":
                            MainWindow.UserSettings[nameElem] = Convert.ToBoolean(SettingsList[i]);
                            MainWindow.LOG("Параметр " + nameElem + " = " + SettingsList[i]);
                            break;
                        case "String":
                            MainWindow.UserSettings[nameElem] = SettingsList[i];
                            MainWindow.LOG("Параметр " + nameElem + " = " + SettingsList[i]);
                            break;
                        case "Int32":
                            MainWindow.UserSettings[nameElem] = Convert.ToInt32(SettingsList[i]);
                            MainWindow.LOG("Параметр " + nameElem + " = " + SettingsList[i]);
                            break;
                        default:
                            MainWindow.LOG("ОШИБКА::: Неучтенный тип объекта nameElem", true);
                            break;

                    }


                }
                MainWindow.LOG("Настройки считаны");
                return true;
            }
            else
            {
                MainWindow.LOG("КРИТИЧЕСКАЯ ОШИБКА::: длинна перечислители и длинна списка загруженных параметров отличаются", true);
                MainWindow.LOG("\t\tБудут использованы значения настроек по умолчанию", true);
                return false;
            }

        }




        public static bool SaveUserSettings(List<string> userSettings, string userName = null)
        {
            bool flag = true;
            int length = Properties.Settings.Default.UserSettings.Count;
            List<string> linesUserSet = new List<string>();//Список всех настроек построчно

            for (int i = 0; i < length; i++)
            {
                linesUserSet.Add(Properties.Settings.Default.UserSettings[i]);//получаем строку из настроек
                string[] subs = linesUserSet[i].Split(new string[] { "," }, StringSplitOptions.None); //разбиваем строку на массив
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
        //enum ElementType
        //{
        //    ComboBox,
        //    CheckBox
        //    //TextBox,
        //    //RadioButton,
        //}
        public enum SettingsElem
        {
            NoShowStartPageCheckBox,
            ThemeComboBox,
            SaveWinSizeCheckBox,
            WindowState,
            WindowSizeW,
            WindowSizeH
        }        

        public object this[string propertyName]
        {
            get { return this.GetType().GetProperty(propertyName).GetValue(this, null); }
            set { this.GetType().GetProperty(propertyName).SetValue(this, value, null); }
        }
        public static bool NoShowStartPageCheckBox { get; set; } = false;
        public static int ThemeComboBox { get; set; } = 0;
        public static bool SaveWinSizeCheckBox { get; set; } = false;
        public static int WindowState { get; set; } = 0;
        public static int WindowSizeW { get; set; } = 1024;
        public static int WindowSizeH { get; set; } = 768;
    }

    

 
}
