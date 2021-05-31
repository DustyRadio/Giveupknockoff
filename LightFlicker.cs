// Clase modificada de otro trabajo para obtener realismo en las luces, otro efecto cosmetico 
// Puede causar error si no la version de android es antigua. Teoria probada y cofirmada en el movil de mi abuela,
//La clase ahora se deshabilita si juegas en movil AKA solo funciona en ordenador

// Aileen: Σας αρέσουν τα καλλυντικά
// Dani: REALISMO, ademas, parte de esta la pude conseguir gratis, hay clases que implemetan estos metodos mejor pero por un precio

using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    public float amount;	
    public float speed;		
    
    Light localLight;		
    float intensity;		
	float offset;			//An offset so all flickers are different, Kinda


	void Awake()
	{
		//If this is a mobile platform, remove this script
		if(Application.isMobilePlatform)
			Destroy(this);
	}

	void Start()
    {
		//Get a reference to the Light component on the child game object
		localLight = GetComponentInChildren<Light>();

		//Record the intensity and pick a random seed number to start,
		//if I put more it the game will turn in a "rave strove light party"
        intensity = localLight.intensity;
        offset = Random.Range(0, 10000);
    }

	void Update ()
	{
		//Light Noise
		float amt = Mathf.PerlinNoise(Time.time * speed + offset, Time.time * speed + offset) * amount;
		localLight.intensity = intensity + amt;
	}
}
