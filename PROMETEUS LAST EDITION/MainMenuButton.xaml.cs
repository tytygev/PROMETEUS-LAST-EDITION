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
    public partial class MainMenuButton : UserControl
    {

        private bool _checked;
        private SolidColorBrush checkedBrush, uncheckedBrush;

        public static Action<MainMenuButton> OnMainMenuButtonChecked;

        public bool Checked
        {
            get => _checked;
            protected set
            {
                if (value != _checked)
                    Background = value ? checkedBrush : uncheckedBrush;

                _checked = value;
            }
        }

        //выставить свойство элемента Label (чтение/запись)
        public string LabelButtonProperty
        {
            get { return MainMenuButtonText.Text; }
            set { MainMenuButtonText.Text = value; }
        }

        //выставить свойство элемента Image (чтение/запись)
        public ImageSource ImageSourceProperty
        {
            get { return MainMenuButtonImage.Source; }
            set { MainMenuButtonImage.Source = value; }
        }

        public MainMenuButton()
        {
            InitializeComponent();

            checkedBrush = new SolidColorBrush((Color)Application.Current.Resources[key: "ColorSub"]);
            uncheckedBrush = new SolidColorBrush((Color)Application.Current.Resources[key: "ColorMain"]);

            MainMenuButton.OnMainMenuButtonChecked += OnButtonChecked;
        }


        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);

            OnMainMenuButtonChecked?.Invoke(this);
        }

        protected void OnButtonChecked(MainMenuButton activeButton)
        {
            Checked = activeButton == this;
        }

    }

    enum MainMenuButtonsEnum
    {
        KitSetButton,
        PriceButton,
        DBEditButton,
        SettingsButton,
        AboutButton,
        ExitButton
    }
    enum ViewPagesEnum
    {
        KitSetPage,
        PricePage,
        DBEditPage,
        SettingsPage,
        AboutPage,
        StartPage
    }
}
