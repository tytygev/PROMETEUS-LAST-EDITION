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
        private Grid currentVisibleView;
        public static DefUserSettings UserSettings = new DefUserSettings();//static
        private bool flagInitEnd = false;

        public static List<List<string>> cars;
        public static List<List<string>> companies;
        public static List<List<string>> drivers;
        public static List<List<string>> medical;
        public static List<List<string>> staff;
              
        public string dbDefPath = "db.xml";

        public MainWindow()
        {
            File.Delete("LOG.txt");

            bool flag = new UI().InitializeDefaultTheme(); 
            LOG(">>> Применение словаря ресурсов по умолчанию - " + flag.ToString());

            string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name.Split('\\')[1];//имя юзера без домена!
            new SettingsFX().ParsingUserSettings(new SettingsFX().LoadSettingsStringCollect(Properties.Settings.Default.UserSettings, userName));//чтение и парсинг параметров

            flag= new UI().InitializeUserSettingsTheme((int)UserSettings["ThemeComboBox"]);
            if(flag) LOG(">>> Тема изменена");
            else LOG("ОШИБКА::: Значение параметра SettingsThemeComboBox не соответствует ни одному значению в перечеслителе Themes. Настройки темы не были изменены", true);

            InitializeComponent();
            InitializeButtons();
            
            //загрузка БД
            if (new DBMS().XMLloadManager(dbDefPath))
            {
                new DBMS().XMLbackup(dbDefPath);
                LOG(">>> Загрузка из файла "+ dbDefPath+" прошла успешно. Бекап перезаписан.");
                new DBMS().XMLInfoExtractor(this, File.ReadAllText(dbDefPath),true);
            }
            else
            {
                new DBMS().XMLloadManager("backup_db.xml");
                LOG(">>> Загрузка из файла " + dbDefPath + " не удалась. Загружены данные из файла бекапа.");
                new DBMS().XMLInfoExtractor(this, File.ReadAllText("backup_db.xml"),false);
                dbDefPath = "backup_db.xml";
            }

            //Завершение загрузки
            wav = new SoundPlayer();
            currentVisibleView = StartPage;//
            new UI().VersionIngect(this);//Получение информации о версии программы           
            new UI().FooterPromtShow(this,"Программа загружена и готова к работе");
            LOG("< < < Программа загружена и готова к работе > > >");
            flagInitEnd = true;
            
        }

        



        //установка значений свойств объектов в соотвествии с настройками
        private void SettingsInitialized(object sender, EventArgs e)
        {
            LOG(">>> Инициализация объекта типа: " + sender.GetType().ToString());
            switch (sender.GetType().ToString().Substring(sender.GetType().ToString().LastIndexOf('.') + 1))
            {
                case "ComboBox":
                    LOG("\t объект типа " + sender.GetType().ToString().Substring(sender.GetType().ToString().LastIndexOf('.') + 1));
                    ComboBox cb = sender as ComboBox;
                    LOG("\t поле Name = " + cb.Name);
                    cb.SelectedIndex = Convert.ToInt32(UserSettings[cb.Name]);
                    LOG("\t полю SelectedIndex присвоено значение: " + Convert.ToString(UserSettings[cb.Name]) + " из объекта UserSettings");
                    break;
                case "TextBox":
                    LOG("\t объект типа " + sender.GetType().ToString().Substring(sender.GetType().ToString().LastIndexOf('.') + 1));
                    TextBox tb = sender as TextBox;
                    LOG("\t поле Name = " + tb.Name);
                    tb.Text = Convert.ToString(UserSettings[tb.Name]);
                    LOG("\t полю Text присвоено значение: " + Convert.ToString(UserSettings[tb.Name]) + " из объекта UserSettings");
                    break;
                case "CheckBox":
                    LOG("\t объект типа " + sender.GetType().ToString().Substring(sender.GetType().ToString().LastIndexOf('.') + 1));
                    CheckBox chb = sender as CheckBox;
                    LOG("\t поле Name = " + chb.Name);
                    chb.IsChecked = Convert.ToBoolean(UserSettings[chb.Name]);
                    LOG("\t полю IsChecked присвоено значение: " + Convert.ToString(UserSettings[chb.Name]) + " из объекта UserSettings");
                    break;
                case "RadioButton":
                    LOG("\t объект типа " + sender.GetType().ToString().Substring(sender.GetType().ToString().LastIndexOf('.') + 1));
                    RadioButton rb = sender as RadioButton;
                    LOG("\t поле Name = " + rb.Name);
                    rb.IsChecked = Convert.ToBoolean(UserSettings[rb.Name]);
                    LOG("\t полю IsChecked присвоено значение: " + Convert.ToString(UserSettings[rb.Name]) + " из объекта UserSettings");
                    break;
                default:
                    LOG("ОШИБКА::: неизвестный тип объекта!");
                    break;
            }
        }
        private void CheckChange(object sender, EventArgs e)
        {
            if (flagInitEnd)
            {
                string typeElem = sender.GetType().ToString().Substring(sender.GetType().ToString().LastIndexOf('.') + 1);

                LOG("!!! СОБЫТИЕ Checked/UnChecked на объекте " + typeElem + ": ");
                switch (typeElem)
                {
                    case "RadioButton":
                        RadioButton rb = sender as RadioButton;
                        UserSettings[rb.Name] = rb.IsChecked;
                        LOG("\tПолю " + rb.Name + " класса UserSettings присвоено значение " + Convert.ToString(rb.IsChecked));
                        break;
                    case "CheckBox":
                        CheckBox chb = sender as CheckBox;
                        UserSettings[chb.Name] = chb.IsChecked;
                        LOG("\tПолю " + chb.Name + " класса UserSettings присвоено значение " + Convert.ToString(chb.IsChecked));
                        break;
                    case "TextBox":
                        TextBox tb = sender as TextBox;
                        UserSettings[tb.Name] = tb.Text;
                        LOG("\tПолю " + tb.Name + " класса UserSettings присвоено значение " + Convert.ToString(tb.Text));
                        break;
                    case "ComboBox":
                        ComboBox cb = sender as ComboBox;
                        UserSettings[cb.Name] = cb.SelectedIndex;
                        LOG("\tПолю " + cb.Name + " класса UserSettings присвоено значение " + Convert.ToString(cb.SelectedIndex));

                        //if (cb.Name== "ThemeComboBox")
                        //{

                        //    bool flag = new UI().InitializeUserSettingsTheme((int)UserSettings["ThemeComboBox"]);
                        //    if (flag) LOG(">>> Тема изменена");
                        //    else LOG("ОШИБКА::: Значение параметра SettingsThemeComboBox не соответствует ни одному значению в перечеслителе Themes. Настройки темы не были изменены", true);
                        //}
                        break;
                    default:
                        //код, выполняемый если выражение не имеет ни одно из выше указанных значений
                        LOG("ОШИБКА::: не известный тип объекта", true);
                        break;
                }
            }
        }
        private void NoShowStartPageCheckBox2_CheckedUnChecked(object sender, RoutedEventArgs e)
        {
            if (flagInitEnd)
            {
                LOG("!!! СОБЫТИЕ Checked/UnChecked на объекте NoShowStartPageCheckBox2: ");
                NoShowStartPageCheckBox.IsChecked = NoShowStartPageCheckBox2.IsChecked;
                UserSettings["NoShowStartPageCheckBox"] = NoShowStartPageCheckBox2.IsChecked;
                LOG("\tПолю NoShowStartPageCheckBox класса UserSettings присвоено значение " + Convert.ToString(NoShowStartPageCheckBox2.IsChecked));
                //Type myTypeA = typeof(CheckBox);
                //FieldInfo myFieldInfo = typeof(CheckBox).GetField("NoShowStartPageCheckBox");
                //CheckChange(myFieldInfo, e);
            }
        }
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
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            new UI().ViewPageInitVisible(this); //перечислить все странички и выставить им невидимость
            //переопределение свойств объектов (в соответствии с настройками) после инициализации            
            bool flag = new UI().SetValUserSettings(this);//listSettingsOfUser в первой версии передавался как параметр
            LOG(">>> Свойства окна переназначены из свойств класса UserSettings - " + flag.ToString());
            new GlobalSettings().LoadGeneralProperties(this);
        }
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            //не помню чё это и зачем
            //параметр="{Binding Source={x:Static p:Settings.Default}, Path=параметр, Mode=TwoWay}"
            //SettingsBindableAttribute.Default.Save();

            //Сохранение параметров при закрытии
            new GlobalSettings().UploadGeneralProperties(this);
            string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name.Split('\\')[1];//имя юзера без домена!
            //string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name; //имя юзера с доменом!
            bool flag = new SettingsFX().SaveUserSettings(new SettingsFX().CollectingUserSettings(), userName); // ответ от SettingsFX
            LOG(">>> Настройки записаны? - " + flag.ToString());
            base.OnClosing(e);
        }

        private void ResetUserSettingsProp(object sender, RoutedEventArgs e) //Очистка параметра с настройками юзера
        {
            if (MessageBox.Show("Это действие полностью и безвозвратно аннигилирует ВСЕ данные пользователей касательно настроек программы. \n\nВы уверены???",
                "АХТУНГ!", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                new SettingsFX().SettingsClear();

            }
        }
        private void ResetGlobalProperties(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Это действие сбросит глобальные настройки программы на значения по умолчанию. \n\nВы уверены???",
               "АХТУНГ!", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                new GlobalSettings().ResetGeneralProperties(this);
                LOG("Глобальные настройки сброшены. Перезагрузите программу.", true);
            }
        }
        private void Button_ClickUserDef(object sender, RoutedEventArgs e)
        {
            new SettingsFX().SetupDefaultUser();
            InitializeComponent();
            InitializeButtons();
            OnInitialized(e);
            new UI().ViewPageInitVisible(this);
            StartPage.Visibility = Visibility.Hidden;
            SettingsPage.Visibility = Visibility.Visible;
        }
        private void Button_ClickCloseStart(object sender, RoutedEventArgs e) { this.StartPage.Visibility = Visibility.Hidden; }
        private void FullInfoVersionShow(object sender, RoutedEventArgs e) { if (FullInfoVersionTextBlock.Visibility == Visibility.Collapsed) FullInfoVersionTextBlock.Visibility = Visibility.Visible; else FullInfoVersionTextBlock.Visibility = Visibility.Collapsed; }
        private void FullInfoEditRulesShow(object sender, RoutedEventArgs e) { if (FullInfoEditRulesTextBlock.Visibility == Visibility.Collapsed) FullInfoEditRulesTextBlock.Visibility = Visibility.Visible; else FullInfoEditRulesTextBlock.Visibility = Visibility.Collapsed; }



        //////  //////  //////    //    //
        //      //  //  //  //  //  //  //
        ////    //  //  ////    //  //  //
        //      //  //  ////    //////  //
        //      //////  //  //  //  //  //////  //foral

        //наработки к старой бд
        //работа меню
        private void OpenDB(object sender, RoutedEventArgs e)
        {
            //new DBMS().CreateDataGrid(this, new DBMS().ParseDB(this, 0));
            MenuItem menuI = sender as MenuItem;
            int file=100; bool flag = true;
            switch (menuI.Header.ToString())
            {
                case "Авто":
                    file = 0;
                    break;
                case "Водители":
                    file = 1;
                    break;
                case "Компании":
                    file = 2;
                    break;
                case "Медицина":
                    file = 3;
                    break;
                case "Персонал":
                    file = 4;
                    break;
                default:
                    flag = false;
                    break;                   
            }
           
        }
        //скроллинг
        private void ScrollChanged(object sender, ScrollChangedEventArgs e)
        {

            //DataGridHeaderScroll.ScrollToVerticalOffset(e.VerticalOffset);
            DataGridHeaderScroll.ScrollToHorizontalOffset(e.HorizontalOffset);
           
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

      

        private void OpenDBinExcel(object sender, RoutedEventArgs e)
        {
            new UI().FooterPromtShow(this, "Открытие Excel...");
            new FileFX().OpenDBinExcel(System.IO.Directory.GetCurrentDirectory() + "\\" +dbDefPath);
            //Application.Current.Shutdown();
        }
    }




}
