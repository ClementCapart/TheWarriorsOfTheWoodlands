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
	public GameObject m_GameplayPointsOfInterest = null;
	public float m_ScriptedCameraSmoothingRate = 0.5f;

	public GameObject m_Enemies = null;

	[TextArea(4, 4)]
	public List<string> m_FirstIntroText = new List<string>();
	public float m_DelayBetweenTexts = 3.0f;

	public float m_TextSpeed = 30.0f;

	public PlayerController[] m_Controllers = null;

	void Awake()
	{
		StartCoroutine(IntroScript());
	}

	IEnumerator IntroScript()
	{
		m_GameplayPointsOfInterest.SetActive(false);
		m_Enemies.SetActive(false);

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

		yield return m_GameCamera.SetTargetAndWaitForSettle(m_ScriptedSwordSlasherPoint.transform);		
		
		while(!m_SwordSlasherAIController.m_HasReachedDestination)
		{
			yield return 0;
		}

		m_SwordSlasherAIController.RequestAttack();

		yield return new WaitForSeconds(3.0f);

		yield return m_GameCamera.SetTargetAndWaitForSettle(m_ScriptedHousePoint.transform);

		for (int i = 0; i < m_Controllers.Length; i++)
		{
			m_Controllers[i].UnlockControl();
		}
	}

}
