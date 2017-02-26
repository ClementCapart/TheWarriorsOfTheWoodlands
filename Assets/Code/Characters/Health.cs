using UnityEngine;

public class Health : MonoBehaviour
{
	public float m_StartingHealthPoints = 100.0f;
	private float m_CurrentHealthPoints = 0.0f;

	private CharacterDataModule m_Character = null;

	void Start()
	{
		m_CurrentHealthPoints = m_StartingHealthPoints;
		m_Character = GetComponent<CharacterDataModule>();
		m_Character.CurrentHealthPoints = m_CurrentHealthPoints;
		m_Character.MaxHealthPoints = m_StartingHealthPoints;
	}

	public void TakeDamage(float value)
	{
		if(m_Character.State == CharacterState.Dead)
		{
			return;
		}

		m_CurrentHealthPoints -= value;
		if (m_CurrentHealthPoints <= 0.0f)
		{
			m_CurrentHealthPoints = 0.0f;
			Die();
		}

		m_Character.CurrentHealthPoints = m_CurrentHealthPoints;
	}

	public void Die()
	{
		m_Character.ChangeState(CharacterState.Dead);
	}
}
