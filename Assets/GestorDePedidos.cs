using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GestorDePedidos : MonoBehaviour
{
    public List<GameObject> objetivos;
    public bool puedePedir;
    public TextMeshProUGUI textoPedirPizza;
    public PedidosJugador jugador;
    

    // Start is called before the first frame update
    void Start()
    {
        inicializarObjetivos();
        puedePedir = false;
        textoPedirPizza.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(puedePedir){
            if (Input.GetKey(KeyCode.E)) {
                    Debug.Log("TomaPizza");
                    int indexObjetivo = Random.Range(0, objetivos.Count);
                    GameObject objetivoSeleccionado = objetivos[indexObjetivo];
                    objetivoSeleccionado.SetActive(true);
                    textoPedirPizza.gameObject.SetActive(false);
                    puedePedir = false;
                }
        }
        // if (Input.GetKey(KeyCode.E)&&puedePedir) {
        //     Debug.Log("TomaPizza");
        // }
    }

    // Funcion que busca en el mapa todos los GameObject con tag "Objetivo" y los mete en la Lista objetivos
    void inicializarObjetivos(){
        objetivos = new List<GameObject>(GameObject.FindGameObjectsWithTag("Objetivo"));
        foreach (GameObject objetivo in objetivos){
        objetivo.SetActive(false);
        }
    }
    void OnTriggerEnter(Collider other) {
        // if(other.gameObject.tag=="Player"){
        //     PedidosJugador pedidosJugador = other.GetComponent<PedidosJugador>();
            
        //     if(pedidosJugador.tengoPedido&&!pedidosJugador.tengoPizza){
        //         int indexObjetivo = Random.Range(0, objetivos.Count);
        //         GameObject objetivoSeleccionado = objetivos[indexObjetivo];
        //         objetivoSeleccionado.SetActive(true);

        //     }
        
        if(other.gameObject.tag=="Player"){
            PedidosJugador pedidosJugador = other.GetComponent<PedidosJugador>();

            if(pedidosJugador.tengoPedido&&!pedidosJugador.tengoPizza){
                puedePedir  = true;
                textoPedirPizza.gameObject.SetActive(true);
                // if (Input.GetKey(KeyCode.E)) {
                //     Debug.Log("TomaPizza");
                //     int indexObjetivo = Random.Range(0, objetivos.Count);
                //     GameObject objetivoSeleccionado = objetivos[indexObjetivo];
                //     objetivoSeleccionado.SetActive(true);
                //     textoPedirPizza.gameObject.SetActive(false);
                // }
            }
            
        }
    }

    void OnTriggerExit(Collider other) {
        if(other.gameObject.tag=="Player"){
            // puedePedir = false;
            textoPedirPizza.gameObject.SetActive(false);
        }
    }
}
