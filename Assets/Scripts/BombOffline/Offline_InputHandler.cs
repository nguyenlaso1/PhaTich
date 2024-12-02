// @sonhg: class: BombOffline.Offline_InputHandler
using System;
using InControl;
using UnityEngine;

namespace BombOffline
{
	public class Offline_InputHandler : MonoBehaviour
	{
		private void Awake()
		{
		}

		private void Start()
		{
			if (PlayerPrefs.GetInt("Joystick", 0) == 1)
			{
				this.InitFixedJoystick();
			}
			else
			{
				PlayerPrefs.SetInt("Joystick", 0);
				this.InitDynamicJoystick();
			}
		}

		public void InitDynamicJoystick()
		{
			this.isDynamic = true;
			this.joyStick.allowDragging = true;
			this.joyStick.snapToInitialTouch = true;
			this.joyStick.ActiveArea = new Rect(0f, 0f, 70f, 100f);
			this.joyStick.ring.Size = new Vector2(25f, 25f);
			this.joyStick.knob.Size = new Vector2(25f, 25f);
			this.joyStick.knobRange = 7.5f;
			this.joyStick.ring.IdleSprite = this.ringImageDynamic;
			this.joyStick.ring.BusySprite = this.ringImageDynamic;
			this.joyStick.knob.IdleColor = this.defaultColor;
			this.joyStick.knob.BusyColor = this.defaultColor;
			this.joyStick.lowerDeadZone = 0.1f;
		}

		public void InitFixedJoystick()
		{
			this.isDynamic = false;
			this.joyStick.allowDragging = false;
			this.joyStick.snapToInitialTouch = false;
			this.joyStick.ActiveArea = new Rect(0f, 0f, 25f, 45f);
			this.joyStick.ring.Size = new Vector2(50f, 50f);
			this.joyStick.knob.Size = new Vector2(50f, 50f);
			this.joyStick.knobRange = 5f;
			this.joyStick.ring.IdleSprite = this.ringImageStatic;
			this.joyStick.ring.BusySprite = this.ringImageStatic;
			this.joyStick.knob.IdleColor = new Color(0f, 0f, 0f, 0f);
			this.joyStick.knob.BusyColor = new Color(0f, 0f, 0f, 0f);
			this.joyStick.lowerDeadZone = 0.3f;
		}
		//bool phoneBuild = false;
		private void Update()
		{
			if (this.actor != null)
			{
				if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer) {
					this.inputDevice = InputManager.ActiveDevice;
					if (this.joyStickEnable)
					{
						float y = this.inputDevice.Direction.Y;
						float x = this.inputDevice.Direction.X;
						//Debug.Log(":Offline_InputHandler: joyStick ===");
						new Offline_MoveCommand(y, x, this.actor, this.isDynamic).Execute(this.actor);
					}
					if (this.inputDevice.Action1)
					{
						//Debug.Log(":Offline_InputHandler: Action1 ===");
						this.actor.PlaceBomb();
					}
				}
				else
                {
					Vector2 movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
					new Offline_MoveCommand(movement.y, movement.x, this.actor, this.isDynamic).Execute(this.actor);
					if (Input.GetKey(KeyCode.Space))
					{
						this.actor.PlaceBomb();
					}
				}
			}
		}

		public Offline_PlayerController actor;

		public TouchStickControl joyStick;

		[SerializeField]
		private Sprite ringImageStatic;

		[SerializeField]
		private Sprite ringImageDynamic;

		[SerializeField]
		private Color defaultColor;

		private InputDevice inputDevice;

		private bool isDynamic = true;

		[HideInInspector]
		public bool joyStickEnable = true;
	}
}
