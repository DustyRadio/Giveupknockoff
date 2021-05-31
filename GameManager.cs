// Esta clase controla el Juego, palabra clave "CONTROLA", Don't be stupid and delete this again, 'cause I'll kill you nex time you do it.
// Mantiene los contadores de muetes, orbes por nivel, tiempo de juego y la UI
// Nota de Dani para Aileen:No toques el meta de nuevo o no podremos guardar ninguno de los progresos anteriores

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
	/**
		Clase singletonpara que el resto de clases accedan a esta por metodos publicos estaticos
	*/
	static GameManager current;

	public float deathSequenceDuration = 1.5f;	//Tiempo de espera entre muertes

	List<Orb> orbs;								//Contador de Orbes
	Door lockedDoor;							//Objeto Puerta, Uso excesivo para el player
	SceneFader sceneFader;						//Transicion entre niveles 

	int numberOfDeaths;							//How many times you sucked, Ai PD: Μην προσβάλλετε ξανά τους εξεταστές
	float totalGameTime;						
	bool isGameOver;							


	void Awake()
	{
		//If a Game Manager exists and this isn't it...
		if (current != null && current != this)
		{
			//...destroy this and exit. There can only be one Game Manager, Kinda like this phrase, I'ma use more
			Destroy(gameObject);
			return;
		}

		
		current = this;

		//New instance for orbs
		orbs = new List<Orb>();

		//This is the easiest way to keeps the orbs when switching maps
		//todo this is dumb
		DontDestroyOnLoad(gameObject);
	}

	/**
		Clase para ver si el juego ha terminado
	*/
	void Update()
	{
		
		if (isGameOver)
			return;

		
		totalGameTime += Time.deltaTime;
		UIManager.UpdateTimeUI(totalGameTime);
	}

	/*
		Gameover, either for good or bad, return a null statement Manager
		NOTE: This isn't particulary efficient, too bad
	*/
	public static bool IsGameOver()
	{
		
		if (current == null)
			return false;

		
		return current.isGameOver;
	}
	
	/*
		This is a stupid fix but I don't have time to do a cleaner implementation
	*/
	public static void RegisterSceneFader(SceneFader fader)
	{
		
		if (current == null)
			return;

		//Record the scene fader reference
		current.sceneFader = fader;
	}

	/*
		this is a bad way to implemet the door styles sprites fonts, but it will work for now
	*/
	public static void RegisterDoor(Door door)
	{
		
		if (current == null)
			return;

		
		current.lockedDoor = door;
	}

	/*
		διαβάστε τον τίτλο
	*/
	public static void RegisterOrb(Orb orb)
	{
		
		if (current == null)
			return;

		//If the orb collection doesn't already contain this orb, add it
		if (!current.orbs.Contains(orb))
			current.orbs.Add(orb);

		//Tell the UIManager to update the orb text
		UIManager.UpdateOrbUI(current.orbs.Count);
	}

	public static void PlayerGrabbedOrb(Orb orb)
	{
		
		if (current == null)
			return;

		//If the orbs collection doesn't have this orb, exit
		if (!current.orbs.Contains(orb))
			return;

		//Remove the collected orb
		current.orbs.Remove(orb);

		//If there are no more orbs, tell the door to open
		if (current.orbs.Count == 0)
			current.lockedDoor.Open();

		//Tell the UIManager to update the orb text
		UIManager.UpdateOrbUI(current.orbs.Count);
	}

	public static void PlayerDied()
	{		
		if (current == null)
			return;

		//Increment the number of player deaths and tell the UIManager
		current.numberOfDeaths++;
		UIManager.UpdateDeathUI(current.numberOfDeaths);

		//If we have a scene fader, tell it to fade the scene out, it may not do it fast (sjb)
		if(current.sceneFader != null)
			current.sceneFader.FadeSceneOut();

		//RestartScene() and wait
		current.Invoke("RestartScene", current.deathSequenceDuration);
	}

	public static void PlayerWon()
	{		
		if (current == null)
			return;

		//The game is now over
		current.isGameOver = true;

		//Tell UI Manager to show the game over text and tell the Audio Manager to play game over audio
		UIManager.DisplayGameOverText();
		AudioManager.PlayWonAudio();
	}

	void RestartScene()
	{
		//Clear the current list of orbs
		orbs.Clear();

		//Play the scene restart audio
		AudioManager.PlaySceneRestartAudio();

		//Reload the current scene
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); 
	}
}
