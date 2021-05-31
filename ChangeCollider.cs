//Clase que sirve para cambiar de escena
//Es muy simple

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeCollider : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    /*
        Al entrar en contacto con el collider del player cambia de escena
    */
    void OnTriggerEnter2D(Collider2D other){
        if (other.gameObject.tag == "Robbie")
        {
            SceneManager.LoadScene("Assets/Scenes/Nivel02.unity",LoadSceneMode.Single);
        }
    }

}
