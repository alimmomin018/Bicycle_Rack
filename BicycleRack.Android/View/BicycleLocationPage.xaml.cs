using Plugin.ExternalMaps;
using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace BicycleRack.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BicycleLocationPage : ContentPage
    {
        private Pin savedPin;

        public BicycleLocationPage(double latitude, double longitude, string address)
        {
            InitializeComponent();
            GetGPS();
            this.BindingContext = new
            {
                Title = address
            };

            Map.MoveToRegion(MapSpan.FromCenterAndRadius(new Xamarin.Forms.Maps.Position(latitude,
                longitude),
                Distance.FromMiles(1)));
            var pin = new Pin
            {
                Type = PinType.Place,
                Label = address,
                Address = "Get direction",
                Position = new Xamarin.Forms.Maps.Position(latitude, longitude)
            };
            Map.Pins.Add(pin);

            pin.Clicked += Pin_Clicked;
        }

        private void Pin_Clicked(object sender, EventArgs e)
        {
            var selectedPin = sender as Pin;
            savedPin = selectedPin;
            CrossExternalMaps.Current.NavigateTo(selectedPin.Label, selectedPin.Position.Latitude, selectedPin.Position.Longitude, Plugin.ExternalMaps.Abstractions.NavigationType.Default);
        }

        protected async void GetGPS()
        {
            var locator = CrossGeolocator.Current;
            var position = await locator.GetPositionAsync(TimeSpan.FromMilliseconds(10000));
        }
    }
}