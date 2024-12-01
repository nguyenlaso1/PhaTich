// @sonhg: class: AnimationDictionary
using System;
using System.Collections.Generic;

public class AnimationDictionary
{
	public AnimationDictionary()
	{
		this._dictionaryAnimation = new Dictionary<int, BaseAnimation>();
	}

	public void ForceCompleteAnimation()
	{
		List<int> list = new List<int>(this._dictionaryAnimation.Keys);
		foreach (int key in list)
		{
			if (this._dictionaryAnimation.ContainsKey(key))
			{
				BaseAnimation baseAnimation = this._dictionaryAnimation[key];
				baseAnimation.ForceComplete();
			}
		}
		this._dictionaryAnimation.Clear();
	}

	public void ForceCompleteAnimation(int animationType)
	{
		if (this._dictionaryAnimation.ContainsKey(animationType))
		{
			this._dictionaryAnimation[animationType].ForceComplete();
		}
	}

	public void Remove(int type)
	{
		this._dictionaryAnimation.Remove(type);
	}

	public void Add(int type, BaseAnimation animation)
	{
		this.ForceCompleteAnimation(type);
		this._dictionaryAnimation.Add(type, animation);
	}

	public bool ContainsKey(int key)
	{
		return this._dictionaryAnimation.ContainsKey(key);
	}

	public void Clear()
	{
		this._dictionaryAnimation.Clear();
	}

	private Dictionary<int, BaseAnimation> _dictionaryAnimation;
}
