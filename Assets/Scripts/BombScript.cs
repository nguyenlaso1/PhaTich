// @sonhg: class: BombScript
using System;
using BombOffline;
using UnityEngine;

public class BombScript : MonoBehaviour
{
	public void SetSprite(int bombID)
	{
		string text = ResourcesUltis.ItemIdToLink(bombID.ToString());
		Sprite sprite = Resources.Load<Sprite>("Textures/" + text.Substring(0, text.Length - 4));
		if (sprite == null)
		{
			sprite = ResourcesManager.SpriteList[text];
		}
		if (sprite != null)
		{
			this.spriteRender.sprite = sprite;
		}
	}

	public virtual void DestroyBomb()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	public SpriteRenderer spriteRender;

	[HideInInspector]
	public Offline_GameController board;
}
