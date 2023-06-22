using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows;
using System.Windows.Media;

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
                RowDefinition rowDef = new RowDefinition();
                mw.DBMSGrid.RowDefinitions.Add(rowDef);

                for (int c = 0; c < cols; c++)
                {
                    ColumnDefinition colDef = new ColumnDefinition();
                    mw.DBMSGrid.ColumnDefinitions.Add(colDef);

                    //System.Windows.UIElement body;
                    System.Windows.UIElement body =new System.Windows.UIElement();
                    if (r == 0)
                    {                       //Button
                                            //Margin = "0"
                                            //Padding = "5,1"
                                            //MinWidth = "10"
                                            //MinHeight = "10"
                                            //FontSize = "10"
                                            //FontWeight = "Bold"
                                            //HorizontalContentAlignment = "Left"
                                            //VerticalContentAlignment = "Center"
                                            //Content = ""
                                            //< Button.Foreground >< SolidColorBrush Color = "{DynamicResource ColorFont}" /></ Button.Foreground >
                       
                        Button butt = new Button() as Button;
                        mw.DBMSGrid.Children.Add(butt);
                        butt.Margin = new Thickness(0);
                        butt.Padding = new Thickness(5, 1, 5, 1);
                        butt.MinWidth = 10;
                        butt.MinHeight = 10;
                        butt.FontSize = 10;
                        butt.FontWeight = FontWeights.Bold;
                        butt.FontFamily = new FontFamily("Segoe UI");
                        butt.HorizontalContentAlignment = HorizontalAlignment.Left;
                        butt.VerticalContentAlignment = VerticalAlignment.Center;
                        //butt.Foreground = (System.Windows.Media.SolidColorBrush)Application.Current.Resources["ColorFont"];
                        Color color = new Color();
                        color = (Color)Application.Current.Resources["ColorFont"];
                        butt.Foreground = new SolidColorBrush (color);
                        butt.Content = listOfLists[r][c];

                        body = butt as System.Windows.UIElement;
                    }
                    else
                    {                       //TextBox
                                            //Margin = "0"
                                            //Padding = "0,-5"
                                            //MinWidth = "10"
                                            //MinHeight = "10"
                                            //FontSize = "10"
                                            //Text = ""                
                                            //< TextBox.Foreground >< SolidColorBrush Color = "{DynamicResource ColorFont}" />                
                                            //< TextBox.SelectionBrush >< SolidColorBrush Color = "{DynamicResource ColorAltNuans}" />               

                        TextBox text = new TextBox();
                        mw.DBMSGrid.Children.Add(text);
                        text.Margin = new Thickness(0);
                        text.Padding = new Thickness(0, -5, 0, -5);
                        text.MinWidth = 10;
                        text.MinHeight = 10;
                        text.FontSize = 10;
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
                        text.Text = listOfLists[r][c];
                        
                        body = text as System.Windows.UIElement;
                    }

                        Grid.SetRow(body, r);
                        Grid.SetColumn(body, c+1);


                
                    


                   

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
