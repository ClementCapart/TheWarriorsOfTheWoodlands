using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxSprite : MonoBehaviour 
{
	[Range(0.0f, 2.0f)]
	public float m_HorizontalParallaxSpeed = 1.0f;

	[Range(0.0f, 2.0f)]
	public float m_VerticalParallaxSpeed = 1.0f;
}
