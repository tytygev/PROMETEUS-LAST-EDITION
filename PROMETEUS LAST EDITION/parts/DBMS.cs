using System;
using System.Collections.Generic;
using System.Linq;


namespace PROMETEUS_LAST_EDITION
{
    public class DBMS
    {
        public enum DBList
        {
            cars,
            drivers,
            companies,
            medical,
            staff
        }

        /// <summary>
        /// Парсит XML таблицу Excel 
        /// </summary>
        /// <param name = "xml" >Принимает на вход строку, содержащую таблицу Excel в формате Таблица XML 2003</param >
        /// <returns>Возвращает список строковых списков</returns>
        public List<List<string>> HandParseXML(string xml)
        {
            List<List<string>> listOfLists = new List<List<string>>(); //экземпляр списка списков
            string[] rowWords;
            string[] cellWords;
            //string[] dataWords;

            //обрезаем строку от первого ROW до последнего//
            int isubstring = xml.IndexOf("<Row");
            xml = xml.Substring(isubstring);
            int jsubstring = xml.IndexOf("</Table>");
            xml = xml.Remove(jsubstring);
            ////////////////////////////////////////////////

            //получаем массив строк ROW
            //ssIndex игнорируется и пустые строки будут пропущены
            string[] rowSeparatingStrings = { "<Row" };
            rowWords = xml.Split(rowSeparatingStrings, System.StringSplitOptions.RemoveEmptyEntries);

            string[] cellSeparatingStrings = { "<Cell" };
            for (int j = 0; j < rowWords.Count(); j++)
            {
                //получаем массив ячеек каждой отдельной строки
                cellWords = rowWords[j].Split(cellSeparatingStrings, System.StringSplitOptions.RemoveEmptyEntries);
                //анализируем массив ячеек и загоняем в list
                List<string> listOfCells = new List<string>(); //Создаём новый экземпляр
                for (int i = 1; i < cellWords.Count(); i++)
                {

                    cellWords[i] = cellWords[i].Trim();

                    int ssIndexSubstring = cellWords[i].IndexOf("ss:Index");
                    int dataSubstring = cellWords[i].IndexOf("<Data");

                    //если ss:Index найден раньше <Data...>, т.е. относится к <Cell...>
                    if (ssIndexSubstring != -1 && ssIndexSubstring <= dataSubstring)
                    {
                        //позиция начала числа
                        int intSubstring = cellWords[i].IndexOf("\"", ssIndexSubstring) + 1;
                        //позиция конца числа
                        int intEndSubstring = cellWords[i].IndexOf("\"", intSubstring);
                        //количество символов числа
                        int ssIndexLengt = intEndSubstring - intSubstring;

                        Int32.TryParse(cellWords[i].Substring(intSubstring, ssIndexLengt), out int ssIndexCell);

                        //добавить пустые строки в list в количестве значение_ss:Index - 1 - длинна_List
                        for (int k = 0; k < ssIndexCell - listOfCells.Count(); k++) { listOfCells.Add(""); }
                    }
                    int dataContentSubstring=0;
                    int dataEndContentSubstring=0;
                    if (dataSubstring >= 0) 
                    { 
                    dataContentSubstring = cellWords[i].IndexOf(">", dataSubstring) + 1;
                    dataEndContentSubstring = cellWords[i].IndexOf("</Data>", dataContentSubstring);
                    }
                    int dataLength = dataEndContentSubstring- dataContentSubstring;
                    if (dataLength < 0)
                    {
                        int a = 0;
                    }
                    listOfCells.Add(cellWords[i].Substring(dataContentSubstring, dataLength));

                }
                listOfLists.Add(listOfCells); //полученый список добавляем в список списков (как двумерный массив)

            }
            return listOfLists;
        }

        //добавить создание резервной копии перед выходом

        //добавить сравнение текущей версии и резервной копии

        //пример использования функции сохранения и загрузки БД:
        //List<List<string>> listOfLists = new List<List<string>>();
        //listOfLists=FileFX.LoadDSV("prop.txt", char.Parse(";"));
        //listOfLists[1][1] = "эщкере";
        //FileFX.SaveDSV(listOfLists,"prop.txt", char.Parse(";"));//изикатка    
        //public List<List<string>> ParseDB(MainWindow mw, int item, string path = "")
        //{
        //    var db = (DBList)item;
        //    path = "data\\" + db + ".dsv";

        //    List<List<string>> listOfLists = new FileFX().LoadDSV(path, char.Parse("|"));



        //    return listOfLists;
        //}

        //public bool ParseToDGrid(MainWindow mw, int item, string path = "")
        //{
        //    var db = (DBList)item;
        //    path = "data\\" + db + ".dsv";

        //    List<List<string>> listOfLists = new FileFX().LoadDSV(path, char.Parse("|"));


        //    //mw.DBMSdataGrid.ItemsSource = listOfLists;

        //    int rows = listOfLists.Count();
        //    int cols = listOfLists[0].Count();



        //    return true;
        //}

    }

   

}