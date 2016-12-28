
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
using Android.Support.Design.Widget;
using SQLite;

namespace checkbooks
{
	[Activity(Label = "Activity")]
	public class AddTransaction : Activity
	{
		protected TextInputEditText _amount;
		protected Button _add;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Create your application here
			SetContentView(Resource.Layout.AddTransaction);

			_amount = FindViewById<TextInputEditText>(Resource.Id.Amount);
			_add = FindViewById<Button>(Resource.Id.AddButton);
			
			var filters = new Android.Text.IInputFilter[] { new NumericRangeFilter() };
			View.IOnFocusChangeListener onFocus = new AmountOnFocusChangeListener();

			_amount.SetFilters(filters);
			_amount.OnFocusChangeListener = onFocus;

			_add.Click += (sender, e) => {
				Transaction transaction = new Transaction
				{
					Amount = Convert.ToDecimal(_amount.Text),
					// Type = _typeSpinner.SelectedItem.ToString(),
					// Amount = (decimal)25.52,
					Type = "Test",
					Date = DateTime.Now
				};

				/* 
					var folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
					_transactionAdapter = new TransactionAdapter(this, System.IO.Path.Combine(folder, Resources.GetString(Resource.String.transaction_db))); 
				*/
				var folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
				var conn = new SQLiteConnection(System.IO.Path.Combine(folder, Resources.GetString(Resource.String.transaction_db)));
				conn.CreateTable<Transaction>();
				conn.Insert(transaction);
				conn.Close();
				// var shit = Android.Content.Context.InputMethodService;
				// InputMethodManager imm = (InputMethodManager)GetSystemService(shit);
				// imm.HideSoftInputFromWindow(_amount.WindowToken, HideSoftInputFlags.None);

				/* _transactionAdapter.Insert(transaction, 0);
				_transactionListView.SmoothScrollToPosition(0); */
				Toast.MakeText(this, "Transaction added!", ToastLength.Short).Show();
				// _amount.SetText("", TextView.BufferType.Editable);
			}; // TODO: Move this into a function. And make prettier. Current display hideous. TODO: Improve logic for type/subtype;

		}
	}
}
