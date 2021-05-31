// Esta clase es para el movimiento y lo implementa tanto en movil como ordenador.


using UnityEngine;

//ESTO MUY IMPORTANTE. RETRASO EN IMPUS SI NO ESTA BIEN IMPLEMETADO
[DefaultExecutionOrder(-100)]
public class PlayerInput : MonoBehaviour
{
	public bool testTouchControlsInEditor = false;	
	public float verticalDPadThreshold = .5f;		
	public Thumbstick thumbstick;					
	public TouchButton jumpButton;					

	[HideInInspector] public float horizontal;		
	[HideInInspector] public bool jumpHeld;			
	[HideInInspector] public bool jumpPressed;		
	[HideInInspector] public bool crouchHeld;		
	[HideInInspector] public bool crouchPressed;	
	
	bool dPadCrouchPrev;							
	bool readyToClear;								


	void Update()
	{
		//Clear out existing input values
		ClearInput();

		//If the Game Manager says the game is over, IT'S OVER
		if (GameManager.IsGameOver())
			return;

		//Kinda re used to use imputs from standalone (mouse, keyboard, etc)
		ProcessInputs();
		//Process mobile (touch) inputs
		ProcessTouchInputs();

		//Clamp the horizontal input to be between -1 and 1
		horizontal = Mathf.Clamp(horizontal, -1f, 1f);
	}

	void FixedUpdate()
	{
		readyToClear = true;
	}

	void ClearInput()
	{
		//If we're not ready to clear input, exit and do nothing
		if (!readyToClear)
			return;

		//Reset all inputs
		horizontal		= 0f;
		jumpPressed		= false;
		jumpHeld		= false;
		crouchPressed	= false;
		crouchHeld		= false;

		readyToClear	= false;
	}

	void ProcessInputs()
	{
		//Accumulate horizontal axis input
		horizontal		+= Input.GetAxis("Horizontal");

		//Accumulate button inputs, don't get stuck
		jumpPressed		= jumpPressed || Input.GetButtonDown("Jump");
		jumpHeld		= jumpHeld || Input.GetButton("Jump");

		crouchPressed	= crouchPressed || Input.GetButtonDown("Crouch");
		crouchHeld		= crouchHeld || Input.GetButton("Crouch");
	}

	/**
		Stackoverflow life savior
	*/
	void ProcessTouchInputs()
	{
		//If this isn't a mobile platform AND we aren't testing in editor, Gito
		if (!Application.isMobilePlatform && !testTouchControlsInEditor)
			return;

		//Record inputs from screen thumbstick
		Vector2 thumbstickInput = thumbstick.GetDirection();

		//Accumulate horizontal input
		horizontal		+= thumbstickInput.x;

		//Accumulate jump button input
		jumpPressed		= jumpPressed || jumpButton.GetButtonDown();
		jumpHeld		= jumpHeld || jumpButton.GetButton();

		//Using thumbstick, accumulate crouch input
		bool dPadCrouch = thumbstickInput.y <= -verticalDPadThreshold;
		crouchPressed	= crouchPressed || (dPadCrouch && !dPadCrouchPrev);
		crouchHeld		= crouchHeld || dPadCrouch;
		
		//if button is pressed for first time or held
		dPadCrouchPrev	= dPadCrouch;
	}
}
