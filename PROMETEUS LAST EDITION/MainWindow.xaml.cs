using System.Collections.Generic;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace PROMETEUS_LAST_EDITION
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SoundPlayer wav;
        public List<MainMenuButton> ListMainMenuButtons = new List<MainMenuButton>();
        public void AddMainMenuButtons()
        {
            ListMainMenuButtons.Add(this.KitSetButton);
            ListMainMenuButtons.Add(this.PriceButton);
            ListMainMenuButtons.Add(this.DBEditButton);
            ListMainMenuButtons.Add(this.SettingsButton);
            ListMainMenuButtons.Add(this.AboutButton);
            ListMainMenuButtons.Add(this.ExitButton);
        }
        public List<Grid> ListViewPages = new List<Grid>();
        public void AddListViewPages()
        {
            ListViewPages.Add(this.KitSetPage);
            ListViewPages.Add(this.PricePage);
            ListViewPages.Add(this.DBEditPage);
            ListViewPages.Add(this.SettingsPage);
            ListViewPages.Add(this.AboutPage);
            ListViewPages.Add(this.StartPage);
        }
        public MainWindow()
        {
            InitializeComponent();
            AddMainMenuButtons();
            AddListViewPages();
            wav = new SoundPlayer();
            wav.Stream = Properties.Resources.ding;
        }
        private void MenuButton_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is MainMenuButton) ((MainMenuButton)sender).Background = new SolidColorBrush((Color)Application.Current.Resources[key: "ColorSub"]);
            else ((SubMenuButton)sender).Background = new SolidColorBrush((Color)Application.Current.Resources[key: "ColorNuans"]);
        }
        private void MenuButton_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is MainMenuButton) ((MainMenuButton)sender).Background = new SolidColorBrush((Color)Application.Current.Resources[key: "ColorMain"]);
            else ((SubMenuButton)sender).Background = new SolidColorBrush((Color)Application.Current.Resources[key: "ColorSub"]);
        }
        
        private void MenuButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            wav.Play();
            // var obj = (object)sender;
            // string name = obj.Name;

            for (int i = 0; i < ListMainMenuButtons.Count; i++)
            {               
                if (Equals((MainMenuButton)sender, ListMainMenuButtons[i] as MainMenuButton))
                {                    
                    for (int j=0; j< ListViewPages.Count; j++) ListViewPages[j].Visibility = Visibility.Hidden;
                   ListViewPages[i].Visibility = Visibility.Visible;
                }
            }
        }


    }
   
}
