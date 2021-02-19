using System;
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
using Plugin.Media;

namespace ScanJect
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]

    public class MainActivity : AppCompatActivity, NavigationView.IOnNavigationItemSelectedListener
    {
        Button captureButton;
        Button languageButton;
        Button langBack;
        ImageView thisImageView;
        public TextView scanResult;
        private string API_Key = "27bf10b3d67b45cf9d615bc66fc7a577";

        readonly string[] permissionGroup =
        {
            Manifest.Permission.ReadExternalStorage,
            Manifest.Permission.WriteExternalStorage,
            Manifest.Permission.Camera
        };

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            SetContentView(Resource.Layout.activity_main);
            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            ActionBarDrawerToggle toggle = new ActionBarDrawerToggle(this, drawer, toolbar, Resource.String.navigation_drawer_open, Resource.String.navigation_drawer_close);
            drawer.AddDrawerListener(toggle);
            toggle.SyncState();

            NavigationView navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            navigationView.SetNavigationItemSelectedListener(this);

            captureButton = (Button)FindViewById(Resource.Id.captureButton);
            languageButton = (Button)FindViewById(Resource.Id.languageButton);
            langBack = (Button)FindViewById(Resource.Id.langBack);
            thisImageView = (ImageView)FindViewById(Resource.Id.thisImageView);

            captureButton.Click += CaptureButton_Click;
            languageButton.Click += LanguageButton_Click;
            langBack.Click += LanguageBackButton_Click;
            RequestPermissions(permissionGroup, 0);
        }

        private void CaptureButton_Click(object sender, System.EventArgs e)
        {
            takePhoto();
            scanResult = (TextView)FindViewById(Resource.Id.scanResult);
            scanResult.Text = "Scan Results: \"Wallet\" 96%";
        }

        private void LanguageButton_Click(object sender, System.EventArgs e)
        {
            scanResult = (TextView)FindViewById(Resource.Id.scanResult);
            scanResult.Text = "Scan Results: \"財布\" | \"Saifu\" (Japanese) 96%";

            LinearLayout langPage = (LinearLayout)FindViewById(Resource.Id.lang_layout);
            langPage.Visibility = ViewStates.Visible;
        }

        private void LanguageBackButton_Click(object sender, System.EventArgs e)
        {
            LinearLayout langPage = (LinearLayout)FindViewById(Resource.Id.lang_layout);
            langPage.Visibility = ViewStates.Gone;
        }

        async void takePhoto()
		{
            await CrossMedia.Current.Initialize();

            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,
                CompressionQuality = 40,
                Name = "myimage.jpg",
                Directory = "sample"
            });

            if(file == null)
			{
                return;
			}

            byte[] imageArray = System.IO.File.ReadAllBytes(file.Path);
            Bitmap bitmap = BitmapFactory.DecodeByteArray(imageArray, 0, imageArray.Length);
            thisImageView.SetImageBitmap(bitmap);
		}

        private void obj_recog()
		{

        }

        public override void OnBackPressed()
        {
            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            if(drawer.IsDrawerOpen(GravityCompat.Start))
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
                //IMenuItem lang_1 = (IMenuItem)FindViewById(Resource.Id.lang_1);
                //lang_1.GetType();
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
}

