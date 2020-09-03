//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Android.App;
//using Android.Content;
//using Android.Gms.Tasks;
//using Android.OS;
//using Android.Runtime;
//using Android.Views;
//using Android.Widget;
//using Firebase.Firestore;
//using Java.Interop;
//using Newtonsoft.Json;

//namespace LoveCCA.Droid
//{





//    class DatabaseDroid
//    {
//        private FirebaseFirestore _instance;

//        public DatabaseDroid()
//        {

//        }

//        public void OnComplete(Android.Gms.Tasks.Task task)
//        {
//            if (task.IsSuccessful)
//            {
//                DocumentSnapshot doc = task.Result as DocumentSnapshot;
//                if (doc.Exists())
//                {
//                    Console.WriteLine($"Doc all read OK: {doc.Data}");
//                }
//            }
//        }

//        public void OnFailure(Java.Lang.Exception e)
//        {
//            Console.WriteLine($"Exception reading doc: {e.Message}");
//        }

//        public void Connect(Firebase.FirebaseApp app)
//        {
//            _instance = FirebaseFirestore.GetInstance(app);
//        }



//        public async Task SetUser()
//        {
//            var document = _instance.Collection("users").Document("022dbf27-f887-4209-89b2-535e0d738e70");
//            document.Get()
//            .AddOnCompleteListener(this)
//            .AddOnFailureListener(this);

//            //document.Get().AddOnCompleteListener(new OnCompleteEventHandleListener((Android.Gms.Tasks.Task obj) => {
//            //    if (obj.IsSuccessful) { DocumentSnapshot res = obj.GetResult(Class.FromType(typeof(DocumentSnapshot)))
//            //        .JavaCast<DocumentSnapshot>(); 
//            //        recipe = JsonConvert.DeserializeObject<RecipeModel>(res.GetBlob("FileData").ToString()); }
//            //    else
//            //    {
//            //        //TODO: Do something with the error recipe = null; 
//            //    }
//            //}));



//            //var dict = new Android.Runtime.JavaDictionary<string, object>(){
//            //    { "Name", "San Francisco" },
//            //    { "State", "CA" },
//            //    { "Country", "USA" },
//            //    { "Capital", false },
//            //    { "Population", 860000 },
//            //    { "Regions", new ArrayList{"west_coast", "norcal"} }
//            //};

//            //usersRef.Document("SF").Set(dict);
//        }

//    }
//}