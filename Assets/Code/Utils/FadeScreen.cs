using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeScreen : MonoBehaviour 
{
	private static FadeScreen s_instance;
	public static FadeScreen Instance
	{
		get { return s_instance; }
	}

	private UnityEngine.UI.Image m_BlackScreen = null;
	private Coroutine m_CurrentCoroutine = null;

	void Awake()
	{
		if (s_instance != null)
		{
			Destroy(this.gameObject);
			return;
		}
		DontDestroyOnLoad(gameObject);
		s_instance = this;
		m_BlackScreen = GetComponentInChildren<UnityEngine.UI.Image>();
	}

	public static void FadeToBlack(float duration, Action endCallback = null)
	{
		if (Instance)
		{
			if(Instance.m_CurrentCoroutine != null)
			{
				Instance.StopCoroutine(Instance.m_CurrentCoroutine);				
			}
			if (Instance.isActiveAndEnabled)
			{
				Instance.m_CurrentCoroutine = Instance.StartCoroutine(Instance.Fade(true, duration, endCallback));
			}
			else
			{
				if (endCallback != null)
				{
					endCallback.Invoke();
				}
			}
		}
		else
		{
			if (endCallback != null)
			{
				endCallback.Invoke();
			}
		}
	}

	public static void FadeFromBlack(float duration, Action endCallback = null)
	{
		if (Instance)
		{
			if (Instance.m_CurrentCoroutine != null)
			{
				Instance.StopCoroutine(Instance.m_CurrentCoroutine);
			}

			if(Instance.isActiveAndEnabled)
			{
				Instance.m_CurrentCoroutine = Instance.StartCoroutine(Instance.Fade(false, duration, endCallback));
			}
			else
			{
				if (endCallback != null)
				{
					endCallback.Invoke();
				}
			}			
		}
		else
		{
			if (endCallback != null)
			{
				endCallback.Invoke();
			}
		}
	}

	IEnumerator Fade(bool toBlack, float duration, Action endCallback)
	{
		float totalDuration = duration;
		duration = Mathf.Lerp(duration, 0.0f, (toBlack ? m_BlackScreen.color.a : (1 - m_BlackScreen.color.a)));

		if (toBlack) m_BlackScreen.enabled = true;

		while(duration > 0.0f)
		{
			yield return 0;
			duration -= Time.deltaTime;

			float alpha = duration / totalDuration;
			if (toBlack) alpha = 1 - alpha;

			m_BlackScreen.color = new Color(0.0f, 0.0f, 0.0f, alpha);
		}

		if (toBlack)
		{
			m_BlackScreen.color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
		}
		else
		{
			m_BlackScreen.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
			m_BlackScreen.enabled = false;
		}

		if(endCallback != null)
		{
			endCallback.Invoke();
		}
	}
}
