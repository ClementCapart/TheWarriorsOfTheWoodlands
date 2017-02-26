using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTarget : MonoBehaviour 
{
	public CharacterDataModule m_Character = null;
	public Health m_Health = null;
	public Rigidbody2D m_Rigidbody = null;

	private Coroutine m_HitLockCoroutine = null;

	void Awake()
	{
		m_Character = GetComponent<CharacterDataModule>();
	}

	public void TakeDamage(float value, Vector3 impulse, float hitlockDuration = 1.0f)
	{
		if (m_Character && m_Character.State == CharacterState.Default)
		{
			if (m_Health) m_Health.TakeDamage(value);
			if (m_Rigidbody) m_Rigidbody.AddForce(impulse, ForceMode2D.Impulse);
			if(m_HitLockCoroutine != null)
			{
				StopCoroutine(m_HitLockCoroutine);
			}
			m_HitLockCoroutine = StartCoroutine(Hitlock(hitlockDuration));
		}
	}

	public IEnumerator Hitlock(float duration)
	{
		m_Character.ChangeState(CharacterState.Hitlocked);

		while (duration > 0.0f)
		{
			yield return null;
			duration -= Time.deltaTime;
		}

		m_Character.ChangeState(CharacterState.Default);
		m_HitLockCoroutine = null;
	}
}
