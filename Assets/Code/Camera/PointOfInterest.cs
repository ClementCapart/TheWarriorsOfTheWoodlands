using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointOfInterest : MonoBehaviour
{
	public float m_InnerRadius = 1.0f;
	public float m_OuterRadius = 2.0f;
	public float m_Distance = 15.0f;

	void OnValidate()
	{
		m_InnerRadius = Mathf.Min(m_InnerRadius, m_OuterRadius);
	}

	public float GetInfluencedDisplacementAndDistance(Vector3 position, float distance, out Vector3 displacement, out float distanceDisplacement)
	{
		Vector3 currentPosition = transform.position;
		float influence = GetInfluence(position);

		displacement = Vector3.Lerp(position, currentPosition, influence) - position;
		distanceDisplacement = Mathf.Lerp(distance, m_Distance, influence) - distance;

		return influence;
	}

	private float GetInfluence(Vector3 position)
	{
		Vector3 currentPosition = transform.position;
		Vector3 line = position - currentPosition;
		float distance = line.magnitude;

		return 1 - Mathf.Clamp01((distance - m_InnerRadius) / (m_OuterRadius - m_InnerRadius));
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.white;
		Gizmos.DrawSphere(transform.position, 0.1f);

#if UNITY_EDITOR
		UnityEditor.Handles.color = Color.white;
		UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.forward, m_InnerRadius);
		UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.forward, m_OuterRadius);
#endif
	}
}
