using System.Collections.Generic;
using System.ComponentModel;
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

            wav = new SoundPlayer();
            wav.Stream = Properties.Resources.ding;

            currentVisibleView = StartPage;
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
 if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // можно же перетянуть много файлов, так что....
                //string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                // делаешь что-то
                dropfilelabel.Text = files[0];
            }
        }

        private void runchecksummButton_Click(object sender, KeyEventArgs e)
        {
            //поиск файла Excel
            //OpenFileDialog ofd = new OpenFileDialog();
            //ofd.Multiselect = false;
            //ofd.DefaultExt = "*.xls;*.xlsx";
            //ofd.Filter = "Microsoft Excel (*.xls*)|*.xls*";
            //ofd.Title = "Выберите документ Excel";
            //if (ofd.ShowDialog() != DialogResult.OK)
            //{
            //    MessageBox.Show("Вы не выбрали файл для открытия", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    return;
            //}
            //string xlFileName = ofd.FileName; //имя нашего Excel файла
            string xlFileName = dropfilelabel.Text;

           //рабоата с Excel
           Excel.Range Rng;
            Excel.Workbook xlWB;
            Excel.Worksheet xlSht;
            int iLastRow, iLastCol;

            Excel.Application xlApp = new Excel.Application(); //создаём приложение Excel
            xlWB = xlApp.Workbooks.Open(xlFileName); //открываем наш файл           
            xlSht = xlWB.ActiveSheet; //или так  xlSht = xlWB.Worksheets["Лист1"];//активный лист

            iLastRow = xlSht.Cells[xlSht.Rows.Count, "A"].End[Excel.XlDirection.xlUp].Row; //последняя заполненная строка в столбце А
            iLastCol = xlSht.Cells[1, xlSht.Columns.Count].End[Excel.XlDirection.xlToLeft].Column; //последний заполненный столбец в 1-й строке

            Rng = (Excel.Range)xlSht.Range["A1", xlSht.Cells[iLastRow, iLastCol]]; //пример записи диапазона ячеек в переменную Rng
                                                                                   //Rng = xlSht.get_Range("A1", "B10"); //пример записи диапазона ячеек в переменную Rng
                                                                                   //Rng = xlSht.get_Range("A1:B10"); //пример записи диапазона ячеек в переменную Rng
                                                                                   //Rng = xlSht.UsedRange; //пример записи диапазона ячеек в переменную Rng

            var dataArr = (object[,])Rng.Value; //чтение данных из ячеек в массив            
                                                //xlSht.get_Range("K1").get_Resize(dataArr.GetUpperBound(0), dataArr.GetUpperBound(1)).Value = dataArr; //выгрузка массива на лист

            //закрытие Excel
            xlWB.Close(true); //сохраняем и закрываем файл
            xlApp.Quit();
            releaseObject(xlSht);
            releaseObject(xlWB);
            releaseObject(xlApp);
            taxichecksumm(dataArr);

        }
        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Win32Exception ex)
            {
                obj = null;
                MessageBox.Show("Unable to release the Object " + ex.ToString());
            }
            finally
            {
                System.GC.Collect();
            }
        }
        private void taxichecksumm(object[,]dataArr) {

            //добавляем столбцы в DataTable
            for (int i = 1; i <= dataArr.GetUpperBound(1); i++)
                dt.Columns.Add((string)dataArr[1, i]);

            //цикл по строкам массива
            for (int i = 2; i <= dataArr.GetUpperBound(0); i++)
            {
                dtRow = dt.NewRow();
                //цикл по столбцам массива
                for (int n = 1; n <= dataArr.GetUpperBound(1); n++)
                {
                    dtRow[n - 1] = dataArr[i, n];
                }
                dt.Rows.Add(dtRow);
            }

            this.dataGridView1.DataSource = dt; //заполняем dataGridView

            MessageBox.Show("Конец", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);

        }

    }
   



}
