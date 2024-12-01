// @sonhg: class: InputHandler
using System;
using InControl;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
	private void Awake()
	{
	}

	private void Update()
	{
		if (this.actor != null)
		{
			this.inputDevice = InputManager.ActiveDevice;
			float y = this.inputDevice.Direction.Y;
			float x = this.inputDevice.Direction.X;
			new MoveCommand(y, x).Execute(this.actor);
			if (this.inputDevice.Action1)
			{
				new PlaceBombCommand().Execute(this.actor);
			}
		}
	}

	public BaseCharactersController actor;

	private InputDevice inputDevice;
}
