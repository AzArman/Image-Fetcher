//---------------------------------------------------------------------------
//
// <copyright file="FacebookListPage.xaml.cs" company="Microsoft">
//    Copyright (C) 2015 by Microsoft Corporation.  All rights reserved.
// </copyright>
//
// <createdOn>2/26/2016 6:51:21 PM</createdOn>
//
//---------------------------------------------------------------------------

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml;
using AppStudio.DataProviders.Facebook;
using AzizArmanPhotography.Sections;
using AzizArmanPhotography.ViewModels;
using AppStudio.Uwp;

namespace AzizArmanPhotography.Pages
{
    public sealed partial class FacebookListPage : Page
    {
	    public ListViewModel ViewModel { get; set; }
        public FacebookListPage()
        {
			this.ViewModel = ListViewModel.CreateNew(Singleton<FacebookConfig>.Instance);

            this.InitializeComponent();

        }


        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            await this.ViewModel.LoadDataAsync();
            base.OnNavigatedTo(e);
        }

    }
}
