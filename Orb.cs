// Esta clase controla los  orbes, tanto las colisiones que reporta al Manager como su registro en el juego
// siendo parte de este


using UnityEngine;

public class Orb : MonoBehaviour
{
	public GameObject explosionVFXPrefab;	//The visual effects

	int playerLayer;						//The layer the player game object is on, need this to reed when the player colides with the class orb


	void Start()
	{
		//Get "Player" Layer
		playerLayer = LayerMask.NameToLayer("Player");

		
		GameManager.RegisterOrb(this);
	}

	/*
		Ok, for starters, this is a clusterFuck.
		I'll try to clean it before implementing it, but Ai force my hand into this	
	*/
	void OnTriggerEnter2D(Collider2D collision)
	{
		//If the collided object isn't on the Player layer, exit. This is more eficient than string comparisons using Tags
		if (collision.gameObject.layer != playerLayer)
			return;

		//The orb has been touched by the Player °ω°, so BOOM at this location and rotation
		Instantiate(explosionVFXPrefab, transform.position, transform.rotation);		
		AudioManager.PlayOrbCollectionAudio();
		//Register the orb
		GameManager.PlayerGrabbedOrb(this);
		//Deactivate the orb
		gameObject.SetActive(false);
	}
}
