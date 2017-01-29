using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttack : MonoBehaviour 
{
	public CharacterDataModule m_Character = null;
	public Controller m_Controller = null;

	public Attack m_Attack = null;

	void Start()
	{
		m_Character = GetComponent<CharacterDataModule>();
	}

	void Update()
	{
		if (m_Controller == null) return;

		if(m_Controller.AttackRequested)
		{
			if(m_Attack == null || m_Attack.TryExecute())
			{		
				m_Controller.CancelAttackRequest();
			}			
		}

		if(m_Attack != null && m_Attack.State != AttackState.Idle)
		{
			if (m_Character) m_Character.IsAttacking = true;
		}
		else
		{
			if (m_Character) m_Character.IsAttacking = false;
		}
	}
}
