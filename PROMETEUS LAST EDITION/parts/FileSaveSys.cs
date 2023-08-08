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
using System.IO;

namespace PROMETEUS_LAST_EDITION
{
 
 public class FileFX 
    {
        public string LoadXML(string path){string xml = File.ReadAllText(path);return xml;}
        
        public List<List<string>> LoadDSV(string path, char separator)
        {
            string[] lines = File.ReadAllLines(path); //считываем построчно в массив строк
            List<List<string>> listOfLists = new List<List<string>>(); //экземпляр списка списков
            foreach (var line in lines) //проходим по строкам
            {
                string[] subs = line.Split(separator); //разбиваем строку на массив
                List<string> list = new List<string>(); //Создаём новый экземпляр
                foreach (var sub in subs) //проходим по элементам
                {
                    list.Add(sub); //добавляем в список
                }
                listOfLists.Add(list); //полученый список добавляем в список списков (как двумерный массив)
            }   
            return listOfLists; //возвращаем 

        }

        public void SaveDSV(List<List<string>> listOfLists, string path, char separator)
        {
          string[] lines = new string[listOfLists.Count];
            for (int i = 0; i < listOfLists.Count; i++) //формируем массив строк с сепараторами из списка списков
            {
                string line = "";
                for (int j = 0; j < listOfLists[i].Count; j++)
                { line += listOfLists[i][j]; if (j - listOfLists[i].Count + 1 != 0) { line += separator; } }
                lines[i] = line;
            }
            File.WriteAllLines(path, lines);
        }

        public static object[,] LoadXLS(string xlFileName)
        {
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
            ReleaseObject(xlSht);
            ReleaseObject(xlWB);
            ReleaseObject(xlApp);
            return dataArr;
        }
        public static void ReleaseObject(object obj)
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

    }






   
}
