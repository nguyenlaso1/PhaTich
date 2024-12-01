// @plugin: class: TransactionHitBuilder
using System;
using UnityEngine;

public class TransactionHitBuilder : HitBuilder<TransactionHitBuilder>
{
	public string GetTransactionID()
	{
		return this.transactionID;
	}

	public TransactionHitBuilder SetTransactionID(string transactionID)
	{
		if (transactionID != null)
		{
			this.transactionID = transactionID;
		}
		return this;
	}

	public string GetAffiliation()
	{
		return this.affiliation;
	}

	public TransactionHitBuilder SetAffiliation(string affiliation)
	{
		if (affiliation != null)
		{
			this.affiliation = affiliation;
		}
		return this;
	}

	public double GetRevenue()
	{
		return this.revenue;
	}

	public TransactionHitBuilder SetRevenue(double revenue)
	{
		this.revenue = revenue;
		return this;
	}

	public double GetTax()
	{
		return this.tax;
	}

	public TransactionHitBuilder SetTax(double tax)
	{
		this.tax = tax;
		return this;
	}

	public double GetShipping()
	{
		return this.shipping;
	}

	public TransactionHitBuilder SetShipping(double shipping)
	{
		this.shipping = shipping;
		return this;
	}

	public string GetCurrencyCode()
	{
		return this.currencyCode;
	}

	public TransactionHitBuilder SetCurrencyCode(string currencyCode)
	{
		if (currencyCode != null)
		{
			this.currencyCode = currencyCode;
		}
		return this;
	}

	public override TransactionHitBuilder GetThis()
	{
		return this;
	}

	public override TransactionHitBuilder Validate()
	{
		if (string.IsNullOrEmpty(this.transactionID))
		{
			UnityEngine.Debug.LogWarning("No transaction ID provided - Transaction hit cannot be sent.");
			return null;
		}
		if (string.IsNullOrEmpty(this.affiliation))
		{
			UnityEngine.Debug.LogWarning("No affiliation provided - Transaction hit cannot be sent.");
			return null;
		}
		if (this.revenue == 0.0)
		{
			UnityEngine.Debug.Log("Revenue in transaction hit is 0.");
		}
		if (this.tax == 0.0)
		{
			UnityEngine.Debug.Log("Tax in transaction hit is 0.");
		}
		if (this.shipping == 0.0)
		{
			UnityEngine.Debug.Log("Shipping in transaction hit is 0.");
		}
		return this;
	}

	private string transactionID = string.Empty;

	private string affiliation = string.Empty;

	private double revenue;

	private double tax;

	private double shipping;

	private string currencyCode = string.Empty;
}
