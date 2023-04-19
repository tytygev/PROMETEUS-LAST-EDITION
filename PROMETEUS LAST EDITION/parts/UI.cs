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


    }
}