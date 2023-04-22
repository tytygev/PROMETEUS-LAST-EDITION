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

using System.IO;
using System.Collections;

namespace PROMETEUS_LAST_EDITION
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public SoundPlayer wav = new SoundPlayer(); //тупо эксперименты
        private Grid currentVisibleView; //это что то для такси (уже не помню)
        public static DefUserSettings UserSettings = new DefUserSettings();
        
       public MainWindow()
        {
            File.Delete("LOG.txt");

            bool flag = new UI().InitializeDefaultTheme(); 
            LOG(">>> Применение словаря ресурсов по умолчанию - " + flag.ToString());

            string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name.Split('\\')[1];//имя юзера без домена!
            new SettingsFX().ParsingUserSettings(new SettingsFX().LoadSettingsStringCollect(Properties.Settings.Default.UserSettings, userName));//чтение и парсинг параметров

            flag= new UI().InitializeUserSettingsTheme(DefUserSettings.ThemeComboBox);
            if(flag) LOG(">>> Тема изменена");
            else LOG("ОШИБКА::: Значение параметра SettingsThemeComboBox не соответствует ни одному значению в перечеслителе Themes. Настройки темы не были изменены", true);

            InitializeComponent();
            InitializeButtons();

            wav = new SoundPlayer();
            currentVisibleView = StartPage;//

            //пример использования функции сохранения и загрузки БД:
            //List<List<string>> listOfLists = new List<List<string>>();
            //listOfLists=FileFX.LoadDSV("prop.txt", char.Parse(";"));
            //listOfLists[1][1] = "эщкере";
            //FileFX.SaveDSV(listOfLists,"prop.txt", char.Parse(";"));//изикатка       

            new UI().FooterPromtShow(this,"Программа загружена и готова к работе");
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
            wav.Stream = Properties.Resources.ding; wav.Play();
            //
        }
        //установка значений свойств объектов в соотвествии с настройками
        private void SettingsInitialized(object sender, EventArgs e)
        {
            LOG(">>> Инициализация объекта типа: " + sender.GetType().ToString());
            switch (sender.GetType().ToString().Substring(sender.GetType().ToString().LastIndexOf('.') + 1))
            {
                case "ComboBox":
                    ComboBox cb = sender as ComboBox;
                    cb.SelectedIndex = Convert.ToInt32(UserSettings[cb.Name]);
                    LOG("\t объект типа " + sender.GetType().ToString().Substring(sender.GetType().ToString().LastIndexOf('.') + 1));
                    LOG("\t поле Name = " + cb.Name);
                    LOG("\t полю SelectedIndex присвоено значение: " + Convert.ToString(UserSettings[cb.Name]) + " из объекта UserSettings");
                    break;
                case "TextBox":
                    TextBox tb = sender as TextBox;
                    tb.Text = Convert.ToString(UserSettings[tb.Name]);
                    LOG("\t объект типа " + sender.GetType().ToString().Substring(sender.GetType().ToString().LastIndexOf('.') + 1));
                    LOG("\t поле Name = " + tb.Name);
                    LOG("\t полю Text присвоено значение: " + Convert.ToString(UserSettings[tb.Name]) + " из объекта UserSettings");
                    break;
                case "CheckBox":
                    CheckBox chb = sender as CheckBox;
                    chb.IsChecked = Convert.ToBoolean(UserSettings[chb.Name]);
                    LOG("\t объект типа " + sender.GetType().ToString().Substring(sender.GetType().ToString().LastIndexOf('.') + 1));
                    LOG("\t поле Name = " + chb.Name);
                    LOG("\t полю IsChecked присвоено значение: " + Convert.ToString(UserSettings[chb.Name]) + " из объекта UserSettings");
                    break;
                case "RadioButton":
                    RadioButton rb = sender as RadioButton;
                    rb.IsChecked = Convert.ToBoolean(UserSettings[rb.Name]);
                    LOG("\t объект типа " + sender.GetType().ToString().Substring(sender.GetType().ToString().LastIndexOf('.') + 1));
                    LOG("\t поле Name = " + rb.Name);
                    LOG("\t полю IsChecked присвоено значение: " + Convert.ToString(UserSettings[rb.Name]) + " из объекта UserSettings");
                    break;
                default:
                    LOG("ОШИБКА::: неизвестный тип объекта!");
                    break;
            }
        }
        private void CheckChange(object sender, EventArgs e)
        {
            string typeElem = sender.GetType().ToString().Substring(sender.GetType().ToString().LastIndexOf('.') + 1);
            switch (typeElem)
            {
                case "RadioButton":
                    RadioButton rb = sender as RadioButton;
                    UserSettings[rb.Name] = rb.IsChecked;
                    LOG("Объекту "+ rb.Name+" присвоено значение " + Convert.ToString(rb.IsChecked));
                    break;
                case "CheckBox":
                    CheckBox chb = sender as CheckBox;
                    UserSettings[chb.Name] = chb.IsChecked;
                    LOG("Объекту " + chb.Name + " присвоено значение " + Convert.ToString(chb.IsChecked));
                    break;
                case "TextBox":
                    TextBox tb = sender as TextBox;
                    UserSettings[tb.Name] = tb.Text;
                    LOG("Объекту " + tb.Name + " присвоено значение " + Convert.ToString(tb.Text));
                    break;
                case "ComboBox":
                    ComboBox cb = sender as ComboBox;
                    UserSettings[cb.Name] = cb.SelectedIndex;
                    LOG("Объекту " + cb.Name + " присвоено значение " + Convert.ToString(cb.SelectedIndex));
                    break;
                default:
                    //код, выполняемый если выражение не имеет ни одно из выше указанных значений
                    LOG("ОШИБКА::: не известный тип объекта", true);
                    break;
            }
            
        }
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e) {
            if ((bool)UserSettings["SaveWinSizeCheckBox"])
            {
                UserSettings["WindowSizeW"] = Convert.ToInt32(this.Width);
                UserSettings["WindowSizeH"] = Convert.ToInt32(this.Height);
                LOG("~~~ Поле UserSettings.WindowSizeW = " + Convert.ToString(this.Width));
                LOG("~~~ Поле UserSettings.WindowSizeH = " + Convert.ToString(this.Height));
            }
        }
        private void Window_StateChanged(object sender, EventArgs e) { if ((bool)UserSettings["SaveWinSizeCheckBox"]) { UserSettings["WindowState"] = this.WindowState; LOG("~~~ Поле UserSettings.WindowState = " + Convert.ToString(this.WindowState)); } }
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            new UI().ViewPageInitVisible(this); //перечислить все странички и выставить им невидимость
            //переопределение свойств объектов (в соответствии с настройками) после инициализации            
            bool flag = new UI().SetValUserSettings(this);//listSettingsOfUser в первой версии передавался как параметр
            LOG(">>> Свойства окна переназначены из свойств класса UserSettings - " + flag.ToString());
        }
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            //не помню чё это и зачем
            //параметр="{Binding Source={x:Static p:Settings.Default}, Path=параметр, Mode=TwoWay}"
            //SettingsBindableAttribute.Default.Save();

            //Сохранение параметров при закрытии
            string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name.Split('\\')[1];//имя юзера без домена!
            //string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name; //имя юзера с доменом!
            bool flag = SettingsFX.SaveUserSettings(new SettingsFX().CollectingUserSettings(), userName); // ответ от SettingsFX
            LOG(">>> Настройки записаны? - " + flag.ToString());
            base.OnClosing(e);
        }

        
        //избавиться от этого ужаса в два приёма:
        //1-
        //Список формируется в классе SettingsFX 
        //значения парсятся из класса UserSettings
        //2-
        //актуализация значений полей класса UserSettings
        //происходит по событиям Changed и подобным
        //
        //Но пока оно работает и так зае....

        //Добавить кнопку сброс - приравнивание UserSettings из DefUserSettings    

       

        private void Button_ClickReset(object sender, RoutedEventArgs e) //Очистка параметра с настройками юзера
        {
            if (MessageBox.Show("Это действие полностью и безвозвратно аннигилирует ВСЕ данные пользователей касательно настроек программы. \n\nВы уверены???",
                "АХТУНГ!", MessageBoxButton.YesNo,MessageBoxImage.Warning) == MessageBoxResult.Yes){new SettingsFX().SettingsClear();}
        }
        private void Button_ClickUserDef(object sender, RoutedEventArgs e) {new SettingsFX().SetupDefaultUser();}
        private void Button_ClickCloseStart(object sender, RoutedEventArgs e) {this.StartPage.Visibility = Visibility.Hidden;}

        /// <summary>
        /// Выводит сообщение в файл LOG.txt и в окно MessageBox
        /// </summary>
        /// <param name = "m" >Значение для вывода</param >
        /// <param name = "newLine" >добавляет в конце строки символ новой (true по умолчанию)</param >
        /// <param name = "showMB" >выводит сообщение в MessageBox (false по умолчанию)</param >
        /// <returns>Ничего не возвращает</returns>
        public static void LOG(object m, bool showMB = false, bool newLine = true)//
        {
            string sm = m.ToString();
            if (newLine) { sm += Environment.NewLine; }
            File.AppendAllText("LOG.txt", sm);           
            if (showMB) { MessageBox.Show(sm); }
            //return;
        }


        //////  //////  //////    //    //
        //      //  //  //  //  //  //  //
        ////    //  //  ////    //  //  //
        //      //  //  ////    //////  //
        //      //////  //  //  //  //  //////  //foral

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
        private void TaxiComboBoxesSetSel(object sender, SelectionChangedEventArgs e)
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
        private void Droppanel_Drop(object sender, DragEventArgs e)
        {
            string xlFileName = TaxiAnalyzer.GetReportFileName(e); //получение имени файла
            dropfilelabel.Text = xlFileName;//вывод имени файла в label
            //Передача имени вайла на загрузку и анализ
            //TaxiAnalyzer.Taxichecksumm(dataArr);
            //TaxiAnalyzer.Filtration(TaxiAnalyzer.LoadReport(xlFileName));
        }
        private void RunchecksummButton_Click(object sender, KeyEventArgs e)
        {

        }

        //экспериметы с поиском детей
        /// <summary>
        /// Возвращает все элементы UIElement заданного типа.
        /// </summary>
        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if ((depObj != null))
            {
                for (int i = 0; i <= VisualTreeHelper.GetChildrenCount(depObj) - 1; i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if ((child != null) && (child is T))
                        yield return (T)((object)child);
                    foreach (T current in MainWindow.FindVisualChildren<T>(child))
                        yield return current;
                }
            }
            yield break;
        }
        private List<string> GrabAllSettingsVal()//Возвращает список отдельных свойств объектов 4х фиксированных типов, 
        //имена которых перечислены в SettingsElem 
        //использует визуальное дерево потомков 
        //пока никак не используется
        {
            int a = 0;
            List<string> ListSettings = new List<string>();//Список значений всех элементов
            int count = TabControlSettings.Items.Count;
            for (int y = 0; y < count; y++)
            {
                TabControlSettings.SelectedIndex = y;
                LOG(">>> TabControlSettings.SelectedIndex=" + y.ToString());
                // List<RadioButton> radioButtonElements = new List<RadioButton>();//радиокнопки
                foreach (RadioButton rb in FindVisualChildren<RadioButton>(TabControlSettings)) { ListSettings.Add(rb.IsChecked.ToString()); a++; LOG("Найден " + a.ToString() + "-й элемент типа RADIOBUTTON со значением " + rb.IsChecked.ToString()); }//родительский контейнер
                                                                                                                                                                                                                                                          // List<CheckBox> checkBoxElements = new List<CheckBox>();  //CheckBox
                foreach (CheckBox chb in FindVisualChildren<CheckBox>(TabControlSettings)) { ListSettings.Add(chb.IsChecked.ToString()); a++; LOG("Найден " + a.ToString() + "-й элемент типа CHECKBOX со значением " + chb.IsChecked.ToString()); }//родительский контейнер
                                                                                                                                                                                                                                                    // List<TextBox> textBoxElements = new List<TextBox>();  //TextBox
                foreach (TextBox tb in FindVisualChildren<TextBox>(TabControlSettings)) { ListSettings.Add(tb.Text); a++; LOG("Найден " + a.ToString() + "-й элемент типа TEXTBOX со значением " + tb.Text); }//родительский контейнер
                                                                                                                                                                                                              // List<ComboBox> comboBoxElements = new List<ComboBox>();  //ComboBox
                foreach (ComboBox cb in FindVisualChildren<ComboBox>(TabControlSettings)) { ListSettings.Add(cb.SelectedIndex.ToString()); a++; LOG("Найден " + a.ToString() + "-й элемент типа COMBOBOX со значением " + cb.SelectedIndex.ToString()); }//родительский контейнер
            }
            return ListSettings;
        }

        
    }




}
