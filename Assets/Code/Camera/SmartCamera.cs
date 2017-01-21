using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartCamera : MonoBehaviour
{
    public Transform m_MainTarget = null;
	public float m_DefaultDistance = 10.0f;

	public float m_TargetPositionSmoothRate = 1.0f;
	public float m_TargetDistanceSmoothRate = 1.0f;
	public float m_OrthographicDistanceFactor = 0.6f;

	private SmoothPosition m_TargetPosition;
    private SmoothFloat m_TargetDistance;

	private Camera m_Camera = null;

	private List<PointOfInterest> m_CurrentPointsOfInterest = new List<PointOfInterest>();

    private void Start()
    {
		m_TargetPosition = new SmoothPosition(m_TargetPositionSmoothRate);
		m_TargetDistance = new SmoothFloat(m_TargetDistanceSmoothRate);
		m_Camera = GetComponent<Camera>();

		m_CurrentPointsOfInterest = new List<PointOfInterest>(Object.FindObjectsOfType<PointOfInterest>());
    }

    private void Update()
    {
		Vector3 displacement = Vector3.zero;
		float distanceDisplacement = 0.0f;

		for (int i = 0; i < m_CurrentPointsOfInterest.Count; i++)
		{			
			Vector3 currentDisplacement = Vector3.zero;
			float currentDistanceDisplacement = 0.0f;
			m_CurrentPointsOfInterest[i].GetInfluencedDisplacementAndDistance(m_MainTarget.position, m_DefaultDistance, out currentDisplacement, out currentDistanceDisplacement);

			displacement += currentDisplacement;
			distanceDisplacement += currentDistanceDisplacement;
		}

		m_TargetPosition.Value = m_MainTarget.position + displacement;
		m_TargetDistance.Value = m_DefaultDistance + distanceDisplacement;

		m_TargetPosition.Update(Time.deltaTime);
		m_TargetDistance.Update(Time.deltaTime);		

		transform.position = m_TargetPosition.Value - new Vector3(0.0f, 0.0f, m_TargetDistance.Value);
		if(m_Camera != null && m_Camera.orthographic)
		{
			m_Camera.orthographicSize = m_TargetDistance.Value * m_OrthographicDistanceFactor;
		}
    }

	void OnDrawGizmos()
	{
		if (Application.isPlaying)
		{
#if UNITY_EDITOR
			UnityEditor.Handles.color = Color.red;
			UnityEditor.Handles.DrawWireDisc(m_TargetPosition.Target, Vector3.forward, 0.3f);
#endif
		}
	}
}
