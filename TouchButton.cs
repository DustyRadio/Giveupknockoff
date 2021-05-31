// Clase que controla el boton saltar en el juego 

using UnityEngine;
using UnityEngine.EventSystems;

public class TouchButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
	int pointerID;
	bool buttonHeld;
	bool buttonPressed;
	
	void Awake ()
	{
		pointerID = -999;
	}


	/*
		Register if the button is first touch and if it's held
	*/
	public void OnPointerDown(PointerEventData data)
	{
		if (pointerID != -999)
			return;
		
		pointerID = data.pointerId;
		
		buttonHeld = true;
		buttonPressed = true;
	}

	/*
		Release the screen-button
	*/
	public void OnPointerUp (PointerEventData data)
	{
		if (data.pointerId != pointerID)
			return;
		
		pointerID = -999;
		buttonHeld = false;
		buttonPressed = false;
	}

	/*
		Function that whatches if the button is press and the next one if it's held to return a the parameter to the playermovement.class to jump
	*/
	public bool GetButtonDown () 
	{
		if (buttonPressed)
		{
			buttonPressed = false;
			return true;
		}

		return false;
	}

	
	public bool GetButton()
	{
		return buttonHeld;
	}
}