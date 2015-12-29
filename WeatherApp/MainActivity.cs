using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using WeatherTracker;
using Android.Content.Res;
using Android.Graphics.Drawables;
using System.Threading;

namespace WeatherApp
{
    [Activity(Label = "WeatherApp", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        public TextView locationText;
        public TextView stateText;
        public TextView atmText;
        public TextView windText;
        public TextView lastUpd;
        public ImageView mainImage;
        public Timer myTimer;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            mainImage = FindViewById<ImageView>(Resource.Id.weatherStateImg);
            stateText = FindViewById<TextView>(Resource.Id.strStateTxt);
            locationText = FindViewById<TextView>(Resource.Id.locationTxt);
            windText = FindViewById<TextView>(Resource.Id.windTxt);
            atmText = FindViewById<TextView>(Resource.Id.atmTxt);
            lastUpd = FindViewById<TextView>(Resource.Id.lastUpdate);
            weatherQuery();
        }


        public void bindData(WeatherInfo wi)
        {
            RunOnUiThread(() =>
            {
                locationText.Text = wi.City;
                stateText.Text = wi.State;
                atmText.Text = wi.Temperature + "°C  " + wi.Humidity + " %  " + wi.Pressure + "hPa";
                windText.Text = wi.WindState + " - " + wi.WindDirectionValue;
                lastUpd.Text = wi.Date.ToShortTimeString();
                Context context = this;
                // Get the Resources object from our context
                Resources res = context.Resources;
                string fileName = "_" + wi.IconId;//+ ".png";

                int resId = res.GetIdentifier(fileName, "drawable", this.PackageName);
                mainImage.SetImageResource(resId);
            });
        }

        protected void weatherQuery()
        {
            WeatherInfo wi = new WeatherInfo();
            TimerCallback trCB = async (object state) =>
           {
               await ((WeatherInfo)state).WeatherQuery();
               bindData((WeatherInfo)state);
           };
            myTimer = new Timer(trCB, wi, 0, (int)wi.nHours * 3600000);
        }

    }
}

