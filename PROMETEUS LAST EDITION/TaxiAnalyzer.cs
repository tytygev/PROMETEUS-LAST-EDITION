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
    public static class TaxiAnalyzer
    {



        public static object[,] LoadReport(string xlFileName)
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
            return dataArr;
        }

        public static object[,] Filtration (object[,] dataArr)
        {
            var DSettingsTaxiComboBoxes = MainWindow.DSettingsTaxiGrid.Children.OfType<ComboBox>().ToList();
            for (int i = 1; i <= dataArr.GetUpperBound(1); i++)                       {
               
                for (int n = 1; n <= dataArr.GetUpperBound(1); n++)
                {
                    //       dtRow[n - 1] = dataArr[i, n];
                    if (dataArr[i,n]=)
                }
            }


            return dataArr;
        }

        public static void TaxiCheckSumm(object[,] dataArr)
        {

          

            MessageBox.Show("Конец", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);

        }


        public static void releaseObject(object obj)
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

        public static string GetReportFileName(DragEventArgs e)
        {
            string[] files=null;
             if (e.Data.GetDataPresent(DataFormats.FileDrop))
             {
                // можно же перетянуть много файлов, так что....
                files = (string[])e.Data.GetData(DataFormats.FileDrop);
                // делаешь что-то                
             }
            return files[0];
        }
    }

    public class TaxiReport
    {

    }
    
}

//data grid
////добавляем столбцы в DataTable
//for (int i = 1; i <= dataArr.GetUpperBound(1); i++)
////   dt.Columns.Add((string)dataArr[1, i]);

////цикл по строкам массива
////for (int i = 2; i <= dataArr.GetUpperBound(0); i++)
//{
//    //    dtRow = dt.NewRow();
//    //цикл по столбцам массива
//    for (int n = 1; n <= dataArr.GetUpperBound(1); n++)
//    {
//        //       dtRow[n - 1] = dataArr[i, n];
//    }
//    //   dt.Rows.Add(dtRow);
//}

//// this.dataGridView1.DataSource = dt; //заполняем dataGridView