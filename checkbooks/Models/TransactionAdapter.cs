using System;
using System.Collections.Generic;
using Android.Support.V7.Widget;
using Android.App;
using Android.Content;
using Android.Provider;
using Android.Views;
using Android.Widget;
using SQLite;

namespace checkbooks
{
	public class TransactionAdapter : RecyclerView.Adapter
	{
		List<Transaction> _transactionList;
		Activity _activity;
		int _numResults;
		string _path;

		public TransactionAdapter(Activity activity, string path)
		{
			// TODO: Figure out what this to-do is.
			_activity = activity;
			_numResults = 50;
			_path = path;

			FillTransactions();
		}

		void FillTransactions() {
			
			_transactionList = new List<Transaction>();

			var conn = CreateAndReturnMyDB();
			var query = conn.Table<Transaction>().Take(_numResults).OrderByDescending(x => x.Date);

			foreach (var row in query) {
				_transactionList.Add(new Transaction
				{
					Amount = row.Amount,
					Type = row.Type,
					Subtype = row.Subtype,
					Date = row.Date
				});
			}
			conn.Close();
		}

		/** Inserts a new element into the adapter and the database? */
		public void Insert(Transaction transaction, int location) {
			_transactionList.Insert(location, transaction);
			var conn = CreateAndReturnMyDB();
			conn.Insert(transaction);
			conn.Close();
		}

		public override int ItemCount {
			get { return _transactionList.Count; }
		}

		public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
		{
			var id = Resource.Layout.TransactionCard;
			var itemView = LayoutInflater.From(parent.Context).Inflate(id, parent, false);

			return new TransactionAdapterViewHolder(itemView);
		}

		public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
		{
			var item = _transactionList[position];

			// Replace the contents of the view with that element
			var h = holder as TransactionAdapterViewHolder;
			h.Amnt.Text = "$" + item.Amount.ToString();
			h.Date.Text = item.Date.ToString();
			h.Type.Text = item.Type.ToString();

		}

		public override long GetItemId(int position)
		{
			return _transactionList[position].ID;
		}

		/** Creates an SQLite Connection to the Database stored in _path. Don't forget to close this database! */
		protected SQLiteConnection CreateAndReturnMyDB()
		{
			var conn = new SQLiteConnection(_path);
			conn.CreateTable<Transaction>();
			return conn;
		}
	}
}
