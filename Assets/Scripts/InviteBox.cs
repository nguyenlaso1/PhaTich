// @sonhg: class: InviteBox
using System;
using System.Collections.Generic;
using Sfs2X.Entities;
using UnityEngine;

public class InviteBox : BaseBox
{
	public void ShowListInvite(List<User> arr)
	{
		this._listUser = arr;
		this._scrollView = this.scrollView.GetComponent<ScrollViewCommon>();
		this.updateList();
	}

	protected override void Start()
	{
		UnityEngine.Debug.Log("IviteBox.start");
	}

	private void updateList()
	{
	}

	public void RemoveUser(int userId)
	{
		for (int i = 0; i < this._listUser.Count; i++)
		{
			if (this._listUser[i].Id == userId)
			{
				this._listUser.RemoveAt(i);
				break;
			}
		}
		if (this._listUser.Count <= 0)
		{
			this.CloseBox();
			return;
		}
		this.updateList();
	}

	public GameObject scrollView;

	private ScrollViewCommon _scrollView;

	private List<User> _listUser;

	private bool isStart = true;
}
