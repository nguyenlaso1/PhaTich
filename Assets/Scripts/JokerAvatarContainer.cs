// @sonhg: class: JokerAvatarContainer
using System;

public class JokerAvatarContainer : BaseAvatarContainer
{
	public override void UpdateUserName()
	{
		this.UserName.GetComponent<UILabel>().text = JokerUserUtils.GetFormatDisplayName(this._user, 15);
	}

	public override void UpdateUserChip()
	{
		base.UpdateUserChip();
		UILabel component = this.UserChip.GetComponent<UILabel>();
		component.text = component.text.Replace("$", string.Empty);
	}
}
