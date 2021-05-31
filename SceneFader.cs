// Clase que maneja las animaciones de la escena, las fader (Las de muerte, si no rompiera el juego en movil)

using UnityEngine;

public class SceneFader : MonoBehaviour
{
	Animator anim;		//Reference to the Animator component
	int fadeParamID;    //The id parameter that fades the image

	void Start()
	{
		//Get reference Animator
		anim = GetComponent<Animator>();

		//Get the integer of the "Fade"
		fadeParamID = Animator.StringToHash("Fade");

		//Register this Scene Fader with the Game Manager
		GameManager.RegisterSceneFader(this);
	}


	/**ç
		Play the animation of Fade
	*/
	public void FadeSceneOut()
	{		
		anim.SetTrigger(fadeParamID);
	}
}
