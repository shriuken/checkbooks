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
		private string createDatabase(string path)
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

		private ListView mListView;
		private List<string> mItems;
		private Button mDeposit;
		private Button mWithdraw;
		private EditText mAmount;
		private ArrayAdapter<string> mListAdapter;


		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.Main);

			// create db connection?
			string folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
			var conn = new SQLiteAsyncConnection(System.IO.Path.Combine(folder, Resources.GetString(Resource.String.transaction_db)));
			conn.CreateTableAsync<Transaction>();

			var syncConn = new SQLiteConnection(System.IO.Path.Combine(folder, Resources.GetString(Resource.String.transaction_db)));
			syncConn.CreateTable<Transaction>();

			mDeposit = FindViewById<Button>(Resource.Id.Deposit);
			mWithdraw = FindViewById<Button>(Resource.Id.Withdraw);
			mAmount = FindViewById<EditText>(Resource.Id.Amount);

			mListView = FindViewById<ListView>(Resource.Id.RecentActivity);

			mDeposit.Click += (sender, e) =>
			{
				Transaction transaction = new Transaction
				{
					Amount = Convert.ToDecimal(mAmount.Text),
					Type = null,
					Date = DateTime.Now
				};

				conn.InsertAsync(transaction).ContinueWith((arg) =>
				{
					var shit = Android.Content.Context.InputMethodService;
					InputMethodManager imm = (InputMethodManager)GetSystemService(shit);
					imm.HideSoftInputFromWindow(mAmount.WindowToken, HideSoftInputFlags.None);
					
					mListAdapter.Insert("Deposit\t$" + Math.Abs(transaction.Amount).ToString() + "\t" + transaction.Date.ToString(), 0);
				});
				Toast.MakeText(this, "Transaction added!", ToastLength.Short).Show();
				mAmount.SetText("", TextView.BufferType.Editable);
			}; // TODO: Move this into a function. And make prettier. Current display hideous.
			mWithdraw.Click += (sender, e) => 
			{
				Transaction transaction = new Transaction
				{
					Amount = Convert.ToDecimal(mAmount.Text) * -1,
					Type = null,
					Date = DateTime.Now
				};

				conn.InsertAsync(transaction).ContinueWith((arg) =>
				{
					var shit = Android.Content.Context.InputMethodService;
					InputMethodManager imm = (InputMethodManager)GetSystemService(shit);
					imm.HideSoftInputFromWindow(mAmount.WindowToken, HideSoftInputFlags.None);

					mListAdapter.Insert("Withdrawal\t$" + Math.Abs(transaction.Amount).ToString() + "\t" + transaction.Date.ToString(), 0);
					mAmount.ClearComposingText();
				});
				Toast.MakeText(this, "Transaction added!", ToastLength.Short).Show();
				mAmount.SetText("", TextView.BufferType.Editable);
			};// TODO: Move this into a function. And make prettier. Current display hideous.

			var query = syncConn.Table<Transaction>().Take(30).OrderByDescending(x => x.Date);
			mItems = new List<string>();

			foreach (var row in query) {
				string type;
				if (row.Amount < 0)
					type = "Withdrawal";
				else
					type = "Deposit";
				
				mItems.Add(type + "\t$" + Math.Abs(row.Amount).ToString() + "\t" + row.Date.ToString());
			}
			// ArrayAdapter<String> adapter = new ArrayAdapter<String>(this, R.layout.simple_list_item_1, R.id.text1, new String[] { "a", "b"});

			mListAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, mItems);

			mListView.Adapter = mListAdapter;
		}
	}
}

