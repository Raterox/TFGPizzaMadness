using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
///Clase que usaremos como recurso para hacer los Cambios de Pantallas que iremos usando en funciones OnClick()
/// </summary>
public class CambioPantalla : MonoBehaviour
{
    /// <summary>
    ///Funcion que se inicia nada mas el script esta activo. Desbloqueara el movimiento del Cursor (Mouse) y lo hara visible
    /// </summary>
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    /// <summary>
    ///Funcion que se ejecuta una vez por cada frame. No se realiza ninguna funcion
    /// </summary>    
    void Update()
    {
        
    }
    /// <summary>
    ///Funcion que cambiara de pantalla a la pantalla con nombre CiudadConLago.
    /// </summary>
    public void cambiarPantallaLago(){
        SceneManager.LoadScene("CiudadConLago");
    }
    /// <summary>
    ///Funcion que cambiara de pantalla a la pantalla con nombre CiudadDesierto.
    /// </summary>
    public void cambiarPantallaDesierto(){
        SceneManager.LoadScene("CiudadDesierto");
    }
    /// <summary>
    ///Funcion que cambiara de pantalla a la pantalla con nombre MenuInicial.
    /// </summary>
    public void cambiarPantallaMenuInicial(){
        SceneManager.LoadScene("MenuInicial");
    }
    /// <summary>
    ///Funcion que cambiara de pantalla a la pantalla con nombre ComoJugar.
    /// </summary>
    public void cambiarPantallaComoJugar(){
        SceneManager.LoadScene("ComoJugar");
    }
    /// <summary>
    ///Funcion que cambiara de pantalla a la pantalla con nombre Menu.
    /// </summary>
    public void cambiarPantallaMenu(){
        SceneManager.LoadScene("Menu");
    }
}
