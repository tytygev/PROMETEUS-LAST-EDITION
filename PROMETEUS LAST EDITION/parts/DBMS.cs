using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

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
        //пример использования функции сохранения и загрузки БД:
        //List<List<string>> listOfLists = new List<List<string>>();
        //listOfLists=FileFX.LoadDSV("prop.txt", char.Parse(";"));
        //listOfLists[1][1] = "эщкере";
        //FileFX.SaveDSV(listOfLists,"prop.txt", char.Parse(";"));//изикатка    
        public List<List<string>> ParseDB(MainWindow mw, int item, string path = "")
        {
            var db = (DBList)item;
            path = "data\\" + db + ".dsv";

            List<List<string>> listOfLists = new FileFX().LoadDSV(path, char.Parse("|"));


            //mw.DBMSdataGrid.ItemsSource = listOfLists;

            int rows = listOfLists.Count();
            int cols = listOfLists[0].Count();

            //
            //mw.DBMSdataGrid.Row = N;
            //dataGridView1.ColumnCount = M;
            //int r, c;

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    ColumnDefinition colDef = new ColumnDefinition();
                    mw.DBMSGrid.ColumnDefinitions.Add(colDef);


                }

            }
                //    
                //        mw.DBMSdataGrid.Rows[rr][cc + 1] = data[rr, cc];
                //mw.DBMSdataGrid.Row

                //DataGridTextColumn textColumn = new DataGridTextColumn();
                //                    textColumn.Header = "First Name";
                //                    textColumn.Binding = new Binding("хуета");
                //                    mw.DBMSdataGrid.Columns.Add(textColumn);
                //            mw.DBMSdataGrid.Items.Add(mw.DBMSdataGrid.Columns);

                //            for (int r = 0; r < rows; r++)
                //            {

                //                for (int c = 0; c < cols; c++)

                //                {



                //                }

                //            }








                return listOfLists;
        }
        public bool ParseToDGrid(MainWindow mw,int item,string path="")
        {
            var db = (DBList)item;
            path="data\\"+db+".dsv";

            List < List<string> > listOfLists = new FileFX().LoadDSV(path, char.Parse("|"));


            //mw.DBMSdataGrid.ItemsSource = listOfLists;

            int rows = listOfLists.Count();
            int cols = listOfLists[0].Count();

            //mw.DBMSdataGrid.Row = N;
            //dataGridView1.ColumnCount = M;
            //int r, c;

            //for (r = 0; r < Rows; r++)

            //    for (c = 0; c < Cols; ++j)
            //        mw.DBMSdataGrid.Rows[rr][cc + 1] = data[rr, cc];
            //mw.DBMSdataGrid.Row

            //DataGridTextColumn textColumn = new DataGridTextColumn();
            //                    textColumn.Header = "First Name";
            //                    textColumn.Binding = new Binding("хуета");
            //                    mw.DBMSdataGrid.Columns.Add(textColumn);
            //            mw.DBMSdataGrid.Items.Add(mw.DBMSdataGrid.Columns);

            //            for (int r = 0; r < rows; r++)
            //            {

            //                for (int c = 0; c < cols; c++)

            //                {



            //                }

            //            }





            //DataGridTextColumn col1 = new DataGridTextColumn();
            //DataGridTextColumn col2 = new DataGridTextColumn();
            //DataGridTextColumn col3 = new DataGridTextColumn();
            //DataGridTextColumn col4 = new DataGridTextColumn();
            //DataGridTextColumn col5 = new DataGridTextColumn();
            //mw.DBMSdataGrid.Columns.Add(col1);
            //mw.DBMSdataGrid.Columns.Add(col2);
            //mw.DBMSdataGrid.Columns.Add(col3);
            //mw.DBMSdataGrid.Columns.Add(col4);
            //mw.DBMSdataGrid.Columns.Add(col5);
            //col1.Binding = new Binding("id");
            //col2.Binding = new Binding("title");
            //col3.Binding = new Binding("jobint");
            //col4.Binding = new Binding("lastrun");
            //col5.Binding = new Binding("nextrun");
            //col1.Header = "ID";
            //col2.Header = "title";
            //col3.Header = "jobint";
            //col4.Header = "lastrun";
            //col5.Header = "nextrun";

            //mw.DBMSdataGrid.Items.Add(new MyData { id = 1, title = "Test", jobint = 2, lastrun = new DateTime(), nextrun = new DateTime() });
            //mw.DBMSdataGrid.Items.Add(new MyData { id = 12, title = "Test2", jobint = 24, lastrun = new DateTime(), nextrun = new DateTime() });





            return true;
        }

        //            Решение 3
        //Привет!
        //Используя учебник, найденный здесь,
        //я, программист
        //, проработал пример и адаптировал его для ваших нужд.
        //Развернуть ▼   

        public struct MyData
        {
            public int id { set; get; }
            public string title { set; get; }
            public int jobint { set; get; }
            public DateTime lastrun { set; get; }
            public DateTime nextrun { set; get; }
        }



        //Код, показанный выше, кажется довольно простым.XAML выглядит следующим образом:

        //<DataGrid x:Name= "myDataGrid" />

    }
}
