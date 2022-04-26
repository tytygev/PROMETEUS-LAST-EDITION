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
    /// Логика взаимодействия для MainMenuButton.xaml
    /// </summary>
    public partial class MainMenuButton : UserControl
    {
        // выставить текст элемента управления RichText (только для чтения)
        // public string TextOfRichTextBox
        // {
        //     get { return richTextBox.Text; }
        //  }
        // выставить проверенное свойство флажка (чтение/запись)
        // public bool CheckBoxProperty
        // {
        //     get { return checkBox.Checked; }
        //     set { checkBox.Checked = value; }
        // }
        // выставить проверенное свойство флажка (чтение/запись)
        public string LabelButtonProperty
        {
            get { return MainMenuButtonText.Text; }
            set { MainMenuButtonText.Text = value; }
        }
        public ImageSource ImageSourceProperty
        {
            get { return MainMenuButtonImage.Source; }
            set { MainMenuButtonImage.Source = value; }
        }
        public MainMenuButton()
        {
            InitializeComponent();
        }
    }
}
