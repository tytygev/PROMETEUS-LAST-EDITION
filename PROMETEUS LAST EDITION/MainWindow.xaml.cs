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
    enum MainMenuButtonEnum
    {
        KitSetButton,
        PriceButton,
        DBEditButton,
        SettingsButton,
        AboutButton,
        ExitButton
    }
    enum ViewPageEnum
    {
        KitSetPage,
        PricePage,
        DBEditPage,
        SettingsPage,
        AboutPage,
        StartPage
    }

    enum Themes
    {
        DictionaryDarkTheme,
        DictionaryLightTheme,
        DictionaryLightThemeRed
    }
    enum SettingsElem//идет в связке с TypeUserSettingsElements
    {
        NoShowStartPageCheckBox,
        ThemeComboBox,
        SaveWinSizeCheckBox
    }
    

    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public SoundPlayer wav = new SoundPlayer();
        private Grid currentVisibleView;
        public DefUserSettings UserSettings = new DefUserSettings();//создаем экземпляр UserSettings объекта DefUserSettings.
                                                                    //поля заполнены по умолчанию
        
     

        /// <summary>
        /// Возвращает список отдельных свойств объектов 4х фиксированных типов, 
        /// имена которых перечислены в SettingsElem
        /// </summary>
        /// <returns>Список строк</returns>
        private List<string> PullUserSettingsVal()
        {
            List<string> ListSettings = new List<string>();//Список значений всех элементов
            var enumCount = Enum.GetNames(typeof(SettingsElem)).Length;//длинна нумератора SettingsElem, учитывающего все элементы считывающиеся для хранения их свойств
            for (int i = 0; i < enumCount; i++)
            {
                string nameElem = Enum.GetName(typeof(SettingsElem), i);
                string typeElem = this.FindName(nameElem).GetType().ToString().Substring(this.FindName(nameElem).GetType().ToString().LastIndexOf('.')+1);              
                switch (typeElem)
                {
                    case "RadioButton":
                        RadioButton rb = this.FindName(nameElem) as RadioButton;
                        if (rb != null) ListSettings.Add(rb.IsChecked.ToString());
                        break;
                    case "CheckBox":
                        CheckBox chb = this.FindName(nameElem) as CheckBox;
                        if (chb != null) ListSettings.Add(chb.IsChecked.ToString());
                        break;
                    case "TextBox":
                        TextBox tb = this.FindName(nameElem) as TextBox;
                        if (tb != null) ListSettings.Add(tb.Text);
                        break;
                    case "ComboBox":
                        ComboBox cb = this.FindName(nameElem) as ComboBox;
                        if (cb != null) ListSettings.Add(cb.SelectedIndex.ToString());
                        break;
                    default:
                        //код, выполняемый если выражение не имеет ни одно из выше указанных значений
                         break;
                }
            }
                return ListSettings;
        }

        private  List<string> GrabAllSettingsVal()//аналогично предыдущему, но не используется
        {
            int a=0;        
            List<string> ListSettings = new List<string>();//Список значений всех элементов
            int count=TabControlSettings.Items.Count;
            for (int y = 0; y < count;y++)
            {           
                TabControlSettings.SelectedIndex=y;
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

   
        public MainWindow()
        {
            File.Delete("LOG.txt");
            bool flag = InitializeDefaultSettings(); LOG("Применение словаря ресурсов по умолчанию. >>> " + flag.ToString(), true, true);

            string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name.Split('\\')[1];//имя юзера без домена!

            ParceUserSettings(SettingsFX.LoadUserSettings(userName));//чтение и парсинг параметров

            InitializeUserSettingsTheme(DefUserSettings.ThemeComboBox);
            InitializeComponent();                       
            InitializeButtons();

            wav = new SoundPlayer();
            currentVisibleView = StartPage;//

            //пример использования функции сохранения и загрузки БД:
            //List<List<string>> listOfLists = new List<List<string>>();
            //listOfLists=FileFX.LoadDSV("prop.txt", char.Parse(";"));
            //listOfLists[1][1] = "эщкере";
            //FileFX.SaveDSV(listOfLists,"prop.txt", char.Parse(";"));//изикатка       
            FooterPromtShow("Программа загружена и готова к работе");
        }

        private bool InitializeDefaultSettings()//Применение словаря ресурсов по умолчанию
        {
            ResourceDictionary dictZ = new ResourceDictionary();
            dictZ.Source = new Uri("DictionaryDarkTheme.xaml", UriKind.Relative);
            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(dictZ);
            return true;
        }

        /// <summary>
        /// Парсит список с настройками пользователя и обновляет значения свойств объекта UserSettings
        /// </summary>
        /// <param name = "SettingsList" >список настроек</param >
        /// <returns>Возвращает булево значение</returns>
        private bool ParceUserSettings(List<string> SettingsList)
        {

            int enumCount = Enum.GetNames(typeof(SettingsElem)).Length;//длинна перечеслителя
            int listCount = SettingsList.Count;//длинна массива с сохраненными параметрами
            LOG("Длинна Enum и listSettingsOfUser совпадают? - " + (enumCount == listCount).ToString());
            if (enumCount == listCount)
            {
                for (int i = 0; i < enumCount; i++)
                {
                    string nameElem = Enum.GetName(typeof(SettingsElem), i);//имя поля перечеслителя
                    
                    //string typeElem = FindName(nameElem).GetType().ToString().Substring(FindName(nameElem).GetType().ToString().LastIndexOf('.') + 1);
                    string typeElem = UserSettings[nameElem].GetType().ToString().Substring(UserSettings[nameElem].GetType().ToString().LastIndexOf('.') + 1);

                    switch (typeElem)
                    {
                        case "Boolean":                            
                            UserSettings[nameElem] = Convert.ToBoolean(SettingsList[i]);
                            LOG("Параметр " + nameElem + " = " + SettingsList[i]);                            
                            break;
                        case "String":
                                UserSettings[nameElem] = SettingsList[i];
                                LOG("Параметр " + nameElem + " = " + SettingsList[i]);                           
                            break;
                        case "Int32":                            
                                UserSettings[nameElem] = Convert.ToInt32(SettingsList[i]);
                                LOG("Параметр " + nameElem + " = " + SettingsList[i]);                            
                            break;
                        default:
                            LOG("ОШИБКА::: Неучтенный тип объекта nameElem");
                            break;

                    }

                    //сравнивает имя из перечислителя с именем объекта и в зависимости от типа последнего, конвертирует значение в нужный тип
                    //if (typeElem != null)
                    //{
                    //    switch (typeElem)
                    //    {
                    //        case "RadioButton":
                    //            RadioButton rb = this.FindName(nameElem) as RadioButton;
                    //            if (rb != null)
                    //            {
                    //                UserSettings[nameElem] = Convert.ToBoolean(SettingsList[i]);
                    //                LOG("Параметр " + nameElem + " = " + SettingsList[i].ToString());
                    //            } else { LOG("ОШИБКА::: Параметр " + nameElem + "существует, но объект с таким именем не найден"); }
                    //            break;
                    //        case "CheckBox":
                    //            CheckBox chb = this.FindName(nameElem) as CheckBox;
                    //            if (chb != null)
                    //            {
                    //                UserSettings[nameElem] = Convert.ToBoolean(SettingsList[i]);
                    //                LOG("Параметр " + nameElem + " = " + SettingsList[i].ToString());
                    //            } else { LOG("ОШИБКА::: Параметр " + nameElem + "существует, но объект с таким именем не найден"); }
                    //            break;
                    //        case "TextBox":
                    //            TextBox tb = this.FindName(nameElem) as TextBox;
                    //            if (tb != null)
                    //            {
                    //                UserSettings[nameElem] = SettingsList[i];
                    //                LOG("Параметр " + nameElem + " = " + SettingsList[i].ToString());
                    //            } else { LOG("ОШИБКА::: Параметр " + nameElem + "существует, но объект с таким именем не найден"); }
                    //            break;
                    //        case "ComboBox":
                    //            ComboBox cb = this.FindName(nameElem) as ComboBox;
                    //            if (cb != null)
                    //            {
                    //                UserSettings[nameElem] = Convert.ToInt32(SettingsList[i]);
                    //                LOG("Параметр " + nameElem + " = " + SettingsList[i].ToString());
                    //            } else { LOG("ОШИБКА::: Параметр " + nameElem + "существует, но объект с таким именем не найден"); }

                    //            break;
                    //        default:
                    //            LOG("ОШИБКА::: Неучтенный тип объекта nameElem");
                    //            break;

                    //    }
                    //} else { LOG("ОШИБКА::: объект с именем {nameElem} не найден. пременная typeElem = null"); }

                }
                LOG("Настройки считаны");
                return true;
            }
            else
            {
                LOG("КРИТИЧЕСКАЯ ОШИБКА::: длинна перечислители и длинна списка загруженных параметров отличаются", true, true);
                LOG("Будут использованы значения настроек по умолчанию");
                return false;
            }

        }

        /// <summary>
        /// Применяет словари ресурсов темы оформления в зависимости от настроек
        /// https://stackoverflow.com/questions/786183/wpf-changing-resources-colors-from-the-app-xaml-during-runtime   
        /// </summary>
        /// <param name = "param" >номер темы в перечислителе</param >
        /// <returns>Возвращает булево значение</returns>
        private bool InitializeUserSettingsTheme(int param)
        {
            bool flag = true;
            //int param = DefUserSettings.SettingsThemeComboBox;
            ResourceDictionary dict = new ResourceDictionary();
            switch (param)
            {
                case (int)Themes.DictionaryDarkTheme:                        
                    dict.Source = new Uri("DictionaryDarkTheme.xaml", UriKind.Relative);
                    flag = false;
                    break;
                case (int)Themes.DictionaryLightTheme:
                    dict.Source = new Uri("DictionaryLightTheme.xaml", UriKind.Relative);                   
                    flag = false;
                    break;
                case (int)Themes.DictionaryLightThemeRed:
                    dict.Source = new Uri("DictionaryLightThemeRed.xaml", UriKind.Relative);
                    flag = false;
                    break;
                default:
                    break;
            }
            //Имейте в виду, что MergedDictionaries — это контейнер. Ресурс в самом последнем добавленном ResourceDictionary выигрывает.
            //Если намерение состоит в том, чтобы переключаться между словарями с какой-либо регулярностью, удаление предыдущего словаря
            //из списка может быть полезным. (В ответе упоминается «выгрузить его в коде» по умолчанию, указанному в XAML, но не показано,
            //как его идентифицировать и удалить.) 
            if (flag) LOG("ОШИБКА::: Значение параметра SettingsThemeComboBox не соответствует ни одному значению в перечеслителе Themes. Настройки темы не были изменены");
            else
            {
                Application.Current.Resources.MergedDictionaries.Clear();
                Application.Current.Resources.MergedDictionaries.Add(dict);
                //(FindResource("documentTemplates") as System.Windows.Data.ObjectDataProvider).Refresh();
                LOG(">>> Тема изменена");
            }
            return !flag;
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
        /// <summary>
        /// Выводит сообщение окне программы
        /// </summary>
        /// <param name = "m" >Строка для вывода</param >
        /// <param name = "m2" >Дополнительная строка (null по умолчанию)</param >
        /// <param name = "newLine" >Дополнительная строка на второй строке (true по умолчанию)</param >
        /// <returns>Ничего не возвращает</returns>
        public void FooterPromtShow(object m, object m2=null, bool newLine = true)
        {
            string sm = m.ToString()+" ";
            if (newLine) { sm = sm + Environment.NewLine; }
            if (m2 != null) { sm=sm+ m2.ToString(); }
            TextBlock tb = this.FindName("FooterPrompt") as TextBlock;
            if (tb != null) tb.Text = sm;
        }
        //переопределение свойств объектов (в соответствии с настройками) после инициализации
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            //здесь устанавливаем свойства объектов в соотвествии с настройками
            //List<string> listSettingsOfUser = SettingsFX.LoadUserSettings(System.Security.Principal.WindowsIdentity.GetCurrent().Name.Split('\\')[1]);
            bool a = SetValUserSettings();//listSettingsOfUser в первой версии передавался как параметр
            LOG("Свойства объектов переназначены из свойств класса DefUserSettings? >>> "+a.ToString(), true,true);

            //ViewPageEnum
            //перечислить все странички и выставить им невидимость

        }

        /// <summary>
        /// Полуавтоматически парсит список отдельных свойств объектов 4х фиксированных типов, 
        /// имена которых перечислены в SettingsElem
        /// и присваивает значения соотвествующим объевктам
        /// </summary>
        /// <param name="userSettings">Список со значениями настроек пользователя</param>
        /// <returns>булево значение</returns>
        private bool SetValUserSettings()//парсинг списка в обратном порядке работает после инициализации //List<string> userSettings убрал из параметров
        {
            int enumCount = Enum.GetNames(typeof(SettingsElem)).Length; //длинна нумератора с сохраняемыми параметрами
            for (int i = 0; i < enumCount; i++)
            {
                string nameElem = Enum.GetName(typeof(SettingsElem), i);
                string typeElem = FindName(nameElem).GetType().ToString().Substring(FindName(nameElem).GetType().ToString().LastIndexOf('.') + 1);
                if (typeElem != null)
                {
                    switch (typeElem)
                    {
                        case "RadioButton":
                            RadioButton rb = this.FindName(nameElem) as RadioButton;
                            if (rb != null)
                            {
                                rb.IsChecked = Convert.ToBoolean(UserSettings[nameElem]);
                                LOG("Свойству IsChecked элемента {nameElem} присвоено значение " + Convert.ToString(UserSettings[nameElem]));
                            }
                            break;
                        case "CheckBox":
                            CheckBox chb = this.FindName(nameElem) as CheckBox;
                            if (chb != null)
                            {
                                chb.IsChecked = Convert.ToBoolean(UserSettings[nameElem]);

                                LOG("Свойству IsChecked элемента {nameElem} присвоено значение " + Convert.ToString(UserSettings[nameElem]));
                            }
                            break;
                        case "TextBox":
                            TextBox tb = this.FindName(nameElem) as TextBox;
                            if (tb != null)
                            {
                                tb.Text = Convert.ToString(UserSettings[nameElem]);
                                LOG("Свойству Text элемента {nameElem} присвоено значение " + Convert.ToString(UserSettings[nameElem]));

                            }
                            break;
                        case "ComboBox":
                            ComboBox cb = this.FindName(nameElem) as ComboBox;
                            if (cb != null)
                            {
                                cb.SelectedIndex = Convert.ToInt32(UserSettings[nameElem]);
                                LOG("Свойству SelectedIndex элемента {nameElem} присвоено значение " + Convert.ToString(UserSettings[nameElem]));

                            }
                            break;
                        default:
                            LOG("ОШИБКА::: обнаружен нестандартный тип {typeElem} (элемент {nameElem}). Значение не было присвоено");

                            break;

                    }
                }
                else { LOG("ОШИБКА::: typeElem = null"); }
            }

            
            return true;
            //первая версия
            // int enumCount = Enum.GetNames(typeof(SettingsElem)).Length; //длинна нумератора с сохраняемыми параметрами
            //int listCount = userSettings.Count;//длинна списка с сохраненными параметрами
            // LOG("Длинна Enum и listSettingsOfUser совпадают? - " + (enumCount == listCount).ToString());
            // if (enumCount == listCount)
            // {
            //     for (int i = 0; i < enumCount; i++)
            //     {
            //         string nameElem = Enum.GetName(typeof(SettingsElem), i);
            //         string typeElem = FindName(nameElem).GetType().ToString().Substring(FindName(nameElem).GetType().ToString().LastIndexOf('.') + 1);
            //         if (typeElem != null)
            //         {
            //             switch (typeElem)
            //             {
            //                 case "RadioButton":
            //                     RadioButton rb = this.FindName(nameElem) as RadioButton;
            //                     if (rb != null) rb.IsChecked = Convert.ToBoolean(userSettings[i]);                   
            //                     break;
            //                 case "CheckBox":
            //                     CheckBox chb = this.FindName(nameElem) as CheckBox;
            //                     if (chb != null) chb.IsChecked = Convert.ToBoolean(userSettings[i]);
            //                     break;
            //                 case "TextBox":
            //                     TextBox tb = this.FindName(nameElem) as TextBox;
            //                     if (tb != null) tb.Text = userSettings[i];
            //                     break;
            //                 case "ComboBox":
            //                     ComboBox cb = this.FindName(nameElem) as ComboBox;
            //                     if (cb != null) cb.SelectedIndex = Convert.ToInt32(userSettings[i]);
            //                     break;
            //                 default:
            //                     return false;

            //             }
            //         }
            //         else { return false; }
            //     }

            //     LOG("Все настройки загружены>>>");
            //     return true;
            // }else            {
            //     LOG("ОШИБКА::: enumCount не равен длинне listCount. Возможно, проблемы с сохранением параметров. Настройки не загружены",true,true);
            //     LOG("Будут загружены настройки по умолчанию");
            //     return false;
            // }
        }
                
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)//Сохранение параметров при закрытии
        {
            //не помню чё это и зачем
            //параметр="{Binding Source={x:Static p:Settings.Default}, Path=параметр, Mode=TwoWay}"
            //SettingsBindableAttribute.Default.Save();

            //сохранение параметров
            string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name.Split('\\')[1];//имя юзера без домена!
            // string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name; //имя юзера с доменом!
            bool a = SettingsFX.SaveUserSettings(userName, PullUserSettingsVal()); // ответ от SettingsFX
            LOG("Настройки записаны? - " + a.ToString(), true, true);

            base.OnClosing(e);
        }

        private void Button_Click(object sender, RoutedEventArgs e) { SettingsClear(); }

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


        /// <summary>
        /// Выводит сообщение в файл LOG.txt и в окно MessageBox
        /// </summary>
        /// <param name = "m" >Значение для вывода</param >
        /// <param name = "newLine" >добавляет в конце строки символ новой (true по умолчанию)</param >
        /// <param name = "showMB" >выводит сообщение в MessageBox (false по умолчанию)</param >
        /// <returns>Ничего не возвращает</returns>
        public static void LOG(object m, bool newLine = true, bool showMB = false)//
        {
            string sm = m.ToString();
            if (newLine) { sm = sm + Environment.NewLine; }
            File.AppendAllText("LOG.txt", sm);           
            if (showMB) { MessageBox.Show(sm); }
            //return;
        }
        
        /// <summary>
        /// Полностью очищает переменную UserSettings!
        /// </summary>
        public void SettingsClear() {Properties.Settings.Default.UserSettings.Clear();Properties.Settings.Default.Save();}

        
    }




}
