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

using System.Reflection;
using System.Diagnostics;
using System.IO;

namespace PROMETEUS_LAST_EDITION
{
    public class UI
    {
        public MainWindow mw;
        enum ViewPage
        {
            KitSetPage,
            PricePage,
            DBEditPage,
            TaxiPage,
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
            MainWindow.LOG(">>> Загрузка параметров главного окна из объекта UserSettings...");
            string nameElem = null;
            int enumCount = Enum.GetNames(typeof(DefUserSettings.UserSettingsElem)).Length; //длинна нумератора с сохраняемыми параметрами
            nameElem = "SaveWinSizeCheckBox";
            if (Convert.ToBoolean(MainWindow.UserSettings[nameElem])) {
                for (int i = 0; i < enumCount; i++)
                {
                    nameElem = Enum.GetName(typeof(DefUserSettings.UserSettingsElem), i);
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
            else { MainWindow.LOG("\t\t...отменена!"); }
            nameElem = "NoShowStartPageCheckBox";
            if (Convert.ToBoolean(MainWindow.UserSettings[nameElem])) { mw.StartPage.Visibility = Visibility.Hidden; MainWindow.LOG(">>> Стартовое окно скрыто по просьбам трудящихся"); }
            nameElem = "NoShowBDMSiWinCheckBox2";
            if (Convert.ToBoolean(MainWindow.UserSettings[nameElem])) { 
                mw.DBEditButton.Visibility = Visibility.Collapsed;
                mw.DBOpenButton.Visibility = Visibility.Visible;
            }

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

        public void VersionIngect(MainWindow mw)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            mw.ProductVersionTextBlock.Text = "Версия " + fileVersionInfo.ProductVersion;
            mw.FileVersionTextBlock.Text = "ver.: " + fileVersionInfo.FileVersion;
            mw.FullInfoVersionTextBlock.Text = fileVersionInfo.ToString();
        }

       
      
    }

    public partial class MainWindow : Window {
        public void InitializeButtons()
        {
            KitSetButton.MouseUp += (s, e) => ShowView(KitSetPage);
            PriceButton.MouseUp += (s, e) => ShowView(PricePage);
            //if ((bool)NoShowBDMSiWinCheckBox2.IsChecked ){
            //    DBEditButton.MouseUp += (s, e) => OpenDB();
            //}
            //else{ 
            //    DBEditButton.MouseUp += (s, e) => ShowView(DBEditPage);
            //}
            DBOpenButton.MouseUp += (s, e) => OpenDB();
            DBEditButton.MouseUp += (s, e) => ShowView(DBEditPage);
            TaxiButton.MouseUp += (s, e) => ShowView(TaxiPage);
            SettingsButton.MouseUp += (s, e) => ShowView(SettingsPage);
            AboutButton.MouseUp += (s, e) => ShowView(AboutPage);
            ExitButton.MouseUp += (s, e) => Application.Current.Shutdown();

            NewKitSetButton.MouseUp += (s, e) => ShowView(KitSetPage);
            OpenKitSetButton.MouseUp += (s, e) => ShowView(KitSetPage);
            SaveKitSetButton.MouseUp += (s, e) => ShowView(KitSetPage);
            SaveasKitSetButton.MouseUp += (s, e) => ShowView(KitSetPage);
            PrintfKitSetButton.MouseUp += (s, e) => ShowView(KitSetPage);
            PrintKitSetButton.MouseUp += (s, e) => ShowView(KitSetPage);
        }
        public void ShowView(Grid view)
        {

            if (view == currentVisibleView)
                return;
            if (currentVisibleView != null)
                currentVisibleView.Visibility = Visibility.Hidden;
            view.Visibility = Visibility.Visible;
            currentVisibleView = view;
        }
        public void OpenDB()
        {
            //DBOpenButton.Checked = false;
            new UI().FooterPromtShow(this, "Открытие Excel...");
            new FileFX().OpenDBinExcel(System.IO.Directory.GetCurrentDirectory() + "\\" + dbDefPath);
            //Application.Current.Shutdown();
        }
        //работа главного меню, анимация, открытие вкладок

        private void MenuButton_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is SubMenuButton) ((SubMenuButton)sender).Background = new SolidColorBrush((Color)Application.Current.Resources[key: "ColorNuans"]);
            if (sender is MainMenuButton) ((MainMenuButton)sender).Background = new SolidColorBrush((Color)Application.Current.Resources[key: "ColorSub"]);
        }
        private void MenuButton_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is SubMenuButton) ((SubMenuButton)sender).Background = new SolidColorBrush((Color)Application.Current.Resources[key: "ColorSub"]);
            if (sender is MainMenuButton) if (((MainMenuButton)sender).Checked != true) ((MainMenuButton)sender).Background = new SolidColorBrush((Color)Application.Current.Resources[key: "ColorMain"]);
        }
        private void MenuButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            wav.Stream = Properties.Resources.ding; wav.Play();           
        }

        //Получает и выводит текст из Tag в трей программы
        private void Object_MouseEnter(object sender, MouseEventArgs e)
        {            
            string sm = "";string sm2 = "";            //string preType = "System.Windows.Controls.";
            string objType = sender.GetType().ToString().Substring(sender.GetType().ToString().LastIndexOf(".")+1);
            switch (objType)
            {                
                case "Button":
                    Button but = (Button)sender;
                    if (but.Tag!=null)
                        sm = but.Tag.ToString();
                    break;
                case "CheckBox":
                    CheckBox chb = (CheckBox)sender;
                    if (chb.Tag != null)
                        sm = chb.Tag.ToString();
                    break;
                case "ComboBox":
                    ComboBox cbx = (ComboBox)sender;
                    if (cbx.Tag != null)
                        sm = cbx.Tag.ToString();
                    break;
                case "RadioButton":
                    RadioButton rbt = (RadioButton)sender;
                    if (rbt.Tag != null)
                        sm = rbt.Tag.ToString();
                    break;
                case "TabItem":
                    TabItem tbi = (TabItem)sender;
                    if (tbi.Tag != null)
                        sm = tbi.Tag.ToString();
                    break;
                case "TextBox":
                    TextBox tbx = (TextBox)sender;
                    if (tbx.Tag != null)
                        sm = tbx.Tag.ToString();
                    break;
                default:
                    sm = sender.GetType().ToString();
                    sm2 = objType;
                    break;
            }
            //sm = sender.GetType().ToString();
            new UI().FooterPromtShow(this, sm,sm2);
        }
        private void Object_MouseLeave(object sender, MouseEventArgs e)
        {
            string sm = "";
            new UI().FooterPromtShow(this, sm);
        }

        /// <summary>
        /// Выводит сообщение в файл LOG.txt и в окно MessageBox
        /// </summary>
        /// <param name = "m" >Значение для вывода</param >
        /// <param name = "newLine" >добавляет в конце строки символ новой (true по умолчанию)</param >
        /// <param name = "showMB" >выводит сообщение в MessageBox (false по умолчанию)</param >
        /// <returns>Ничего не возвращает</returns>
        public static void LOG(object m, bool showMB = false, bool newLine = true)//static
        {
            string sm = m.ToString();
            if (newLine) { sm += Environment.NewLine; }
            File.AppendAllText("LOG.txt", sm);
            if (showMB) { MessageBox.Show(sm); }
            //return;
        }

    }
}