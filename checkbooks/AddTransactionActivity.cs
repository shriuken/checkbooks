
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace checkbooks
{
	[Activity(Label = "Activity")]
	public class AddTransaction : Activity
	{
		protected EditText _amount;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Create your application here
			SetContentView(Resource.Layout.AddTransaction);

			_amount = FindViewById<EditText>(Resource.Id.Amount);
			
			var filters = new Android.Text.IInputFilter[] { new NumericRangeFilter() };
			Android.Views.View.IOnFocusChangeListener onFocus = new AmountOnFocusChangeListener();

			_amount.SetFilters(filters);
			_amount.OnFocusChangeListener = onFocus;

		}
	}
}
