using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;

namespace CocktailBuddy.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Tabs : Xamarin.Forms.TabbedPage
    {
        public Tabs()
        {
            InitializeComponent();
        }
    }
}