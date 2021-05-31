// Control sobre las animaciones de Robbie(Player). No es muy eficiente porque se pueden implementar en la clase de PlayerMovement
// en vez de tener su propia clase. Esta clase fue creada debidos a fallos con las animaciones, pero funcionaba mejor, en parte,
// cuando estaba en la otra clase

// Parte de la clase esta reciclada del proyecto LastSwordTree

using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
	PlayerMovement movement;	
	Rigidbody2D rigidBody;		
	PlayerInput input;			
	Animator anim;				

	int hangingParamID;			
	int groundParamID;			
	int crouchParamID;			
	int speedParamID;			
	int fallParamID;			


	void Start()
	{
		//This is much more efficient than passing strings into the animator
		hangingParamID = Animator.StringToHash("isHanging");
		groundParamID = Animator.StringToHash("isOnGround");
		crouchParamID = Animator.StringToHash("isCrouching");
		speedParamID = Animator.StringToHash("speed");
		fallParamID = Animator.StringToHash("verticalVelocity");

		//Grab a reference to this object's parent transform
		Transform parent = transform.parent;

		//Get references to the needed components
		movement	= parent.GetComponent<PlayerMovement>();
		rigidBody	= parent.GetComponent<Rigidbody2D>();
		input		= parent.GetComponent<PlayerInput>();
		anim		= GetComponent<Animator>();
		
		//If any of the needed components don't exist
		if(movement == null || rigidBody == null || input == null || anim == null)
		{
			//Fucking work already
			Debug.LogError("A needed component is missing from the player");
			Destroy(this);
		}
	}
	
	/*
		This class controls the movement(speed, audio and the crouch audio) to keep everything in sync
	*/
	void Update()
	{
		
		anim.SetBool(hangingParamID, movement.isHanging);
		anim.SetBool(groundParamID, movement.isOnGround);
		anim.SetBool(crouchParamID, movement.isCrouching);
		anim.SetFloat(fallParamID, rigidBody.velocity.y);

		
		anim.SetFloat(speedParamID, Mathf.Abs(input.horizontal));
	}
	
	public void StepAudio()
	{		
		AudioManager.PlayFootstepAudio();
	}	
	public void CrouchStepAudio()
	{
		
		AudioManager.PlayCrouchFootstepAudio();
	}
}
