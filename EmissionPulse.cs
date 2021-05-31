//FUCK THIS CLASS
// Clase para que los backgrounds tengan cosmeticos (Pretty Colors) 
//This causes a MAYOR CRASH, DO NOT USE

using UnityEngine;

public class EmissionPulse : MonoBehaviour
{
	public float maxIntensity = 15f;	//The max emissive intensity
	public float damping = 2f;			//The damping to control the pulse speed

	Material material;					//The material being controlled
	int emissionColorProperty;			//The ID of the emission property


	void Start ()
	{
		//Get a reference to the Renderer component so we can store the material of it
		Renderer renderer = GetComponent<Renderer>();
		material = renderer.material;

		//Convert the property ID string to an integer. This is much more efficient
		//than using strings to control material properties
		emissionColorProperty = Shader.PropertyToID("_EmissionColor");
	}

	void Update()
	{
		//Calculate the emission value based on Time and intensity
		float emission = Mathf.PingPong(Time.time * damping, maxIntensity);

		//Convert this to a color value
		Color finalColor = Color.white * emission;

		//Apply the color to the material
		material.SetColor(emissionColorProperty, finalColor);
	}
}
