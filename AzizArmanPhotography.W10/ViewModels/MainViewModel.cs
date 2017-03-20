using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Windows.Input;
using AppStudio.Uwp;
using AppStudio.Uwp.Actions;
using AppStudio.Uwp.Navigation;
using AppStudio.Uwp.Commands;
using AppStudio.DataProviders;

using AppStudio.DataProviders.Facebook;
using AppStudio.DataProviders.YouTube;
using AppStudio.DataProviders.Bing;
using AppStudio.DataProviders.Flickr;
using AppStudio.DataProviders.LocalStorage;
using AzizArmanPhotography.Sections;


namespace AzizArmanPhotography.ViewModels
{
    public class MainViewModel : ObservableBase
    {
        public MainViewModel(int visibleItems) : base()
        {
            PageTitle = "Aziz Arman Photography";
            Facebook = ListViewModel.CreateNew(Singleton<FacebookConfig>.Instance, visibleItems);
            YouTube = ListViewModel.CreateNew(Singleton<YouTubeConfig>.Instance, visibleItems);
            Bing = ListViewModel.CreateNew(Singleton<BingConfig>.Instance, visibleItems);
            Flickr = ListViewModel.CreateNew(Singleton<FlickrConfig>.Instance, visibleItems);

            Actions = new List<ActionInfo>();

            if (GetViewModels().Any(vm => !vm.HasLocalData))
            {
                Actions.Add(new ActionInfo
                {
                    Command = new RelayCommand(Refresh),
                    Style = ActionKnownStyles.Refresh,
                    Name = "RefreshButton",
                    ActionType = ActionType.Primary
                });
            }
        }

        public string PageTitle { get; set; }
        public ListViewModel Facebook { get; private set; }
        public ListViewModel YouTube { get; private set; }
        public ListViewModel Bing { get; private set; }
        public ListViewModel Flickr { get; private set; }

        public RelayCommand<INavigable> SectionHeaderClickCommand
        {
            get
            {
                return new RelayCommand<INavigable>(item =>
                    {
                        NavigationService.NavigateTo(item);
                    });
            }
        }
		public ICommand SearchCommand
        {
            get
            {
                return new RelayCommand<string>(
                (text) =>
                {
                    NavigationService.NavigateToPage("SearchPage", text);
                }, SearchViewModel.CanSearch);
            }
        }

        public DateTime? LastUpdated
        {
            get
            {
                return GetViewModels().Select(vm => vm.LastUpdated)
                            .OrderByDescending(d => d).FirstOrDefault();
            }
        }

        public List<ActionInfo> Actions { get; private set; }

        public bool HasActions
        {
            get
            {
                return Actions != null && Actions.Count > 0;
            }
        }

        public async Task LoadDataAsync()
        {
            var loadDataTasks = GetViewModels().Select(vm => vm.LoadDataAsync());

            await Task.WhenAll(loadDataTasks);

            OnPropertyChanged("LastUpdated");
        }

        private async void Refresh()
        {
            var refreshDataTasks = GetViewModels()
                                        .Where(vm => !vm.HasLocalData).Select(vm => vm.LoadDataAsync(true));

            await Task.WhenAll(refreshDataTasks);

            OnPropertyChanged("LastUpdated");
        }

        private IEnumerable<ListViewModel> GetViewModels()
        {
            yield return Facebook;
            yield return YouTube;
            yield return Bing;
            yield return Flickr;
        }
    }
}
