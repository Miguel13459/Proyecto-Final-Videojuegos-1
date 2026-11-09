using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class IniciarPartida : MonoBehaviour
{
    Jugador jugador;
    public GameObject pantallaNegraComenzar;
    Image imagenPantalla;
    Color color;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        jugador = GameObject.FindGameObjectWithTag("Player").GetComponent<Jugador>();
        imagenPantalla = pantallaNegraComenzar.GetComponent<Image>();
        StartCoroutine(ComenzarPartida());
    }

    // Update is called once per frame
    void Update()
    {
        color = imagenPantalla.color;
        color.a -= Time.deltaTime;
        if (color.a <= 0) color.a = 0;
        imagenPantalla.color = color;
        if (jugador.arranqueEnCurso == false && imagenPantalla.color.a == 0) this.gameObject.SetActive(false);
    }

    IEnumerator ComenzarPartida()
    {
        jugador.CambiarSonido(jugador.encendido, false);
        yield return new WaitForSeconds(jugador.encendido.length);
        jugador.arranqueEnCurso = false;
        jugador.CambiarSonido(jugador.relentiSonido, true);
    }
}
