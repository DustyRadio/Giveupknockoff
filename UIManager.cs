// Clase que controla las UI de muertes, tiempo y orbes

using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
	static UIManager current;

	public TextMeshProUGUI orbText;			//Text showing number of orbs
	public TextMeshProUGUI timeText;		//Text showing amount of time
	public TextMeshProUGUI deathText;		//Text showing number or deaths
	public TextMeshProUGUI gameOverText;	//Text showing the Game Over message


	void Awake()
	{
		//If an UIManager exists and it is not this
		if (current != null && current != this)
		{
			//destroy this and exit. There can be only one UIManager
			Destroy(gameObject);
			return;
		}

		//This is the current UIManager and it should persist between scene loads
		current = this;
		DontDestroyOnLoad(gameObject);
	}

	public static void UpdateOrbUI(int orbCount)
	{
		//If there is no current UIManager, exit
		if (current == null)
			return;

		//Update the text orb element
		current.orbText.text = orbCount.ToString();
	}

	public static void UpdateTimeUI(float time)
	{
		//If there is no current UIManager, exit
		if (current == null)
			return;

		//Took the time and split it into minutes and seconds
		int minutes = (int)(time / 60);
		float seconds = time % 60f;

		//Create the string in the appropriate format for the time
		current.timeText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
	}

	public static void UpdateDeathUI(int deathCount)
	{		
		if (current == null)
			return;
		
		current.deathText.text = deathCount.ToString();
	}

	public static void DisplayGameOverText()
	{		
		if (current == null)
			return;

		//Show the game over text
		current.gameOverText.enabled = true;
	}
}
