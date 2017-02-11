using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuHandler : MonoBehaviour 
{
	private bool m_StartingGame = false;

	void Start()
	{
		FadeScreen.FadeFromBlack(3.0f);
	}

	void Update()
	{
		if(!m_StartingGame)
		{
			if (Input.anyKeyDown || XInput.GetButtonDown(Buttons.None, 0) || XInput.GetButtonDown(Buttons.None, 1))
			{
				RequestStartGame();
			}
		}		
	}

	[ContextMenu("Start Game")]
	private void RequestStartGame()
	{
		m_StartingGame = true;
		FadeScreen.FadeToBlack(3.0f, StartGame);
	}

	private void StartGame()
	{
		SceneManager.LoadScene("HouseInTheWoods");
	}
}
