using System;
using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Widget;

namespace Phoneword
{
    [Activity(Label = "Read Database")]
    public class ReadDatabaseActivity : ListActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            // Create your application here

            var persons = Intent.Extras.GetStringArrayList("persons") ?? new string[0];
            this.ListAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, persons);
        }
    }
}