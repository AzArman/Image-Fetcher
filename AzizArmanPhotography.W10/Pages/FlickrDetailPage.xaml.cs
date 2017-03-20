//---------------------------------------------------------------------------
//
// <copyright file="FlickrDetailPage.xaml.cs" company="Microsoft">
//    Copyright (C) 2015 by Microsoft Corporation.  All rights reserved.
// </copyright>
//
// <createdOn>2/26/2016 6:51:21 PM</createdOn>
//
//---------------------------------------------------------------------------

using System;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using AppStudio.DataProviders.Flickr;
using AzizArmanPhotography.Sections;
using AzizArmanPhotography.Services;
using AzizArmanPhotography.ViewModels;
using AppStudio.Uwp;

namespace AzizArmanPhotography.Pages
{
    public sealed partial class FlickrDetailPage : Page
    {
        private DataTransferManager _dataTransferManager;

        public FlickrDetailPage()
        {
            this.ViewModel = DetailViewModel.CreateNew(Singleton<FlickrConfig>.Instance);
            this.ViewModel.SupportSlideShow = true;
            this.ViewModel.SupportFullScreen = true;

            this.InitializeComponent();
        }

        public DetailViewModel ViewModel { get; set; }        

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            await this.ViewModel.LoadDataAsync(e.Parameter as ItemViewModel);

            _dataTransferManager = DataTransferManager.GetForCurrentView();
            _dataTransferManager.DataRequested += OnDataRequested;
            FullScreenService.CurrentPageSupportFullScreen = true;

            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            _dataTransferManager.DataRequested -= OnDataRequested;
            FullScreenService.CurrentPageSupportFullScreen = false;

            base.OnNavigatedFrom(e);
        }

        private void OnDataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            ViewModel.ShareContent(args.Request);
        }

        private void ChangeFontSize(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            AppBarButton button = sender as AppBarButton;
            int newFontSize = Int32.Parse(button.Tag.ToString());
            mainPanel.BodyFontSize = newFontSize;
        }
    }
}
