using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Mono.Data.Sqlite; 
using System.Data;
using UnityEngine.SceneManagement;

/// <summary>
///Clase que recopila todos los movimientos y comportamientos del coche y de las puntuaciones. Es la 
///clase base del juego.
///</summary>
/// <param name="PizzaConseguida">
///Canvas que se activa y desactiva cuando recogemos una pizza
///</param>
/// <param name="PizzaEntregada">
///Canvas que se activa y desactiva cuando entregamos una pizza
///</param>
/// <param name="contadorPizzas">
///Texto que muestra el numero de pizzas entregadas
///</param>
/// <param name="Inercia">
///Vector 3 que indica la fuerza remanente tras mover el vehiculo.
///</param>
/// <param name="Velocidad">
///Variable que indica la velocidad del vehiculo.
///</param>
/// <param name="VelocidadCopia">
///Variable copia para poder restaurar la velocidad.
///</param>
/// <param name="VelocidadMax">
///Variable que indica la maxima velocidad a la que puede ir el coche.
///</param>
/// <param name="Rozamiento">
///Valor que indica el rozamiento del coche con el suelo, cuanto mayor sea, mas friccion hara con el coche, y mas le costara moverse.
///</param>
/// <param name="AnguloGiro">
///Variable que indica el angulo de giro maximo del coche.
///</param>
/// <param name="AnguloGiroCopia">
///Variable copia para poder restaurar el angulo de giro.
///</param>
/// <param name="AnguloGiroDrift">
///Variable que indica cuando el coche este drifteando, este sera el valor que tendra el angulo de giro.
///</param>
/// <param name="Traccion">
///Valor que indica la traccion que tiene el coche con el suelo. Cuanto mayor sea, menor sera su velocidad y viceversa
///</param>
/// <param name="tengoPedido">
///Variable que indica si tenemos un pedido o no, nos indicara cual sera nuestro siguiente objetivo.
///</param>
/// <param name="RuedaDrift">
///GameObject Original de RuedaDrift, a partir de esta se copiaran los clones temporales.
///</param>
/// <param name="RuedaDriftTemp1">
///GameObject Temporarl de una rueda que usaremos para el Drift del coche, solo se crearan cuando el coche este haciendo Drift.
///</param>
/// <param name="RuedaDriftTemp2">
///GameObject Temporal de una rueda que usaremos para el Drift del coche, solo se crearan cuando el coche este haciendo Drift.
///</param>
/// <param name="EstoyDrift">
///Variable que indica si el coche esta drifteando o no (Es decir, si tiene el maximo angulo de giro o no)
///</param>
/// <param name="posicionRueda1">
///Posicion de una de las ruedas que usaremos para el Drift del coche.
///</param>
/// <param name="posicionRueda2">
///Posicion de una de las ruedas que usaremos para el Drift del coche.
///</param>
/// <param name="Pizza">
///GameObject original de Pizza, a partir de este se copiaran los clones temporales.
///</param>
/// <param name="PizzaTemp">
///GameObject temporal para la creacion y destruccion de Pizza
///</param>
/// <param name="flecha">
///GameObject que contiene la flecha que apunta en la direccion del objetivo actual (Pizzeria o Objetivo)
///</param>
/// <param name="pizzeria">
///GameObject que indica el punto de recogida de la pizza
///</param>
/// <param name="objetivo">
///GameObject que indica el punto de entrega al que tendremos que llegar
///</param>
/// <param name="posicionPizza">
///GameObject que indica la posicion en la que aparecera la Pizza
///</param>
/// <param name="sonidoMotor">
///AudioSource con el sonido del motor del coche
///</param>
/// <param name="volumenCocheParado">
///Volumen del coche cuando el jugador no este moviendo el vehiculo
///</param>
/// <param name="volumenCocheMovimiento">
///Volumen del coche en el momento que el jugador este pulsando W o S
///</param>
/// <param name="objetivos">
///ArrayList con todos los posibles objetivos a los que habra que entregar
///</param>
/// <param name="r">
///Numero random usado para elegir el objetivo donde habra que entregar el pedido
///</param>
/// <param name="menuPausa">
///Canvas de menu de Pausa.
///</param>
/// <param name="enPausa">
///Variable Booleana que indicara si el juego esta o no en pausa
///</param>
/// <param name="puntuacionEnBD">
///Puntuacion que se usara para ir actualizando la base de datos cada vez que el jugador haga una entrega
///</param>
/// <param name="spawn">
///GameObject que indica la posicion en la que el jugador aparecera en el caso de tocar el agua
///</param>
public class pruebaMovimiento : MonoBehaviour
{
    // public Canvas PizzaConseguida;
    // public Canvas PizzaEntregada;
    // public TextMeshProUGUI contadorPizzas;
    private Vector3 Inercia;
    public float Velocidad;
    private float VelocidadCopia;
    public float VelocidadMax;
    public float Rozamiento = 0.95f;
    public float AnguloGiro = 20;
    private float AnguloGiroCopia;
    private float AnguloGiroDrift = 21;
    public float Traccion = 1; 
    // public bool tengoPedido = false; 
    // public GameObject RuedaDrift;
    private GameObject RuedaDriftTemp1;
    private GameObject RuedaDriftTemp2;
    private bool EstoyDrift; 
    private GameObject posicionRueda1;
    private GameObject posicionRueda2; 
    // public GameObject Pizza;
    private GameObject PizzaTemp;
    private GameObject flecha;
    private GameObject pizzeria; 
    private GameObject objetivo;
    private GameObject posicionPizza; 
    // private AudioSource sonidoMotor;
    private float volumenCocheParado = 0.1F;
    private float volumenCocheMovimiento = 0.4F;
    // public List<GameObject> objetivos = new List<GameObject>();
    private System.Random r;
    // public Canvas menuPausa;
    private bool enPausa;
    private int puntuacionEnBD;
    // public GameObject spawn;
    
