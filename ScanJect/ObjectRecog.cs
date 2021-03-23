using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using Android.Graphics;

using Plugin.Media.Abstractions;
using Org.Tensorflow.Contrib.Android;
using Plugin.Media;

using System.ComponentModel;
using System.Runtime.CompilerServices;

using Xam.Plugins.OnDeviceCustomVision;
using Org.Tensorflow;

//[assembly: Dependency(typeof(AndroidImageClassifier))]
[assembly: Xamarin.Forms.Dependency(typeof(ScanJect.ObjectRecog.AndroidImageClassifier))]
namespace ScanJect.ObjectRecog
{ 
    public class Prediction
    {
        public string TagId { get; set; }
        public string TagName { get; set; }
        public double Probability { get; set; }
    }

    public class CustomVisionResult
    {
        public string Id { get; set; }
        public string Project { get; set; }
        public string Iteration { get; set; }
        public DateTime Created { get; set; }
        public List<Prediction> Predictions { get; set; }
    }

    public interface IImageClassifier
    {
        Task<string> AnalyzeImage(MediaFile image);
    }

    public static class ImageService
    {
        public static async Task<MediaFile> TakePhoto()
        {
            MediaFile picture = null;

            try
            {
                //picture = await CrossMedia.Current.PickPhotoAsync();

                if (CrossMedia.Current.IsCameraAvailable && CrossMedia.Current.IsTakePhotoSupported)
                {
                    picture = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                    {
                        Directory = "Pictures",
                        Name = "photo.jpg"
                    });
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }

            return picture;
        }
    }

    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class AndroidImageClassifier : IImageClassifier
    {
        readonly List<string> labels;
        readonly TensorFlowInferenceInterface inferenceInterface;

        public AndroidImageClassifier()
        {
            try
            {
                var assets = Android.App.Application.Context.Assets;
                inferenceInterface = new TensorFlowInferenceInterface(assets, "model.pb");

                using (var sr = new StreamReader(assets.Open("labels.txt")))
                {
                    var content = sr.ReadToEnd();
                    labels = content.Split('\n').Select(s => s.Trim()).Where(s => !string.IsNullOrEmpty(s)).ToList();
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
        }

        static readonly int InputSize = 227;
        static readonly string InputName = "Placeholder";
        static readonly string OutputName = "loss";

        public async Task<string> AnalyzeImage(MediaFile image)
        {
            var bitmap = await BitmapFactory.DecodeStreamAsync(image.GetStreamWithImageRotatedForExternalStorage());

            var outputNames = new[] { OutputName };
            var floatValues = GetBitmapPixels(bitmap);
            var outputs = new float[labels.Count];

            inferenceInterface.Feed(InputName, floatValues, 1, InputSize, InputSize, 3);
            inferenceInterface.Run(outputNames);
            inferenceInterface.Fetch(OutputName, outputs);

            var results = new List<Tuple<float, string>>();
            for (var i = 0; i < outputs.Length; ++i)
                results.Add(Tuple.Create(outputs[i], labels[i]));

            return results.OrderByDescending(t => t.Item1).First().Item2;
        }

        static float[] GetBitmapPixels(Bitmap bitmap)
        {
            var floatValues = new float[227 * 227 * 3];

            using (var scaledBitmap = Bitmap.CreateScaledBitmap(bitmap, 227, 227, false))
            {
                using (var resizedBitmap = scaledBitmap.Copy(Bitmap.Config.Argb8888, false))
                {
                    var intValues = new int[227 * 227];
                    resizedBitmap.GetPixels(intValues, 0, resizedBitmap.Width, 0, 0, resizedBitmap.Width, resizedBitmap.Height);

                    for (int i = 0; i < intValues.Length; ++i)
                    {
                        var val = intValues[i];

                        floatValues[i * 3 + 0] = ((val & 0xFF) - 104);
                        floatValues[i * 3 + 1] = (((val >> 8) & 0xFF) - 117);
                        floatValues[i * 3 + 2] = (((val >> 16) & 0xFF) - 123);
                    }

                    resizedBitmap.Recycle();
                }

                scaledBitmap.Recycle();
            }

            return floatValues;
        }
    }

    public static class CustomVisionLocalService
    {
        public async static Task<string> ClassifyImage(MediaFile picture)
        {
            var predictions = await CrossImageClassifier.Current.ClassifyImage(picture.GetStreamWithImageRotatedForExternalStorage());
            var topPrediction = predictions.OrderByDescending(t => t.Probability).First();
            return $"{topPrediction.Tag} ({Math.Round(topPrediction.Probability * 100, 2):0.##} %)";
        }
    }
}
