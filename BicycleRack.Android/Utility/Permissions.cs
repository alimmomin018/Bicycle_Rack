using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace BicycleRack.Utility
{
    public class Permissions
    {
        public static Position position { get; set; }
        public static PermissionStatus locationStatus { get; set; }

        public static async void LocaitonPermission()
        {
            //persmission for location
            try
            {
                locationStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
                //status false
                if (locationStatus != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location))
                    {
                        //await DisplayAlert("Need Location", "We need location permission", "OK");
                    }
                    var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location);

                    if (results.ContainsKey(Permission.Location))
                    {
                        locationStatus = results[Permission.Location];
                    }
                }
                //status true
                if (locationStatus == PermissionStatus.Granted)
                {
                    try
                    {
                        //get location
                        var locator = CrossGeolocator.Current;
                        var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(10));
                        //await DisplayAlert("Location", $"{position.Latitude},{position.Longitude}", "OK");
                    }
                    catch (Exception ex)
                    {
                        //await DisplayAlert("error", ex.ToString(), "OK");
                    }
                }//status unknown
                else if (locationStatus != PermissionStatus.Unknown)
                {
                    //await DisplayAlert("Location Denied", "Can not continue", "OK");
                }
            }
            catch (Exception ex)
            {
                //await DisplayAlert("Error", ex.ToString(), "OK");
            }
        }

        public static async void CurrentPosition()
        {
            var locator = CrossGeolocator.Current;
            position = await locator.GetPositionAsync(TimeSpan.FromSeconds(10));
        }
    }
}
