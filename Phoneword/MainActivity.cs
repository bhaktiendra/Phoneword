using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Linq;
using DatabaseOperations;
using DatabaseOperations.Model;

namespace Phoneword
{
    [Activity(Label = "Phone Word", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        static readonly List<string> phoneNumbers = new List<string>();

        // elements
        EditText phoneNumberText;
        Button translateButton;
        Button callButton;
        Button callHistoryButton;
        Button createDatabaseButton;
        Button insertDataButton;
        Button insertManyDataButton;
        Button readDataButton;
        Button serviceActivityButton;
        TextView textResult;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our UI controls from the loaded layout:
            phoneNumberText = FindViewById<EditText>(Resource.Id.PhoneNumberText);
            translateButton = FindViewById<Button>(Resource.Id.TranslateButton);
            callButton = FindViewById<Button>(Resource.Id.CallButton);
            callHistoryButton = FindViewById<Button>(Resource.Id.CallHistoryButton);
            createDatabaseButton = FindViewById<Button>(Resource.Id.CreateDatabaseButton);
            insertDataButton = FindViewById<Button>(Resource.Id.InsertDataButton);
            insertManyDataButton = FindViewById<Button>(Resource.Id.InsertManyDataButton);
            readDataButton = FindViewById<Button>(Resource.Id.ReadDataButton);
            serviceActivityButton = FindViewById<Button>(Resource.Id.ServiceActivityButton);
            textResult = FindViewById<TextView>(Resource.Id.TextResult);

            // Disable the "Call" button
            callButton.Enabled = false;

            // Add code to translate number
            string translatedNumber = string.Empty;

            translateButton.Click += (object sender, EventArgs e) =>
            {
                // Translate user's alphanumeric phone number to numeric
                translatedNumber = Core.PhonewordTranslator.ToNumber(phoneNumberText.Text);
                if (String.IsNullOrWhiteSpace(translatedNumber))
                {
                    callButton.Text = "Call";
                    callButton.Enabled = false;
                }
                else
                {
                    callButton.Text = "Call " + translatedNumber;
                    callButton.Enabled = true;
                }
            };

            callButton.Click += (object sender, EventArgs e) =>
            {
                // On "Call" button click, try to dial phone number.
                var callDialog = new AlertDialog.Builder(this);
                callDialog.SetMessage("Call " + translatedNumber + "?");
                callDialog.SetNeutralButton("Call", delegate
                {
                    // add dialed number to list of called numbers.
                    phoneNumbers.Add(translatedNumber);
                    // enable the Call History button
                    callHistoryButton.Enabled = true;
                    // Create intent to dial phone
                    var callIntent = new Intent(Intent.ActionCall);
                    callIntent.SetData(Android.Net.Uri.Parse("tel:" + translatedNumber));
                    StartActivity(callIntent);
                });
                callDialog.SetNegativeButton("Cancel", delegate { });

                // Show the alert dialog to the user and wait for response.
                callDialog.Show();
            };

            callHistoryButton.Click += (object sender, EventArgs e) =>
            {
                var intent = new Intent(this, typeof(CallHistoryActivity));
                intent.PutStringArrayListExtra("phone_numbers", phoneNumbers);
                StartActivity(intent);
            };

            // Database code
            // var docsFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
            var docsFolder = "sdcard/android/data/";
            var pathToDatabase = System.IO.Path.Combine(docsFolder, "db_sqlnet.db");

            createDatabaseButton.Click += (object sender, EventArgs e) =>
            {
                Operations.CreateDatabase(pathToDatabase);
            };

            insertDataButton.Click += (object sender, EventArgs e) =>
            {
                var result = Operations.InsertUpdateData(new Person { FirstName = string.Format("John {0}", DateTime.Now.Ticks), LastName = "Smith" }, pathToDatabase);
                var records = Operations.FindNumberRecords(pathToDatabase);
                textResult.Text = string.Format("{0}\nNumber of records = {1}\n", result, records);
            };

            insertManyDataButton.Click += (object sender, EventArgs e) =>
            {
                var peopleList = new List<Person>
                {
                    new Person { FirstName = "Miguel", LastName = string.Format("de Icaza ({0})", DateTime.Now.Ticks) },
                    new Person { FirstName = string.Format("Kevin {0}", DateTime.Now.Ticks), LastName = "Mullins" },
                    new Person { FirstName = "Amy", LastName = string.Format("Burns ({0})", DateTime.Now.Ticks) }
                };
                var result = Operations.InsertUpdateAllData(peopleList, pathToDatabase);
                var records = Operations.FindNumberRecords(pathToDatabase);
                textResult.Text = string.Format("{0}\nNumber of records = {1}\n", result, records);
            };

            readDataButton.Click += (object sender, EventArgs e) =>
            {
                var result = Operations.ReadData(pathToDatabase);

                var intent = new Intent(this, typeof(ReadDatabaseActivity));
                intent.PutStringArrayListExtra("persons", result);
                StartActivity(intent);
            };

            serviceActivityButton.Click += (object sender, EventArgs e) =>
            {
                var intent = new Intent(this, typeof(ServiceDemoActivity));
                StartActivity(intent);
            };
        }

        protected override void OnPause()
        {
            base.OnPause();

        }
    }
}

