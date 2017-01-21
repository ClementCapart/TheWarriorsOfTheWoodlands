using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour 
{
	private List<ParallaxSprite> m_ParallaxSprites = new List<ParallaxSprite>();
	private Vector3 m_lastFramePosition = Vector3.zero;

	void Start()
	{
		m_ParallaxSprites = new List<ParallaxSprite>(FindObjectsOfType<ParallaxSprite>());
		m_lastFramePosition = transform.position;
	}

	void LateUpdate()
	{
		for(int i = 0; i < m_ParallaxSprites.Count; i++)
		{
			Vector3 parallaxPosition = (m_lastFramePosition - transform.position) * (m_ParallaxSprites[i].m_ParallaxSpeed - 1.0f) ;
			m_ParallaxSprites[i].transform.position += new Vector3(parallaxPosition.x, parallaxPosition.y, 0.0f);
		}

		m_lastFramePosition = transform.position;
	}
}
