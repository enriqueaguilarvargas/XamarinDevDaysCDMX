// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace MVAV41
{
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		UIKit.UIButton btnCapturar { get; set; }

		[Outlet]
		UIKit.UIButton btnRespaldar { get; set; }

		[Outlet]
		UIKit.UIImageView Imagen { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (btnCapturar != null) {
				btnCapturar.Dispose ();
				btnCapturar = null;
			}

			if (btnRespaldar != null) {
				btnRespaldar.Dispose ();
				btnRespaldar = null;
			}

			if (Imagen != null) {
				Imagen.Dispose ();
				Imagen = null;
			}
		}
	}
}
