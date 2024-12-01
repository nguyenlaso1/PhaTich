// @sonhg: class: InappItem
using System;

public class InappItem
{
	public InappItem(string id, string pay, int chip, int gold)
	{
		this._id = Context.GameInfo.GetSKU(id);
		this._pay = pay;
		this._chip = chip;
		this._gold = gold;
	}

	public string Id
	{
		get
		{
			return this._id;
		}
		set
		{
			this._id = value;
		}
	}

	public string Pay
	{
		get
		{
			return this._pay;
		}
		set
		{
			this._pay = value;
		}
	}

	public int Chip
	{
		get
		{
			return this._chip;
		}
		set
		{
			this._chip = value;
		}
	}

	public int Gold
	{
		get
		{
			return this._gold;
		}
		set
		{
			this._gold = value;
		}
	}

	private int _gold;

	private string _pay;

	private string _id;

	private int _chip;
}