    public GameObject Jugador;
    public GameObject Camara_Coche;
    private bool estoyCerca = false;
    private bool estoyDentro = false;
    public GameObject puertaDelCoche;
    public TextMeshProUGUI textoEntrarCoche;
    
    /// <summary>
    ///Funcion que se inicia nada mas el script esta activo. Inicializara las siguientes variables: Velocidad, VelocidadMax, Time.timeScale, enPausa, 
    ///sonidoMotor, r, PizzaConseguida, PizzaEntregada,VelocidadCopia, AnguloGiroCopia, flecha, pizzeria, posicionRueda1, 
    ///posicionRueda2, contadorPizzas.
    ///Llamara a la funcion TodosLosObjetivosDesactivador y desactivara el menu de pausa
    ///Bloqueara el Cursor en el centro de la pantalla y lo hara invisible
    ///</summary>
    void Start()
    {
        Camara_Coche.SetActive(false);
        textoEntrarCoche.gameObject.SetActive(false);

        // Velocidad = 50;
        // VelocidadMax = 700;
        Time.timeScale = 1;
        // enPausa = false;
        // menuPausa.gameObject.SetActive(false);
        // sonidoMotor = GetComponent<AudioSource>();
        // sonidoMotor.volume = volumenCocheParado;
        r = new System.Random();
        // TodosLosObjetivosDesactivador();
        // PizzaConseguida.enabled = false;
        // PizzaEntregada.enabled = false;
        VelocidadCopia = Velocidad;
        AnguloGiroCopia = AnguloGiro;
        // flecha = GameObject.Find("Flecha");
        // pizzeria = GameObject.Find("Pizzeria");
        // posicionPizza = GameObject.Find("PosicionCaja");
        // posicionRueda1 = GameObject.Find("DriftRueda1");
        // posicionRueda2 = GameObject.Find("DriftRueda2");
        // objetivo.gameObject.SetActive(false); //Ya no hace falta ya que tenemos la funcion que desactiva todos
        // contadorPizzas.text = "0";

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    /// <summary>
    ///Funcion que se ejecuta una vez por cada frame.
    ///Primero comprobara si el juego esta en pausa. En no se esta en pausa, realizara el registro
    ///de movimiento tanto en el eje vertical para avanzar o retroceder el coche, como en el eje horizontal para 
    ///girar el coche. Para que la marcha atras funcione como en la realidad, hay que aniadirle un if que detecte que estamos yendo marcha atras,
    ///Calcula el rozamiento y actualiza la traccion del vehiculo.En el caso de que haya pedido hara que la
    ///flecha indique la posicion del objetivo, en cualquier otro caso eligira la pizzeria como direccion.
    ///Si se detecta que se esta pulsando espacio, se hara el efecto de Drift.
    ///Si no estamos en Pausa y pulsamos ESC, coimprobaremos si estamos o no en pausa, en caso de no estar llamaremos a pausar() y 
    ///en el caso de estar llamaremos a dePausar(). Si se pulsa la tecla E y enPausa en true entonces llamaremos a cambiarPantallaMenu()
    ///</summary>
    void Update()
    {     
        if(!enPausa&&Camara_Coche.activeSelf){
            
             //Movimiento
            Inercia += this.transform.forward * Velocidad * Input.GetAxis("Vertical") * Time.deltaTime;
            this.transform.position += Inercia * Time.deltaTime;

            //Giro
            float giroInput = Input.GetAxis("Horizontal");
            if (Input.GetKey(KeyCode.S)){
                giroInput = -giroInput;
            }
            transform.Rotate(Vector3.up * giroInput * Inercia.magnitude * AnguloGiro * Time.deltaTime);

            //Rozamiento
            Inercia *= Rozamiento;
            Inercia = Vector3.ClampMagnitude(Inercia, VelocidadMax);

            //Traccion
            // if(giroInput==-1 || giroInput == 1 && !EstoyDrift){
            //   RuedaDriftTemp1 = Instantiate(RuedaDrift, posicionRueda1.transform.position, posicionRueda1.transform.rotation, this.transform);
            //   RuedaDriftTemp2 = Instantiate(RuedaDrift, posicionRueda2.transform.position, posicionRueda2.transform.rotation, this.transform);
            //    EstoyDrift = true;
            // }
            // if(giroInput<1||giroInput>-1&&EstoyDrift){
            //     Destroy(RuedaDriftTemp1,0.5f);
            //     Destroy(RuedaDriftTemp2,0.5f);
            //     EstoyDrift = false;
            // }
            Inercia = Vector3.Lerp(Inercia.normalized, this.transform.forward, Traccion * Time.deltaTime) * Inercia.magnitude;

        // objetivo = GameObject.Find("Objetivo");

            // if(tengoPedido){
            //     flecha.transform.LookAt(objetivo.transform.position);
            // }else{
            //     flecha.transform.LookAt(pizzeria.transform.position);
            // }
            if(Input.GetKey(KeyCode.Space)){
                AnguloGiro = AnguloGiroDrift;
                Velocidad = Velocidad * 0.95f;
            }else{
                AnguloGiro = AnguloGiroCopia;
                Velocidad = VelocidadCopia;
            }
            
        
            // if((Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.W))){
            //     sonidoMotor.volume = volumenCocheMovimiento;
            // }else{
            //     sonidoMotor.volume = volumenCocheParado;
            // }
        }
        if(Input.GetKeyDown(KeyCode.E)){
            if(estoyCerca&&!estoyDentro){
                estoyDentro = true;
                Camara_Coche.SetActive(true);
                Jugador.transform.SetParent(this.transform);
                Jugador.SetActive(false);
                textoEntrarCoche.gameObject.SetActive(false);
            }else if(estoyDentro){
                estoyDentro=false;
                Camara_Coche.SetActive(false);
                Jugador.transform.position = puertaDelCoche.transform.position;
                Jugador.transform.SetParent(null);
                Jugador.SetActive(true);
                
                }
        }
        //    if(Input.GetKeyDown(KeyCode.Escape)){
        //         if(!enPausa){
        //             pausar();
        //         }else{
        //             dePausar();
        //         }
        //     }
            // if(Input.GetKeyDown(KeyCode.E)){
            //     if(enPausa){
            //         cambiarPantallaMenu();
            //     }
            // }
            
    }
    
    /// <summary>
    ///Funcion que se ejecutara en el momento que el objeto entre en contacto con otro objeto. En el caso de que 
    ///el objeto tenga un tag Edificio, se transformara la inercia para que impulse el coche hacia atras.
    ///</summary>
    /// <param name="other">Objeto con el que se entrara en collision</param>
    void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag=="Edificio"){
            Inercia = -this.transform.forward * Velocidad * Input.GetAxis("Vertical") * Time.deltaTime * 0;
        }else if(other.gameObject.tag=="Player"){
            estoyCerca = true;
            Debug.Log("DENTRO");
            textoEntrarCoche.gameObject.SetActive(true);
        }
    }
    void OnCollisionExit(Collision other) {
        if(other.gameObject.tag=="Player"){
            estoyCerca = false;
            Debug.Log("FUERA");
            textoEntrarCoche.gameObject.SetActive(false);
        }
    }
