using Android.Util;
using BlackList.UI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using System.Text.Json;
using Xamarin.Forms;
using static Android.Media.Audiofx.DynamicsProcessing;

namespace BlackList.UI
{
    public partial class MainPage : ContentPage
    {
        public   MainPage()
        {
            InitializeComponent();
            //getBlacklistedCities().GetAwaiter().GetResult();
        }

        static readonly HttpClient client = new HttpClient();
        private List<BlackListedStates> BlackListedstatesList;
        CancellationTokenSource cts;
        async void OnButtonClicked(object sender, EventArgs args)
        {

            LocationModel loc = await GetCurrentLocation();
            var city= await GetCurrentCity(loc.Latitude, loc.Longitude);
            await getBlacklistedCities();

            if (checkIfCityIsBlackListed(city))
            {
                result.Text = "Your city is black listed";
            }
            else
            {
                result.Text = "You are not in a blacklisted City";
            }

        }
        public async Task<string> GetCurrentCity(double latitude, double longitude)
        {
            HttpResponseMessage response = await client.GetAsync($"https://api.bigdatacloud.net/data/reverse-geocode-client?latitude={latitude}&longitude={longitude}&localityLanguage=en");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            responseBody = responseBody.Replace("\"", "\'");
            responseBody = responseBody.Replace("\n", " ");
            return responseBody;
        }
        
        public bool checkIfCityIsBlackListed( string city)
        {
            if (String.IsNullOrEmpty(city)) { return false; }
            return BlackListedstatesList.Any(x => x.states == city);
        }
        async Task getBlacklistedCities()
        {
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri("https://192.168.0.97:7120/api/BlackListedStates"),
                Method = HttpMethod.Get,
               
            };
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = await client.SendAsync(request);
            string responseBody = await response.Content.ReadAsStringAsync();
            //JavaScriptSerializer js = new JavaScriptSerializer();
            dynamic json= JsonConvert.DeserializeObject(responseBody);
           // List<BlackListedStates> cityList = json;

           // BlackListedstatesList = cityList;
        }

        
        async Task<LocationModel> GetCurrentLocation()
        {
            LocationModel locationModel = new LocationModel();
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                cts = new CancellationTokenSource();
                var location = await Geolocation.GetLocationAsync(request, cts.Token);

                if (location != null)
                {
                    locationModel.Longitude = location.Longitude;
                    locationModel.Latitude = location.Latitude;
                    locationModel.Altitude = location.Altitude;
                    return locationModel;
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                throw fnsEx;
            }
        
            catch (PermissionException pEx)
            {
                throw pEx;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }
    }
}
