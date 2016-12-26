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

		public override void OnBindViewHolder(RecyclerView.ViewHolder vh, int position)
		{
			var item = _transactionList[position];

			// Replace the contents of the view with that element
			var holder = vh as TransactionAdapterViewHolder;
			holder.Amnt.Text = "$" + item.Amount.ToString();
			holder.Date.Text = item.Date.ToString();
			holder.Type.Text = item.Type.ToString();
			      
			/* transactionType.Text = _transactionList[position].Type;
			transactionAmount.Text = "$" + _transactionList[position].Amount.ToString();
			transactionDate.Text = _transactionList[position].Date.ToShortDateString(); */
			// viewHolder is transactionList[position], i think.
			//TODO: Figure out this adapter and viewholder stuff.

		}

		/* public override Java.Lang.Object GetItem(int position)
		{
			throw new NotImplementedException();
		} */

		public override long GetItemId(int position)
		{
			return _transactionList[position].ID;
		}

		/* public override View GetView(int position, View convertView, ViewGroup parent)
		{
			var view = convertView ?? _activity.LayoutInflater.Inflate(Resource.Layout.TransactionItem, parent, false);

			var transactionType = view.FindViewById<TextView>(Resource.Id.TransactionType);
			var transactionAmount = view.FindViewById<TextView>(Resource.Id.TransactionAmount);
			var transactionDate = view.FindViewById<TextView>(Resource.Id.TransactionDate);

			transactionType.Text = _transactionList[position].Type;
			transactionAmount.Text = "$" + _transactionList[position].Amount.ToString();
			transactionDate.Text = _transactionList[position].Date.ToShortDateString();

			return view;
		} */

		/** Creates an SQLite Connection to the Database stored in _path. Don't forget to close this database! */
		protected SQLiteConnection CreateAndReturnMyDB()
		{
			var conn = new SQLiteConnection(_path);
			conn.CreateTable<Transaction>();
			return conn;
		}
	}
}
