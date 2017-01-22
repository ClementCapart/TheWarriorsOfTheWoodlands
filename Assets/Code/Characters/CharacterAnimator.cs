using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour 
{
	private CharacterDataModule m_Character = null;
	public bool m_HasAlreadyAttacked = false;
	private Animator m_Animator = null;

	void Start()
	{
		m_Character = GetComponent<CharacterDataModule>();
		m_Animator = GetComponent<Animator>();
	}

	void Update()
	{
		if(m_Character.IsMoving)
		{
			m_Animator.SetBool("Walk", true);
		}
		else
		{
			m_Animator.SetBool("Walk", false);
		}

		transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * m_Character.Direction, transform.localScale.y, transform.localScale.z);

		if(m_Character.IsAttacking && !m_HasAlreadyAttacked) 
		{
			m_HasAlreadyAttacked = true;
			m_Animator.SetTrigger("Attack");
		}
		else if(!m_Character.IsAttacking)
		{
			m_HasAlreadyAttacked = false;
		}
	}
}
