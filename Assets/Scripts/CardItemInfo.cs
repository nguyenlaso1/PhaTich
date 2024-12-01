// @sonhg: class: CardItemInfo
using System;

public class CardItemInfo
{
	public CardItemInfo(int pay, int chip, int gold)
	{
		this._pay = pay;
		this._chip = chip;
		this._gold = gold;
	}

	public virtual int Pay
	{
		get
		{
			return this._pay;
		}
	}

	public virtual int Chip
	{
		get
		{
			return this._chip;
		}
	}

	public virtual int Gold
	{
		get
		{
			return this._gold;
		}
	}

	private int _pay;

	private int _chip;

	private int _gold;
}
