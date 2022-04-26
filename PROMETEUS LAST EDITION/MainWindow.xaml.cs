using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
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
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SoundPlayer wav;
        List<MainMenuButton> ListMainMenuButton = new List<MainMenuButton>();

        Enum MainMenuButtonsEnum 
        {
            KitSet
    Price
    DBEdit
    Settings
    About
    ButtonExit
        }
Enum KitSetSubMenuButtonsEnum
        {
            BNew
    BOpen
    BSave
    BSaveAs
    BPrint
    BFastPrint
        }

        public MainWindow()
        {
            InitializeComponent();
            wav = new SoundPlayer();
            wav.Stream = Properties.Resources.ding;


        }

       

        private void MenuButton_MouseEnter(object sender, MouseEventArgs e)
        {
            object mycolor = null;
            
            if (sender is MainMenuButton)
            {
                mycolor = (Color)Application.Current.Resources[key: "ColorSub"];
                Brush br = new SolidColorBrush((Color)mycolor);
                ((MainMenuButton)sender).Background = br;
            }
            else {
                mycolor = (Color)Application.Current.Resources[key: "ColorNuans"];
                Brush br = new SolidColorBrush((Color)mycolor);
                ((SubMenuButton)sender).Background = br;
            }
           
        }
        private void MenuButton_MouseLeave(object sender, MouseEventArgs e)
        {
            object mycolor = null;

            if (sender is MainMenuButton)
            {
                mycolor = (Color)Application.Current.Resources[key: "ColorMain"];
                Brush br = new SolidColorBrush((Color)mycolor);
                ((MainMenuButton)sender).Background = br;
            }
            else
            {
                mycolor = (Color)Application.Current.Resources[key: "ColorSub"];
                Brush br = new SolidColorBrush((Color)mycolor);
                ((SubMenuButton)sender).Background = br;
            }
            // wav.Stop();
        }
        private void MenuButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
  wav.Play();
            //Временные
            object mycolor = null;
            if (sender is MainMenuButton)
            {
                mycolor = (Color)Application.Current.Resources[key: "ColorNuans"];
                Brush br = new SolidColorBrush((Color)mycolor);
                ((MainMenuButton)sender).Background = br;

                //  if (ListMainMenuButton==null) return;
                //  int number = ListMainMenuButton.Count - 1;
                //    if (number < 0) return;

                //int number = ListMainMenuButton.Count;
                MainMenuButton current  = ListMainMenuButton(ListMainMenuButton.Count);

            }
            else
            {
                mycolor = (Color)Application.Current.Resources[key: "ColorAltNuans"];
                Brush br = new SolidColorBrush((Color)mycolor);
                ((SubMenuButton)sender).Background = br;
            }
            
        }

     
    }
}
