using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BicycleRack.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NearByLocationPage : ContentPage
    {
        public NearByLocationPage()
        {
            InitializeComponent();
            this.BindingContext = new
            {
                Title = "Near by me"
            };
        }
    }
}