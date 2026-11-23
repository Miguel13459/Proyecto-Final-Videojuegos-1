using System;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ManejadorPartida : MonoBehaviour
{
    GameObject jugadorGO;
    Jugador jugadorScript;
    public GameObject musica;
    MusicPlayer musicPlayerSript;
    CamaraOrbital camaraOrbital;
    public ManejadorUI manejadorUI;
    float duracionPartida;
    public Volume globalVolume;
    

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
        musicPlayerSript = musica.GetComponent<MusicPlayer>();
        camaraOrbital = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CamaraOrbital>();
        imagen = pantallaNegra.GetComponent<Image>();
        color = imagen.color;
        duracionPartida = 300f;

        pizzasEntregadas = 0;
        pizzasPerdidas = 0;
        
    }

    void Update()
    {
        
        if (jugadorScript.siguiente) CambiarSiguienteCancion();
        if (jugadorScript.anterior) CambiarAnteriorCancion();
        if (jugadorScript.pausarMusica) PausarCancion();

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

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("PartidaTerminada");
    }

    public void PausarJuego()
    {
        panelPausa.SetActive(true);
        Time.timeScale = 0f;
        jugadorScript.sonidoMoto.Pause();
        camaraOrbital.enabled = false;
        if (globalVolume.profile.TryGet(out DepthOfField depthOfField))
        {
            depthOfField.focalLength.value = 182;
            depthOfField.aperture.value = 0f;
        }
        ;
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
        if (globalVolume.profile.TryGet(out DepthOfField depthOfField))
        {
            depthOfField.focalLength.value = 50;
            depthOfField.aperture.value = 5.6f;
        }
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

    public void SalirAlMenu()
    {
        Time.timeScale = 1f;
        jugadorScript.sonidoMoto.Play();
        camaraOrbital.enabled = true;
        isPausado = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("MenuInicio");
    }

    public void PausarCancion()
    {
        musicPlayerSript.PlayPauseToggle();
    }

    public void CambiarAnteriorCancion()
    {
        musicPlayerSript.PreviousSong();
        jugadorScript.anterior = false;
    }

    public void CambiarSiguienteCancion()
    {
        musicPlayerSript.NextSong();
        jugadorScript.siguiente = false;
    }
}
