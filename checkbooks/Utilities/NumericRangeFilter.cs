using System;

using Android.Text;
using Android.Runtime;

namespace checkbooks
{
	public class NumericRangeFilter : Java.Lang.Object, IInputFilter
	{
		double _max;
		double _min;

		public NumericRangeFilter() : this(0.00, 999999.99) {}

		public NumericRangeFilter(double min, double max) {
			_min = min;
			_max = max;
		}

		public Java.Lang.ICharSequence FilterFormatted(Java.Lang.ICharSequence source, int start, int end, ISpanned dest, int destStart, int destEnd)
		{
			try {
				String valStr = String.Concat(dest.ToString(), source.ToString());
				double val = Double.Parse(valStr);
				if (val >= _min && val <= _max) { return null; }	
			}
			catch (NotFiniteNumberException ex) {
				// TODO: Handle this error.
				Console.WriteLine(ex);
			}
			return new Java.Lang.String("");
		}
	}
}
