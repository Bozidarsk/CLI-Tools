using System;
using System.Collections;
using System.Collections.Generic;

public static class CurrencyConverter 
{
	private static List<Rate> rates = new List<Rate>() 
	{
		new Rate("USD", "BGN", 1.89f),
		new Rate("EUR", "BGN", 1.96f),
		new Rate("GBP", "BGN", 2.25f),
		new Rate("TRY", "BGN", 0.10f),

		new Rate("EUR", "USD", 1.03f),
		new Rate("GBP", "USD", 1.19f),
		new Rate("TRY", "USD", 0.054f),

		new Rate("GBP", "EUR", 1.15f),
		new Rate("TRY", "EUR", 0.052f),

		new Rate("TRY", "GBP", 0.045f)
	};

	public static void AddRate(Rate rate) { rates.Add(rate); }
	public static float Exchange(string from, string to, float x) { return ExchangeRate(from, to) * x; }
	public static float ExchangeRate(string from, string to) 
	{
		if (string.IsNullOrEmpty(from) || string.IsNullOrEmpty(to)) { throw new NullReferenceException("From and to must not be null."); }

		if (from == to) { return 1f; }
		for (int i = 0; i < rates.Count; i++) 
		{
			if (rates[i].from == from && rates[i].to == to) { return rates[i].value; }
			if (rates[i].from == to && rates[i].to == from) { return 1f / rates[i].value; }
		}

		throw new ArgumentException("ExchangeRate rate not found for " + from + " and " + to + ".");
	}

	public sealed class Rate 
	{
		public string from;
		public string to;
		public float value;

		public Rate() {}
		public Rate(string from, string to, float value) 
		{
			this.from = from;
			this.to = to;
			this.value = value;
		}
	}
}