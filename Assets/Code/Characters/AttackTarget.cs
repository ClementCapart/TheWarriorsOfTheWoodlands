using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTarget : MonoBehaviour 
{
	public Health m_Health = null;
	public Rigidbody2D m_Rigidbody = null;

	public void TakeDamage(float value, Vector3 impulse)
	{
		if (m_Health) m_Health.TakeDamage(value);
		if (m_Rigidbody) m_Rigidbody.AddForce(impulse, ForceMode2D.Impulse);
	}
}
