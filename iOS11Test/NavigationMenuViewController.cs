using System;
using UIKit;

namespace iOS11Test
{
    public class NavigationMenuViewController : UITableViewController
    {
        private NavigationMenuTableViewSource tableSource;
        public NavigationMenuViewController()
        {
            TableView.Source = tableSource = new NavigationMenuTableViewSource(this);
        }

        public override void RowSelected(UITableView tableView, Foundation.NSIndexPath indexPath)
        {
			var selectedItem = tableSource.MenuItems[indexPath.Row];

			if (selectedItem == "Machine Learning")
			{
                NavigationController.PushViewController(new MachineLearning.MachineLearningViewController(), true);
			}
        }
    }
}
