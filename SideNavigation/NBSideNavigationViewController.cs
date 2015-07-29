
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace SideNavigation
{
	public partial class NBSideNavigationViewController : UIViewController
	{

		String[] tableViewData;
		public Boolean isMenuVisible;
		public Point startPosition;

		public enum mainMenuCells {
			KTFeedbackCell = 0,
			KTOffersCell,
			KTSettingCell,
			KTRateusCell,
			KTShareCell,
			KTReviewAppCell,
			KTDisclaimerCell
		};

		public NBSideNavigationViewController () : base ("NBSideNavigationViewController", null)
		{
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			this.EdgesForExtendedLayout = UIRectEdge.None;

			UIBarButtonItem menuButton = new UIBarButtonItem("Menu",UIBarButtonItemStyle.Plain,(sender,args) => {
				this.animateToPage();
			});
			this.NavigationItem.LeftBarButtonItem = menuButton;

			// Always make sure intially hide the sliderView from screen.It should visible only tapping on menu button;
			this.sideMenuView.Hidden = true;
			this.sideContentView.Hidden = true;
			// Intilize the tableview
			tableViewData = new string[] {"View1","View2"};
			this.tableView.Source = new TableSource(this);


			// Changing X-position of SlideMenu & SliderContentView because we have to show the animation for bring the menuview from left to right.
			this.sideMenuView.Frame = new  RectangleF (-this.View.Frame.Size.Width,this.View.Frame.Location.Y,this.View.Frame.Size.Width,this.View.Frame.Size.Height-64);

			this.sideContentView.Frame = new RectangleF (-sideContentView.Frame.Size.Width,this.View.Frame.Location.Y,this.View.Frame.Size.Width,this.View.Frame.Size.Height-64);

			this.isMenuVisible = false;
			didSelectRowAtIndexPath (0); // Zero is feedback view contorller which is home page of mainView.


			UITapGestureRecognizer tapGesture = new UITapGestureRecognizer ();
			tapGesture.NumberOfTapsRequired = 1;

			tapGesture.AddTarget(()=> handelTapGesture(tapGesture)); 

			this.tapView.AddGestureRecognizer (tapGesture);

			// Swipe gesture for swipe to show side navigation.

			UISwipeGestureRecognizer leftSwipeGesture = new UISwipeGestureRecognizer ();
			leftSwipeGesture.AddTarget (()=> swipeLeftToDisplay(leftSwipeGesture));
			leftSwipeGesture.Direction = UISwipeGestureRecognizerDirection.Left;
			this.tapView.AddGestureRecognizer (leftSwipeGesture);

			UISwipeGestureRecognizer rightSwipeGesture = new UISwipeGestureRecognizer ();
			rightSwipeGesture.AddTarget (()=> swipeRightToDisplay(rightSwipeGesture));
			rightSwipeGesture.Direction = UISwipeGestureRecognizerDirection.Right;
			this.contentView.AddGestureRecognizer (rightSwipeGesture);

		}


		public void swipeLeftToDisplay(UISwipeGestureRecognizer gesture) {

			this.isMenuVisible = true;
			this.animateToPage ();
		}

		public void swipeRightToDisplay(UISwipeGestureRecognizer gesture) {

			this.isMenuVisible = false;
			this.animateToPage ();
		}

		public void  handelTapGesture (UITapGestureRecognizer gesture) {
			animateToPage ();
		}

		public void animateToPage() {

			this.sideMenuView.Alpha = 0;
			float minusXPosition = 120;
			if (!isMenuVisible) {
				this.sideMenuView.Hidden = false;
				UIView.Animate (0.3, 0, UIViewAnimationOptions.CurveEaseIn | UIViewAnimationOptions.CurveLinear,
					() => {
						this.sideMenuView.Alpha =  0.75f;
						RectangleF frame  = this.sideMenuView.Frame;
						frame.X = View.Frame.Location.X;
						this.sideMenuView.Frame = frame;

						frame = this.sideContentView.Frame;
						frame.X = View.Frame.Location.X - minusXPosition;
						this.sideContentView.Frame = frame;
					},
					() => {
						this.sideContentView.Hidden = false;
						UIView.Animate (0.3, 0, UIViewAnimationOptions.CurveEaseIn | UIViewAnimationOptions.CurveLinear,
							() => {
								RectangleF frame = this.sideContentView.Frame;
								frame.X = View.Frame.Location.X;
								this.sideContentView.Frame = frame;
							},
							() => {
								this.isMenuVisible = true;
							}
						);
					}
				);
			} else {
				this.sideMenuView.Alpha =  0.75f;
				UIView.Animate (0.3, 0, UIViewAnimationOptions.CurveEaseIn | UIViewAnimationOptions.CurveLinear,
					() => {

						RectangleF frame  = this.sideMenuView.Frame;
						frame.X = -View.Frame.Location.X;
						this.sideMenuView.Frame = frame;

						frame = this.sideContentView.Frame;
						frame.X = -minusXPosition;
						this.sideContentView.Frame = frame;
					},
					() => {
						UIView.Animate (0.3, 0, UIViewAnimationOptions.CurveEaseIn | UIViewAnimationOptions.CurveLinear,
							() => {
								RectangleF frame = this.sideContentView.Frame;
								frame.X = -this.sideMenuView.Frame.Size.Width;
								this.sideContentView.Frame = frame;
								this.sideMenuView.Alpha =  0.0f;

							},
							() => {
								this.isMenuVisible = false;
								this.sideContentView.Hidden = true;
								this.sideMenuView.Hidden = true;
							}
						);
					}
				);


			}
		}

		public void didSelectRowAtIndexPath(int indexPath) {

			UIViewController controller = null;

			switch (indexPath) {
			case (int)mainMenuCells.KTFeedbackCell:
				{
					controller = new ViewController1 ();
					this.Title = "View1";
					break;
				}

			case (int)mainMenuCells.KTOffersCell:
				{
					controller = new ViewController2 ();
					this.Title = "View2";
					break;
				}



			default:
				break;
			}

			if (controller == null) { // Use by default as feedback view controller.
				controller = new ViewController1 ();
				this.Title = "View1";

			}

			// add the navigation controller as child view controller of feedBackView.
			AddChildViewController(controller);

			// insert the navigationView below to mainContentView
			contentView.InsertSubviewBelow(controller.View,contentView);



		}
		class TableSource : UITableViewSource {

			string[] tableItems{ get; set; }
			NBSideNavigationViewController controller{ get; set; }
			string cellIdentifier = "TableCell";

			public TableSource (NBSideNavigationViewController controller)
			{
				this.controller = controller;
				tableItems = controller.tableViewData;
			}

			public override int RowsInSection (UITableView tableview, int section)
			{
				return tableItems.Length;
			}

			public override UITableViewCell GetCell (UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
			{
				UITableViewCell cell = tableView.DequeueReusableCell (cellIdentifier);
				// if there are no cells to reuse, create a new one
				if (cell == null)
					cell = new UITableViewCell (UITableViewCellStyle.Default, cellIdentifier);
				cell.TextLabel.Text = tableItems[indexPath.Row];
				cell.BackgroundColor = UIColor.FromRGB (181,37,44);
				cell.TextLabel.TextColor = UIColor.White;

				UIView view = new UIView (cell.Bounds);
				view.BackgroundColor = UIColor.FromRGB (175,61,50);
				cell.SelectedBackgroundView = view;
				return cell;
			}

			public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
			{
				controller.didSelectRowAtIndexPath (indexPath.Row);
				// Hide the slideMenu.
				controller.animateToPage ();

			}

		}
	}
}

