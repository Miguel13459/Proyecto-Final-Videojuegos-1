using TMPro;
using UnityEngine;

public class ManejadorUI : MonoBehaviour
{
    GameObject jugador;
    Jugador scriptJugador;
    public TMP_Text textoVelocimetro;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        jugador = GameObject.FindGameObjectWithTag("Player");
        scriptJugador = jugador.GetComponent<Jugador>();
    }

    // Update is called once per frame
    void Update()
    {
        textoVelocimetro.text = Mathf.FloorToInt(scriptJugador.velocidadActual*3).ToString();
    }
}