/**
    /// <summary>
    ///Funcion que se ejecutara en el momento que el objeto entre en contacto con otro objeto.
    ///En el caso de que el objeto tenga un tag Objetivo, se activara el objeto Pizzeria y se desactivara el 
    ///objetivo actual. La variable PizzaEntregada pasara a ser true y se llamara a la funcion DesactivarCanvasPizzaEntregada tras 3 segundos.
    ///El contador de pizzas se le sumara uno. La variable tengoPedido pasara a ser false.
    ///Llamaremos a la funcion EliminarPizza() y pizzaEnBD()
    ///En el caso que el objeto tenga un tag Pizzeria, se creara un objeto pizza encima del coche, y llamaremos a la funcion
    ///ElegirUnObjetivo(). Posteriormente activaremos el objeto Objetivo que indicara el destino de entrega. Se desactivara
    ///el objeto Pizzeria. La variable PizzaConseguida pasara a ser true y se llamara a la funcion DesactivarCanvasPizzaConseguida tras 3 segundos.
    ///La variable tengoPedido pasara a ser true.
    ///En el caso que el objeto tenga un tag Oceano, movera el vehiculo a la posicion del objeto spawn, esto se dara
    ///cuando el coche se unda en el oceano.
    ///</summary>
    /// <param name="other">Objeto con el que se entrara en trigger</param>
    void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Objetivo"){
            objetivo.gameObject.SetActive(false);
            pizzeria.gameObject.SetActive(true);
            PizzaEntregada.enabled = true;
            Invoke("DesactivarCanvasPizzaEntregada", 3f);
            contadorPizzas.text = "" + (System.Int32.Parse(contadorPizzas.text) + 1);
            tengoPedido = false;
            EliminarPizza();
            pizzaEnBD();
        }else if(other.gameObject.tag == "Pizzeria"){
            PizzaTemp = Instantiate(Pizza, posicionPizza.transform.position, posicionPizza.transform.rotation, this.transform);
            ElegirUnObjetivo();
            objetivo.gameObject.SetActive(true);
            pizzeria.gameObject.SetActive(false);
            PizzaConseguida.enabled = true;
            Invoke("DesactivarCanvasPizzaConseguida", 3f);
            tengoPedido = true;
        }else if(other.gameObject.tag == "Oceano"){
            this.transform.position = spawn.transform.position;
        }
        
    }

    /// <summary>
    ///Funcion que desactivara el canvas pizzaConseguida
    ///</summary>
    void DesactivarCanvasPizzaConseguida(){
        PizzaConseguida.enabled = false;
    }

    /// <summary>
    ///Funcion que desactivara el canvas pizzaEntregada
    ///</summary>
    void DesactivarCanvasPizzaEntregada(){
        PizzaEntregada.enabled = false;
    }

    /// <summary>
    ///Funcion que eligira un objetivo random dentro de los valores que hay en el ArrayList objetivos
    ///</summary>
    void ElegirUnObjetivo(){
        objetivo = objetivos[r.Next(0,17)];//r.Next(0,17)
        objetivo.SetActive(true);
    }

    /// <summary>
    ///Funcion que recorrera el ArrayList objetivos e ira desactivando cada uno de los objetivos
    ///</summary>
    void TodosLosObjetivosDesactivador(){
        for(int i = 0; i<objetivos.Count; i++){
            objetivos[i].SetActive(false);
        }
    }

    /// <summary>
    ///Funcion que eliminara el objeto Pizza que hay sobre el vehiculo
    ///</summary>
    
    void EliminarPizza(){
        Destroy(PizzaTemp);
    }
**/
    /// <summary>
    ///Funcion que activara el Canvas menuPausa y desbloqueara el Cursor ademas de hacerlo visible.
    ///Parara el tiempo y la variable enPausa pasara a ser true.
    ///Tambien pausara el sonidoMotor
    ///</summary>
    // void pausar(){
    //     Debug.Log("Activo");
    //     menuPausa.gameObject.SetActive(true);
    //     Cursor.lockState = CursorLockMode.None;
    //     Cursor.visible = true;
    //     Time.timeScale = 0;
    //     enPausa = true;
    //     sonidoMotor.Pause();
    // }

    // /// <summary>
    // ///Funcion que desactivara el Canvas menuPausa y bloqueara el Cursor ademas de hacerlo invisible.
    // ///Restaurara el tiempo y la variable enPausa pasara a ser false.
    // ///Tambien reanudara el sonidoMotor
    // ///</summary>
    // void dePausar(){
    //     Debug.Log("Desactivo");
    //     menuPausa.gameObject.SetActive(false);
    //     Cursor.lockState = CursorLockMode.Locked;
    //     Cursor.visible = false;
    //     Time.timeScale = 1;
    //     enPausa = false;
    //     sonidoMotor.Play();
    // }

    /// <summary>
    ///Funcion que consulta el valor de PuntuacionTemporal en la base de datos donde el nombre sea phNombre,
    ///guarda esa variable en puntuacionEnBD y despues actualizara este mismo valor sumandole uno.
    ///</summary>
    /**
    void pizzaEnBD(){
        string conexion = "URI=file:" + Application.dataPath + "/Plugins/bdRecords.db" ; //Path to database.
        IDbConnection conexionBD = (IDbConnection) new SqliteConnection(conexion);
        conexionBD.Open(); //Open connection to the database.
        IDbCommand comando = conexionBD.CreateCommand();
        

        comando.CommandText = "SELECT PuntuacionTemporal FROM records WHERE Nombre = 'phNombre'";
        
        IDataReader reader = comando.ExecuteReader();

        puntuacionEnBD = Convert.ToInt32(reader["PuntuacionTemporal"]);
        

        //reader.GetInt32(reader.GetOrdinal("PuntuacionTemporal"));

        reader.Close();



        comando.CommandText = "UPDATE records SET PuntuacionTemporal=" + (puntuacionEnBD+ 1) + " WHERE Nombre = 'phNombre'";

        comando.ExecuteNonQuery();

        comando.Dispose();
        conexionBD.Close();
    }
    **/
    /// <summary>
    ///Funcion que cambiara de pantalla a la pantalla con nombre Menu.
    ///</summary>
    void cambiarPantallaMenu(){
        SceneManager.LoadScene("Menu");
    }
}
