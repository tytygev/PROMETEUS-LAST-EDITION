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

namespace PROMETEUS_LAST_EDITION
{
    /// <summary>
    /// Логика взаимодействия для SubMenuButton.xaml
    /// </summary>
    public partial class SubMenuButton : UserControl
    {
        //выставить свойство элемента Image (чтение/запись)
        public ImageSource ImageSourceProperty
        {
            get { return SubMenuButtonImage.Source; }
            set {SubMenuButtonImage.Source = value; }
        }
        
        public SubMenuButton()
        {
            InitializeComponent();  
        }

       
      
    }
}
