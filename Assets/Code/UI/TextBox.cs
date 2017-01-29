using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBox : MonoBehaviour 
{
	public static TextBox Instance = null;

	public Text m_TextArea;

	void Awake()
	{
		Instance = this;
	}

	public IEnumerator DisplayText(string text, float speed)
	{
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
}
