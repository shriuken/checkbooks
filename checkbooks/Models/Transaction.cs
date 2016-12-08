using System;
using SQLite;

namespace checkbooks
{
	public class Transaction
	{
		[PrimaryKey, AutoIncrement]
		public int ID { get; set; }

		public decimal Amount { get; set; }

		public string Type { get; set; }

		public string Subtype { get; set; } // TODO: Decide on if this is needed or not.

		public DateTime Date { get; set; }
	}
}
