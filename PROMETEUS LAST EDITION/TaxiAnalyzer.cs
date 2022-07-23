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
        public static void LoadReport(string xlFileName)
        { 
          
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

    
}
