// @sonhg: class: ChooseModeBox
using System;

public class ChooseModeBox : BaseBox
{
	public void OnClickVSFriends()
	{
		if (this._onVSFriends != null)
		{
			this._onVSFriends(null);
		}
		this.CloseBox();
	}

	public void OnClickVSOthers()
	{
		if (this._onVSOthers != null)
		{
			this._onVSOthers(null);
		}
		this.CloseBox();
	}

	public void AddDelegate(Context.OnDeletegateObject onVSFriends = null, Context.OnDeletegateObject onVSOthers = null)
	{
		this._onVSFriends = onVSFriends;
		this._onVSOthers = onVSOthers;
	}

	private Context.OnDeletegateObject _onVSFriends;

	private Context.OnDeletegateObject _onVSOthers;
}
