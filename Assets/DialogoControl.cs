using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogoControl : MonoBehaviour
{
    public TextMeshProUGUI dialogoTexto;
    public string[] lineas;
    public float velocidad;
    private int puntero;
    // Start is called before the first frame update
    void Start()
    {
        // dialogoTexto.text = "";
        // empezarDialogo();
    }

    // Update is called once per frame
    void Update()
    {
        // if(Input.GetMouseButtonDown(0)){
        //     if(dialogoTexto.text == lineas[puntero]){
        //         siguienteLinea();
        //     }else{
        //         StopAllCoroutines();
        //         dialogoTexto.text = lineas[puntero];
        //     }
        // }
    }

    void empezarDialogo(){
        puntero = 0;
        StartCoroutine(escribirLinea());
    }

    void siguienteLinea(){
        if(puntero < lineas.Length - 1){
            puntero++;
            dialogoTexto.text = "";
            StartCoroutine(escribirLinea());
        }else
        {
            gameObject.SetActive(false);
        }
    }

    IEnumerator escribirLinea(){
        foreach(char c in lineas[puntero].ToCharArray()){
            dialogoTexto.text += c;
            yield return new WaitForSeconds(velocidad);
        }
    }
}
