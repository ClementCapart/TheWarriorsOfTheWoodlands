using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : Controller 
{
	public int m_Direction = 0;

	public float m_DelayBetweenJumps = -1.0f;
	private float m_CurrentJumpDelay = 0.0f;

	void Start()
	{
		m_CurrentJumpDelay = m_DelayBetweenJumps;
	}

	protected override void Update()
	{
		m_moveAxis = m_Direction;

		if(m_CurrentJumpDelay >= 0.0f)
		{
			m_CurrentJumpDelay -= Time.deltaTime;
			if(m_CurrentJumpDelay <= 0.0f)
			{
				m_CurrentJumpDelay = m_DelayBetweenJumps;
				RequestJump();
			}
		}

		base.Update();
	}
}
