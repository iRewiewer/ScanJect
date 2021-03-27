using System.IO;
using System.Diagnostics;
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

//using https://www.nuget.org/packages/Microsoft.ProjectOxford.Vision/

using Plugin.Media;
using Plugin.Media.Abstractions;
using Xam.Plugins.OnDeviceCustomVision;

using System;

using Plugin.CurrentActivity;

namespace ScanJect
{
    [Activity(Label = "ScanJect", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, NavigationView.IOnNavigationItemSelectedListener
    {
        Button captureButton;
        Button languageButton;
        Button langBack;
        ImageView thisImageView;
        public TextView scanResult;
        bool hasTakenPhoto = false;

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

            // Get buttons
            captureButton = (Button)FindViewById(Resource.Id.captureButton);
            languageButton = (Button)FindViewById(Resource.Id.languageButton);
            langBack = (Button)FindViewById(Resource.Id.langBack);
            thisImageView = (ImageView)FindViewById(Resource.Id.thisImageView);

            // Run functions corresponding to buttons on click
            captureButton.Click += CaptureButton_Click;
            languageButton.Click += LanguageButton_Click;
            langBack.Click += LanguageBackButton_Click;

            // Request permissions
            RequestPermissions(permissionGroup, 0);
        }

        private void CaptureButton_Click(object sender, EventArgs e)
        {
            TakePhoto(sender, e);
        }

        private void LanguageButton_Click(object sender, EventArgs e)
        {
            scanResult = (TextView)FindViewById(Resource.Id.scanResult);

            LinearLayout langPage = (LinearLayout)FindViewById(Resource.Id.lang_layout);
            langPage.Visibility = ViewStates.Visible;
        }

        private void LanguageBackButton_Click(object sender, EventArgs e)
        {
            scanResult = (TextView)FindViewById(Resource.Id.scanResult);
            string translation = Translator.TranslateSample.TranslateText2("en", "ru", "Chair");
            scanResult.Text = scanResult.Text + translation;
            LinearLayout langPage = (LinearLayout)FindViewById(Resource.Id.lang_layout);
            langPage.Visibility = ViewStates.Gone;
        }

        private async void TakePhoto(object sender, System.EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                SaveToAlbum = true,
                PhotoSize = PhotoSize.Full,
                CompressionQuality = 40,
                Name = "myimage.png",
                Directory = "ScanJect"
            });

            if (file == null)
                return;

            byte[] imageArray = File.ReadAllBytes(file.Path);
            Bitmap bitmap = BitmapFactory.DecodeByteArray(imageArray, 0, imageArray.Length);
            thisImageView.SetImageBitmap(bitmap);

            AndroidImageClassifier.Init("model.pb", "labels.txt", ModelType.General);
            scanResult = (TextView)FindViewById(Resource.Id.scanResult);
            scanResult.Text = "Scan Results: " + await ObjectRecog.CustomVisionLocalService.ClassifyImage(file) + " ; ";
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

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        public bool OnNavigationItemSelected(IMenuItem item)
        {
            int id = item.ItemId;

            if (id == Resource.Id.lang_1)
            {
                item.SetTitle("it works!");

            }
            else if (id == Resource.Id.lang_2)
            {

            }
            else if (id == Resource.Id.lang_3)
            {

            }
            else if (id == Resource.Id.obj_1)
            {

            }
            else if (id == Resource.Id.obj_2)
            {

            }
            else if (id == Resource.Id.obj_3)
            {

            }

            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            drawer.CloseDrawer(GravityCompat.Start);
            return true;
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