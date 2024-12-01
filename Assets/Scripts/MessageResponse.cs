// @sonhg: class: MessageResponse
using System;
//using Facebook.Unity;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using UnityEngine;

public class MessageResponse : BaseResponse
{
	public static void RunMessage(BaseEvent evt, BaseScene gameScene)
	{
		BaseResponse baseResponse = new MessageResponse();
		baseResponse.SetParams(evt, gameScene);
		baseResponse.Run(true);
	}

	public override void UpdateBusiness()
	{
		SFSObject sfsobject = (SFSObject)this.evt.Params["params"];
		this.actionType = sfsobject.GetInt("sysm-action-type");
		this.msgType = sfsobject.GetInt("sysm_type");
		this.title = string.Empty;
		if (!sfsobject.IsNull("sysm_title"))
		{
			this.title = sfsobject.GetUtfString("sysm_title");
		}
		this.messenger = sfsobject.GetUtfString("sysm_content");
		this.link = sfsobject.GetUtfString("sysm_link");
		this.textButtonYes = Language.MSG_OK;
		if (!sfsobject.IsNull("sysm_ye"))
		{
			this.textButtonYes = sfsobject.GetUtfString("sysm_ye");
		}
		this.textButtonNo = Language.MSG_NO;
		if (!sfsobject.IsNull("sysm_no"))
		{
			this.textButtonNo = sfsobject.GetUtfString("sysm_no");
		}
		this.displayTime = 10;
		if (!sfsobject.IsNull("sysm_time_show"))
		{
			this.displayTime = sfsobject.GetInt("sysm_time_show");
		}
		this.data = string.Empty;
		if (!sfsobject.IsNull("data"))
		{
			this.data = sfsobject.GetUtfString("data");
		}
		this.force = 0;
		if (!sfsobject.IsNull("sysm_force"))
		{
			this.force = sfsobject.GetInt("sysm_force");
		}
	}

	public override void UpdateGUI()
	{
		if (this.msgType == 0)
		{
			this.OnPopupShow();
		}
		else
		{
			this.OnTooltipShow();
		}
	}

	private void OnPopupShow()
	{
		Context.OnDeletegateObject onNoClick = null;
		object yesObject = null;
		object noObject = null;
		switch (this.actionType)
		{
		case 0:
			Context.Messenger.AddOkMessage(this.messenger, null, null, string.Empty, string.Empty);
			return;
		case 1:
		{
			Context.OnDeletegateObject onYesClick = this.goTopupCard;
			if (this.force == 0)
			{
				Context.Messenger.AddYesNoMessage(this.messenger, onYesClick, onNoClick, yesObject, noObject, this.title, this.textButtonYes, this.textButtonNo, true);
			}
			else
			{
				Context.Messenger.AddOkMessage(this.messenger, onYesClick, yesObject, this.title, this.textButtonYes);
			}
			return;
		}
		case 2:
		{
			Context.OnDeletegateObject onYesClick = this.goLinkClick;
			yesObject = this.link;
			if (this.force == 0)
			{
				Context.Messenger.AddYesNoMessage(this.messenger, onYesClick, onNoClick, yesObject, noObject, this.title, this.textButtonYes, this.textButtonNo, true);
			}
			else
			{
				Context.Messenger.AddOkMessage(this.messenger, onYesClick, yesObject, this.title, this.textButtonYes);
			}
			return;
		}
		case 3:
		{
			Context.OnDeletegateObject onYesClick = this.goTopupInapp;
			if (this.force == 0)
			{
				Context.Messenger.AddYesNoMessage(this.messenger, onYesClick, onNoClick, yesObject, noObject, this.title, this.textButtonYes, this.textButtonNo, true);
			}
			else
			{
				Context.Messenger.AddOkMessage(this.messenger, onYesClick, yesObject, this.title, this.textButtonYes);
			}
			return;
		}
		case 4:
		{
			Context.OnDeletegateObject onYesClick = this.backServerYes;
			onNoClick = this.backServerNo;
			yesObject = this.data;
			noObject = this.data;
			if (this.force == 0)
			{
				Context.Messenger.AddYesNoMessage(this.messenger, onYesClick, onNoClick, yesObject, noObject, this.title, this.textButtonYes, this.textButtonNo, true);
			}
			else
			{
				Context.Messenger.AddOkMessage(this.messenger, onYesClick, yesObject, this.title, this.textButtonYes);
			}
			return;
		}
		case 6:
		{
			Context.OnDeletegateObject onYesClick = this.goRanking;
			if (this.force == 0)
			{
				Context.Messenger.AddYesNoMessage(this.messenger, onYesClick, onNoClick, yesObject, noObject, this.title, this.textButtonYes, this.textButtonNo, true);
			}
			else
			{
				Context.Messenger.AddOkMessage(this.messenger, onYesClick, yesObject, this.title, this.textButtonYes);
			}
			return;
		}
		case 7:
		{
			Context.OnDeletegateObject onYesClick = this.goMergeFB;
			if (this.force == 0)
			{
				Context.Messenger.AddYesNoMessage(this.messenger, onYesClick, onNoClick, yesObject, noObject, this.title, this.textButtonYes, this.textButtonNo, true);
			}
			else
			{
				Context.Messenger.AddOkMessage(this.messenger, onYesClick, yesObject, this.title, this.textButtonYes);
			}
			return;
		}
		case 8:
			Application.OpenURL(this.link);
			return;
		}
		Context.Messenger.AddOkMessage(this.messenger, null, null, string.Empty, string.Empty);
	}

