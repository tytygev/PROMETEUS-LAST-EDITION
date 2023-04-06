using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Office.Interop;
using Excel = Microsoft.Office.Interop.Excel;

namespace PROMETEUS_LAST_EDITION
{
  

    enum MainMenuButtonsEnum
    {
        KitSetButton,
        PriceButton,
        DBEditButton,
        SettingsButton,
        AboutButton,
        ExitButton
    }
    enum ViewPagesEnum
    {
        KitSetPage,
        PricePage,
        DBEditPage,
        SettingsPage,
        AboutPage,
        StartPage
    }

    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
       
        //Сохранение параметров
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            //не помню чё это и зачем
            //параметр="{Binding Source={x:Static p:Settings.Default}, Path=параметр, Mode=TwoWay}"
            //SettingsBindableAttribute.Default.Save();


            //это  просто для проверки
                        List<string> listUS = new List<string> { System.Security.Principal.WindowsIdentity.GetCurrent().Name.Split('\\')[1], themeCB.IsChecked.ToString(), "false" };
                        bool a = SettingsFX.SaveUserSettings(System.Security.Principal.WindowsIdentity.GetCurrent().Name.Split('\\')[1], listUS);
                        MessageBox.Show("Настройки записаны? - "+a.ToString());
            //

            base.OnClosing(e);
        }
        
        private SoundPlayer wav;
        private Grid currentVisibleView;

        public MainWindow()
        {

           
            InitializeDefaultSettings();

            InitializeComponent();           
            


            InitializeButtons();

            wav = new SoundPlayer();           
             currentVisibleView = StartPage;//

            

            //пример использования функции сохранения и загрузки:
            //List<List<string>> listOfLists = new List<List<string>>();
            //listOfLists=FileFX.LoadDSV("prop.txt", char.Parse(";"));
            //listOfLists[1][1] = "эщкере";
            //FileFX.SaveDSV(listOfLists,"prop.txt", char.Parse(";"));//изикатка       

        }
        private void InitializeDefaultSettings()
        {

            //проверочка
            List<string> listSettingsOfUser = SettingsFX.LoadUserSettings(System.Security.Principal.WindowsIdentity.GetCurrent().Name.Split('\\')[1]);
            // string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name.Split('\\')[1]; //имя юзера без домена!
            // string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name; 
            
            MessageBox.Show("Настройки прочитаны: "+listSettingsOfUser[0] + ", " + listSettingsOfUser[1] + ", " + listSettingsOfUser[2]);
            bool flag;


            //сделать это полюбому для умолчания и похуй на настройки
            ResourceDictionary dictZ = new ResourceDictionary();
            dictZ.Source = new Uri("DictionaryDarkTheme.xaml", UriKind.Relative);
            Application.Current.Resources.MergedDictionaries.Add(dictZ);

            //listSettingsOfUser[1] = "true";

            //применение словарей ресурсов
            if (Boolean.TryParse(listSettingsOfUser[1], out flag) & flag)
            {
                // https://stackoverflow.com/questions/786183/wpf-changing-resources-colors-from-the-app-xaml-during-runtime
                ResourceDictionary dict = new ResourceDictionary();
                dict.Source = new Uri("DictionaryDarkTheme.xaml", UriKind.Relative);
                Application.Current.Resources.MergedDictionaries.Add(dict);
                
            }
            else
            {
                 // https://stackoverflow.com/questions/786183/wpf-changing-resources-colors-from-the-app-xaml-during-runtime
                ResourceDictionary dict = new ResourceDictionary();
                dict.Source = new Uri("DictionaryLightTheme.xaml", UriKind.Relative);
                Application.Current.Resources.MergedDictionaries.Add(dict);
//                Имейте в виду, что MergedDictionaries — это контейнер. Ресурс в самом последнем добавленном ResourceDictionary выигрывает.
//                Если намерение состоит в том, чтобы переключаться между словарями с какой-либо регулярностью, удаление предыдущего словаря
//                из списка может быть полезным. (В ответе упоминается «выгрузить его в коде» по умолчанию, указанному в XAML, но не показано,
//                как его идентифицировать и удалить.) 


            }
         //   (FindResource("documentTemplates") as System.Windows.Data.ObjectDataProvider).Refresh();

        }
        

        private void InitializeButtons()
        {
            KitSetButton.MouseUp += (s, e) => ShowView(KitSetPage);
            PriceButton.MouseUp += (s, e) => ShowView(PricePage);
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

        private void ShowView(Grid view)
        {
            if (view == currentVisibleView)
                return;
            if (currentVisibleView != null)
                currentVisibleView.Visibility = Visibility.Hidden;
            view.Visibility = Visibility.Visible;
            currentVisibleView = view;
        }
        private void MenuButton_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is SubMenuButton ) ((SubMenuButton)sender).Background = new SolidColorBrush((Color)Application.Current.Resources[key: "ColorNuans"]);
            if (sender is MainMenuButton) ((MainMenuButton)sender).Background = new SolidColorBrush((Color)Application.Current.Resources[key: "ColorSub"]);
        }
        private void MenuButton_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is SubMenuButton) ((SubMenuButton)sender).Background = new SolidColorBrush((Color)Application.Current.Resources[key: "ColorSub"]);
            if (sender is MainMenuButton) if (((MainMenuButton)sender).Checked !=true) ((MainMenuButton)sender).Background = new SolidColorBrush((Color)Application.Current.Resources[key: "ColorMain"] );
        }
        private void MenuButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            //
        }



        

     






        //эксперименты с такси
        private void InitializeTaxiSettings()
        {// habr.com/ru/post/271483/
            var DSettingsTaxiComboBoxes = DSettingsTaxiGrid.Children.OfType<ComboBox>().ToList(); //Все элементы типа ComboBox в таблице выбора идентефикации ячеек
            for (int i = 0; i < DSettingsTaxiComboBoxes.Count; i++)
            {
                DSettingsTaxiComboBoxes[i].SelectedIndex = Int32.Parse(Properties.Settings.Default.report_parser_setsel[i]);//установка значений из DefaultSettings
            }
            var DSettingsTaxiTextBoxes = DSettingsTaxiGrid.Children.OfType<TextBox>().ToList(); //Все элементы типаTextBox в таблице выбора идентефикации ячеек
            for (int i = 0; i < DSettingsTaxiTextBoxes.Count; i++)
            {
                DSettingsTaxiTextBoxes[i].Text = Properties.Settings.Default.report_parser_setval[i];//установка значений из DefaultSettings
            }

        }
        private void TaxiComboBoxesSetSel (object sender, SelectionChangedEventArgs e)
        {
            var DSettingsTaxiComboBoxes = DSettingsTaxiGrid.Children.OfType<ComboBox>().ToList(); //Все элементы типа ComboBox в таблице выбора идентефикации ячеек
            for (int i = 0; i < DSettingsTaxiComboBoxes.Count; i++)
            {
                if (ReferenceEquals(sender, DSettingsTaxiComboBoxes[i]))//Если есть совпадение
                {
                    Properties.Settings.Default.report_parser_setsel[i] = DSettingsTaxiComboBoxes[i].SelectedIndex.ToString();//Записываем индекс выбранного пункта в параметр
                     Properties.Settings.Default.Save(); // Сохраняем переменные.
                }
                
            }

        }
        private void TaxiTextBoxesSetVal(object sender, TextChangedEventArgs e)
        {
            var DSettingsTaxiTextBoxes = DSettingsTaxiGrid.Children.OfType<TextBox>().ToList(); //Все элементы типа TextBox в таблице выбора идентефикации ячеек
            for (int i = 0; i < DSettingsTaxiTextBoxes.Count; i++)
            {
                if (ReferenceEquals(sender, DSettingsTaxiTextBoxes[i]))//Если есть совпадение
                {
                    Properties.Settings.Default.report_parser_setval[i] = DSettingsTaxiTextBoxes[i].Text;//Записываем значение выбранного пункта в параметр
                    Properties.Settings.Default.Save(); // Сохраняем переменные.
                }

            }

        }
        private void droppanel_Drop(object sender, DragEventArgs e)
        {
            string xlFileName = TaxiAnalyzer.GetReportFileName(e); //получение имени файла
            dropfilelabel.Text = xlFileName;//вывод имени файла в label
            //Передача имени вайла на загрузку и анализ
            //TaxiAnalyzer.Taxichecksumm(dataArr);
            //TaxiAnalyzer.Filtration(TaxiAnalyzer.LoadReport(xlFileName));
        }
        private void runchecksummButton_Click(object sender, KeyEventArgs e)
        {

        }

        private void KitSetButton_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
   



}
