using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ManejadorPartida : MonoBehaviour
{
    GameObject jugadorGO;
    Jugador jugadorScript;
    float duracionPartida;
    float limiteTiempoDePartida;

    public static int pizzasEntregadas;
    public static int pizzasPerdidas;
    int mayorPizzasEntregadas;

    public GameObject pantallaNegra;
    Image imagen;
    Color color;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        jugadorGO = GameObject.FindGameObjectWithTag("Player");
        jugadorScript = jugadorGO.GetComponent<Jugador>();
        imagen = pantallaNegra.GetComponent<Image>();
        color = imagen.color;
        duracionPartida = 0;
        limiteTiempoDePartida = 30f;

        pizzasEntregadas = 0;
        pizzasPerdidas = 0;
    }

    // Update is called once per frame
    void Update()
    {
        duracionPartida += Time.deltaTime;
        Debug.Log(duracionPartida);
        if (duracionPartida >= limiteTiempoDePartida)
        {
            
            color.a += Time.deltaTime;
            if (color.a >= 1) color.a = 1;
            imagen.color = color;
            
            if (imagen.color.a == 1)
            {
                jugadorScript.enabled = false;
                TerminarPartida();
            }
            
        } 
    }

    void TerminarPartida()
    {
        if (pizzasEntregadas > mayorPizzasEntregadas)
        {
            mayorPizzasEntregadas = pizzasEntregadas;

            PlayerPrefs.SetInt("mayorPizzasEntregadas", mayorPizzasEntregadas);
            PlayerPrefs.Save();
        }
        SceneManager.LoadScene("PartidaTerminada");
    }

}
