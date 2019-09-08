using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BicycleRack.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddNewLocation : ContentPage
    {
        private const string url = "http://192.168.2.94:9191/api/allBicycleRacks";
        private readonly HttpClient _client = new HttpClient();

        public AddNewLocation()
        {
            InitializeComponent();
            this.BindingContext = new
            {
                Title = "Add new location"
            };
        }

        private void ToolbarItem_Activated(object sender, EventArgs e)
        {
            var position = Map.VisibleRegion.Center;


        }
    }
}