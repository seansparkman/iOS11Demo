using System;
using System.Collections.Generic;
using Foundation;
using UIKit;

namespace iOS11Test
{
    public class NavigationMenuTableViewSource : UITableViewSource
    {
        private UIViewController _parentController;
        public NavigationMenuTableViewSource(UIViewController parentController)
        {
            _parentController = parentController;
        }

        private string CellIdentifier = "TableCell";

        public List<string> MenuItems { get; set; }
            = new List<string> { "Machine Learning", "Augmented Reality" };

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell(CellIdentifier);
            if (cell == null)
            {
                cell = new UITableViewCell(UITableViewCellStyle.Default, CellIdentifier);
            }

            cell.TextLabel.Text = MenuItems[indexPath.Row];

            return cell;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return MenuItems.Count;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
		{
			var selectedItem = MenuItems[indexPath.Row];

			if (selectedItem == "Machine Learning")
			{
				_parentController.NavigationController.PushViewController(new MachineLearning.MachineLearningViewController(), true);
			}
			else if (selectedItem == "Augmented Reality")
			{
                _parentController.NavigationController.PushViewController(new AugmentedReality.AugmentedRealityViewController(), true);
			}
        }
    }
}