	private void OnTooltipShow()
	{
		object yesObject = null;
		Context.OnDeletegateObject onYesClick;
		switch (this.actionType)
		{
		case 0:
			onYesClick = delegate(object o)
			{
				UnityEngine.Debug.Log("goTopupInapp");
				Context.Tooltip.onDestroy();
			};
			Context.Tooltip.AddMessageYes(this.messenger, this.displayTime, onYesClick, yesObject, this.textButtonYes);
			return;
		case 1:
			onYesClick = this.goTopupCard;
			Context.Tooltip.AddMessageYes(this.messenger, this.displayTime, onYesClick, yesObject, this.textButtonYes);
			return;
		case 2:
			onYesClick = this.goLinkClick;
			yesObject = this.link;
			Context.Tooltip.AddMessageYes(this.messenger, this.displayTime, onYesClick, yesObject, this.textButtonYes);
			return;
		case 3:
			onYesClick = this.goTopupInapp;
			Context.Tooltip.AddMessageYes(this.messenger, this.displayTime, onYesClick, yesObject, this.textButtonYes);
			return;
		case 4:
		{
			onYesClick = this.backServerYes;
			Context.OnDeletegateObject onDeletegateObject = this.backServerNo;
			Context.Tooltip.AddMessageYes(this.messenger, this.displayTime, onYesClick, yesObject, this.textButtonYes);
			return;
		}
		case 6:
			onYesClick = this.goRanking;
			Context.Tooltip.AddMessageYes(this.messenger, this.displayTime, onYesClick, yesObject, this.textButtonYes);
			return;
		case 7:
			onYesClick = this.goMergeFB;
			Context.Tooltip.AddMessageYes(this.messenger, this.displayTime, onYesClick, yesObject, this.textButtonYes);
			return;
		}
		onYesClick = delegate(object o)
		{
			UnityEngine.Debug.Log("goTopupInapp");
			Context.Tooltip.onDestroy();
		};
		Context.Tooltip.AddMessageYes(this.messenger, this.displayTime, onYesClick, yesObject, this.textButtonYes);
	}

	protected int actionType;

	protected int msgType;

	protected string title;

	protected string messenger;

	protected string link;

	protected string textButtonYes;

	protected string textButtonNo;

	protected int roomId;

	protected string groupid = string.Empty;

	protected int groupIndex = -1;

	protected string sysParams = string.Empty;

	protected int displayTime;

	protected string data = string.Empty;

	protected int force;

	private Context.OnDeletegateObject goLinkClick = delegate(object o)
	{
		UnityEngine.Debug.Log("goLinkClick");
		if (o != null)
		{
			Application.OpenURL(o.ToString());
		}
	};

	private Context.OnDeletegateObject goTopupCard = delegate(object o)
	{
		UnityEngine.Debug.Log("goTopupCard");
		CardSMSRequest.SendMessage(null);
	};

	private Context.OnDeletegateObject goTopupInapp = delegate(object o)
	{
		UnityEngine.Debug.Log("goTopupInapp");
		InappRequest.SendMessage(null);
	};

	private Context.OnDeletegateObject backServerYes = delegate(object o)
	{
		UnityEngine.Debug.Log("backServer");
		string text = o.ToString();
		BackServerRequest.SendMessage(0, SmartFoxConnection.Connection.MySelf.Id, text);
	};

	private Context.OnDeletegateObject backServerNo = delegate(object o)
	{
		UnityEngine.Debug.Log("backServerNo");
		string text = o.ToString();
		BackServerRequest.SendMessage(1, SmartFoxConnection.Connection.MySelf.Id, text);
	};

	private Context.OnDeletegateObject goRanking = delegate(object o)
	{
		UnityEngine.Debug.Log("goRanking");
		SFSObject sfsobject = new SFSObject();
		sfsobject.PutInt("type-id", 0);
		sfsobject.PutInt("p-page", 0);
		sfsobject.PutInt("p-length", 30);
		RankingRequest.SendMessage(sfsobject);
	};

	private Context.OnDeletegateObject goMergeFB = delegate(object o)
	{
		if (Context.GameInfo.LastAccounType == 0)
		{
			Context.Waiting.ShowWaiting();
			//FacebookAPI.Instance.LoginFB(new FacebookDelegate<ILoginResult>(Context.currentMono.MergeFBCallBack), Context.currentMono.onLoginFBSuccess);
		}
	};
}
