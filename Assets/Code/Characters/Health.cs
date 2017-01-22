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
	}

	public void TakeDamage(float value)
	{
		if(m_Character.IsDead)
		{
			return;
		}

		m_CurrentHealthPoints -= value;
		if (m_CurrentHealthPoints <= 0.0f)
		{
			m_CurrentHealthPoints = 0.0f;
			Die();
		}
	}

	public void Die()
	{
		m_Character.IsDead = true;
	}
}
