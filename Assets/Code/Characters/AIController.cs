using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : Controller 
{
	public int m_Direction = 0;

	public float m_DelayBetweenJumps = -1.0f;
	private float m_CurrentJumpDelay = 0.0f;

	private Transform m_TargetPosition = null;
	public bool m_HasReachedDestination = false;

	void Start()
	{
		m_CurrentJumpDelay = m_DelayBetweenJumps;
	}

	protected override void Update()
	{
		if(m_TargetPosition != null && Mathf.Abs((m_TargetPosition.position.x - transform.position.x)) > 0.1f)
		{
			m_HasReachedDestination = false;
			m_Direction = m_TargetPosition.position.x - transform.position.x < 0 ? -1 : 1;
		}
		else if(m_TargetPosition != null)
		{
			m_HasReachedDestination = true;
			m_Direction = 0;
		}

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

	public void SetTargetPosition(Transform target)
	{
		m_TargetPosition = target;
		m_Direction = 0;
		if(m_TargetPosition == null)
		{
			m_HasReachedDestination = true;
		}
		else
		{
			m_HasReachedDestination = false;
		}
	}
}
