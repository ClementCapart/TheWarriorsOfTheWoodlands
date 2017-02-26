using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtBox : MonoBehaviour 
{
	public float m_Damage = 10.0f;
	public float m_ImpulseForce = 10.0f;

	void OnTriggerEnter2D(Collider2D col)
	{
		AttackTarget target = col.GetComponent<AttackTarget>();
		if(target != null)
		{
			target.TakeDamage(m_Damage, new Vector3(((col.transform.position.x - transform.position.x) < 0 ? -1 : 1), 1.0f, 0.0f) * m_ImpulseForce);
		}
	}
}
