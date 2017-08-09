﻿using System.Windows;
using System.Windows.Controls;
using RecipeSelectHelper.Resources;

namespace RecipeSelectHelper.View.Miscellaneous
{
    /// <summary>
    /// Interaction logic for AddElementBasePage.xaml
    /// </summary>
    public partial class AddElementBasePage : Page
    {
        private IAddElement _content;
        private string _title;
        private MainWindow _parent;
        private object _finalizeButtonContent;

        public AddElementBasePage(MainWindow parent) : this(null, "Page Not Implemented", parent, string.Empty) { }

        // Rename this class maybe. Its no longer only for adding elements, but also for editing elements. 
        // Anything with a back button and a finalize button!
        public AddElementBasePage(IAddElement content, string title, MainWindow parent, string contentOfFinalizeButton = "Add")
        {
            _content = content;
            _title = title;
            _parent = parent;
            _finalizeButtonContent = contentOfFinalizeButton;

            Loaded += AddElementBasePage_Loaded;
            InitializeComponent();
        }

        private void AddElementBasePage_Loaded(object sender, RoutedEventArgs e)
        {
            var addItemPage = _content as IAddElement;
            if (addItemPage != null)
            {
                this.Button_Add.Click -= addItemPage.AddItem; 
                // This is done to remove old (duplicate) subscriptions caused by clicking the BACK-button and navigating to a new version of the page.
                this.Button_Add.Click += addItemPage.AddItem;
            }

            this.TextBlock_PageTitle.Text = _title;
            this.Button_Add.Content = _finalizeButtonContent;
            this.ContentControl_AddNewItem.Content = _content;
        }

        private void Button_Back_Click(object sender, RoutedEventArgs e)
        {
            if (_parent.ContentControl.NavigationService.CanGoBack)
            {
                _parent.ContentControl.NavigationService.GoBack();
            }
        }

        private void Button_Add_Click(object sender, RoutedEventArgs e)
        {
        }

        //TOdo cant I just delete the above?
    }
}