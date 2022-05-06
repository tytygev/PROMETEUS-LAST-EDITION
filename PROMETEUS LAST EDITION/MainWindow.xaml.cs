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
        private SoundPlayer wav;

        private Grid currentVisibleView;


        public MainWindow()
        {
            InitializeComponent();
            InitializeButtons();

            wav = new SoundPlayer();
            wav.Stream = Properties.Resources.ding;

            currentVisibleView = StartPage;
        }

        private void InitializeButtons()
        {
            KitSetButton.MouseUp += (s, e) => ShowView(KitSetPage);
            PriceButton.MouseUp += (s, e) => ShowView(PricePage);
            DBEditButton.MouseUp += (s, e) => ShowView(DBEditPage);
            SettingsButton.MouseUp += (s, e) => ShowView(SettingsPage);
            AboutButton.MouseUp += (s, e) => ShowView(AboutPage);
            ExitButton.MouseUp += (s, e) => Application.Current.Shutdown();
        }

        private void ShowView(Grid view)
        {
            if (view == currentVisibleView)
                return;

            if (currentVisibleView != null)
                currentVisibleView.Visibility = Visibility.Hidden;

            view.Visibility = Visibility.Visible;

            currentVisibleView = view;
        }





        private void MenuButton_MouseEnter(object sender, MouseEventArgs e)
        {
            //if (sender is MainMenuButton ) ((MainMenuButton)sender).Background = new SolidColorBrush((Color)Application.Current.Resources[key: "ColorSub"]);
            //else ((SubMenuButton)sender).Background = new SolidColorBrush((Color)Application.Current.Resources[key: "ColorNuans"]);
        }
        private void MenuButton_MouseLeave(object sender, MouseEventArgs e)
        {
            //if (sender is MainMenuButton) ((MainMenuButton)sender).Background = new SolidColorBrush((Color)Application.Current.Resources[key: "ColorMain"]);
            //else ((SubMenuButton)sender).Background = new SolidColorBrush((Color)Application.Current.Resources[key: "ColorSub"]);
        }
        private void MenuButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            //
        }

    }
   
}
