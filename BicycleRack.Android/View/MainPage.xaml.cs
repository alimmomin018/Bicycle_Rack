using BicycleRack.Model;
using BicycleRack.Utility;
using BicycleRack.View;
using Newtonsoft.Json;
using Plugin.Geolocator;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BicycleRack
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        private const string url = "http://192.168.2.94:9191/api/allBicycleRacks";
        private readonly HttpClient _client = new HttpClient();
        protected ObservableCollection<RackLocation> _rackLocations { get; set; }
        private RackLocation _selectedLocation = null;

        public MainPage()
        {
            InitializeComponent();
            //check for permission
            //Permissions permissions = new Permissions();
            //permissions.LocaitonPermission();
            Permissions.LocaitonPermission();
            if (Permissions.locationStatus == PermissionStatus.Granted)
            {
                Permissions.CurrentPosition();
            }
        }

        protected async override void OnAppearing()
        {
            string requestData = await _client.GetStringAsync(url);
            var response = JsonConvert.DeserializeObject<List<RackLocation>>(requestData);
            _rackLocations = new ObservableCollection<RackLocation>(response);
            rackLocation.ItemsSource = _rackLocations;
            //persmission for location
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
                //status false
                if (status != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location))
                    {
                        await DisplayAlert("Need Location", "We need location permission", "OK");
                    }
                    var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location);

                    if (results.ContainsKey(Permission.Location))
                    {
                        status = results[Permission.Location];
                    }
                }
                //status true
                if (status == PermissionStatus.Granted)
                {
                    try
                    {
                        //get location
                        var locator = CrossGeolocator.Current;
                        var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(10));
                        await DisplayAlert("Location", $"{position.Latitude},{position.Longitude}", "OK");
                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert("error", ex.ToString(), "OK");
                    }
                }//status unknown
                else if (status != PermissionStatus.Unknown)
                {
                    await DisplayAlert("Location Denied", "Can not continue", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.ToString(), "OK");
            }
            base.OnAppearing();
        }

        private void GetLocation_Tapped(object sender, EventArgs e)
        {
            var Item = _selectedLocation;
            Navigation.PushAsync(new BicycleLocationPage(double.Parse(Item.Latitude), double.Parse(Item.Longitude), Item.Address));
        }

        private void rackLocation_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var itemTapped = e.Item as RackLocation;
            var data = _rackLocations.ElementAt(itemTapped.Id - 1);
            //new item selected is not null or is same as previously selected is before
            if (_selectedLocation == null || itemTapped == _selectedLocation)
            {   //collapse the selected item if it is expanded
                if (data.IsVisible == true)
                {
                    data.IsVisible = false;
                }
                //if it is not expanded...expand it
                else if (data.IsVisible == false)
                {
                    data.IsVisible = true;
                }
            }
            //selected item is not same
            else if (_selectedLocation != itemTapped)
            {
                _selectedLocation.IsVisible = false;
                itemTapped.IsVisible = true;
            }
            _selectedLocation = itemTapped;
            rackLocation.SelectedItem = null;
        }

        private void searchLocation_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(e.NewTextValue))
            {
                rackLocation.ItemsSource = _rackLocations;
            }
            else
            {
                rackLocation.ItemsSource = _rackLocations.Where(x => x.Address.ToLower().Contains(e.NewTextValue.ToLower()));
            }
        }

        private void NearByMe(object sender, EventArgs e)
        {
            Navigation.PushAsync(new NearByLocationPage());
        }

        private void AddNewLocation(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddNewLocation());
        }
    }
}
