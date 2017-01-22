using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTarget : MonoBehaviour 
{
	public Health m_Health = null;

	public void TakeDamage(float value)
	{
		if (m_Health) m_Health.TakeDamage(value);
	}
}
