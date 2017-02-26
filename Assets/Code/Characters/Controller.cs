using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Controller : MonoBehaviour
{
	public CharacterDataModule m_Character = null;

	protected float m_moveAxis = 0.0f;
	public float MoveAxis { get { return m_moveAxis; } }

	protected bool m_jumpRequested = false;
	public bool JumpRequested { get { return m_jumpRequested; } }

	protected bool m_attackRequested = false;
	public bool AttackRequested { get { return m_attackRequested; } }

	public float m_MaxJumpRequestDelay = 0.1f;
	public float m_MaxAttackRequestDelay = 0.1f;
	private float m_currentJumpRequestDelay = 0.0f;
	private float m_currentAttackRequestDelay = 0.0f;

	protected virtual void Update()
	{
		if(m_jumpRequested && m_MaxJumpRequestDelay >= 0.0f && m_currentJumpRequestDelay > m_MaxJumpRequestDelay)
		{
			CancelJumpRequest();
		}
		else if(m_jumpRequested)
		{
			m_currentJumpRequestDelay += Time.deltaTime;
		}

		if(m_attackRequested && m_MaxAttackRequestDelay >= 0.0f && m_currentAttackRequestDelay > m_MaxAttackRequestDelay)
		{
			CancelAttackRequest();
		}
		else if(m_attackRequested)
		{
			m_currentAttackRequestDelay += Time.deltaTime;
		}		
	}

	public void CancelJumpRequest()
	{
		m_jumpRequested = false;
		m_currentJumpRequestDelay = 0.0f;
	}

	public void CancelAttackRequest()
	{
		m_attackRequested = false;
		m_currentAttackRequestDelay = 0.0f;
	}

	public void RequestJump()
	{
		m_jumpRequested = true;
		m_currentJumpRequestDelay = 0.0f;
	}

	public void RequestAttack()
	{
		m_attackRequested = true;
		m_currentJumpRequestDelay = 0.0f;
	}


}
