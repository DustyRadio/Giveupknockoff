// Control the door to win... Forehead
// Tiene que estar registrado en el gameManager

using UnityEngine;

public class Door : MonoBehaviour
{
	Animator anim;			
	int openParameterID;	


	void Start()
	{
		//Get a reference to the Animator component
		anim = GetComponent<Animator>();

		//Get the integer hash of the "Open" parameter. This is much more efficient than passing strings into the animator
		openParameterID = Animator.StringToHash("Open");

		//Register this door with the Game Manager
		GameManager.RegisterDoor(this);
	}

	public void Open()
	{
		//Play the animation that opens the door, and win
		anim.SetTrigger(openParameterID);
		AudioManager.PlayDoorOpenAudio();
	}
}
