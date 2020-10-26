using LoveCCA.iOS.Renderers;
using System;
using UIKit;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(DatePicker), typeof(DatePickerRenderer))]
namespace LoveCCA.iOS.Renderers
{
	public class DatePickerRenderer : Xamarin.Forms.Platform.iOS.DatePickerRenderer
	{
		protected override void OnElementChanged(Xamarin.Forms.Platform.iOS.ElementChangedEventArgs<DatePicker> e)
		{
			base.OnElementChanged(e);

			if (Control != null)
			{
				UITextField entry = Control;
				UIDatePicker picker = (UIDatePicker)entry.InputView;
				picker.PreferredDatePickerStyle = UIDatePickerStyle.Wheels;
			}
		}
	}
}