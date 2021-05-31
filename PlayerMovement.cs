// Control del personaje y las fisicas, media clase ya hecha

using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public bool drawDebugRaycasts = true;	

	[Header("Movement Properties")]
	public float speed = 8f;				
	public float crouchSpeedDivisor = 3f;	
	public float coyoteDuration = .05f;		
	public float maxFallSpeed = -25f;		

	[Header("Jump Properties")]
	public float jumpForce = 6.3f;			
	public float crouchJumpBoost = 2.5f;	
	public float hangingJumpForce = 15f;	
	public float jumpHoldForce = 1.9f;		
	public float jumpHoldDuration = .1f;	

	[Header("Environment Check Properties")]
	public float footOffset = .4f;			
	public float eyeHeight = 1.5f;			
	public float reachOffset = .7f;			
	public float headClearance = .5f;		
	public float groundDistance = .2f;		
	public float grabDistance = .4f;		
	public LayerMask groundLayer;			

	[Header ("Status Flags")]
	public bool isOnGround;					
	public bool isJumping;					
	public bool isHanging;					
	public bool isCrouching;				
	public bool isHeadBlocked;

	PlayerInput input;					
	BoxCollider2D bodyCollider;			
	Rigidbody2D rigidBody;				
	
	float jumpTime;						
	float coyoteTime;						
	float playerHeight;					

	float originalXScale;					
	int direction = 1;					

	Vector2 colliderStandSize;				
	Vector2 colliderStandOffset;			
	Vector2 colliderCrouchSize;			
	Vector2 colliderCrouchOffset;		

	const float smallAmount = .05f;			


	void Start ()
	{
		input = GetComponent<PlayerInput>();
		rigidBody = GetComponent<Rigidbody2D>();
		bodyCollider = GetComponent<BoxCollider2D>();

		//save the original x scale of the player
		originalXScale = transform.localScale.x;
	
		playerHeight = bodyCollider.size.y;

		colliderStandSize = bodyCollider.size;
		colliderStandOffset = bodyCollider.offset;

		colliderCrouchSize = new Vector2(bodyCollider.size.x, bodyCollider.size.y / 2f);
		colliderCrouchOffset = new Vector2(bodyCollider.offset.x, bodyCollider.offset.y / 2f);
	}

	void FixedUpdate()
	{
		//Check the environment to determine status
		PhysicsCheck();

		//Process ground and air movements
		GroundMovement();		
		MidAirMovement();
	}

	void PhysicsCheck()
	{
		//Start by assuming the player isn't on the ground and the head isn't blocked
		isOnGround = false;
		isHeadBlocked = false;

		//Cast rays for the left and right foot
		RaycastHit2D leftCheck = Raycast(new Vector2(-footOffset, 0f), Vector2.down, groundDistance);
		RaycastHit2D rightCheck = Raycast(new Vector2(footOffset, 0f), Vector2.down, groundDistance);

		//If either ray hit the ground, the player is on the ground
		if (leftCheck || rightCheck)
			isOnGround = true;
		
		RaycastHit2D headCheck = Raycast(new Vector2(0f, bodyCollider.size.y), Vector2.up, headClearance);
	
		if (headCheck)
			isHeadBlocked = true;

		//Determine the direction of the wall grab attempt
		Vector2 grabDir = new Vector2(direction, 0f);
		
		RaycastHit2D blockedCheck = Raycast(new Vector2(footOffset * direction, playerHeight), grabDir, grabDistance);
		RaycastHit2D ledgeCheck = Raycast(new Vector2(reachOffset * direction, playerHeight), Vector2.down, grabDistance);
		RaycastHit2D wallCheck = Raycast(new Vector2(footOffset * direction, eyeHeight), grabDir, grabDistance);

		//If the player is off the ground AND is not hanging AND is falling AND
		//found a ledge AND found a wall AND the grab is NOT blocked
		if (!isOnGround && !isHanging && rigidBody.velocity.y < 0f && 
			ledgeCheck && wallCheck && !blockedCheck)
		{ 
			//we have a ledge grab. Record the current position
			Vector3 pos = transform.position;
			//move the distance to the wall (minus a small amount)
			pos.x += (wallCheck.distance - smallAmount) * direction;
			//move the player down to grab onto the ledge
			pos.y -= ledgeCheck.distance;
			//apply this position to the platform
			transform.position = pos;
			//set the rigidbody to static
			rigidBody.bodyType = RigidbodyType2D.Static;
			//finally, set isHanging to true
			isHanging = true;
		}
	}

	void GroundMovement()
	{
		if (isHanging)
			return;

		//Handle crouching input. If holding the crouch button but not crouching, crouch
		if (input.crouchHeld && !isCrouching && isOnGround)
			Crouch();
		//Otherwise, if not holding crouch but currently crouching, stand up
		else if (!input.crouchHeld && isCrouching)
			StandUp();
		//Otherwise, if crouching and no longer on the ground, stand up
		else if (!isOnGround && isCrouching)
			StandUp();

		//Calculate the desired velocity based on inputs
		float xVelocity = speed * input.horizontal;

		//If the sign of the velocity and direction don't match, flip the character
		if (xVelocity * direction < 0f)
			FlipCharacterDirection();

		//If the player is crouching, reduce the velocity
		if (isCrouching)
			xVelocity /= crouchSpeedDivisor;

		//Apply the desired velocity 
		rigidBody.velocity = new Vector2(xVelocity, rigidBody.velocity.y);

		//If the player is on the ground, extend the coyote time window,
		//this one is messy
		if (isOnGround)
			coyoteTime = Time.time + coyoteDuration;
	}

	void MidAirMovement()
	{
		//If the player is currently hanging
		if (isHanging)
		{
			//If crouch is pressed
			if (input.crouchPressed)
			{
				isHanging = false;
				rigidBody.bodyType = RigidbodyType2D.Dynamic;
				return;
			}

			if (input.jumpPressed)
			{
				isHanging = false;
				rigidBody.bodyType = RigidbodyType2D.Dynamic;
				rigidBody.AddForce(new Vector2(0f, hangingJumpForce), ForceMode2D.Impulse);
				return;
			}
		}

		//If the jump key is pressed AND the player isn't already jumping AND EITHER
		//the player is on the ground or within the coyote time window
		if (input.jumpPressed && !isJumping && (isOnGround || coyoteTime > Time.time))
		{
			//check to see if crouching AND not blocked. If so
			if (isCrouching && !isHeadBlocked)
			{
				StandUp();
				rigidBody.AddForce(new Vector2(0f, crouchJumpBoost), ForceMode2D.Impulse);
			}

			isOnGround = false;
			isJumping = true;

			jumpTime = Time.time + jumpHoldDuration;

			rigidBody.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);

			AudioManager.PlayJumpAudio();
		}
		//Otherwise, if currently within the jump time window
		else if (isJumping)
		{
			if (input.jumpHeld)
				rigidBody.AddForce(new Vector2(0f, jumpHoldForce), ForceMode2D.Impulse);

			if (jumpTime <= Time.time)
				isJumping = false;
		}

		//If player is falling to fast, go fast down
		if (rigidBody.velocity.y < maxFallSpeed)
			rigidBody.velocity = new Vector2(rigidBody.velocity.x, maxFallSpeed);
	}

	void FlipCharacterDirection()
	{
		direction *= -1;

		Vector3 scale = transform.localScale;

		scale.x = originalXScale * direction;

		transform.localScale = scale;
	}

	void Crouch()
	{
		isCrouching = true;

		bodyCollider.size = colliderCrouchSize;
		bodyCollider.offset = colliderCrouchOffset;
	}

	void StandUp()
	{
		if (isHeadBlocked)
			return;

		isCrouching = false;
	
		bodyCollider.size = colliderStandSize;
		bodyCollider.offset = colliderStandOffset;
	}


	
	RaycastHit2D Raycast(Vector2 offset, Vector2 rayDirection, float length)
	{		
		return Raycast(offset, rayDirection, length, groundLayer);
	}

	RaycastHit2D Raycast(Vector2 offset, Vector2 rayDirection, float length, LayerMask mask)
	{
		Vector2 pos = transform.position;

		RaycastHit2D hit = Physics2D.Raycast(pos + offset, rayDirection, length, mask);

		if (drawDebugRaycasts)
		{
			Color color = hit ? Color.red : Color.green;
			Debug.DrawRay(pos + offset, rayDirection * length, color);
		}

		return hit;
	}
}
