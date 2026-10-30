using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ManejadorUI : MonoBehaviour
{
    GameObject jugador;
    Jugador scriptJugador;

    public GameObject velocimetroBarra;
    public GameObject barraVida;
    Image rellenoBarraVelocimetro;
    Image rellenoBarraVida;
    public TMP_Text textoVelocimetro;

    int velocidad;
    int vidaActual;
    int numVidas;

    public GameObject dialogos;
    public TMP_Text personaje;
    public TMP_Text dialogo;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        jugador = GameObject.FindGameObjectWithTag("Player");
        scriptJugador = jugador.GetComponent<Jugador>();
        numVidas = scriptJugador.vidaPizza;
        vidaActual = scriptJugador.vidaPizza;
        rellenoBarraVelocimetro = velocimetroBarra.GetComponent<Image>();
        rellenoBarraVelocimetro.fillAmount = 0;
        rellenoBarraVida = barraVida.GetComponent<Image>();
        rellenoBarraVida.fillAmount = 1;
        dialogos.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        vidaActual = scriptJugador.vidaPizza;
        //Debug.Log(vidaActual);
        rellenoBarraVida.fillAmount = vidaActual / numVidas;
        //Debug.Log(vidaActual / numVidas);

        velocidad = Mathf.FloorToInt(scriptJugador.velocidadActual * 5);
        velocidad = velocidad < 0 ? 0 : velocidad;
        textoVelocimetro.text = velocidad.ToString();
        rellenoBarraVelocimetro.fillAmount = (scriptJugador.velocidadActual * 5) * 0.01f;
    }

    public void MostrarDialogoJackson(DialogueData dialogoJackson, string lugar)
    {
        dialogos.SetActive(true);
        personaje.text = "Jefe el loco Jackson";
        dialogo.text = dialogoJackson.introduccion + " " + dialogoJackson.lugar + " " + lugar + " " + dialogoJackson.final;
    }

    public void MostrarDialogoClientes(DialogoClientes dialogoClientes, string nombre)
    {
        dialogos.SetActive(true);
        personaje.text = nombre;
        dialogo.text = dialogoClientes.dialogo;
    }

    internal void CerrarDialogos()
    {
        dialogos.SetActive(false);
    }
}
