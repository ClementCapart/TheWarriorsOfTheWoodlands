using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerEvent : MonoBehaviour 
{
	public UnityEngine.Events.UnityEvent m_Events = null;

	public bool m_TriggerOnlyOnce = false;
	private bool m_AlreadyTriggered = false;

	public void OnTriggerEnter2D(Collider2D collider)
	{
		TryTrigger();
	}

	public void TryTrigger()
	{
		if(!m_AlreadyTriggered || !m_TriggerOnlyOnce)
		{
			m_Events.Invoke();
			m_AlreadyTriggered = true;
		}
	}
}
