using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointOfInterest : MonoBehaviour
{
	public enum ShapeType
	{
		Circle,
		Line
	}

	public ShapeType m_ShapeType = ShapeType.Circle;

	public float m_InnerRadius = 1.0f;
	public float m_OuterRadius = 2.0f;
	public float m_Distance = 15.0f;

	public Transform m_SecondLinePoint = null;

	void OnValidate()
	{
		m_InnerRadius = Mathf.Min(m_InnerRadius, m_OuterRadius);
	}

	public float GetInfluencedDisplacementAndDistance(Vector3 position, float distance, out Vector3 displacement, out float distanceDisplacement)
	{
		Vector3 currentPosition = transform.position;

		if(m_ShapeType == ShapeType.Line && m_SecondLinePoint != null)
		{
			currentPosition = MathUtils.NearpointOnLine(currentPosition, m_SecondLinePoint.position, position);
		}

		float influence = GetInfluence(position);

		displacement = Vector3.Lerp(position, currentPosition, influence) - position;
		distanceDisplacement = Mathf.Lerp(distance, m_Distance, influence) - distance;

		return influence;
	}

	private float GetInfluence(Vector3 position)
	{
		Vector3 currentPosition = transform.position;
		if (m_ShapeType == ShapeType.Line && m_SecondLinePoint != null)
		{
			currentPosition = MathUtils.NearpointOnLine(currentPosition, m_SecondLinePoint.position, position);
		}

		Vector3 line = position - currentPosition;
		float distance = line.magnitude;

		return 1 - Mathf.Clamp01((distance - m_InnerRadius) / (m_OuterRadius - m_InnerRadius));
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.white;

		if (m_ShapeType == ShapeType.Circle)
		{
			Gizmos.DrawSphere(transform.position, 0.1f);
#if UNITY_EDITOR
			UnityEditor.Handles.color = Color.white;
			UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.forward, m_InnerRadius);
			UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.forward, m_OuterRadius);
#endif
		}
		else if(m_ShapeType == ShapeType.Line)
		{
			if (m_SecondLinePoint != null)
			{
				Gizmos.DrawLine(transform.position, m_SecondLinePoint.position);

#if UNITY_EDITOR
				Vector3 widthDirection = Vector3.Cross(m_SecondLinePoint.position - transform.position, Vector3.forward);
				widthDirection.Normalize();

				Gizmos.color = Color.grey;
				UnityEditor.Handles.color = Color.grey;
				UnityEditor.Handles.DrawWireArc(transform.position, Vector3.forward, -widthDirection, 180.0f, m_InnerRadius);
				UnityEditor.Handles.DrawWireArc(m_SecondLinePoint.position, Vector3.forward, widthDirection, 180.0f, m_InnerRadius);
				Gizmos.DrawLine(transform.position + widthDirection * m_InnerRadius, m_SecondLinePoint.position + widthDirection * m_InnerRadius);
				Gizmos.DrawLine(transform.position - widthDirection * m_InnerRadius, m_SecondLinePoint.position - widthDirection * m_InnerRadius);

				UnityEditor.Handles.DrawWireArc(transform.position, Vector3.forward, -widthDirection, 180.0f, m_OuterRadius);
				UnityEditor.Handles.DrawWireArc(m_SecondLinePoint.position, Vector3.forward, widthDirection, 180.0f, m_OuterRadius);
				Gizmos.DrawLine(transform.position + widthDirection * m_OuterRadius, m_SecondLinePoint.position + widthDirection * m_OuterRadius);
				Gizmos.DrawLine(transform.position - widthDirection * m_OuterRadius, m_SecondLinePoint.position - widthDirection * m_OuterRadius);
#endif
			}
		}
	}
}
