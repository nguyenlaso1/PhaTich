// @plugin: class: ItemHitBuilder
using System;
using UnityEngine;

public class ItemHitBuilder : HitBuilder<ItemHitBuilder>
{
	public string GetTransactionID()
	{
		return this.transactionID;
	}

	public ItemHitBuilder SetTransactionID(string transactionID)
	{
		if (transactionID != null)
		{
			this.transactionID = transactionID;
		}
		return this;
	}

	public string GetName()
	{
		return this.name;
	}

	public ItemHitBuilder SetName(string name)
	{
		if (name != null)
		{
			this.name = name;
		}
		return this;
	}

	public string GetSKU()
	{
		return this.name;
	}

	public ItemHitBuilder SetSKU(string SKU)
	{
		if (SKU != null)
		{
			this.SKU = SKU;
		}
		return this;
	}

	public double GetPrice()
	{
		return this.price;
	}

	public ItemHitBuilder SetPrice(double price)
	{
		this.price = price;
		return this;
	}

	public string GetCategory()
	{
		return this.category;
	}

	public ItemHitBuilder SetCategory(string category)
	{
		if (category != null)
		{
			this.category = category;
		}
		return this;
	}

	public long GetQuantity()
	{
		return this.quantity;
	}

	public ItemHitBuilder SetQuantity(long quantity)
	{
		this.quantity = quantity;
		return this;
	}

	public string GetCurrencyCode()
	{
		return this.currencyCode;
	}

	public ItemHitBuilder SetCurrencyCode(string currencyCode)
	{
		if (currencyCode != null)
		{
			this.currencyCode = currencyCode;
		}
		return this;
	}

	public override ItemHitBuilder GetThis()
	{
		return this;
	}

	public override ItemHitBuilder Validate()
	{
		if (string.IsNullOrEmpty(this.transactionID))
		{
			UnityEngine.Debug.LogWarning("No transaction ID provided - Item hit cannot be sent.");
			return null;
		}
		if (string.IsNullOrEmpty(this.name))
		{
			UnityEngine.Debug.LogWarning("No name provided - Item hit cannot be sent.");
			return null;
		}
		if (string.IsNullOrEmpty(this.SKU))
		{
			UnityEngine.Debug.LogWarning("No SKU provided - Item hit cannot be sent.");
			return null;
		}
		if (this.price == 0.0)
		{
			UnityEngine.Debug.Log("Price in item hit is 0.");
		}
		if (this.quantity == 0L)
		{
			UnityEngine.Debug.Log("Quantity in item hit is 0.");
		}
		return this;
	}

	private string transactionID = string.Empty;

	private string name = string.Empty;

	private string SKU = string.Empty;

	private double price;

	private string category = string.Empty;

	private long quantity;

	private string currencyCode = string.Empty;
}
