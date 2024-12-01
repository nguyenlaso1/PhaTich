// dnSpy decompiler from Assembly-CSharp.dll class: KeyboardProfile
using System;
using InControl;
using UnityEngine;

public class KeyboardProfile : CustomInputDeviceProfile
{
	public KeyboardProfile()
	{
		base.Name = "Keyboard";
		base.Meta = "A keyboard and mouse combination profile appropriate for FPS.";
		base.ButtonMappings = new InputControlMapping[]
		{
			new InputControlMapping
			{
				Handle = "Fire - Keyboard",
				Target = InputControlType.Action1,
				Source = CustomInputDeviceProfile.KeyCodeButton(new KeyCode[]
				{
					KeyCode.Space,
					KeyCode.Return
				})
			}
		};
		base.AnalogMappings = new InputControlMapping[]
		{
			new InputControlMapping
			{
				Handle = "Move Up",
				Target = InputControlType.LeftStickUp,
				Source = CustomInputDeviceProfile.KeyCodeButton(new KeyCode[]
				{
					KeyCode.W,
					KeyCode.UpArrow
				})
			},
			new InputControlMapping
			{
				Handle = "Move Down",
				Target = InputControlType.LeftStickDown,
				Source = CustomInputDeviceProfile.KeyCodeButton(new KeyCode[]
				{
					KeyCode.S,
					KeyCode.DownArrow
				})
			},
			new InputControlMapping
			{
				Handle = "Move Left",
				Target = InputControlType.LeftStickLeft,
				Source = CustomInputDeviceProfile.KeyCodeButton(new KeyCode[]
				{
					KeyCode.A,
					KeyCode.LeftArrow
				})
			},
			new InputControlMapping
			{
				Handle = "Move Right",
				Target = InputControlType.LeftStickRight,
				Source = CustomInputDeviceProfile.KeyCodeButton(new KeyCode[]
				{
					KeyCode.D,
					KeyCode.RightArrow
				})
			}
		};
	}
}
