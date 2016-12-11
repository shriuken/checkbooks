using System;

using Java.Text;
using Java.Lang;
using Java.Math;

namespace checkbooks
{
	public static class Utils
	{
		// Might break it?
		private static NumberFormat FORMAT_CURRENCY = NumberFormat.GetCurrencyInstance(Java.Util.Locale.Default);

		public static int ParseAmountToCents(string val) {
			try {
				Number valNum = FORMAT_CURRENCY.Parse(val);
				BigDecimal bigDec = new BigDecimal(valNum.DoubleValue());
				bigDec = bigDec.SetScale(2, RoundOptions.HalfUp);
				return bigDec.MovePointRight(2).IntValue();
			}
			catch (ParseException ex1)
			{
				try
				{
					Console.WriteLine(ex1);
					BigDecimal bigDec = new BigDecimal(val);
					bigDec = bigDec.SetScale(2, RoundOptions.HalfUp);
					return bigDec.MovePointRight(2).IntValue();
				}
				catch (NumberFormatException ex2)
				{
					Console.WriteLine(ex2);
					return -1;
				}
			}
		}

		public static string FormatCentsToAmount(int val)
		{
			BigDecimal bigDec = new BigDecimal(val);
			bigDec = bigDec.SetScale(2, RoundOptions.HalfUp);
			bigDec = bigDec.MovePointLeft(2);
			string currency = FORMAT_CURRENCY.Format(bigDec.DoubleValue());
			return currency.Replace(FORMAT_CURRENCY.Currency.GetSymbol(Java.Util.Locale.Default), "").Replace(",", "");
		}

		public static string FormatCentsToCurrency(int val)
		{
			BigDecimal bigDec = new BigDecimal(val);
			bigDec = bigDec.SetScale(2, RoundOptions.HalfUp);
			bigDec = bigDec.MovePointLeft(2);
			return FORMAT_CURRENCY.Format(bigDec.DoubleValue());
		}
	}
}