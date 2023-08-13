using System;
using System.Collections.Generic;
using System.IO;
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

        public bool XMLloadManager(string path)
        {
            bool f = true;
            string xml = File.ReadAllText(path);

            List<List<string>>listOfLists = XMLseparateWorksheet(xml);

            for (int i=0; i < listOfLists.Count();i++)
            {
                List<string> lists = listOfLists.ElementAt(i);

                switch (lists.ElementAt(0))
                {
                    case "cars":
                        MainWindow.cars = XMLhandParse(lists.ElementAt(1));
                        break;
                    case "drivers":
                        MainWindow.drivers = XMLhandParse(lists.ElementAt(1));
                        break;
                    case "companies":
                        MainWindow.companies = XMLhandParse(lists.ElementAt(1));
                        break;
                    case "medical":
                        MainWindow.medical = XMLhandParse(lists.ElementAt(1));
                        break;
                    case "staff":
                        MainWindow.staff = XMLhandParse(lists.ElementAt(1));
                        break;
                    default:
                        f = false;
                        break;
                }
            }

            return f;
        }
        //добавить создание резервной копии перед выходом
        public bool XMLbackup(string path)
        {
            File.Copy(path, "backup_db.xml", true);
            return true;
        }

        /// <summary>
        /// Парсит XML таблицу Excel 
        /// </summary>
        /// <param name = "xml" >Принимает на вход строку, содержащую таблицу Excel в формате Таблица XML 2003</param >
        /// <returns>Возвращает списки имён рабочих листов XML таблицы Excel <br/>с фрагментами таблицы, соотвествующих имени, <br/>вложенные в список</returns>
        public List<List<string>> XMLseparateWorksheet(string xml)
        {
            List<List<string>> xmlSeparateWSh = new List<List<string>>();

            //обрезаем строку от первого Worksheet до последнего//
            int iStartWShs = xml.IndexOf("<Worksheet ss:Name");
            xml = xml.Substring(iStartWShs);
            int iEndWShs = xml.IndexOf("</Workbook>");
            xml = xml.Remove(iEndWShs);
            ////////////////////////////////////////////////

            //получаем массив строк Worksheet
            string[] worksheetSeparateStrings = { "<Worksheet ss:Name" };
            string[] worksheets = xml.Split(worksheetSeparateStrings, System.StringSplitOptions.RemoveEmptyEntries);

            //анализируем массив worksheets и загоняем в list
            for (int i = 0; i < worksheets.Count(); i++)
            {
                worksheets[i] = worksheets[i].Trim();

                //int ssNameWShIndexSubstring = worksheets[i].IndexOf("<Worksheet ss:Name=\"");
                int ssNameEndIndex = worksheets[i].IndexOf("\">");
                string ssNameWSh = worksheets[i].Remove(ssNameEndIndex);

                int ssNameStartIndex = ssNameWSh.IndexOf("\"") + 1;
                ssNameWSh = ssNameWSh.Substring(ssNameStartIndex);


                List<string> listOfWorksheets = new List<string>();
                listOfWorksheets.Add(ssNameWSh);
                listOfWorksheets.Add(worksheets[i]);

                xmlSeparateWSh.Add(listOfWorksheets);
            }
            return xmlSeparateWSh;
        }

        /// <summary>
        /// Парсит XML таблицу Excel 
        /// </summary>
        /// <param name = "xml" >Принимает на вход строку, содержащую таблицу Excel в формате Таблица XML 2003</param >
        /// <returns>Возвращает списки с данными ячеек XML таблицы Excel, <br/>вложенные в список</returns>
        public List<List<string>> XMLhandParse(string xml)
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
                if (j != 1) { //пропускаем вторую строку, которая с заголовками
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
            }
            return listOfLists;
        }

        public bool XMLInfoExtractor(MainWindow mw,string xml,bool f)
        {           
            //<DocumentProperties xmlns="urn:schemas-microsoft-com:office:office"> 
            //<Author>SuperUser</Author> 
            //<LastAuthor>SuperUser</LastAuthor> 
            //<Created> 2023 - 08 - 08T10: 31:29Z </Created>         
            //<LastSaved> 2023 - 08 - 11T15: 55:17Z </LastSaved>                 
            //<Version> 16.00 </Version>                 
            //</DocumentProperties>

            int iStartXML = xml.IndexOf("<DocumentProperties");
            xml = xml.Substring(iStartXML);
            int iEndXML = xml.IndexOf("<OfficeDocumentSettings");
            xml = xml.Remove(iEndXML);
            ////////////////////////////////////////////////
            string text;
            if (f) text = "БД загружена из основного файла: \n\n " + xml; else text = "БД загружена из архивной копии: \n\n " + xml;
            mw.DBInfoTextBlock.Text = text;
            return true;
        }

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