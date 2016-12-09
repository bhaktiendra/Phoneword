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
using SQLite;
using DatabaseOperations.Model;

namespace DatabaseOperations
{
    public static class Operations
    {
        public static string CreateDatabase(string path)
        {
            try
            {
                var connection = new SQLiteAsyncConnection(path);
                {
                    connection.CreateTableAsync<Person>();
                    return "Database created";
                }
            }
            catch (SQLiteException ex)
            {
                return ex.Message;
            }
        }

        public static string InsertUpdateData(Person data, string path)
        {
            try
            {
                var db = new SQLiteConnection(path);
                if (db.Insert(data) != 0)
                    db.Update(data);
                return "Single data file inserted or updated";
            }
            catch (SQLiteException ex)
            {
                return ex.Message;
            }
        }

        public static string InsertUpdateAllData(IEnumerable<Person> data, string path)
        {
            try
            {
                var db = new SQLiteConnection(path);
                if (db.InsertAll(data) != 0)
                    db.UpdateAll(data);
                return "List of data inserted or updated";
            }
            catch (SQLiteException ex)
            {
                return ex.Message;
            }
        }

        public static int FindNumberRecords(string path)
        {
            try
            {
                var db = new SQLiteConnection(path);
                // this counts all records in the database, it can be slow depending on the size of the database
                var count = db.ExecuteScalar<int>("SELECT Count(*) FROM Person");

                // for a non-parameterless query
                // var count = db.ExecuteScalar<int>("SELECT Count(*) FROM Person WHERE FirstName="Amy");

                return count;
            }
            catch (SQLiteException)
            {
                return -1;
            }
        }

        public static List<string> ReadData(string path)
        {
            var list = new List<string>();

            try
            {
                var db = new SQLiteConnection(path);

                foreach (var p in db.Table<Person>())
                {
                    list.Add(p.FirstName);
                }

                return list;
            }
            catch (SQLiteException)
            {
                return list;
            }
        }
    }
}