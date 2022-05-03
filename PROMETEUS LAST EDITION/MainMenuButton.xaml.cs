﻿using System;
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

              //public class CustomButton : Button, ICanDoSomething
        //{
        //    public void DoSomething()
        //    {
        //        //реализация
        //    }

        //    public int Index
        //    {
        //        get
        //        {
        //            // реализация
        //            return 0;
        //        }
        //    }
        //}

        //public interface ICanDoSomething
        //{
        //    void DoSomething();
        //    int Index { get; }
        //}


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
