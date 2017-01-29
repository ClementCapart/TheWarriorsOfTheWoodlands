using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBox : MonoBehaviour 
{
	public static TextBox Instance = null;

	public Text m_TextArea;
	public GameObject m_Parent = null;

	void Awake()
	{
		Instance = this;
	}

	public IEnumerator DisplayText(string text, float speed)
	{
		ShowTextBox();

		m_TextArea.text = "";

		float substring = 0;

		while (text.Length > 0)
		{
			substring += Mathf.Clamp(speed * Time.deltaTime, 0, text.Length);
			if(substring >= 1.0f)
			{
				m_TextArea.text += text.Substring(0, (int)substring);
				text = text.Remove(0, (int)substring);
				substring = 0.0f;
			}			
			
			yield return 0;
		}
	}

	public void HideTextBox()
	{
		if(m_Parent != null)
		{
			m_Parent.SetActive(false);
		}
	}

	public void ShowTextBox()
	{
		if(m_Parent != null)
		{
			m_Parent.SetActive(true);
		}
	}
}
