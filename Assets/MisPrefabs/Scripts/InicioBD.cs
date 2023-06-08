using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite; 
using System.Data;
using System;
/// <summary>
///Clase que se iniciara la Base de Datos creando una fila con un placeHolder del nombre y la Puntuacion y Puntuacion Temporal seran 0.
/// </summary>
public class InicioBD : MonoBehaviour
{
    /// <summary>
    ///Funcion que se inicia nada mas el script esta activo. Ejecutara el comando que se encuentra en el campo comando. Este insertara 
    ///una nueva fila en la base de datos
    /// </summary>
    void Start()
    {
        string conexion = "URI=file:" + Application.dataPath + "/Plugins/bdRecords.db" ; //Path to database.
        IDbConnection conexionBD = (IDbConnection) new SqliteConnection(conexion);
        conexionBD.Open(); //Open connection to the database.
        IDbCommand comando = conexionBD.CreateCommand();
        
        comando.CommandText = "INSERT INTO records VALUES('phNombre',0,0) ON CONFLICT(nombre) DO UPDATE SET nombre = 'phNombre' WHERE nombre = 'phNombre'";

        comando.ExecuteNonQuery();

        comando.Dispose();
        conexionBD.Close();

        Debug.Log("Insertado phNombre");
    }

    /// <summary>
    ///Funcion que se ejecuta una vez por cada frame. No se realiza ninguna funcion
    /// </summary>  
    void Update()
    {
        
    }
}
