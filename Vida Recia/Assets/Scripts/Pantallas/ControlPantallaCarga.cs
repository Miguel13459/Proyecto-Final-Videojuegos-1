using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlPantallaCarga : MonoBehaviour
{
    [Header("Elementos de interfaz")]
    [SerializeField] private CanvasGroup grupoTransicion;
    [SerializeField] private CanvasGroup grupoLogo;
    [SerializeField] private Animator animadorFade;
    [SerializeField] private GameObject logoCarga;
    [SerializeField] private Animator animadorLogo;

    [Header("Configuracion")]
    [SerializeField] private string escenaDestino = "Juego";

    [Header("Duraciones")]
    [SerializeField] private float duracionFadeOut = 2f;
    [SerializeField] private float esperaNegro = 0.15f;
    [SerializeField] private float aparicionLogo = 0.3f;
    [SerializeField] private float tiempoCarga = 5f;
    [SerializeField] private float desaparicionLogo = 0.35f;
    [SerializeField] private float duracionFadeIn = 2f;

    private bool cargando = false;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        grupoTransicion.alpha = 0f;
        grupoTransicion.blocksRaycasts = false;
        grupoTransicion.interactable = false;

        grupoLogo.alpha = 0f;
        logoCarga.SetActive(false);

        animadorFade.updateMode =
            AnimatorUpdateMode.UnscaledTime;

        if (animadorLogo != null)
        {
            animadorLogo.updateMode = AnimatorUpdateMode.UnscaledTime;
        }
    }

    public void IniciarPartida()
    {
        if (!cargando)
        {
            StartCoroutine(SecuenciaCarga());
        }
    }

    private IEnumerator SecuenciaCarga()
    {
        cargando = true;

        grupoTransicion.blocksRaycasts = true;

        // OCULTAR EL LOGO
        // Durante el FadeOut solo aparece el fondo negro.
        grupoLogo.alpha = 0f;
        logoCarga.SetActive(false);

        // FADEOUT DEL MENU
        animadorFade.Play("FadeOut", 0, 0f);

        yield return new WaitForSecondsRealtime(
            duracionFadeOut
        );

        // Esperar a que termine de actualizarse el Animator.
        yield return null;

        // El clip FadeOut termina en Alpha = 1.
        // El fondo este totalmente negro.
        grupoTransicion.alpha = 1f;

        // PAUSA CON FONDO NEGRO
        yield return new WaitForSecondsRealtime(
            esperaNegro
        );

        // MOSTRAR EL LOGO SUAVEMENTE
        logoCarga.SetActive(true);
        grupoLogo.alpha = 0f;

        yield return StartCoroutine(
            AnimarAlpha(grupoLogo, 0f, 1f, aparicionLogo)
        );

        // INICIAR CARGA DE LA ESCENA
        AsyncOperation carga =
            SceneManager.LoadSceneAsync(escenaDestino);

        if (carga == null)
        {
            Debug.LogError(
                "No se pudo cargar la escena: " + escenaDestino
            );
            yield break;
        }

        carga.allowSceneActivation = false;

        // Mantener el logo visible durante 5 segundos.
        yield return new WaitForSecondsRealtime(tiempoCarga);

        // Esperar a que Juego este listo.
        while (carga.progress < 0.9f)
        {
            yield return null;
        }

        // DESAPARECER EL LOGO
        // El fondo sigue completamente negro.
        yield return StartCoroutine(
            AnimarAlpha(
                grupoLogo,
                1f,
                0f,
                desaparicionLogo
            )
        );

        logoCarga.SetActive(false);

        // ACTIVAR LA ESCENA JUEGO
        carga.allowSceneActivation = true;

        while (!carga.isDone)
        {
            yield return null;
        }

        yield return null;

        // FADEIN PARA REVELAR LA PARTIDA
        animadorFade.Play("FadeIn", 0, 0f);

        yield return new WaitForSecondsRealtime(
            duracionFadeIn
        );

        grupoTransicion.blocksRaycasts = false;

        cargando = false;

        Destroy(gameObject);
    }

    // Funcion para animar la transparencia del logo.
    private IEnumerator AnimarAlpha(
        CanvasGroup grupo,
        float inicio,
        float final,
        float duracion
    )
    {
        if (duracion <= 0f)
        {
            grupo.alpha = final;
            yield break;
        }

        float tiempo = 0f;

        grupo.alpha = inicio;

        while (tiempo < duracion)
        {
            tiempo += Time.unscaledDeltaTime;

            float porcentaje = Mathf.Clamp01(
                tiempo / duracion
            );

            grupo.alpha = Mathf.Lerp(
                inicio,
                final,
                porcentaje
            );

            yield return null;
        }

        grupo.alpha = final;
    }
}
