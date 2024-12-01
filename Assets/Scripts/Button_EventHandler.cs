// @sonhg: class: Button_EventHandler
using System;
using BombOffline;
using UnityEngine;
using UnityEngine.EventSystems;

public class Button_EventHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IEventSystemHandler
{
	public void OnPointerDown(PointerEventData data)
	{
		this._isPressed = true;
	}

	private void Update()
	{
		if (this._isPressed && this.actor != null)
		{
			if (this.keyCode.Equals("U"))
			{
				new Offline_MoveCommand(global::MoveDirection.UP).Execute(this.actor);
			}
			if (this.keyCode.Equals("D"))
			{
				new Offline_MoveCommand(global::MoveDirection.DOWN).Execute(this.actor);
			}
			if (this.keyCode.Equals("L"))
			{
				new Offline_MoveCommand(global::MoveDirection.LEFT).Execute(this.actor);
			}
			if (this.keyCode.Equals("R"))
			{
				new Offline_MoveCommand(global::MoveDirection.RIGHT).Execute(this.actor);
			}
		}
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		new Offline_MoveCommand(global::MoveDirection.STAND).Execute(this.actor);
		this._isPressed = false;
	}

	public Offline_PlayerController actor;

	public string keyCode = string.Empty;

	private bool _isPressed;
}
