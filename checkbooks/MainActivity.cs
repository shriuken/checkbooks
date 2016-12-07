using Android.App;
using Android.Widget;
using Android.OS;
using System;
using SQLite;
using Android.Views.InputMethods;
using System.Collections.Generic;

namespace checkbooks
{
	[Activity(Label = "checkbooks", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : Activity
	{
		string createDatabase(string path)
		{
			try
			{
				var connection = new SQLiteAsyncConnection(path);
				{
					connection.CreateTableAsync<Transaction>();
					return "Database created";
				}
			}
			catch (SQLiteException ex)
			{
				return ex.Message;
			}
		}

		protected ListView _transactionListView;
		protected List<string> mItems;
		protected Button _deposit;
		protected Button _withdraw;
		protected EditText _amount;
		protected TransactionAdapter _transactionAdapter;
		protected ArrayAdapter _typeAdapter;
		protected Spinner _typeSpinner;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.Main);

			// create db connection?
			string folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
			/* var conn = new SQLiteAsyncConnection(System.IO.Path.Combine(folder, Resources.GetString(Resource.String.transaction_db)));
			conn.CreateTableAsync<Transaction>();

			var syncConn = new SQLiteConnection(System.IO.Path.Combine(folder, Resources.GetString(Resource.String.transaction_db)));
			syncConn.CreateTable<Transaction>();*/

			_deposit = FindViewById<Button>(Resource.Id.Deposit);
			_withdraw = FindViewById<Button>(Resource.Id.Withdraw);
			_amount = FindViewById<EditText>(Resource.Id.Amount);
			_transactionListView = FindViewById<ListView>(Resource.Id.RecentActivity);
			_typeSpinner = FindViewById<Spinner>(Resource.Id.TypeSpinner);

			_typeAdapter = ArrayAdapter.CreateFromResource(
				this, Resource.Array.subtype_array, Android.Resource.Layout.SimpleSpinnerItem);
			
			_typeAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
			_typeSpinner.Adapter = _typeAdapter;

			_deposit.Click += (sender, e) =>
			{
				Transaction transaction = new Transaction
				{
					Amount = Convert.ToDecimal(_amount.Text),
					Type = "Deposit",
					Subtype = _typeSpinner.SelectedItem.ToString(),
					Date = DateTime.Now
				};

				var shit = Android.Content.Context.InputMethodService;
				InputMethodManager imm = (InputMethodManager)GetSystemService(shit);
				imm.HideSoftInputFromWindow(_amount.WindowToken, HideSoftInputFlags.None);

				_transactionAdapter.Insert(transaction, 0);
				_transactionListView.SmoothScrollToPosition(0);
				Toast.MakeText(this, "Transaction added!", ToastLength.Short).Show();
				_amount.SetText("", TextView.BufferType.Editable);
			}; // TODO: Move this into a function. And make prettier. Current display hideous. TODO: Add logic for type/subtype

			_withdraw.Click += (sender, e) =>
			{
				Transaction transaction = new Transaction
				{
					Amount = Convert.ToDecimal(_amount.Text),
					Type = "Withdrawal",
					Subtype = _typeSpinner.SelectedItem.ToString(),
					Date = DateTime.Now
				};

				var shit = Android.Content.Context.InputMethodService;
				InputMethodManager imm = (InputMethodManager)GetSystemService(shit);
				imm.HideSoftInputFromWindow(_amount.WindowToken, HideSoftInputFlags.None);

				_transactionAdapter.Insert(transaction, 0);
				_transactionListView.SmoothScrollToPosition(0);
				Toast.MakeText(this, "Transaction added!", ToastLength.Short).Show();
				_amount.SetText("", TextView.BufferType.Editable);

			};// TODO: Move this into a function. And make prettier. Current display hideous. TODO: Add logic for type/subtype.

			// var query = syncConn.Table<Transaction>().Take(30).OrderByDescending(x => x.Date);

			// ArrayAdapter<String> adapter = new ArrayAdapter<String>(this, R.layout.simple_list_item_1, R.id.text1, new String[] { "a", "b"});

			// mListAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, mItems);

			// I think this needs to somehow get all the.. oh! From the database.abase.
			_transactionAdapter = new TransactionAdapter(this, System.IO.Path.Combine(folder, Resources.GetString(Resource.String.transaction_db)));
			_transactionListView.Adapter = _transactionAdapter;
		}
	}
}

