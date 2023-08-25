using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using System.IO;
using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace PROMETEUS_LAST_EDITION
{
    public partial class MainWindow : Window
    {
        public string dbDefPath = "db.xml";
      
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if ((bool)UserSettings["SaveWinSizeCheckBox"])
            {
                UserSettings["WindowSizeW"] = Convert.ToInt32(this.Width);
                UserSettings["WindowSizeH"] = Convert.ToInt32(this.Height);
                LOG("~~~ Поле UserSettings.WindowSizeW = " + Convert.ToString(this.Width));
                LOG("~~~ Поле UserSettings.WindowSizeH = " + Convert.ToString(this.Height));
            }
        }
        private void Window_StateChanged(object sender, EventArgs e) { if ((bool)UserSettings["SaveWinSizeCheckBox"]) { UserSettings["WindowState"] = this.WindowState; LOG("~~~ Поле UserSettings.WindowState = " + Convert.ToString(this.WindowState)); } }

    }
    public class SettingsFX
    {
        public MainWindow mw;

        #region Пользовательские настройки
        /// <summary>
        /// Полностью очищает переменную UserSettings!
        /// </summary>
        public void SettingsClear() { Properties.Settings.Default.UserSettings.Clear(); Properties.Settings.Default.Save(); MainWindow.LOG("\n< < < Properties.Settings.Default.UserSettings очищен! > > >\n",true); }

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
            MainWindow.LOG(">>> Параметр "+ PropertiesSettings.ToString() + " из Properties прочитан. Количество строк "+length.ToString ());                     
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
                        MainWindow.LOG(">>> Пользователь "+userName+ " найден в "+ PropertiesSettings.ToString() +". Все значения параметров добавлены в список");
                    flag = false;
                }
            }
            if (flag) MainWindow.LOG(">>> Пользователь " + userName + " не найден в " + PropertiesSettings.ToString() + ". Функция вернёт пустой список");
            return SettingsList;
            }
            return PropertiesSettingsList;
        }

        /// <summary>
        /// Парсит список с настройками пользователя и обновляет значения свойств объекта UserSettings
        /// </summary>
        /// <param name = "SettingsList" >список настроек</param >
        /// <returns>Возвращает булево значение</returns>
        public bool ParsingUserSettings(List<string> SettingsList)
        {
            
            int enumCount = Enum.GetNames(typeof(DefUserSettings.UserSettingsElem)).Length;//длинна перечеслителя
            int listCount = SettingsList.Count;//длинна массива с сохраненными параметрами
            MainWindow.LOG(">>> Длинна SettingsElem и listSettingsOfUser совпадают? - " + (enumCount == listCount).ToString());
            if (enumCount == listCount)
            {
                MainWindow.LOG(">>> Начат парсинг списка с настройками:");
                for (int i = 0; i < enumCount; i++)
                {
                    string nameElem = Enum.GetName(typeof(DefUserSettings.UserSettingsElem), i);//имя поля перечеслителя

                    //string typeElem = FindName(nameElem).GetType().ToString().Substring(FindName(nameElem).GetType().ToString().LastIndexOf('.') + 1);
                    string typeElem = MainWindow.UserSettings[nameElem].GetType().ToString().Substring(MainWindow.UserSettings[nameElem].GetType().ToString().LastIndexOf('.') + 1);

                    switch (typeElem)
                    {
                        case "Boolean":
                            MainWindow.UserSettings[nameElem] = Convert.ToBoolean(SettingsList[i]);
                            MainWindow.LOG("\tПараметр " + nameElem + " = " + SettingsList[i]);
                            break;
                        case "String":
                            MainWindow.UserSettings[nameElem] = SettingsList[i];
                            MainWindow.LOG("\tПараметр " + nameElem + " = " + SettingsList[i]);
                            break;
                        case "Int32":
                            MainWindow.UserSettings[nameElem] = Convert.ToInt32(SettingsList[i]);
                            MainWindow.LOG("\tПараметр " + nameElem + " = " + SettingsList[i]);
                            break;
                        default:
                            MainWindow.LOG("ОШИБКА::: Неучтенный тип " + typeElem+" объекта с именем "+ nameElem, true);
                            break;

                    }


                }
                MainWindow.LOG(">>> Парсинг закончен");
                return true;
            }
            else
            {
                MainWindow.LOG("КРИТИЧЕСКАЯ ОШИБКА::: длинна перечислители и длинна списка загруженных параметров отличаются", true);
                MainWindow.LOG("\tБудут использованы значения настроек по умолчанию", true);
                return false;
            }

        }

        /// <summary>
        /// Возвращает список значений полей класса UserSettings, 
        /// имена которых перечислены в SettingsElem
        /// </summary>
        /// <returns>Список строк</returns>
        public List<string> CollectingUserSettings()
        {
            MainWindow.LOG(">>> Подготовка списка значений для сохранения: ");
            List<string> ListSettings = new List<string>();//Список значений всех элементов
            var enumCount = Enum.GetNames(typeof(DefUserSettings.UserSettingsElem)).Length;//длинна нумератора SettingsElem, учитывающего все элементы считывающиеся для хранения их свойств
            for (int i = 0; i < enumCount; i++)
            {
                string nameElem = Enum.GetName(typeof(DefUserSettings.UserSettingsElem), i);
                ListSettings.Add(Convert.ToString(MainWindow.UserSettings[nameElem]));
                MainWindow.LOG("\tСвойство объекта "+ nameElem +" = "+ Convert.ToString(MainWindow.UserSettings[nameElem]));
            }
            return ListSettings;
        }
        
        public  bool SaveUserSettings(List<string> userSettings, string userName = null)//static
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

        public bool SetupDefaultUser()
        {
            MainWindow.LOG(">>> Сброс списка настроек: ");
            DefUserSettings defUseSet = new DefUserSettings();
            var enumCount = Enum.GetNames(typeof(DefUserSettings.UserSettingsElem)).Length;//длинна нумератора SettingsElem, учитывающего все элементы считывающиеся для хранения их свойств
            for (int i = 0; i < enumCount; i++)
            {
                string nameElem = Enum.GetName(typeof(DefUserSettings.UserSettingsElem), i);
                MainWindow.UserSettings[nameElem] = defUseSet[nameElem];
                MainWindow.LOG("\tСвойство объекта " + nameElem + " = " + Convert.ToString(MainWindow.UserSettings[nameElem]));
            }
            return true;
        }
        #endregion

        #region Глобальные настройки
       
        #endregion


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
        public enum UserSettingsElem
        {
            //flagResetUserSettings,
            NoShowStartPageCheckBox,
            NoShowStartPageCheckBox2,
            ThemeComboBox,
            SaveWinSizeCheckBox,
            WindowState,
            WindowSizeW,
            WindowSizeH,
            NoShowBDMSiWinCheckBox,
            NoShowBDMSiWinCheckBox2
        }        

        public object this[string propertyName]
        {
            get { return this.GetType().GetProperty(propertyName).GetValue(this, null); }
            set { this.GetType().GetProperty(propertyName).SetValue(this, value, null); }
        }
        //public static bool flagResetUserSettings { get; set; } = false;
        public bool NoShowStartPageCheckBox { get; set; } = false;//static
        public bool NoShowStartPageCheckBox2 { get; set; } = false;
        public  int ThemeComboBox { get; set; } = 0;
        public  bool SaveWinSizeCheckBox { get; set; } = false;
        public  int WindowState { get; set; } = 0;
        public  int WindowSizeW { get; set; } = 1024;
        public  int WindowSizeH { get; set; } = 768;
        public bool NoShowBDMSiWinCheckBox { get; set; } = false;
        public bool NoShowBDMSiWinCheckBox2 { get; set; } = false;
    }


    public class GlobalSettings
    {
        public enum GlobalSettingsElem
        {
            DefDelRadioButton,
            CustomDelRadioButton,
            SeparatorTextBox
        }
        public object this[string propertyName]
        {
            get { return this.GetType().GetProperty(propertyName).GetValue(this, null); }
            set { this.GetType().GetProperty(propertyName).SetValue(this, value, null); }
        }
        public bool DefDelRadioButton { get; set; } = true;
        public bool CustomDelRadioButton { get; set; } = false;
        public string SeparatorTextBox { get; set; } = "";

        public void LoadGeneralProperties(MainWindow mw)
        {
            mw.DefDelRadioButton.IsChecked = Properties.Settings.Default.DefDelRadioButton;
            mw.CustomDelRadioButton.IsChecked = Properties.Settings.Default.CustomDelRadioButton;
            mw.SeparatorTextBox.Text = Properties.Settings.Default.SeparatorTextBox;
        }

        public void UploadGeneralProperties(MainWindow mw)
        {          
           Properties.Settings.Default.DefDelRadioButton = (bool)mw.DefDelRadioButton.IsChecked;                   
           Properties.Settings.Default.CustomDelRadioButton = (bool)mw.CustomDelRadioButton.IsChecked;                    
           Properties.Settings.Default.SeparatorTextBox = mw.SeparatorTextBox.Text;
        }

        public void ResetGeneralProperties(MainWindow mw)
        {

           
                    Properties.Settings.Default.DefDelRadioButton = DefDelRadioButton;                   
                    Properties.Settings.Default.CustomDelRadioButton =CustomDelRadioButton;                  
                    Properties.Settings.Default.SeparatorTextBox =SeparatorTextBox;

            mw.DefDelRadioButton.IsChecked =DefDelRadioButton;
            mw.CustomDelRadioButton.IsChecked =CustomDelRadioButton;
            mw.SeparatorTextBox.Text =SeparatorTextBox;
        
        }
        
    }


}
