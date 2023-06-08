using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
/// <summary>
///Este script recopila las funciones que se usan para salir del juego cuando el jugador esta cerca de la cama cuya collision box tiene un trigger.
///</summary>
/// <param name="textoSalir">
///Texto que indica el boton a pulsar para salir, solo se activara cuando el jugador este dentro del trigger.
/// </param>
/// <param name="estoyDentro">
///Variable booleana que es true cuando el jugador esta dentro del trigger y falsa cuando esta fuera de este
/// </param>
public class triggerSalir : MonoBehaviour
{
    public GameObject textoSalir;
    private bool estoyDentro = false;
    /// <summary>
    ///Funcion que se inicia nada mas el script esta activo. Desactivara el objeto textoSalir para que no pueda ser visto por el usuario. El spawn inicial esta fuera del
    ///trigger, entonces no deberia mostrarse nada mas empezar.
    /// </summary>
    void Start()
    {
        textoSalir.SetActive(false);
    }

    /// <summary>
    ///Funcion que se ejecuta una vez por cada frame. Comprobaremos si la variable interna estoyDentro es verdadera, en ese caso, si el usuario pulsa la 
    ///tecla E, se ejecutara la funcion salir()
    /// </summary>
    void Update()
    {
        if(estoyDentro == true){
            if((Input.GetKeyDown(KeyCode.E))){
                salir();
            }
        }
    }
    /// <summary>
    ///Funcion que se ejecutara en el momento que el objeto entre en contacto con otro objeto. En el caso de que 
    ///el objeto tenga un tag Player, se activara el textoSalir y la variable estoyDentro pasara a ser true
    /// </summary>
    /// <param name="other">Objeto con el que se entrara en trigger </param>
    void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")){
            textoSalir.SetActive(true);
            estoyDentro = true;
        }
    }
     /// <summary>
    ///Funcion que se ejecutara en el momento que el objeto salga del contacto con otro objeto. En el caso de que 
    ///el objeto tenga un tag Player, se desactivara el textoSalir y la variable estoyDentro pasara a ser false
    /// </summary>
    /// <param name="other">Objeto con el que se saldra del trigger </param>
    void OnTriggerExit(Collider other) {
        if(other.CompareTag("Player")){
            textoSalir.SetActive(false);
            estoyDentro = false;
        }
    }
    /// <summary>
    ///Funcion que cerrara la aplicacion
    /// </summary>
    void salir(){
        Application.Quit();
    }
}
