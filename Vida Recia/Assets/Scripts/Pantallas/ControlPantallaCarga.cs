using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlPantallaCarga : MonoBehaviour
{
    [Header("Elementos de interfaz")]
    [SerializeField] private CanvasGroup grupoTransicion;
    [SerializeField] private Animator animadorFade;
    [SerializeField] private GameObject logoCarga;
    [SerializeField] private Animator animadorLogo;

    [Header("Configuracion")]
    [SerializeField] private string escenaDestino = "Juego";
    [SerializeField] private float duracionFadeOut = 0.8f;
    [SerializeField] private float tiempoCarga = 5f;
    [SerializeField] private float duracionFadeIn = 0.8f;

    private bool cargando = false;

    private void Awake()
    {
        // Mantener esta interfaz durante el cambio de escena.
        DontDestroyOnLoad(gameObject);

        grupoTransicion.alpha = 0f;
        grupoTransicion.blocksRaycasts = false;
        grupoTransicion.interactable = false;

        logoCarga.SetActive(false);

        // Las animaciones continuan si Time.timeScale es 0.
        animadorFade.updateMode = AnimatorUpdateMode.UnscaledTime;

        if (animadorLogo != null)
            animadorLogo.updateMode = AnimatorUpdateMode.UnscaledTime;
    }

    // Este metodo se asigna al boton Nuevo Juego.
    public void IniciarPartida()
    {
        if (!cargando)
            StartCoroutine(SecuenciaCarga());
    }

    private IEnumerator SecuenciaCarga()
    {
        cargando = true;

        // Bloquear los clics durante la transicion.
        grupoTransicion.blocksRaycasts = true;

        // PASO 1: Oscurecer el menu.
        logoCarga.SetActive(false);
        animadorFade.Play("FadeOut", 0, 0f);

        yield return new WaitForSecondsRealtime(duracionFadeOut);

        // PASO 2: Mostrar el logo animado.
        logoCarga.SetActive(true);

        // PASO 3: Cargar la nueva escena sin mostrarla aun.
        AsyncOperation carga =
            SceneManager.LoadSceneAsync(escenaDestino);

        if (carga == null)
        {
            Debug.LogError("No se pudo cargar la escena: " + escenaDestino);
            yield break;
        }

        carga.allowSceneActivation = false;

        // Mostrar la pantalla de carga al menos 5 segundos.
        yield return new WaitForSecondsRealtime(tiempoCarga);

        // Esperar a que Unity termine de preparar la escena.
        while (carga.progress < 0.9f)
            yield return null;

        // PASO 4: Activar Juego.
        carga.allowSceneActivation = true;

        while (!carga.isDone)
            yield return null;

        // Dar un fotograma para renderizar la nueva escena.
        yield return null;

        // PASO 5: Desvanecer el negro y el logo.
        animadorFade.Play("FadeIn", 0, 0f);

        yield return new WaitForSecondsRealtime(duracionFadeIn);

        grupoTransicion.blocksRaycasts = false;
        logoCarga.SetActive(false);

        cargando = false;

        // Eliminar la interfaz temporal.
        Destroy(gameObject);
    }
}
