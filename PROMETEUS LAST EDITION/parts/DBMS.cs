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

        public static void LoadParserXml(string xml)
        {
//            var xml =
//@"<note>
//           <to>Tove</to>
//           <from>Jani</from>
//           <heading>Reminder</heading>
//           <body>Don't forget me this weekend!</body>
//        </note>";

            Console.WriteLine("== LEXEMS ==");

            foreach (var lexem in LexemAnalyser.ParseLexems(xml))
                Console.WriteLine(lexem);

            Console.WriteLine();
            Console.WriteLine("== XML TREE ==");

            var root = XmlParser.Parse(xml);
            TypeXmlTree(root);

            Console.ReadLine();
        }

        static void TypeXmlTree(XmlNode node, string prefix = "")
        {
            Console.WriteLine(prefix + node.Content);
            foreach (var child in node.Children)
                TypeXmlTree(child, prefix + "\t");
        }


    }


    static class XmlParser
    {
        public static XmlNode Parse(string xml)
        {
            //get lexems
            var lexems = LexemAnalyser.ParseLexems(xml).ToList();
            //check
            if (lexems.Count < 2) throw new Exception("Пустой XML");
            if (lexems[0].Type != LexemType.OpenTag) throw new Exception("XML should start with tag");
            //build node tree
            var stack = new Stack<XmlNode>();
            foreach (var lexem in lexems)
                switch (lexem.Type)
                {
                    case LexemType.OpenTag:
                        var node = new XmlNode() { Type = lexem.Type, Content = lexem.Text };
                        if (stack.Count > 0)
                            stack.Peek().Children.Add(node);
                        stack.Push(node);
                        break;
                    case LexemType.CloseTag:
                        var open = stack.Pop();
                        if (open.Content != lexem.Text)
                            throw new Exception("Close tag does not correspond to open tag");
                        if (stack.Count == 0)
                            return open;
                        break;
                    case LexemType.Content:
                        var textNode = new XmlNode() { Type = lexem.Type, Content = lexem.Text };
                        stack.Peek().Children.Add(textNode);
                        break;
                }

            throw new Exception("No close tag");
        }
    }

    class XmlNode
    {
        public List<XmlNode> Children = new List<XmlNode>();
        public LexemType Type;
        public string Content;
    }

    static class LexemAnalyser
    {
        public static IEnumerable<Lexem> ParseLexems(string xml)
        {
            return ParseLexemsRaw(xml).Where(lexem => lexem.Type != LexemType.Content || lexem.Text.Trim() != "");//ignore empty content lexems
        }

        private static IEnumerable<Lexem> ParseLexemsRaw(string xml)
        {
            LexemType type = LexemType.Content;
            string text = "";

            foreach (var c in xml)
                switch (c)
                {
                    case '<':
                        yield return new Lexem(type, text);
                        type = LexemType.OpenTag; text = "";
                        break;
                    case '/':
                        if (type == LexemType.OpenTag && text == "")
                            type = LexemType.CloseTag;
                        else
                            goto default;
                        break;
                    case '>':
                        if (type == LexemType.Content)
                            goto default;
                        yield return new Lexem(type, text);
                        type = LexemType.Content; text = "";
                        break;
                    default:
                        text += c;
                        break;
                }

            yield return new Lexem(type, text);
        }
    }

    enum LexemType
    {
        OpenTag, CloseTag, Content
    }

    class Lexem
    {
        public LexemType Type { get; set; }
        public string Text { get; set; }

        public Lexem(LexemType type, string text)
        {
            Type = type;
            Text = text;
        }

        public override string ToString()
        {
            return Type + ": " + Text;
        }
    }


}
