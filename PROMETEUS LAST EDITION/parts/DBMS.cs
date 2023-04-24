using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public bool ParseToDGrid(MainWindow mw,int item,string path="")
        {
            var db = (DBList)item;
            path="data\\"+db+".dsv";

            List < List<string> > listOfLists = new FileFX().LoadDSV(path, char.Parse("|"));


            //mw.DBMSdataGrid.ItemsSource = listOfLists;

            int Rows = listOfLists.Count();
            int Cols = listOfLists[0].Count();

            //mw.DBMSdataGrid.Row = N;
            //dataGridView1.ColumnCount = M;
            //int r, c;

            //for (r = 0; r < Rows; r++)

            //    for (c = 0; c < Cols; ++j)
            //        mw.DBMSdataGrid.Rows[rr][cc + 1] = data[rr, cc];
            //mw.DBMSdataGrid.Row
            return true;
        }

          
    }
}
