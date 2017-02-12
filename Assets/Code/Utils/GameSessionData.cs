using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Characters
{
	None = 0,
	Panthodeo = 1,
	Ziggy = 2,
	All = Panthodeo | Ziggy,
}

public class GameSessionData : MonoBehaviour 
{
	public static Characters s_CurrentCharacters = Characters.None;
}
