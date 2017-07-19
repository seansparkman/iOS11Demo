using System;
using ARKit;
using UIKit;
using SceneKit;
using CoreGraphics;

namespace iOS11Test.AugmentedReality
{
    public class AugmentedRealityViewController : UIViewController
    {
		ARSCNView sceneView = new ARSCNView();
        UILabel counterLabel;

		public override void ViewDidLoad()
		{
			Title = "ARKit";

			View = sceneView;

			sceneView.ShowsStatistics = true;
			sceneView.AutomaticallyUpdatesLighting = true;

			sceneView.Scene = SCNScene.Create();
			var root = sceneView.Scene.RootNode;

			var cameraNode = SCNNode.Create();
			cameraNode.Camera = SCNCamera.Create();
			root.AddChildNode(cameraNode);

			var lightNode = SCNNode.Create();
			cameraNode.Light = SCNLight.Create();
			root.AddChildNode(lightNode);

            var label = new UILabel(new CGRect(10, 10, 300, 30));

            View.AddSubview(label);
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			var config = new ARWorldTrackingSessionConfiguration
			{
				PlaneDetection = ARPlaneDetection.Horizontal,
				WorldAlignment = ARWorldAlignment.Gravity,
				LightEstimationEnabled = true,
			};

            AddObject();

			sceneView.Session.Run(config);
		}

        public void AddObject()
		{
			var spaceShip = new SpaceShip();
			spaceShip.LoadModal();

            var xPos = RandomPosition(-3f, 3f);
            var yPos = RandomPosition(-3f, 3f);

            spaceShip.Position = new SCNVector3(xPos, yPos, -1);

            sceneView.Scene.RootNode.AddChildNode(spaceShip);
        }

        Random random = new Random();
        public float RandomPosition(float lowerBound, float upperBound)
        {
            return random.NextFloat() / float.MaxValue * (lowerBound - upperBound) * upperBound;
        }

		public override void ViewWillDisappear(bool animated)
		{
			base.ViewWillDisappear(animated);

			sceneView.Session.Pause();
		}

        public override void TouchesBegan(Foundation.NSSet touches, UIEvent evt)
        {
            if (touches.Count != 0)
            {
                var touch = touches.ToArray<UITouch>()[0];
                var location = touch.LocationInView(sceneView);

                var hitList = sceneView.HitTest(location, new SCNHitTestOptions());

                if (hitList.Length > 0)
                {
                    var hitObject = hitList[0];

                    if (hitObject.Node.Name == "ARShip")
                    {
                        hitObject.Node.RemoveFromParentNode();

                        AddObject();
                    }
                }
            }
        }
    }

    public static class RandomExtension
    {
		public static float NextFloat(this Random random)
		{
			double mantissa = (random.NextDouble() * 2.0) - 1.0;
			double exponent = Math.Pow(2.0, random.Next(-126, 128));
			return (float)(mantissa * exponent);
		}
    }
}
