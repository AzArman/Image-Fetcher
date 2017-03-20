


using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using AppStudio.DataProviders;
using AppStudio.DataProviders.Core;
using AppStudio.DataProviders.Flickr;
using AppStudio.Uwp.Actions;
using AppStudio.Uwp.Commands;
using AppStudio.Uwp.Navigation;
using AppStudio.Uwp;
using System.Linq;
using AzizArmanPhotography.Config;
using AzizArmanPhotography.ViewModels;

namespace AzizArmanPhotography.Sections
{
    public class FlickrConfig : SectionConfigBase<FlickrSchema>
    {
		public override Func<Task<IEnumerable<FlickrSchema>>> LoadDataAsyncFunc
        {
            get
            {
                var config = new FlickrDataConfig
                {
                    QueryType = FlickrQueryType.Id,
                    Query = @"AzizArman"
                };

                return () => Singleton<FlickrDataProvider>.Instance.LoadDataAsync(config, MaxRecords);
            }
        }

        public override ListPageConfig<FlickrSchema> ListPage
        {
            get 
            {
                return new ListPageConfig<FlickrSchema>
                {
                    Title = "Flickr",

					PageTitle = "Flickr",

                    ListNavigationInfo = NavigationInfo.FromPage("FlickrListPage"),

                    LayoutBindings = (viewModel, item) =>
                    {
                        viewModel.Title = item.Title.ToSafeString();
                        viewModel.SubTitle = item.Summary.ToSafeString();
                        viewModel.ImageUrl = ItemViewModel.LoadSafeUrl(item.ImageUrl.ToSafeString());
                    },
                    DetailNavigation = (item) =>
                    {
                        return NavigationInfo.FromPage("FlickrDetailPage", true);
                    }
                };
            }
        }

        public override DetailPageConfig<FlickrSchema> DetailPage
        {
            get
            {
                var bindings = new List<Action<ItemViewModel, FlickrSchema>>();
                bindings.Add((viewModel, item) =>
                {
                    viewModel.PageTitle = item.Title.ToSafeString();
                    viewModel.Title = item.Title.ToSafeString();
                    viewModel.Description = item.Summary.ToSafeString();
                    viewModel.ImageUrl = ItemViewModel.LoadSafeUrl(item.ImageUrl.ToSafeString());
                    viewModel.Content = null;
					viewModel.Source = item.FeedUrl;
                });

                var actions = new List<ActionConfig<FlickrSchema>>
                {
                    ActionConfig<FlickrSchema>.Link("Go To Source", (item) => item.FeedUrl.ToSafeString()),
                };

                return new DetailPageConfig<FlickrSchema>
                {
                    Title = "Flickr",
                    LayoutBindings = bindings,
                    Actions = actions
                };
            }
        }
    }
}
