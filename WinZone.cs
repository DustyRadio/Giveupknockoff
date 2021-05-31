// Clase que permite al gameManager detectar la colision con la puerta

using UnityEngine;

public class WinZone : MonoBehaviour
{
	int playerLayer;    //The layer the player game object is on


	void Start()
	{
		//Get the int of the player-layer
		playerLayer = LayerMask.NameToLayer("Player");
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		//If the collision wasn't with the player
		if (collision.gameObject.layer != playerLayer)
			return;

		//Write "Player Won" to the console and tell the Game Manager that the player won
		Debug.Log("Player Won!");
		GameManager.PlayerWon();
	}
}
