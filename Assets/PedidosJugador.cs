using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PedidosJugador : MonoBehaviour
{
    public GameObject llamada;
    public GameObject fondoPantalla;
    public GameObject telefono;
    public bool tengoPedido;
    public bool tengoPizza;
    private float tiempoSiguienteLlamada;
    public Material cogida;
    public Material espera;
    public TextMeshProUGUI textoCogerLlamada;
    public TextMeshProUGUI textoColgarLlamada;
    public TextMeshProUGUI textoDinero;
    public TextMeshProUGUI textoDineroTotal;
    private bool primeraLlamada;
    private float inicioTiempoLlamada;
    private float finTiempoLlamada;
    private bool puedoPedir;


    //Dialogo
    public GameObject dialogoContenedor;
    public TextMeshProUGUI dialogoTexto;
    public TextMeshProUGUI ingredientesTexto;
    public GameObject ingredientesContenedor;
    //Es nuevo
    public string[] conjuntoDialogo;

    public string[] lineas;
    public string[] ingredientes;

    private int numDialogo;
    public float velocidad;
    private int puntero;
    private bool enDialogo;

    //Temporizador
    private float tiempoInicioCronometro;
    private bool cronometroActivo = false;

    private float dineroGanadoInicial = 20.0f;
    private float decrementoDinero = 0.10f;
    private float tiempoDecremento = 2.0f;



    // Start is called before the first frame update
    void Start()
    {
        inicioTiempoLlamada = 15f;
        finTiempoLlamada = 30f;
        primeraLlamada = false;
        textoCogerLlamada.gameObject.SetActive(false);
        textoColgarLlamada.gameObject.SetActive(false);
        fondoPantalla.GetComponent<Renderer>().material = espera;
        llamada.SetActive(false);
        tengoPedido = false;
        tengoPizza = false;
        tiempoSiguienteLlamada  = Random.Range(inicioTiempoLlamada, finTiempoLlamada);
        telefono.SetActive(false);
        dialogoContenedor.SetActive(false);
        enDialogo = false;
        ingredientesContenedor.SetActive(false);
        ingredientesTexto.text="";
        crearDialogos();
        puedoPedir=false;
        textoDineroTotal.text = "0.00";
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= tiempoSiguienteLlamada && !tengoPedido && !tengoPizza) {
            llamada.SetActive(true);
            telefono.SetActive(true);
            if(!primeraLlamada){
                textoCogerLlamada.gameObject.SetActive(true);
                textoColgarLlamada.gameObject.SetActive(true);
                primeraLlamada = true;
            }
        }

        if (llamada.activeSelf) {
            if (Input.GetKey(KeyCode.Q)) {
                llamada.SetActive(false);
                tiempoSiguienteLlamada = Time.time + Random.Range(inicioTiempoLlamada, finTiempoLlamada);
                textoCogerLlamada.gameObject.SetActive(false);
                textoColgarLlamada.gameObject.SetActive(false);
                fondoPantalla.GetComponent<Renderer>().material = espera;
                telefono.SetActive(false);
        } else if (Input.GetKey(KeyCode.E)) {
            numDialogo = Random.Range(0, conjuntoDialogo.Length);
            //Es nuevo
            // Debug.Log(conjuntoDialogo[numDialogo]);
            lineas = conjuntoDialogo[numDialogo].Split(';');
            ingredientes = lineas[lineas.Length-1].Split('|');

            fondoPantalla.GetComponent<Renderer>().material = cogida;
            tengoPedido = true;
            llamada.SetActive(false);
            tiempoSiguienteLlamada = 0;
            textoCogerLlamada.gameObject.SetActive(false);
            textoColgarLlamada.gameObject.SetActive(false);
            dialogoTexto.text = "";
            dialogoContenedor.SetActive(true);
            empezarDialogo();
            enDialogo = true;
            }
        }

        if(enDialogo){
            if(Input.GetMouseButtonDown(0)){
                if(dialogoTexto.text == lineas[puntero]){
                  siguienteLinea();
                }else{
                    dialogoTexto.text = lineas[puntero];
                    StopAllCoroutines();
                }
            }
        }

        if(puedoPedir){
            if (Input.GetKey(KeyCode.E)) {
                    tengoPizza=true;
                    puedoPedir = false;
                    empezarCronometro();
                }
        }

        if (cronometroActivo)
        {
            float tiempoTranscurrido = Time.time - tiempoInicioCronometro;
            float dineroGanado = dineroGanadoInicial - Mathf.Floor(tiempoTranscurrido / tiempoDecremento) * decrementoDinero;
            dineroGanado = Mathf.Max(dineroGanado, 0.0f); // Asegurar que el dinero no sea negativo
            textoDinero.text = dineroGanado.ToString("F2");
        }


        // Debug.Log(this.tengoPizza);
    }

    void empezarDialogo(){
        Debug.Log("Funcion empezarDialogo");
        puntero = 0;
        StartCoroutine(escribirLinea());
    }

    IEnumerator escribirLinea(){
        foreach(char c in lineas[puntero].ToCharArray()){
            dialogoTexto.text += c;
            yield return new WaitForSeconds(velocidad);
        }
    }

    void siguienteLinea(){
        if(puntero < lineas.Length - 1){
            puntero++;
            dialogoTexto.text = "";
            StartCoroutine(escribirLinea());
        }else
        {
            dialogoContenedor.SetActive(false);
            telefono.SetActive(false);
            enDialogo=false;
            foreach(string ingrediente in ingredientes){
                ingredientesTexto.text+=ingrediente + "\n";
            }
            ingredientesContenedor.SetActive(true);
        }
    }

    void crearDialogos()
    {
        // Crear los 10 diálogos
        for (int i = 0; i < conjuntoDialogo.Length; i++)
        {
            // Generar ingredientes aleatorios
            string ingredientes = generarIngredientesAleatorios();

            // Generar comentario aleatorio
            string comentario = generarComentarioAleatorio();

            string[] clientes = { "Miguel", "Carlos", "Manoli", "Rodolfo", "Rosember", "Alejandra", "Jorge", "Rodrigo" };

            int indiceCliente = Random.Range(0,clientes.Length);


            // Crear el diálogo
            string dialogo = $"Hola soy {clientes[indiceCliente]} {comentario};Quiero una pizza con ;{ingredientes}";

            // Añadir el diálogo a la lista
            conjuntoDialogo[i] = (dialogo);
        }
    }

    string generarIngredientesAleatorios()
    {
        string[] ingredientes = { "Queso", "Tomate", "Pepperoni", "Cebolla", "Pimiento", "Jamon", "Aceitunas", "Champinones" };
        string ingredientesDialogo = "";

        // Generar hasta 5 ingredientes aleatorios
        int cantidadIngredientes = Random.Range(1, 6);
        for (int i = 0; i < cantidadIngredientes; i++)
        {
            // Seleccionar un ingrediente aleatorio
            string ingrediente = ingredientes[Random.Range(0, ingredientes.Length)];

            // Añadir el ingrediente al diálogo
            ingredientesDialogo += ingrediente;

            // Añadir el separador '|' entre ingredientes
            if (i < cantidadIngredientes - 1)
            {
                ingredientesDialogo += "|";
            }
        }

        return ingredientesDialogo;
    }

    string generarComentarioAleatorio()
    {
        string[] comentarios = {
            "Estoy deseando probarla. He oido que hacen las mejores pizzas de la ciudad y estoy ansioso por comprobarlo.;" +
            "Espero que la masa este perfectamente horneada y crujiente. Me gusta cuando tiene un toque dorado y un sabor delicioso.;" +
            "Tambien, si pueden agregar un poco mas de salsa de tomate, seria genial. Me encanta el sabor intenso del tomate.;" +
            "Por ultimo, por favor, eviten poner demasiado queso. Me gusta en su justa medida para que no domine los otros sabores.;" +
            "Espero con ansias esta pizza y estoy seguro de que sera deliciosa. Gracias de antemano por su atencion.",
            "Sin cebolla por favor. No me gusta el sabor ni el olor que deja la cebolla en la pizza.;" +
            "Me gustaria una masa bien delgada y crujiente, para que cada bocado sea un placer.;" +
            "Ademas, si pueden añadir un poco de albahaca fresca por encima, le daria un toque de frescura y aroma.;" +
            "Me encanta cuando la pizza tiene un equilibrio perfecto entre los ingredientes. Nada debe dominar sobre el resto.;" +
            "Espero que puedan cumplir con mis preferencias y disfrutare cada bocado de esta pizza. ¡Gracias!",
            "¡Que sea bien crujiente! Me gusta cuando la masa tiene una textura crujiente por fuera y suave por dentro.;" +
            "Agreguen un poco mas de queso, por favor. Me encanta cuando la pizza esta bien cubierta y el queso se derrite.;" +
            "Tambien, me gustaria que incluyan champiñones. Su sabor terroso combina perfectamente con los demas ingredientes.;" +
            "Si pueden aniadir un poco de oregano, le dara un toque aromatico que realzara todos los sabores.;" +
            "Estoy emocionado por probar esta pizza y estoy seguro de que superara mis expectativas. ¡Gracias!",
            "Extra queso, por favor. Soy un amante del queso y no puedo tener suficiente en mi pizza.;" +
            "Tambien, agreguen un poco de ajo picado. El ajo le da un sabor intenso y delicioso a la pizza.;" +
            "Me gustaria una variedad de ingredientes. Pueden sorprenderme con una combinacion unica y sabrosa.;" +
            "Si tienen alguna salsa especial, estaria encantado de probarla. Me gusta experimentar nuevos sabores.;" +
            "Estoy ansioso por disfrutar de esta pizza y estoy seguro de que sera una experiencia increible. ¡Gracias!",
            "No me gustan las aceitunas. Por favor, eviten poner aceitunas en mi pizza, no puedo soportar su sabor.;" +
        "Me gustaria una masa bien esponjosa y suave. Disfruto de la textura ligera y aireada en cada bocado.;"
        };

        // Seleccionar un comentario aleatorio
        string comentario = comentarios[Random.Range(0, comentarios.Length)];

        return comentario;
    }

    void empezarCronometro()
    {
        cronometroActivo = true;
        tiempoInicioCronometro = Time.time;
        textoDinero.text = dineroGanadoInicial.ToString();
    }

    void terminarCronometro()
    {
        cronometroActivo = false;
        float tiempoTranscurrido = Time.time - tiempoInicioCronometro;
        float dineroGanado = dineroGanadoInicial - (tiempoTranscurrido / tiempoDecremento) * decrementoDinero;
        dineroGanado = Mathf.Max(dineroGanado, 0.0f); // Asegurar que el dinero no sea negativo
        textoDinero.text = dineroGanado.ToString("F2");
        Debug.Log("Tiempo total del cronómetro: " + tiempoTranscurrido.ToString("F2"));
        Debug.Log("Dinero conseguido: " + dineroGanado.ToString("F2"));
        float dineroActual = float.Parse(textoDineroTotal.text);
        dineroActual += dineroGanado;
        textoDineroTotal.text = dineroActual.ToString("F2");
    }

    void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag=="Pizzeria"){
            if(tengoPedido&&!tengoPizza){
                // tengoPizza = true;
                // Debug.Log("Acabo de conseguir una Pizza");
                puedoPedir = true;
            }else if(tengoPedido&&tengoPizza){
                Debug.Log("Ya tengo una Pizza, tengo que entregarla");
            }else if(!tengoPedido){
                Debug.Log("Aun no tengo Pedido, deberia de esperar a que me llamen");
            }
        }else if(other.gameObject.tag=="Objetivo"){
            if(tengoPedido&&tengoPizza){
                tengoPedido=false;
                tengoPizza=false;
                Debug.Log("Pizza Entregada");
                other.gameObject.SetActive(false);
                ingredientesTexto.text="";
                ingredientesContenedor.SetActive(false);
                fondoPantalla.GetComponent<Renderer>().material = espera;
                tiempoSiguienteLlamada = Time.time + Random.Range(inicioTiempoLlamada, finTiempoLlamada);
                terminarCronometro();
            }
        }
    }

    void OnTriggerExit(Collider other){
        if(other.gameObject.tag=="Pizzeria"){
            puedoPedir = false;
        }
    }
    

}
