using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackState
{
	Idle,
	Execute,
	Cooldown,
}

public class Attack : MonoBehaviour 
{
	private AttackState m_currentState = AttackState.Idle;
	public AttackState State { get { return m_currentState; } }
	public CharacterDataModule m_Character;
	public float m_Cooldown = 1.0f;
	public float m_Duration = 0.5f;

	public float m_Damage = 50.0f;
	public Vector3 m_Impulse = Vector3.zero;

	public Collider2D m_Collider = null;

	private List<AttackTarget> m_AlreadyHitTargets = new List<AttackTarget>();

	void OnTriggerEnter2D(Collider2D col)
	{
		AttackTarget target = col.GetComponent<AttackTarget>();
		if(target != null && !m_AlreadyHitTargets.Contains(target))
		{
			m_AlreadyHitTargets.Add(target);
			Vector3 impulse = m_Impulse;
			if(m_Character)
			{
				impulse = new Vector3(impulse.x * (m_Character.Direction >= 0 ? 1 : -1), impulse.y, impulse.z); 
			}
			target.TakeDamage(m_Damage, impulse);
		}
	}

	public bool TryExecute()
	{
		if(State != AttackState.Idle)
		{
			return false;
		}
		else
		{
			StartCoroutine(AttackCoroutine());
		}

		return false;
	}

	IEnumerator AttackCoroutine()
	{
		float currentTimer = 0.0f;

		m_AlreadyHitTargets.Clear();

		if (m_Collider) m_Collider.enabled = true;

		currentTimer = m_Duration;

		m_currentState = AttackState.Execute;

		while(currentTimer > 0.0f)
		{
			currentTimer -= Time.deltaTime;
			yield return 0;
		}

		if (m_Collider) m_Collider.enabled = false;

		currentTimer = m_Cooldown;
		m_currentState = AttackState.Cooldown;

		while(currentTimer > 0.0f)
		{
			currentTimer -= Time.deltaTime;
			yield return 0;
		}

		m_currentState = AttackState.Idle;
	}
}
