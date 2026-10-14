using TMPro;
using UnityEngine;


public class ManejadorUI : MonoBehaviour
{
    public UnityEngine.UI.Image barraVelocimetro;
    Material materialVelocimetro;
    GameObject jugador;
    Jugador scriptJugador;
    public TMP_Text textoVelocimetro;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        jugador = GameObject.FindGameObjectWithTag("Player");
        scriptJugador = jugador.GetComponent<Jugador>();

        materialVelocimetro = barraVelocimetro.material;
    }

    // Update is called once per frame
    void Update()
    {
        /*float velocidad = Mathf.Abs(scriptJugador.velocidadActual) * 3f;

        float progreso = Mathf.Clamp01(velocidad / scriptJugador.velocidadMaxima);

        materialVelocimetro.SetFloat("_Progress", progreso);*/

        textoVelocimetro.text = Mathf.FloorToInt(scriptJugador.velocidadActual*3).ToString();
    }
}
