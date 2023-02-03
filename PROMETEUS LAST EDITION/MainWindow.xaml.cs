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
            //параметр="{Binding Source={x:Static p:Settings.Default}, Path=параметр, Mode=TwoWay}"
            //SettingsBindableAttribute.Default.Save();
            base.OnClosing(e);
        }
        
        private SoundPlayer wav;
        private Grid currentVisibleView;

        public MainWindow()
        {
            InitializeComponent();
            InitializeButtons();
            InitializeDefaultSettings();

            wav = new SoundPlayer();
            wav.Stream = Properties.Resources.ding;

            currentVisibleView = StartPage;//

            //пример использования функции сохранения и загрузки:
            //List<List<string>> listOfLists = new List<List<string>>();
            //listOfLists=FileFX.LoadDSV("prop.txt", char.Parse(";"));
            //listOfLists[1][1] = "эщкере";
            //FileFX.SaveDSV(listOfLists,"prop.txt", char.Parse(";"));//изикатка

           


        }

        private void InitializeDefaultSettings()
        {// habr.com/ru/post/271483/
            var DSettingsTaxiComboBoxes = DSettingsTaxiGrid.Children.OfType<ComboBox>().ToList(); //Все элементы типа ComboBox в таблице выбора идентефикации ячеек
           for (int i = 0; i< DSettingsTaxiComboBoxes.Count; i++)
            {
                           DSettingsTaxiComboBoxes[i].SelectedIndex = Int32.Parse(Properties.Settings.Default.report_parser_setsel[i]);//установка значений из DefaultSettings
                            }
            var DSettingsTaxiTextBoxes = DSettingsTaxiGrid.Children.OfType<TextBox>().ToList(); //Все элементы типаTextBox в таблице выбора идентефикации ячеек
            for (int i = 0; i < DSettingsTaxiTextBoxes.Count; i++)
            {
                DSettingsTaxiTextBoxes[i].Text = Properties.Settings.Default.report_parser_setval[i];//установка значений из DefaultSettings
            }



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
            //if (sender is MainMenuButton ) ((MainMenuButton)sender).Background = new SolidColorBrush((Color)Application.Current.Resources[key: "ColorSub"]);
            //else ((SubMenuButton)sender).Background = new SolidColorBrush((Color)Application.Current.Resources[key: "ColorNuans"]);
        }
        private void MenuButton_MouseLeave(object sender, MouseEventArgs e)
        {
            //if (sender is MainMenuButton) ((MainMenuButton)sender).Background = new SolidColorBrush((Color)Application.Current.Resources[key: "ColorMain"]);
            //else ((SubMenuButton)sender).Background = new SolidColorBrush((Color)Application.Current.Resources[key: "ColorSub"]);
        }
        private void MenuButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            //
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

      
    }
   



}
