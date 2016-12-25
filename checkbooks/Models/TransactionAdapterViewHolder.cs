using System;
using Android.Support.V7.Widget;
using Android.Widget;
using Android.Views;

namespace checkbooks
{
	public class TransactionAdapterViewHolder : RecyclerView.ViewHolder
	{
		public TextView Amnt { get; private set; } // TODO: Change to AppcompatTextView? 
		public TextView Type { get; private set; }
		public TextView Date { get; private set; }

		public TransactionAdapterViewHolder(View itemView) : base(itemView) {
			Amnt = itemView.FindViewById<TextView>(Resource.Id.Amount);
			Type = itemView.FindViewById<TextView>(Resource.Id.TransactionType);
			Date = itemView.FindViewById<TextView>(Resource.Id.TransactionDate);
		}
	}
}
