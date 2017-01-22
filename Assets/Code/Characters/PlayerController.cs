﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ControlDevice
{
	None = 0,
	Joystic = 1,
	Keyboard = 2,
	Both = Joystic | Keyboard,
}

public class PlayerController : Controller 
{
	public ControlDevice m_ControlDevice = ControlDevice.Both;

	[Header("Joystick Controls")]
	public int m_PlayerIndex = 0;
	public Axis m_MoveAxisControl = Axis.LeftStickHorizontal;
	public Buttons m_JumpButton = Buttons.A;

	[Header("Keyboard Controls")]
	public KeyCode m_MoveLeftKeyboard = KeyCode.LeftArrow;
	public KeyCode m_MoveRightKeyboard = KeyCode.LeftArrow;
	public KeyCode m_JumpKeyboard = KeyCode.Space;

	protected override void Update()
	{
		if((m_ControlDevice & ControlDevice.Joystic) != 0)
		{
			m_moveAxis = XInput.GetAxis(m_MoveAxisControl, m_PlayerIndex);

			if(XInput.GetButtonDown(m_JumpButton, m_PlayerIndex))
			{
				RequestJump();
			}
		}

		if ((m_ControlDevice & ControlDevice.Keyboard) != 0)
		{
			m_moveAxis += Input.GetKey(m_MoveLeftKeyboard) ? -1.0f : Input.GetKey(m_MoveRightKeyboard) ? 1.0f : 0.0f;

			if(Input.GetKeyDown(m_JumpKeyboard))
			{
				RequestJump();
			}
		}

		base.Update();
	}
}