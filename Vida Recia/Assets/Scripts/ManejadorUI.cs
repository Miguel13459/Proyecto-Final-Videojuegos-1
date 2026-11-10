using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ManejadorUI : MonoBehaviour
{
    [Header("Referencias jugador")]
    GameObject jugador;
    Jugador scriptJugador;

    [Header("UI jugador")]
    public GameObject velocimetroBarra;
    public GameObject barraVida;
    Image rellenoBarraVelocimetro;
    Image rellenoBarraVida;
    public TMP_Text textoVelocimetro;
    public GameObject vidaPizza;

    int velocidad;
    int vidaActual;
    int numVidas;

    [Header("Dialogos")]
    public GameObject dialogos;
    public TMP_Text personaje;
    public TMP_Text dialogo;

    [Header("Misiones")]
    public GameObject misiones;
    public TMP_Text titulo;
    public TMP_Text descripcion;
    public GameObject statusMisiones;
    public TMP_Text tituloStatus;
    public TMP_Text descripcionStatus;

    public Image pantallaNegroComenzar;

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

        //dialogos
        dialogos.SetActive(false);

        //misiones
        misiones.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        vidaActual = scriptJugador.vidaPizza;
        //Debug.Log(vidaActual);
        rellenoBarraVida.fillAmount = vidaActual / numVidas;
        //Debug.Log(vidaActual / numVidas);

        velocidad = Mathf.FloorToInt(scriptJugador.velocidadActual / 2);
        velocidad = velocidad < 0 ? 0 : velocidad;
        textoVelocimetro.text = velocidad.ToString();
        rellenoBarraVelocimetro.fillAmount = (scriptJugador.velocidadActual / 2) * 0.01f;
    }

    public void MostrarDialogoJackson(DialogueData dialogoJackson, string lugar)
    {
        Debug.Log("===== DIAGNÓSTICO DE DIÁLOGOS =====");

        Debug.Log("ManejadorUI: " + this.name);
        Debug.Log("¿ManejadorUI sigue activo?: " + (this != null));

        Debug.Log("¿Dialogos es null?: " + (dialogos == null));

        if (dialogos == null)
        {
            Debug.LogError("¡LA REFERENCIA DIALOGOS ESTÁ DESTRUIDA!");
            return;
        }

        Debug.Log("Objeto Dialogos: " + dialogos.name);
        Debug.Log("ID de Dialogos: " + dialogos.GetEntityId());

        dialogos.SetActive(true);

        personaje.text = "Jefe el loco Jackson";

        dialogo.text = dialogoJackson.introduccion + " "
            + dialogoJackson.lugar + " "
            + lugar + " "
            + dialogoJackson.final;
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

    public void MostrarMision(string titulo, string descripcion)
    {
        misiones.SetActive(true);
        this.titulo.text = titulo;
        this.descripcion.text = descripcion;

    }

    public void CerrarMision()
    {
        misiones.SetActive(false);
    }

    public void MostrarStatus(string titulo, string descripcion)
    {
        statusMisiones.SetActive(true);
        this.tituloStatus.text = titulo;
        this.descripcionStatus.text = descripcion;

    }

    public void CerrarStatus()
    {
        statusMisiones.SetActive(false);
    }

    public void AparecerVidaPizza()
    {
        vidaPizza.SetActive(true);
    }

    public void CerrarVidaPizza()
    {
        vidaPizza.SetActive(false);
    }

    public IEnumerator MostrarStatusCorrutina(string titulo, string descripcion, ManejadorDialogos manejadorDialogos)
    {
        Debug.Log("Entra a la corrutina");
        while (manejadorDialogos.dialogoEstaActivo)
        {
            Debug.Log("Entra al while");
            yield return null;
        }
        yield return new WaitForSeconds(0.3f);
        MostrarStatus(titulo, descripcion);
    }
}
