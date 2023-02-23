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

        private bool _checked = true;
        private SolidColorBrush checkedBrush, uncheckedBrush;
        private Color highlightColor;

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
            InitColors();

            MainMenuButton.OnMainMenuButtonChecked += OnButtonChecked;

            Checked = false;
        }

        private void InitColors()
        {
            // это и подобное надо вынести в отдельный статический класс

            checkedBrush = new SolidColorBrush((Color)Application.Current.Resources[key: "ColorSub"]);
            uncheckedBrush = new SolidColorBrush((Color)Application.Current.Resources[key: "ColorMain"]);

            highlightColor = Colors.White;
            highlightColor.R /= 8;
            highlightColor.G /= 8;
            highlightColor.B /= 8;
        }


        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);

            OnMainMenuButtonChecked?.Invoke(this);
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);

            uncheckedBrush.Color += highlightColor;
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);

            uncheckedBrush.Color -= highlightColor;
        }

        protected void OnButtonChecked(MainMenuButton activeButton)
        {
            Checked = activeButton == this;
        }

    }

    
}
