using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Controller : MonoBehaviour 
{
	protected float m_moveAxis = 0.0f;
	public float MoveAxis { get { return m_moveAxis; } }

	protected bool m_jumpRequested = false;
	public bool JumpRequested { get { return m_jumpRequested; } }

	public float m_MaxJumpRequestDelay = 0.1f;
	private float m_currentJumpRequestDelay = 0.0f;

	protected virtual void Update()
	{
		if(m_jumpRequested && m_currentJumpRequestDelay > m_MaxJumpRequestDelay)
		{
			CancelJumpRequest();
		}
		else if(m_jumpRequested)
		{
			m_currentJumpRequestDelay += Time.deltaTime;
		}
	}

	public void CancelJumpRequest()
	{
		m_jumpRequested = false;
		m_currentJumpRequestDelay = 0.0f;
	}

	public void RequestJump()
	{
		m_jumpRequested = true;
		m_currentJumpRequestDelay = 0.0f;
	}
}
