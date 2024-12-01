// @sonhg: class: InviteListResponse
using System;
using System.Collections.Generic;
using Sfs2X.Core;
using Sfs2X.Entities;

public class InviteListResponse : BaseResponse
{
	public override void UpdateBusiness()
	{
		this._userList = (List<User>)this.evt.Params["users"];
	}

	public override void UpdateGUI()
	{
		((GameScene)this.baseScene).InviteUI.SetActive(true);
		((GameScene)this.baseScene).InviteUI.GetComponent<InviteController>().GenerateItem(this._userList);
	}

	public static void RunMessage(BaseEvent evt, BaseScene gameScene)
	{
		BaseResponse baseResponse = new InviteListResponse();
		baseResponse.SetParams(evt, gameScene);
		baseResponse.Run(true);
	}

	private List<User> _userList;
}
