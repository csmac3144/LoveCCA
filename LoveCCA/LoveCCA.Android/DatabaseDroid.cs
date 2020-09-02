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
using Firebase.Firestore;

namespace LoveCCA.Droid
{
    class DatabaseDroid
    {
        public void Connect(Firebase.FirebaseApp app)
        {
            var conn = FirebaseFirestore.GetInstance(app);

        }
    }
}