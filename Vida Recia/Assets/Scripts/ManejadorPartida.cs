using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
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

    public GameObject panelPausa;
    private bool isPausado = false;
    public EventSystem eventSystem;
    

    void Start()
    {
        jugadorGO = GameObject.FindGameObjectWithTag("Player");
        jugadorScript = jugadorGO.GetComponent<Jugador>();
        imagen = pantallaNegra.GetComponent<Image>();
        color = imagen.color;
        duracionPartida = 0;
        limiteTiempoDePartida = 300f;

        pizzasEntregadas = 0;
        pizzasPerdidas = 0;
        
    }

    void Update()
    {
        duracionPartida += Time.deltaTime;
        //Debug.Log(duracionPartida);
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

    public void OnPausa()
    {
        if (isPausado)
        {
            ReaunudarJuego();
        }
        else
        {
            PausarJuego();
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

    public void PausarJuego()
    {
        panelPausa.SetActive(true);
        Time.timeScale = 0f;
        isPausado = true;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ReaunudarJuego()
    {
        panelPausa.SetActive(false);
        Time.timeScale = 1f;
        isPausado = false;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Ajustes()
    {
        Debug.Log("No hay presupuesto para desarrollar esta parte");
    }

    public void salirAlMenu()
    {
        SceneManager.LoadScene("MenuInicio");
    }
}
