﻿// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;
using System.CodeDom.Compiler;

namespace SideNavigation
{
	[Register ("NBSideNavigationViewController")]
	partial class NBSideNavigationViewController
	{
		[Outlet]
		MonoTouch.UIKit.UIView contentView { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIView sideContentView { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIView sideMenuView { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITableView tableView { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIView tapView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (contentView != null) {
				contentView.Dispose ();
				contentView = null;
			}

			if (sideMenuView != null) {
				sideMenuView.Dispose ();
				sideMenuView = null;
			}

			if (sideContentView != null) {
				sideContentView.Dispose ();
				sideContentView = null;
			}

			if (tableView != null) {
				tableView.Dispose ();
				tableView = null;
			}

			if (tapView != null) {
				tapView.Dispose ();
				tapView = null;
			}
		}
	}
}
