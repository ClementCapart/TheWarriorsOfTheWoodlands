using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsUtils : MonoBehaviour 
{
	public void FadeFromBlack()
	{
		FadeScreen.FadeFromBlack(0.5f);
	}

	public void GotoMainMenu()
	{
		FadeScreen.FadeToBlack(0.5f, LoadMainMenu);
	}

	void LoadMainMenu()
	{
		SceneManager.LoadScene("StartMenu");
	}
}
