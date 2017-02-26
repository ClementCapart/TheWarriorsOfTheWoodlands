using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour 
{
	private CharacterDataModule m_Character = null;
	public bool m_HasAlreadyAttacked = false;
	public bool m_HasAlreadyBeenHurt = false;
	public bool m_LoopAttack = false;
	private Animator m_Animator = null;

	private void Start()
	{
		m_Character = GetComponent<CharacterDataModule>();
		m_Animator = GetComponent<Animator>();
	}

	private void Update()
	{
		if (m_Character.State != CharacterState.Dead)
		{

			if (m_Character.State == CharacterState.Hitlocked)
			{
				if (!m_HasAlreadyBeenHurt)
				{
					m_Animator.SetTrigger("Hurt");
				}
				m_HasAlreadyBeenHurt = true;
				
			}
			else
			{
				m_HasAlreadyBeenHurt = false;

				if (m_Character.IsMoving)
				{
					m_Animator.SetBool("Walk", true);
				}
				else
				{
					m_Animator.SetBool("Walk", false);
				}

				transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * m_Character.Direction, transform.localScale.y,
					transform.localScale.z);

				if (m_Character.IsAttacking && (m_LoopAttack || !m_HasAlreadyAttacked))
				{
					m_HasAlreadyAttacked = true;
					m_Animator.SetTrigger("Attack");
				}
				else if (!m_Character.IsAttacking)
				{
					m_HasAlreadyAttacked = false;
				}
			}			
		}
		else if (m_Character.State == CharacterState.Dead)
		{
			if (!m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Death"))
			{
				m_Animator.SetTrigger("Death");
			}
			else if (m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
			{
				Destroy(gameObject);
			}
		}
	}
}
