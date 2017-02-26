using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenuHandler : MonoBehaviour 
{
	enum StartMenuState
	{
		Invalid,
		PressAnyKey,
		SelectCharacter,
		Launching,
	}

	private StartMenuState m_State = StartMenuState.Invalid;

	public List<Graphic> m_PressAnyKeyImages = null;
	public List<Graphic> m_CharacterSelectImages = null;
	public Graphic m_PanthodeoImage = null;
	public Graphic m_ZiggyImage = null;
	public Graphic m_StartGameGraphic = null;

	void Start()
	{
		FadeScreen.FadeFromBlack(3.0f);
		for (int i = 0; i < m_PressAnyKeyImages.Count; i++)
		{
			m_PressAnyKeyImages[i].CrossFadeAlpha(0.0f, 0.0f, false);
		}

		for (int i = 0; i < m_CharacterSelectImages.Count; i++)
		{
			m_CharacterSelectImages[i].CrossFadeAlpha(0.0f, 0.0f, false);
		}

		m_ZiggyImage.CrossFadeAlpha(0.0f, 0.0f, false);
		m_PanthodeoImage.CrossFadeAlpha(0.0f, 0.0f, false);
		m_StartGameGraphic.CrossFadeAlpha(0.0f, 0.0f, false);

		RequestState(StartMenuState.PressAnyKey);
	}

	void Update()
	{
		UpdateState();	
	}

	void RequestState(StartMenuState newState)
	{
		if (newState != m_State)
		{
			OnExitState();
			m_State = newState;
			OnEnterState();
		}
	}

	void OnEnterState()
	{
		switch (m_State)
		{
			case StartMenuState.PressAnyKey:
				for (int i = 0; i < m_PressAnyKeyImages.Count; i++)
				{
					m_PressAnyKeyImages[i].CrossFadeAlpha(1.0f, 0.5f, false);
				}
				break;

			case StartMenuState.SelectCharacter:
				GameSessionData.s_CurrentCharacters = Characters.None;

				m_PanthodeoImage.CrossFadeAlpha(0.0f, 0.0f, false);
				m_ZiggyImage.CrossFadeAlpha(0.0f, 0.0f, false);
				for (int i = 0; i < m_PressAnyKeyImages.Count; i++)
				{
					m_PressAnyKeyImages[i].CrossFadeAlpha(0.0f, 0.5f, false);
				}

				for (int i = 0; i < m_CharacterSelectImages.Count; i++)
				{
					m_CharacterSelectImages[i].CrossFadeAlpha(1.0f, 0.5f, false);
				}
				break;

			case StartMenuState.Launching:
				RequestStartGame();
				break;
		}
	}

	void UpdateState()
	{
		switch(m_State)
		{
			case StartMenuState.PressAnyKey:
				if (Input.anyKeyDown || XInput.GetButtonDown(Buttons.None, 0) || XInput.GetButtonDown(Buttons.None, 1))
				{
					RequestState(StartMenuState.SelectCharacter);
				}
				break;

			case StartMenuState.SelectCharacter:
				if (Input.GetKeyDown(KeyCode.Space) || XInput.GetButtonDown(Buttons.A, 0))
				{
					if ((GameSessionData.s_CurrentCharacters & Characters.Panthodeo) == 0)
					{
						if (GameSessionData.s_CurrentCharacters == 0)
						{
							m_StartGameGraphic.CrossFadeAlpha(1.0f, 0.5f, false);
						}
						GameSessionData.s_CurrentCharacters |= Characters.Panthodeo;
						m_PanthodeoImage.CrossFadeAlpha(1.0f, 0.5f, false);
					}
				}

				if (Input.GetKeyDown(KeyCode.RightControl) || XInput.GetButtonDown(Buttons.A, 1))
				{
					if ((GameSessionData.s_CurrentCharacters & Characters.Ziggy) == 0)
					{
						if(GameSessionData.s_CurrentCharacters == 0)
						{
							m_StartGameGraphic.CrossFadeAlpha(1.0f, 0.5f, false);
						}
						GameSessionData.s_CurrentCharacters |= Characters.Ziggy;
						m_ZiggyImage.CrossFadeAlpha(1.0f, 0.5f, false);
					}
				}

				if(GameSessionData.s_CurrentCharacters != 0 && (Input.GetKeyDown(KeyCode.Return) || XInput.GetButtonDown(Buttons.Start, 0) || XInput.GetButtonDown(Buttons.Start, 1)))
				{
					RequestState(StartMenuState.Launching);
				}

				break;

			case StartMenuState.Launching:
				break;
		}
	}

	void OnExitState()
	{
		switch (m_State)
		{
			case StartMenuState.PressAnyKey:
				break;

			case StartMenuState.SelectCharacter:
				break;

			case StartMenuState.Launching:
				break;
		}
	}
	
	private void RequestStartGame()
	{
		FadeScreen.FadeToBlack(3.0f, StartGame);
	}

	private void StartGame()
	{
		SceneManager.LoadScene("HouseInTheWoods");
	}

	[CommandLine("start_game", "Start the game if in main menu")]
	static void StartGameFromMenu()
	{
		StartMenuHandler menuHandler = FindObjectOfType<StartMenuHandler>();
		if(menuHandler != null)
		{
			menuHandler.RequestStartGame();
		}
	}
}
