﻿using System.Windows;
using System.Windows.Controls;
using RecipeSelectHelper.Model;
using RecipeSelectHelper.Resources;

namespace RecipeSelectHelper.View.BoughtProducts
{
    /// <summary>
    /// Interaction logic for AddBoughtProductPage.xaml
    /// </summary>
    public partial class AddBoughtProductPage : Page, IAddElement
    {
        private MainWindow _parent;

        public AddBoughtProductPage(MainWindow parent)
        {
            _parent = parent;
            this.Loaded += AddBoughtProductPage_Loaded;
            InitializeComponent();
        }

        private void AddBoughtProductPage_Loaded(object sender, RoutedEventArgs e)
        {
            CheckBox_ProductCanExpire.IsChecked = true;
        }

        private void CheckBox_ProductCanExpire_Toggled(object sender, RoutedEventArgs e)
        {
            if (CheckBox_ProductCanExpire.IsChecked.Value)
            {
                StackPanel_ExpirationInfo.Visibility = Visibility.Visible;
            }
            else
            {
                StackPanel_ExpirationInfo.Visibility = Visibility.Collapsed;
            }
        }



        public void AddItem(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Success");
        }
    }
}