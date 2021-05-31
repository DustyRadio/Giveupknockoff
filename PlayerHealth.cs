// Esta clase comucica las colisiones con las trampas y se las dice al Manager para que te mate
// Tuve que resistir la tentacion de llamar a esta clase youdeadboi.cs

using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
	public GameObject deathVFXPrefab;	

	bool isAlive = true;				
	int trapsLayer;						

	/*
		Guarda el numero de capa en el que estan las trampas, porque siempre pongo uno diferente
	*/
	void Start()
	{
		
		trapsLayer = LayerMask.NameToLayer("Traps");
	}

	/*
		Si el colaider no es una trampa o si el personaje no esta vivo , te mantiene con vida
		Mas facil que estar revisando todo el rato por si estas vivo
	*/
	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.layer != trapsLayer || !isAlive)
			return;

		//You hit a trap. You are dead
		isAlive = false;

		//Explosion
		Instantiate(deathVFXPrefab, transform.position, transform.rotation);
	
		gameObject.SetActive(false);

		//Tell the Game Manager that the player died and play the death audio
		GameManager.PlayerDied();




		AudioManager.PlayDeathAudio();
	}
}
