using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour 
{
	public float m_StartingHealthPoints = 100.0f;
	private float m_CurrentHealthPoints = 0.0f;

	void Start()
	{
		m_CurrentHealthPoints = m_StartingHealthPoints;
	}

	public void TakeDamage(float value)
	{
		m_CurrentHealthPoints -= value;
		if(m_CurrentHealthPoints <= 0.0f)
		{
			m_CurrentHealthPoints = 0.0f;
			Die();
		}
	}

	public void Die()
	{
		Destroy(gameObject);
	}
}
