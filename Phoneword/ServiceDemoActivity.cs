using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Phoneword
{
    [Activity(Label = "Service Demo")]
    public class ServiceDemoActivity : Activity
    {
        Button startServiceButton;
        Button stopServiceButton;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.ServiceDemo);

            startServiceButton = FindViewById<Button>(Resource.Id.StartServiceButton);
            stopServiceButton = FindViewById<Button>(Resource.Id.StopServiceButton);

            startServiceButton.Click += (sender, e) =>
            {
                this.StartService(new Intent(this, typeof(SimpleService)));
            };

            stopServiceButton.Click += (sender, e) =>
            {
                this.StopService(new Intent(this, typeof(SimpleService)));
            };
        }
    }
}