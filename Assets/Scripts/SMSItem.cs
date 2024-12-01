// @sonhg: class: SMSItem
using System;
using UnityEngine;

public class SMSItem
{
	public SMSItem(string number, string text, int pay, string header)
	{
		this._number = number;
		this._text = text;
		this._pay = pay;
		string[] array = header.Split(new char[]
		{
			'|'
		});
		if (array.Length < 2)
		{
			UnityEngine.Debug.LogError("temp.Length < 2: " + header);
		}
		else
		{
			this._chip = int.Parse(array[0].Trim());
			this._gold = int.Parse(array[1].Trim());
		}
	}

	public SMSItem(string number, string text, int pay, int chip, int gold)
	{
		this._number = number;
		this._text = text;
		this._pay = pay;
		this._chip = chip;
		this._gold = gold;
	}

	public string text
	{
		get
		{
			return this._text;
		}
		set
		{
			this._text = value;
		}
	}

	public string number
	{
		get
		{
			return this._number;
		}
		set
		{
			this._number = value;
		}
	}

	public int Pay
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

	private string _number;

	private string _text;

	private int _pay;

	private int _chip;

	private int _gold;
}
