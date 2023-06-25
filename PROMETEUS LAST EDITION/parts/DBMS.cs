using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows;
using System.Windows.Media;
using System.Windows.Input;

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

         

            //
            //mw.DBMSdataGrid.Row = N;
            //dataGridView1.ColumnCount = M;
            //int r, c;

     
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

        private void CreateRow(Grid gr) {RowDefinition rowDef = new RowDefinition(); gr.RowDefinitions.Add(rowDef);}
        private void CreateCol(Grid gr) {ColumnDefinition colDef = new ColumnDefinition(); gr.ColumnDefinitions.Add(colDef);}
        private void CreateCell(string typeCell, Grid gr,  int row, int col,string content="") {
            //System.Windows.UIElement body;
            if (typeCell == "TextBox") {
                TextBox text = new TextBox();
                gr.Children.Add(text);

                SettingsTextBoxGrid(text);

                text.Text = content;
                Grid.SetRow(text, row);
                Grid.SetColumn(text, col);
                //body = text as System.Windows.UIElement;
            }
            else { 
                Button butt = new Button() as Button;
                gr.Children.Add(butt);
                if (typeCell== "RowHeadButt")
                {
                    SettingsRowHeadButtGrid(butt);
                    if (row == 0) butt.BorderThickness = new Thickness(1, 1, 2, 1);
                }
                else if(typeCell == "HeaderButt")
                {
                    SettingsHeaderButtGrid(butt);
                }
                butt.Content = content;
                Grid.SetRow(butt, row);
                Grid.SetColumn(butt, col);
                //body = butt as System.Windows.UIElement;
            }
            //Grid.SetRow(body, row);
            //Grid.SetColumn(body, col);
        }

        public void CreateClearGrid(MainWindow mw)
        {

            mw.DBMSGrid.ColumnDefinitions.Clear();
            mw.DBMSGridHeader.ColumnDefinitions.Clear();
            mw.DBMSGrid.RowDefinitions.Clear();
            mw.DBMSGridHeader.RowDefinitions.Clear();
            //CreateRow(mw.DBMSGridHeader);//создаём строку в таблице заголовка
        }

        public void FinishedCreateGrid(MainWindow mw) {
            int cols = mw.DBMSGrid.ColumnDefinitions.Count;
            int rows = mw.DBMSGrid.RowDefinitions.Count;

            int col =cols/rows;
            
            CreateRow(mw.DBMSGrid);//создаём строку в основной таблице
            for (int i = 0; i < col; i++) {
               CreateCol(mw.DBMSGrid);//создаём колонку в основной таблице
                if (i == 0)//если нулевая колонка (управляющая - заголовки строк)
                {
                    CreateCol(mw.DBMSGrid);//создаём дополнительную колонку
                    CreateCell("RowHeadButt", mw.DBMSGrid, rows+1, i, "▶*");//и управляющую кнопку заголовка строки
                }
                CreateCell("TextBox", mw.DBMSGrid, rows+1, i + 1);//создаём в ячейке текстовое поле, но распологаем со смещение на одну ячеёку вправо
            }

            mw.DataGridScroll.ScrollToEnd();
            List<TextBox> list= mw.DBMSGrid.Children.OfType<TextBox>().ToList();
            for (int l=0;l<list.Count; l++)
            {
                if (Grid.GetRow(list[l])==rows+1 & Grid.GetColumn (list[l])==1) {
                    list[l].Focus();
                    list[l].Focusable = true; 
                    Keyboard.Focus(list[l]);

                }
            }
            
        }

        public bool CreateDataGrid(MainWindow mw, List<List<string>> listOfLists)
        {
            int rows = listOfLists.Count();
            int cols = listOfLists[0].Count();

            for (int r = 0; r < rows; r++)
            {                 
                CreateRow(mw.DBMSGrid);//создаём строку в основной таблице

                for (int c = 0; c < cols; c++)
                {
                    CreateCol(mw.DBMSGrid);//создаём колонку в основной таблице

                    if (r == 0)//если нулевая строка (заголовок)
                    {
                        CreateCol(mw.DBMSGridHeader);//дополнительно создаём колонку в таблице заголовка

                        if (c == 0)//если нулевая колонка (управляющая - заголовки строк)
                        {
                            CreateCol(mw.DBMSGridHeader);//создаём дополнительную колонку в таблице заголовка
                            CreateCol(mw.DBMSGrid);//создаём дополнительную колонку

                            CreateCell("RowHeadButt", mw.DBMSGridHeader,r,c);//создаём в ячейке кнопку заголовка строки в таблице заголовка
                            CreateCell("RowHeadButt", mw.DBMSGrid, r, c);//создаём в ячейке кнопку заголовка строки
                        }                        
                       
                        CreateCell("HeaderButt", mw.DBMSGridHeader, r, c+1, listOfLists[r][c]);//создаём в ячейке кнопку заголовка в таблице заголовка
                        CreateCell("HeaderButt", mw.DBMSGrid, r, c + 1, listOfLists[r][c]);//создаём в ячейке кнопку заголовка в основной таблице (для синхронизации)
                    }
                    else//если остальные строки
                    {
                        
                        if (c == 0)//если нулевая колонка (управляющая - заголовки строк)
                        {
                            CreateCol(mw.DBMSGrid);//создаём дополнительную колонку
                            CreateCell("RowHeadButt", mw.DBMSGrid, r, c);//и управляющую кнопку заголовка строки
                        }
                        CreateCell("TextBox", mw.DBMSGrid, r, c + 1, listOfLists[r][c]);//создаём в ячейке текстовое поле, но распологаем со смещение на одну ячеёку вправо
                    }
                }
            }

            mw.DBMSGrid.RowDefinitions[0].Height = new GridLength(0);
            mw.DBMSGrid.RowDefinitions[0].MinHeight = 0;
            ColumnWidthLeveling(mw.DBMSGridHeader, mw.DBMSGrid);

            return true;
        }


        private void SettingsTextBoxGrid(TextBox text) {
            text.Margin = new Thickness(0);
            text.Padding = new Thickness(0, -5, 0, -5);
            text.BorderThickness = new Thickness(0, 0, 1, 1);
            text.MinWidth = 10;
            text.MinHeight = 10;
            text.FontSize = 12;
            text.FontFamily = new FontFamily("Segoe UI");
            text.HorizontalContentAlignment = HorizontalAlignment.Left;
            text.VerticalContentAlignment = VerticalAlignment.Center;
            //text.Foreground = (System.Windows.Media.SolidColorBrush)Application.Current.Resources["ColorFont"];
            //text.SelectionBrush = (System.Windows.Media.SolidColorBrush)Application.Current.Resources["ColorAltNuans"];
            Color color = new Color();
            color = (Color)Application.Current.Resources["ColorFont"];
            text.Foreground = new SolidColorBrush(color);
            color = (Color)Application.Current.Resources["ColorAltNuans"];
            text.SelectionBrush = new SolidColorBrush(color);
        }
        private void SettingsHeaderButtGrid(Button butt) {
            butt.Margin = new Thickness(0);
            butt.Padding = new Thickness(5, 1, 5, 1);
            butt.BorderThickness = new Thickness(1, 1,2, 0);
            butt.MinWidth = 10;
            butt.MinHeight = 0;
            butt.FontSize = 12;
            butt.FontWeight = FontWeights.Bold;
            butt.FontFamily = new FontFamily("Segoe UI");
            butt.HorizontalContentAlignment = HorizontalAlignment.Left;
            butt.VerticalContentAlignment = VerticalAlignment.Center;
            //butt.Foreground = (System.Windows.Media.SolidColorBrush)Application.Current.Resources["ColorFont"];
            Color color = new Color();
            color = (Color)Application.Current.Resources["ColorFont"];
            butt.Foreground = new SolidColorBrush(color);
        }
        private void SettingsRowHeadButtGrid(Button butt) {
            butt.Margin = new Thickness(0);
            butt.Padding = new Thickness(5, 1, 5, 1);
            butt.BorderThickness = new Thickness(1, 1, 2, 2);
            butt.MinWidth = 10;
            butt.MinHeight = 10;
            butt.FontSize = 12;
            butt.FontWeight = FontWeights.Bold;
            butt.FontFamily = new FontFamily("Segoe UI Symbol");
            Color color = new Color();
            color = (Color)Application.Current.Resources["ColorFont"];
            butt.Foreground = new SolidColorBrush(color);
        }

        public void ColumnWidthLeveling(Grid grid1, Grid grid2)
        {
            int col1 = grid1.ColumnDefinitions.Count;
            int col2 = grid2.ColumnDefinitions.Count;
            int row1 = grid1.RowDefinitions.Count;
            int row2 = grid2.RowDefinitions.Count;

                for (int c = 0; c < col1; c++)
                {

                grid1.ColumnDefinitions[c].Width = new GridLength(grid2.ColumnDefinitions[c].ActualWidth);
                //if (grid1.ColumnDefinitions[c].Width < grid2.ColumnDefinitions[c].Width)
                //{


                //}
            }
            if (col1 == col2)
            {
            }

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
