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

	void Awake()
	{
		StartCoroutine(IntroScript());
	}

	[ContextMenu("Focus on Sword Slasher")]
	public void FocusOnSwordSlasher()
	{
		m_GameCamera.m_MainTarget = m_ScriptedSwordSlasherPoint.transform;
	}

	[ContextMenu("Focus on House")]
	public void FocusOnHouse()
	{
		m_GameCamera.m_MainTarget = m_ScriptedHousePoint.transform;
	}

	IEnumerator IntroScript()
	{
		m_GameplayPointsOfInterest.SetActive(false);
		GameObject obj = Instantiate<GameObject>(m_GameCameraPrefab);
		m_GameCamera = obj.GetComponent<SmartCamera>();
		m_GameCamera.SetPosition(m_ScriptedSwordSlasherPoint.transform.position, m_ScriptedSwordSlasherPoint.m_Distance);
		m_GameCamera.m_MainTarget = m_ScriptedSwordSlasherPoint.transform;
		m_GameCamera.m_TargetPositionSmoothRate = m_ScriptedCameraSmoothingRate;

		m_SwordSlasherAIController.SetTargetPosition(m_SwordSlasherTargetPosition);
		
		while(!m_SwordSlasherAIController.m_HasReachedDestination)
		{
			yield return 0;
		}

		m_SwordSlasherAIController.RequestAttack();

		yield return new WaitForSeconds(3.0f);

		FocusOnHouse();
	}

}
