using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Firebase.Firestore;
using Firebase;
using Java.Util;
using Android.Gms.Tasks;
using Java.Lang;
using System.Collections.Generic;

namespace firestoreArray
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, IOnSuccessListener
    {
        Button saveButton;
        Button retrieveButton;
        Button updateButton;
        TextView resultTextView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            saveButton = (Button)FindViewById(Resource.Id.saveButton);
            retrieveButton = (Button)FindViewById(Resource.Id.retrieveButton);
            updateButton = (Button)FindViewById(Resource.Id.updateButton);
            updateButton.Click += UpdateButton_Click;
            retrieveButton.Click += RetrieveButton_Click;
            saveButton.Click += SaveButton_Click;
            resultTextView = (TextView)FindViewById(Resource.Id.resultTextView);
        }

        private void UpdateButton_Click(object sender, System.EventArgs e)
        {
            DocumentReference phoneref = GetFireStore().Collection("Users").Document("R6t1ueRzrVUc9MnHlK3P");
            phoneref.Update("phone_number.fax", "+45383638738");
        }

        private void RetrieveButton_Click(object sender, System.EventArgs e)
        {
            FirebaseFirestore database = GetFireStore();
            database.Collection("Users").Document("R6t1ueRzrVUc9MnHlK3P").Get().AddOnSuccessListener(this);
        }

        private void SaveButton_Click(object sender, System.EventArgs e)
        {
            HashMap phoneMap = new HashMap();
            phoneMap.Put("mobile", "+16373645383");
            phoneMap.Put("work", "+236894948");
            phoneMap.Put("home", "+3525736523");

            HashMap map = new HashMap();
            map.Put("full_name", "Uchenna Nnodim");
            map.Put("email", "uchenna@gmail.com");
            map.Put("phone_number", phoneMap);

            FirebaseFirestore database;
            database = GetFireStore();

            DocumentReference docRef = database.Collection("Users").Document();
            docRef.Set(map);

        }

        public  FirebaseFirestore GetFireStore()
        {
            var app = FirebaseApp.InitializeApp(this);
            FirebaseFirestore database;
            if (app == null)
            {
                var options = new FirebaseOptions.Builder()
                    .SetProjectId("firestorearray")
                    .SetApplicationId("firestorearray")
                    .SetApiKey("AIzaSyCy7iIY3p9LzTUziW1srCWHCP1tZiEfFTU")
                    .SetDatabaseUrl("https://firestorearray.firebaseio.com")
                    .SetStorageBucket("firestorearray.appspot.com")
                    .Build();

                app = FirebaseApp.InitializeApp(Application.Context, options);
                database = FirebaseFirestore.GetInstance(app);
            }
            else
            {
                database = FirebaseFirestore.GetInstance(app);
            }
            return database;
        }

        public void OnSuccess(Object result)
        {
            var snapshot = (DocumentSnapshot)result;

            string fullname = snapshot.Get("full_name").ToString();
            string email = snapshot.Get("email") != null ? snapshot.Get("email").ToString() : "";
            var phone_number = snapshot.Get("phone_number") != null ? snapshot.Get("phone_number") : null;

            if(phone_number != null)
            {
                var dictionFromHashMap = new Android.Runtime.JavaDictionary<string, string>(phone_number.Handle, Android.Runtime.JniHandleOwnership.DoNotRegister);

                string data_result = "full name = " + fullname + "\n ";
                data_result = data_result + "email = " + email + " \n ";
                foreach (KeyValuePair<string, string> item in dictionFromHashMap)
                {
                    data_result = data_result + item.Key + " = " + item.Value + " \n ";                   
                }

                resultTextView.Text = data_result;
            }
        }
    }
}