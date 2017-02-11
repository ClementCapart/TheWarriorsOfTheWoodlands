using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseInTheWoodsScript : MonoBehaviour 
{
	public GameObject m_GameCameraPrefab = null;
	private SmartCamera m_GameCamera = null;
	public PointOfInterest m_ScriptedSwordSlasherPoint = null;
	public PointOfInterest m_ScriptedHousePoint = null;
	public AIController m_SwordSlasherAIController = null;
	public Transform m_SwordSlasherTargetPosition = null;
	public Transform m_SwordSlasherOutOfScreenPosition = null;
	public GameObject m_GameplayPointsOfInterest = null;
	public GameObject m_ScriptedPointsOfInterest = null;
	public float m_ScriptedCameraSmoothingRate = 0.5f;
	public float m_DefaultCameraSmoothingRate = 3.0f;

	public GameObject m_Enemies = null;
	public GameObject m_CamaastageTrigger = null;

	public AIController m_ToetoebowWallController = null;
	public Transform m_ToetoebowWallFirstTargetPosition = null;
	public Transform m_ToetoebowWallSecondTargetPosition = null;
	public Transform m_ToeToeBowWallAwayTargetPosition = null;

	public Animator m_HouseAnimator = null;

	[TextArea(4, 4)]
	public List<string> m_FirstIntroText = new List<string>();
	[TextArea(4, 4)]
	public List<string> m_AfterSwordSlasherText = new List<string>();
	[TextArea(4, 4)]
	public List<string> m_EndIntroText = new List<string>();

	[TextArea(4, 4)]
	public List<string> m_ToeToeBowWallText = new List<string>();

	public float m_DelayBetweenTexts = 3.0f;

	public float m_TextSpeed = 30.0f;

	public AudioSource m_AudioSource = null;
	public AudioClip m_StartAdventureTheme = null;
	public AudioClip m_SwordSlasherTheme = null;
	public AudioClip m_EscapeTheme = null;
	public AudioClip m_VictoryTheme = null;

	private PlayerController[] m_Controllers = null;

	void Start()
	{
		StartCoroutine(IntroScript());
		FadeScreen.FadeFromBlack(1.0f);
	}

	public void GetBackToHouse()
	{
		StartCoroutine(GetBackToHouseScript());
	}

	IEnumerator IntroScript()
	{
		m_GameplayPointsOfInterest.SetActive(false);
		m_Enemies.SetActive(false);

		m_AudioSource.clip = m_StartAdventureTheme;
		m_AudioSource.Play();

		m_Controllers = FindObjectsOfType<PlayerController>();

		for(int i = 0; i < m_Controllers.Length; i++)
		{
			m_Controllers[i].LockControl();
		}

		GameObject obj = Instantiate<GameObject>(m_GameCameraPrefab);
		m_GameCamera = obj.GetComponent<SmartCamera>();
		m_GameCamera.SetPosition(m_ScriptedHousePoint.transform.position, m_ScriptedHousePoint.m_Distance);
		yield return m_GameCamera.SetTargetAndWaitForSettle(m_ScriptedHousePoint.transform);
		m_GameCamera.m_TargetPositionSmoothRate = m_ScriptedCameraSmoothingRate;

		if(TextBox.Instance != null)
		{
			for (int i = 0; i < m_FirstIntroText.Count; i++)
			{
				yield return TextBox.Instance.DisplayText(m_FirstIntroText[i], m_TextSpeed);
				if(i != m_FirstIntroText.Count - 1)
					yield return new WaitForSeconds(m_DelayBetweenTexts);
			}
		}

		m_Enemies.SetActive(true);
		m_SwordSlasherAIController.SetTargetPosition(m_SwordSlasherTargetPosition);

		m_AudioSource.Stop();
		m_AudioSource.clip = m_SwordSlasherTheme;
		m_AudioSource.Play();

		yield return m_GameCamera.SetTargetAndWaitForSettle(m_ScriptedSwordSlasherPoint.transform);

		if (TextBox.Instance != null)
		{
			for (int i = 0; i < m_AfterSwordSlasherText.Count; i++)
			{
				yield return TextBox.Instance.DisplayText(m_AfterSwordSlasherText[i], m_TextSpeed);
				if (i != m_AfterSwordSlasherText.Count - 1)
					yield return new WaitForSeconds(m_DelayBetweenTexts);
			}
		}

		while (!m_SwordSlasherAIController.m_HasReachedDestination)
		{
			yield return 0;
		}

		m_SwordSlasherAIController.RequestAttack();

		yield return new WaitForSeconds(3.0f);

		yield return m_GameCamera.SetTargetAndWaitForSettle(m_ScriptedHousePoint.transform);

		if (TextBox.Instance != null)
		{
			for (int i = 0; i < m_EndIntroText.Count; i++)
			{
				yield return TextBox.Instance.DisplayText(m_EndIntroText[i], m_TextSpeed);
				if (i != m_EndIntroText.Count - 1)
					yield return new WaitForSeconds(m_DelayBetweenTexts);
			}
		}

		for (int i = 0; i < m_Controllers.Length; i++)
		{
			m_Controllers[i].UnlockControl();
		}

		if(TextBox.Instance)
		{
			TextBox.Instance.HideTextBox();
		}

		m_GameplayPointsOfInterest.SetActive(true);
		m_ScriptedPointsOfInterest.SetActive(false);
		m_GameCamera.m_MainTarget = m_Controllers[0].transform;
		m_GameCamera.m_TargetPositionSmoothRate = m_DefaultCameraSmoothingRate;
	}

	IEnumerator GetBackToHouseScript()
	{
		for (int i = 0; i < m_Controllers.Length; i++)
		{
			m_Controllers[i].LockControl();
		}
		m_SwordSlasherAIController.SetTargetPosition(m_SwordSlasherOutOfScreenPosition);

		while(!m_SwordSlasherAIController.m_HasReachedDestination)
		{
			yield return 0;
		}

		m_ToetoebowWallController.SetTargetPosition(m_ToetoebowWallFirstTargetPosition);

		while(!m_ToetoebowWallController.m_HasReachedDestination)
		{
			yield return 0;
		}

		if (TextBox.Instance != null)
		{
			for (int i = 0; i < m_ToeToeBowWallText.Count; i++)
			{
				yield return TextBox.Instance.DisplayText(m_ToeToeBowWallText[i], m_TextSpeed);
				if (i != m_ToeToeBowWallText.Count - 1)
					yield return new WaitForSeconds(m_DelayBetweenTexts);
			}
		}

		m_GameCamera.m_TargetPositionSmoothRate = m_ScriptedCameraSmoothingRate;

		m_GameplayPointsOfInterest.SetActive(false);
		m_ScriptedPointsOfInterest.SetActive(true);

		yield return m_GameCamera.SetTargetAndWaitForSettle(m_ScriptedHousePoint.transform);

		yield return new WaitForSeconds(0.5f);

		m_GameCamera.m_TargetPositionSmoothRate = m_DefaultCameraSmoothingRate;

		m_GameplayPointsOfInterest.SetActive(true);
		m_ScriptedPointsOfInterest.SetActive(false);
		m_GameCamera.m_MainTarget = m_Controllers[0].transform;

		for (int i = 0; i < m_Controllers.Length; i++)
		{
			m_Controllers[i].UnlockControl();
		}

		m_AudioSource.Stop();
		m_AudioSource.clip = m_EscapeTheme;
		m_AudioSource.Play();

		yield return new WaitForSeconds(1.0f);

		m_ToetoebowWallController.SetTargetPosition(m_ToetoebowWallSecondTargetPosition);
		if (TextBox.Instance) TextBox.Instance.HideTextBox();
		m_CamaastageTrigger.SetActive(true);
	}

	public void ToeToeBowsAway()
	{
		StartCoroutine(ToeToeBowsAwayScript());
	}

	IEnumerator ToeToeBowsAwayScript()
	{
		for(int i = 0; i < m_Controllers.Length; i++)
		{
			m_Controllers[i].gameObject.SetActive(false);
		}

		m_HouseAnimator.Play("HouseCamastaage");

		m_ToetoebowWallController.SetTargetPosition(null);

		yield return new WaitForSeconds(2.0f);

		m_ToetoebowWallController.SetTargetPosition(m_ToeToeBowWallAwayTargetPosition);

		yield return new WaitForSeconds(3.0f);

		m_HouseAnimator.Play("HouseHighTechReveal");

		for (int i = 0; i < m_Controllers.Length; i++)
		{
			m_Controllers[i].gameObject.SetActive(true);
		}

		m_AudioSource.Stop();
		m_AudioSource.clip = m_VictoryTheme;
		m_AudioSource.Play();
	}

}
