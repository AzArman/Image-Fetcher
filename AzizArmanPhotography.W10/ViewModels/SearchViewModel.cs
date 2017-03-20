using System;
using System.Collections.Generic;
using AppStudio.Uwp;
using AppStudio.Uwp.Commands;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AzizArmanPhotography.Sections;
namespace AzizArmanPhotography.ViewModels
{
    public class SearchViewModel : ObservableBase
    {
        public SearchViewModel() : base()
        {
			PageTitle = "Aziz Arman Photography";
            Facebook = ListViewModel.CreateNew(Singleton<FacebookConfig>.Instance);
            YouTube = ListViewModel.CreateNew(Singleton<YouTubeConfig>.Instance);
            Bing = ListViewModel.CreateNew(Singleton<BingConfig>.Instance);
            Flickr = ListViewModel.CreateNew(Singleton<FlickrConfig>.Instance);
                        
        }

        private string _searchText;
        private bool _hasItems = true;

        public string SearchText
        {
            get { return _searchText; }
            set { SetProperty(ref _searchText, value); }
        }

        public bool HasItems
        {
            get { return _hasItems; }
            set { SetProperty(ref _hasItems, value); }
        }

		public ICommand SearchCommand
        {
            get
            {
                return new RelayCommand<string>(
                async (text) =>
                {
                    await SearchDataAsync(text);
                }, SearchViewModel.CanSearch);
            }
        }      
        public ListViewModel Facebook { get; private set; }
        public ListViewModel YouTube { get; private set; }
        public ListViewModel Bing { get; private set; }
        public ListViewModel Flickr { get; private set; }
        
		public string PageTitle { get; set; }
        public async Task SearchDataAsync(string text)
        {
            this.HasItems = true;
            SearchText = text;
            var loadDataTasks = GetViewModels()
                                    .Select(vm => vm.SearchDataAsync(text));

            await Task.WhenAll(loadDataTasks);
			this.HasItems = GetViewModels().Any(vm => vm.HasItems);
        }

        private IEnumerable<ListViewModel> GetViewModels()
        {
            yield return Facebook;
            yield return YouTube;
            yield return Bing;
            yield return Flickr;
        }
		private void CleanItems()
        {
            foreach (var vm in GetViewModels())
            {
                vm.CleanItems();
            }
        }
		public static bool CanSearch(string text) { return !string.IsNullOrWhiteSpace(text) && text.Length >= 3; }
    }
}
