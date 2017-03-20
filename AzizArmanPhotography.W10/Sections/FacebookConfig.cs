


using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using AppStudio.DataProviders;
using AppStudio.DataProviders.Core;
using AppStudio.DataProviders.Facebook;
using AppStudio.Uwp;
using AppStudio.Uwp.Actions;
using AppStudio.Uwp.Commands;
using AppStudio.Uwp.Navigation;
using AzizArmanPhotography.Config;
using AzizArmanPhotography.ViewModels;

namespace AzizArmanPhotography.Sections
{
    public class FacebookConfig : SectionConfigBase<FacebookSchema>
    {
		private readonly FacebookDataProvider _dataProvider = new FacebookDataProvider(new FacebookOAuthTokens
        {
			AppId = "970354833047368",
                    AppSecret = "8f6e9adad497d08d0557dbef23c52383"
        });

		public override Func<Task<IEnumerable<FacebookSchema>>> LoadDataAsyncFunc
        {
            get
            {
                var config = new FacebookDataConfig
                {
                    UserId = "1485886798386316"
                };

                return () => _dataProvider.LoadDataAsync(config, MaxRecords);
            }
        }

        public override ListPageConfig<FacebookSchema> ListPage
        {
            get 
            {
                return new ListPageConfig<FacebookSchema>
                {
                    Title = "Facebook",

					PageTitle = "Facebook",

                    ListNavigationInfo = NavigationInfo.FromPage("FacebookListPage"),

                    LayoutBindings = (viewModel, item) =>
                    {
                        viewModel.Title = item.Title.ToSafeString();
                        viewModel.SubTitle = item.Summary.ToSafeString();
                        viewModel.ImageUrl = ItemViewModel.LoadSafeUrl(item.ImageUrl.ToSafeString());
                    },
                    DetailNavigation = (item) =>
                    {
                        return NavigationInfo.FromPage("FacebookDetailPage", true);
                    }
                };
            }
        }

        public override DetailPageConfig<FacebookSchema> DetailPage
        {
            get
            {
                var bindings = new List<Action<ItemViewModel, FacebookSchema>>();
                bindings.Add((viewModel, item) =>
                {
                    viewModel.PageTitle = item.Author.ToSafeString();
                    viewModel.Title = "";
                    viewModel.Description = item.Content.ToSafeString();
                    viewModel.ImageUrl = ItemViewModel.LoadSafeUrl(item.ImageUrl.ToSafeString());
                    viewModel.Content = null;
					viewModel.Source = item.FeedUrl;
                });

                var actions = new List<ActionConfig<FacebookSchema>>
                {
                    ActionConfig<FacebookSchema>.Link("Go To Source", (item) => item.FeedUrl.ToSafeString()),
                };

                return new DetailPageConfig<FacebookSchema>
                {
                    Title = "Facebook",
                    LayoutBindings = bindings,
                    Actions = actions
                };
            }
        }
    }
}
