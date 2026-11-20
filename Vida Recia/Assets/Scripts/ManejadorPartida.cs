using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ManejadorPartida : MonoBehaviour
{
    GameObject jugadorGO;
    Jugador jugadorScript;
    CamaraOrbital camaraOrbital;
    public ManejadorUI manejadorUI;
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
        camaraOrbital = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CamaraOrbital>();
        imagen = pantallaNegra.GetComponent<Image>();
        color = imagen.color;
        duracionPartida = 300f;

        pizzasEntregadas = 0;
        pizzasPerdidas = 0;
        
    }

    void Update()
    {
        if (duracionPartida > 0)
        {
            duracionPartida -= Time.deltaTime;

            if (duracionPartida <= 0)
            {
                duracionPartida = 0;
            }
        }

        manejadorUI.ActualizadTemporizador(duracionPartida);

        if (duracionPartida <= 0)
        {
            color.a += Time.deltaTime;
            color.a = Mathf.Clamp01(color.a);
            imagen.color = color;

            if (color.a >= 1)
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
        jugadorScript.sonidoMoto.Pause();
        camaraOrbital.enabled = false;
        isPausado = true;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ReaunudarJuego()
    {
        panelPausa.SetActive(false);
        Time.timeScale = 1f;
        jugadorScript.sonidoMoto.Play();
        camaraOrbital.enabled = true;
        isPausado = false;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    /*public void ReiniciarPartida()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }*/

    public void Ajustes()
    {
        Debug.Log("No hay presupuesto para desarrollar esta parte");
    }

    public void salirAlMenu()
    {
        SceneManager.LoadScene("MenuInicio");
    }
}
