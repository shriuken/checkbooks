using System;
using Android.Views;
using Android.Widget;

namespace checkbooks
{
	public class AmountOnFocusChangeListener : Java.Lang.Object, View.IOnFocusChangeListener
	{
		public void OnFocusChange(View view, bool hasFocus) {
			// This listener getes attached to any view containing amounts?
			var amountView = (EditText)view;
			if (hasFocus) {
				string val = amountView.Text;
				var cents = Utils.ParseAmountToCents(val);
				val = Utils.FormatCentsToAmount(cents);
				amountView.SetText(val, TextView.BufferType.Editable);
				amountView.SelectAll(); // optional?
			}
			else {
				string val = amountView.Text;
				var cents = Utils.ParseAmountToCents(val);
				val = Utils.FormatCentsToCurrency(cents);
				amountView.SetText(val, TextView.BufferType.Editable);
			}
		}
	}
}