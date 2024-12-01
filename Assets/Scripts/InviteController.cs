// @sonhg: class: InviteController
using System;
using System.Collections.Generic;
using Sfs2X.Entities;
using UnityEngine;

public class InviteController : MonoBehaviour
{
	public void GenerateItem(List<User> userList)
	{
		this.gridParent.DestroyChildren();
		foreach (User user in userList)
		{
			if (!this.CheckInvitationExits(user))
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.itemPrefab);
				gameObject.transform.SetParent(this.gridParent);
				gameObject.transform.localScale = Vector3.one;
				InviteItem component = gameObject.GetComponent<InviteItem>();
				component.AddInfo(user, Context.position);
				component.invitePanel = base.gameObject;
			}
		}
	}

	public void FilterUser(string name)
	{
		foreach (object obj in this.gridParent)
		{
			Transform transform = (Transform)obj;
			InviteItem component = transform.GetComponent<InviteItem>();
			if (component.nameLabel.text.Contains(name))
			{
				transform.gameObject.SetActive(true);
			}
			else
			{
				transform.gameObject.SetActive(false);
			}
		}
	}

	private bool CheckInvitationExits(User u)
	{
		foreach (object obj in this.gridParent)
		{
			Transform transform = (Transform)obj;
			if (u.Id == transform.GetComponent<InviteItem>().GetUserId())
			{
				return true;
			}
		}
		return false;
	}

	public void CloseInvitePanel()
	{
		this.gridParent.DestroyChildren();
		base.gameObject.SetActive(false);
	}

	public GameObject itemPrefab;

	public Transform gridParent;
}
