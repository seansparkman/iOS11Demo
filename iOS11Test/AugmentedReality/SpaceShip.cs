using System;
using SceneKit;

namespace iOS11Test.AugmentedReality
{
    public class SpaceShip : SCNNode
    {
        public SpaceShip()
        {
        }

        public void LoadModal()
        {

			var scene = SCNScene.FromFile("art.scnassets/ship.scn");

            var wrapperNode = SCNNode.Create();
            foreach(var child in scene.RootNode.ChildNodes)
            {
                wrapperNode.AddChildNode(child);
            }

            this.AddChildNode(wrapperNode);

		}
    }
}
