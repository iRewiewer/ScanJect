using System;
using System.IO;
using Android;
using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using static Android.Widget.AdapterView;
using ScanJect.Translator;
using Plugin.Media;
using Plugin.CurrentActivity;
using Plugin.Media.Abstractions;
using Xam.Plugins.OnDeviceCustomVision;

#pragma warning disable CS0618 // Type or member is obsolete - lang_list.SetAdapter(adapter);
#pragma warning disable IDE0052 // Remove unread private members - for lang_page

namespace ScanJect
{
    [Activity(Label = "ScanJect", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, NavigationView.IOnNavigationItemSelectedListener, IOnItemClickListener
    {
        Button captureButton;
        ImageView scannedImage;
		LinearLayout lang_page;
		TextView lang_text;
        ListView lang_list;

        IMenuItem currentItem;
        String main_lang = "en";
        String second_lang = "ro"; // the lang codes
        String lang_code;
        String lang_name;

        public TextView scanResult;

        readonly string[] permissionGroup =
        {
            Manifest.Permission.ReadExternalStorage,
            Manifest.Permission.WriteExternalStorage,
            Manifest.Permission.Camera
        };


        protected override void OnCreate(Bundle savedInstanceState)
        {
            // Initiate Xamarin, Classifier & App Instance
            base.OnCreate(savedInstanceState);
            AndroidImageClassifier.Init("model.pb", "labels.txt", ModelType.General);
            CrossCurrentActivity.Current.Init(this, savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            // Set app page on launch
            SetContentView(Resource.Layout.activity_main);

            // Widget toolbar
            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            // Drawer Layout
            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            ActionBarDrawerToggle toggle = new ActionBarDrawerToggle(this, drawer, toolbar, Resource.String.navigation_drawer_open, Resource.String.navigation_drawer_close);
            drawer.AddDrawerListener(toggle);
            toggle.SyncState();

            // Navbar - User Profile
            NavigationView navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            navigationView.SetNavigationItemSelectedListener(this);

            // Get xml elements
            captureButton = (Button)FindViewById(Resource.Id.captureButton);
            scannedImage = (ImageView)FindViewById(Resource.Id.scannedImage);

            // Run functions corresponding to buttons on click
            captureButton.Click += ScanButton_Click;

            // Request permissions
            RequestPermissions(permissionGroup, 0);
        }

        private void ScanButton_Click(object sender, EventArgs e)
        {
            // if the user hasn't selected any language yet, don't let them scan
            //if (main_lang == "none" || second_lang == "none") second_lang = "x";
            //else second_lang = "x";
            
            TakePhoto(sender, e);
        }

        private async void TakePhoto(object sender, EventArgs e)
        {
            // Initialitze the plugin
            await CrossMedia.Current.Initialize();

            // Take photo
            var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                SaveToAlbum = true,
                PhotoSize = PhotoSize.Full,
                CompressionQuality = 40,
                Name = "myimage.png",
                Directory = "ScanJect"
            });

            // If photo couldn't be taken
            if (file == null)
                return;
            
            // Convert to bitmap in order to display it
            byte[] imageArray = File.ReadAllBytes(file.Path);
            Bitmap bitmap = BitmapFactory.DecodeByteArray(imageArray, 0, imageArray.Length);
            scannedImage.SetImageBitmap(bitmap);

            // Initialize object recog model
            AndroidImageClassifier.Init("model.pb", "labels.txt", ModelType.General);

            // Get object & confidence
            string scan_res = await ObjectRecog.CustomVisionLocalService.ClassifyImage(file);
            string[] words = scan_res.Split(' ');
            string scanned_object = words[0];
            string confidence = "\nConfidence: " + words[1] + " %";

            // Add to database
            // words[0];
            SQL.Database.db_add("test");

            // Translation part
            string translation;
            if (main_lang != "en") translation = TranslateSample.TranslateText2(main_lang, scanned_object);
            else translation = scanned_object;
            translation += " | ";
            if (second_lang != "en") translation += TranslateSample.TranslateText2(second_lang, scanned_object);
            else translation += scanned_object;

            // Add to scan results
            scanResult = (TextView)FindViewById(Resource.Id.scanResult);
            scanResult.Text = "Scan Results: ";
            scanResult.Text += translation;
            scanResult.Text += confidence;
        }

        public override void OnBackPressed()
        {
            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            if (drawer.IsDrawerOpen(GravityCompat.Start))
            {
                drawer.CloseDrawer(GravityCompat.Start);
            }
            else
            {
                base.OnBackPressed();
            }
        }

        public bool OnNavigationItemSelected(IMenuItem item)
        {
            int id = item.ItemId;

            if (id == Resource.Id.lang_1)
            {
                currentItem = item;
            }
            else if (id == Resource.Id.lang_2)
            {
                currentItem = item;
            }
            else if (id == Resource.Id.lang_3)
            {
                currentItem = item;
            }
            else if (id == Resource.Id.obj_1)
            {
                currentItem = item;
            }
            else if (id == Resource.Id.obj_2)
            {
                currentItem = item;
            }
            else if (id == Resource.Id.obj_3)
            {
                currentItem = item;
            }
            else if (id == Resource.Id.lang_select_1)
            {
                currentItem = item;
                LangPage();
            }
            else if (id == Resource.Id.lang_select_2)
            {
                currentItem = item;
                LangPage();
            }

            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            drawer.CloseDrawer(GravityCompat.Start);
            return true;
        }

		public void LangPage()
		{
            lang_list = (ListView)FindViewById(Resource.Id.lang_list);
            lang_text = (TextView)FindViewById(Resource.Id.lang_text);
            lang_page = (LinearLayout)FindViewById(Resource.Layout.lang_page);

            // Make View visible
            View lang_incl = (View)FindViewById(Resource.Id.lang_incl);
            lang_incl.Visibility = ViewStates.Visible;

            lang_text.MovementMethod = new Android.Text.Method.ScrollingMovementMethod();

            string[] items = Resources.GetStringArray(Resource.Array.language_array);
            ArrayAdapter adapter = new ArrayAdapter<String>(this, Resource.Layout.lang_page, Resource.Id.lang_text, items);
            lang_list.SetAdapter(adapter);

			lang_list.OnItemClickListener = this;
        }

        // Item clicked in lang_text, adapted to lang_list on lang_page
        public void OnItemClick(AdapterView parent, View view, int position, long id)
        {
            ListView lang_list = (ListView)FindViewById(Resource.Id.lang_list);
            String item = (String)lang_list.GetItemAtPosition(position);

            // Make View invisible again
            View lang_incl = (View)FindViewById(Resource.Id.lang_incl);
            lang_incl.Visibility = ViewStates.Gone;

            lang_code = TranslateSample.langSwitch(item);
            lang_name = item;

            currentItem.SetTitle(lang_name);

            if(currentItem.ItemId == Resource.Id.lang_select_1) main_lang = lang_code;
            else if(currentItem.ItemId == Resource.Id.lang_select_2) second_lang = lang_code;
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
	}

    public class MainApplication : Application, Application.IActivityLifecycleCallbacks
    {
        public MainApplication(IntPtr handle, JniHandleOwnership transer) : base(handle, transer)
        {

        }

        public override void OnCreate()
        {
            base.OnCreate();
            RegisterActivityLifecycleCallbacks(this);
        }

        public override void OnTerminate()
        {
            base.OnTerminate();
            UnregisterActivityLifecycleCallbacks(this);
        }

        public void OnActivityCreated(Activity activity, Bundle savedInstanceState)
        {
            CrossCurrentActivity.Current.Activity = activity;
        }

        public void OnActivityDestroyed(Activity activity)
        {
        }

        public void OnActivityPaused(Activity activity)
        {
        }

        public void OnActivityResumed(Activity activity)
        {
            CrossCurrentActivity.Current.Activity = activity;
        }

        public void OnActivitySaveInstanceState(Activity activity, Bundle outState)
        {
        }

        public void OnActivityStarted(Activity activity)
        {
            CrossCurrentActivity.Current.Activity = activity;
        }

        public void OnActivityStopped(Activity activity)
        {
        }
    }
}