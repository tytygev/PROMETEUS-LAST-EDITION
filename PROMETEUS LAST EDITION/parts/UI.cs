using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Media;
using Microsoft.Office.Interop;
using Excel = Microsoft.Office.Interop.Excel;

namespace PROMETEUS_LAST_EDITION
{
    public class UI
    {
        enum ViewPage
        {
            KitSetPage,
            PricePage,
            DBEditPage,
            SettingsPage,
            AboutPage,
            StartPage
        }
        enum MainMenuButton
        {
            KitSetButton,
            PriceButton,
            DBEditButton,
            SettingsButton,
            AboutButton,
            ExitButton
        }
        enum Themes
        {
            DictionaryDarkTheme,
            DictionaryLightTheme,
            DictionaryLightThemeRed
        }
        public bool InitializeDefaultTheme()//Применение словаря ресурсов по умолчанию
        {
            ResourceDictionary dictZ = new ResourceDictionary();
            dictZ.Source = new Uri("DictionaryDarkTheme.xaml", UriKind.Relative);
            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(dictZ);
            return true;
        }

        /// <summary>
        /// Применяет словари ресурсов темы оформления в зависимости от настроек
        /// https://stackoverflow.com/questions/786183/wpf-changing-resources-colors-from-the-app-xaml-during-runtime   
        /// </summary>
        /// <param name = "param" >номер темы в перечислителе</param >
        /// <returns>Возвращает булево значение</returns>
        public bool InitializeUserSettingsTheme(int param)
        {
            bool flag = false;
            //int param = DefUserSettings.SettingsThemeComboBox;
            ResourceDictionary dict = new ResourceDictionary();
            switch (param)
            {
                case (int)Themes.DictionaryDarkTheme:
                    dict.Source = new Uri("DictionaryDarkTheme.xaml", UriKind.Relative);
                    flag = true;
                    break;
                case (int)Themes.DictionaryLightTheme:
                    dict.Source = new Uri("DictionaryLightTheme.xaml", UriKind.Relative);
                    flag = true;
                    break;
                case (int)Themes.DictionaryLightThemeRed:
                    dict.Source = new Uri("DictionaryLightThemeRed.xaml", UriKind.Relative);
                    flag = true;
                    break;
                default:
                    break;
            }
            //Имейте в виду, что MergedDictionaries — это контейнер. Ресурс в самом последнем добавленном ResourceDictionary выигрывает.
            //Если намерение состоит в том, чтобы переключаться между словарями с какой-либо регулярностью, удаление предыдущего словаря
            //из списка может быть полезным. (В ответе упоминается «выгрузить его в коде» по умолчанию, указанному в XAML, но не показано,
            //как его идентифицировать и удалить.) 
            if (flag) 
            {
                Application.Current.Resources.MergedDictionaries.Clear();
                Application.Current.Resources.MergedDictionaries.Add(dict);
                //(FindResource("documentTemplates") as System.Windows.Data.ObjectDataProvider).Refresh();
                
            }
            return flag;
        }
        public MainWindow mw;        
       /// <summary>
       /// Выставить всем страничкам невидимость
       /// </summary>
       /// <param name="mw">ссылка на объект</param>
       /// <returns>булево</returns>
        public bool ViewPageInitVisible(MainWindow mw)
        {
           
            int enumCount = Enum.GetNames(typeof(ViewPage)).Length;
            string nameElem = null;
            Grid gr = null;
            for (int i=0; i< enumCount; i++)
            {
                nameElem = Enum.GetName(typeof(ViewPage), i);
               
                     gr = mw.FindName(nameElem) as Grid;
                gr.Visibility = Visibility.Hidden;
            }
             gr = mw.FindName("StartPage") as Grid;
            gr.Visibility = Visibility.Visible;
            return true;
        }

        /// <summary>
        /// Устанавливает свойства для главного окна
        /// </summary>
        /// <returns>булево значение</returns>
        public bool SetValUserSettings(MainWindow mw)
        {
            MainWindow.LOG(">>> Загрузка параметров главного окна...");
            string nameElem = null;
            int enumCount = Enum.GetNames(typeof(DefUserSettings.SettingsElem)).Length; //длинна нумератора с сохраняемыми параметрами
            nameElem = "SaveWinSizeCheckBox";
            if (Convert.ToBoolean(MainWindow.UserSettings[nameElem])) {
                for (int i = 0; i < enumCount; i++)
                {
                    nameElem = Enum.GetName(typeof(DefUserSettings.SettingsElem), i);
                    switch (nameElem)
                    {
                        case "WindowState":
                            switch (Convert.ToInt32(MainWindow.UserSettings[nameElem]))
                            {
                                case 0: mw.WindowState = WindowState.Normal; MainWindow.LOG("\t WindowState.Normal"); break;
                                case 1: mw.WindowState = WindowState.Minimized; MainWindow.LOG("\t WindowState.Minimized"); break;
                                case 2: mw.WindowState = WindowState.Maximized; MainWindow.LOG("\t WindowState.Maximized"); break;
                                default: break;
                            }
                            break;
                        case "WindowSizeW":
                            mw.Width = Convert.ToInt32(MainWindow.UserSettings[nameElem]);
                            MainWindow.LOG("\t Window Size Width = " + Convert.ToString(MainWindow.UserSettings[nameElem]));
                            break;
                        case "WindowSizeH":
                            mw.Height = Convert.ToInt32(MainWindow.UserSettings[nameElem]);
                            MainWindow.LOG("\t Window Size Height = " + Convert.ToString(MainWindow.UserSettings[nameElem]));
                            break;
                    }
                }
            }
            nameElem = "NoShowStartPageCheckBox";
            if (Convert.ToBoolean(MainWindow.UserSettings[nameElem])) { mw.StartPage.Visibility = Visibility.Hidden; MainWindow.LOG(">>> Стартовое окно скрыто по просьбам трудящихся"); }

            return true;
        }

        /// <summary>
        /// Выводит сообщение окне программы
        /// </summary>
        /// <param name = "mw" >Ссылка на главное окно</param >
        /// <param name = "m" >Строка для вывода</param >
        /// <param name = "m2" >Дополнительная строка (null по умолчанию)</param >
        /// <param name = "newLine" >Дополнительная строка на второй строке (true по умолчанию)</param >
        /// <returns>Ничего не возвращает</returns>
        public void FooterPromtShow(MainWindow mw,object m, object m2 = null, bool newLine = true)
        {
            string sm = m.ToString() + " ";
            if (newLine) { sm += Environment.NewLine; }
            if (m2 != null) { sm += m2.ToString(); }
            TextBlock tb = mw.FindName("FooterPrompt") as TextBlock;
            if (tb != null) tb.Text = sm;
        }
    }
}