using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ManejadorPartida : MonoBehaviour
{
    float duracionPartida;
    float limiteTiempoDePartida;

    public GameObject pantallaNegra;
    Image imagen;
    Color color;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        imagen = pantallaNegra.GetComponent<Image>();
        duracionPartida = 0;
        limiteTiempoDePartida = 5f;

    }

    // Update is called once per frame
    void Update()
    {
        if (duracionPartida >= limiteTiempoDePartida)
        {
            duracionPartida += Time.deltaTime;
            color = imagen.color;
            color.a += Time.deltaTime;
            if (color.a >= 1) color.a = 1;
            imagen.color = color;
            /*if (jugador.arranqueEnCurso == false && imagenPantalla.color.a == 0)
            {
                jugador.enabled = true;
                this.gameObject.SetActive(false);
            }*/
            TerminarPartida();
        } 
    }

    void TerminarPartida()
    {
        SceneManager.LoadScene("PartidaTerminada");
    }
}
