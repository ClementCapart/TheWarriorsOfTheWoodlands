using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour 
{
	public Image m_HealthImage = null;
	public GameObject m_ParentObject = null;
	public string m_AttachedCharacter = "Ziggy";

	private CharacterDataModule m_CharacterDataModule = null;
	private float m_LastHealthRecorded = -1.0f;

	void Update()
	{
		if(!m_CharacterDataModule)
		{
			m_CharacterDataModule = CharacterDataModule.GetCharacterByName(m_AttachedCharacter);
		}

		if(m_CharacterDataModule)
		{
			m_ParentObject.SetActive(true);
			if (m_LastHealthRecorded < 0.0f || m_LastHealthRecorded != m_CharacterDataModule.CurrentHealthPoints)
			{
				UpdateHealthBar();
			}
		}
		else
		{
			m_ParentObject.SetActive(false);
		}
	}

	void UpdateHealthBar()
	{
		m_LastHealthRecorded = m_CharacterDataModule.CurrentHealthPoints;
		m_HealthImage.fillAmount = (m_CharacterDataModule.CurrentHealthPoints / m_CharacterDataModule.MaxHealthPoints);
	}
}
