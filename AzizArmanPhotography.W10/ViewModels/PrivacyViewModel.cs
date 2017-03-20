using System;
using AppStudio.Uwp;

namespace AzizArmanPhotography.ViewModels
{
    public class PrivacyViewModel : ObservableBase
    {
        public Uri Url
        {
            get
            {
                return new Uri(UrlText, UriKind.RelativeOrAbsolute);
            }
        }
        public string UrlText
        {
            get
            {
                return "http://w8privacy.azurewebsites.net/privacy?dev=Aziz%20Arman&app=Aziz%20Arman%20Photography&mail=bnN1YXJtYW4xOEBvdXRsb29rLmNvbQ==&lng=En";
            }
        }
    }
}

