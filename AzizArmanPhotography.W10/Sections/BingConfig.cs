


using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using AppStudio.DataProviders;
using AppStudio.DataProviders.Core;
using AppStudio.DataProviders.Bing;
using AppStudio.Uwp.Actions;
using AppStudio.Uwp.Commands;
using AppStudio.Uwp.Navigation;
using AppStudio.Uwp;
using System.Linq;
using AzizArmanPhotography.Config;
using AzizArmanPhotography.ViewModels;

namespace AzizArmanPhotography.Sections
{
	public class BingConfig : SectionConfigBase<BingSchema>
    {
	    public override Func<Task<IEnumerable<BingSchema>>> LoadDataAsyncFunc
        {
            get
            {
                var config = new BingDataConfig
                {
                    Country = BingCountry.UnitedStates,
                    Query = @"Photography"
                };

                return () => Singleton<BingDataProvider>.Instance.LoadDataAsync(config, MaxRecords);
            }
        }

        public override ListPageConfig<BingSchema> ListPage
        {
            get 
            {
                return new ListPageConfig<BingSchema>
                {
                    Title = "Bing",

					PageTitle = "Bing",

                    ListNavigationInfo = NavigationInfo.FromPage("BingListPage"),

                    LayoutBindings = (viewModel, item) =>
                    {
                        viewModel.Title = item.Title.ToSafeString();
                        viewModel.SubTitle = item.Summary.ToSafeString();
                    },
                    DetailNavigation = (item) =>
                    {
                        return new NavigationInfo
                        {
                            NavigationType = NavigationType.DeepLink,
                            TargetUri = new Uri(item.Link)
                        };
                    }
                };
            }
        }

        public override DetailPageConfig<BingSchema> DetailPage
        {
            get { return null; }
        }
    }
}
