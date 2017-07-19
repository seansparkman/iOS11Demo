using System;
using System.Threading;
using CoreGraphics;
using CoreML;
using Foundation;
using UIKit;

namespace iOS11Test.MachineLearning
{
    public partial class MachineLearningViewController : UIViewController
    {
        MLModel model;
        UIImageView imageView = new UIImageView();
        public MachineLearningViewController()
        {
            imageView.Image = UIImage.FromBundle("Baymax");
            imageView.ContentMode = UIViewContentMode.ScaleAspectFit;
            Title = "Machine Learning";
            View = imageView;
            NavigationItem.RightBarButtonItem = new UIBarButtonItem(UIBarButtonSystemItem.Camera, PickImage);
            LoadModel();
        }

        private void PickImage(object sender, EventArgs args)
        {
            var keyWindow = UIApplication.SharedApplication.KeyWindow;

            var picker = new UIImagePickerController
            {
                AllowsEditing = false,
                SourceType = UIImagePickerControllerSourceType.PhotoLibrary
            };

            picker.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;

            picker.FinishedPickingMedia += (s, e) => {
                DismissViewController(true, () => {
                    keyWindow.MakeKeyWindow();
                    imageView.Image = e.OriginalImage;
                    ThreadPool.QueueUserWorkItem(o => MakePrediction(e.OriginalImage));
                });
            };

            PresentViewController(picker, true, null);
        }

        private void LoadModel()
        {
            var modelUrl = NSBundle.MainBundle.GetUrlForResource("SqueezeNet", "mlmodelc");

            model = MLModel.FromUrl(modelUrl, out var error);

            if (error != null) {
                Console.WriteLine($"Error writing model: {error}");
            }
            else {
                Console.WriteLine($"Loaded model: {model}");
            }
        }

        private void MakePrediction(UIImage image)
        {
            // Get input from the image
            IMLFeatureProvider input = CreateInput(image);

            var output = model.GetPrediction(input, out var error);

            if (error != null) {
                Console.WriteLine($"Error predicting: {error}");
                return;
            }

            var classLabel = output.GetFeatureValue("classLabel").StringValue;

			//
			// Display everything
			//
			Console.WriteLine($"Recognized:");
			foreach (NSString n in output.FeatureNames)
			{
				var value = output.GetFeatureValue(n);
				Console.WriteLine($"  {n} == {value}");
			}

			var message = $"{classLabel.ToUpperInvariant()} with probability {output.GetFeatureValue("classLabelProbs").DictionaryValue[classLabel]}";

			Console.WriteLine(message);

			BeginInvokeOnMainThread(() =>
			{
				var alert = UIAlertController.Create(message, "I hope it's right!", UIAlertControllerStyle.Alert);
				alert.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, (obj) =>
				{
					DismissViewController(true, null);
				}));
				PresentViewController(alert, true, null);
			});
        }

        private IMLFeatureProvider CreateInput(UIImage image)
        {
			var pixelBuffer = image.Resize(new CGSize(227, 227)).ToPixelBuffer();

			var imageValue = MLFeatureValue.FromPixelBuffer(pixelBuffer);

			var inputs = new NSDictionary<NSString, NSObject>(new NSString("image"), imageValue);

			return new MLDictionaryFeatureProvider(inputs, out var error);
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}

