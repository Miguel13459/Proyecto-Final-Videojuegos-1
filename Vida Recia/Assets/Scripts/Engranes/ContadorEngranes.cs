using System.Collections;
using TMPro;
using UnityEngine;

public class ContadorEngranes : MonoBehaviour
{
    public static ContadorEngranes Instancia { get; private set; }

    [Header("HUD")]
    [SerializeField] private TMP_Text textoContador;
    [SerializeField] private TMP_Text textoMasUno;
    [SerializeField] private float duracionMasUno = 0.8f;
    [SerializeField] private float desplazamientoMasUno = 45f;

    [Header("Sonido de recoleccion")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip sonidoRecoger;
    [Range(0f, 1f)][SerializeField] private float volumenSonido = 0.8f;

    public int Cantidad { get; private set; }

    private CanvasGroup grupoMasUno;
    private RectTransform rectMasUno;
    private Vector2 posicionInicialMasUno;
    private Coroutine animacionMasUno;

    private void Awake()
    {
        if (Instancia != null && Instancia != this)
        {
            Debug.LogWarning("Ya existe otro ContadorEngranes en esta escena.");
            Destroy(gameObject);
            return;
        }

        Instancia = this;
        Cantidad = 0;
        ActualizarTexto();

        if (textoMasUno != null)
        {
            rectMasUno = textoMasUno.rectTransform;
            posicionInicialMasUno = rectMasUno.anchoredPosition;

            grupoMasUno = textoMasUno.GetComponent<CanvasGroup>();
            if (grupoMasUno == null)
                grupoMasUno = textoMasUno.gameObject.AddComponent<CanvasGroup>();

            grupoMasUno.alpha = 0f;
        }

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    public void AgregarEngrane()
    {
        Cantidad++;
        ActualizarTexto();

        if (audioSource != null && sonidoRecoger != null)
            audioSource.PlayOneShot(sonidoRecoger, volumenSonido);

        if (grupoMasUno != null && rectMasUno != null)
        {
            if (animacionMasUno != null)
                StopCoroutine(animacionMasUno);

            animacionMasUno = StartCoroutine(AnimarMasUno());
        }
    }

    private void ActualizarTexto()
    {
        if (textoContador != null)
            textoContador.text = Cantidad.ToString("000");
    }

    private IEnumerator AnimarMasUno()
    {
        textoMasUno.text = "+1";
        rectMasUno.anchoredPosition = posicionInicialMasUno;
        grupoMasUno.alpha = 1f;

        float transcurrido = 0f;
        float duracion = Mathf.Max(0.01f, duracionMasUno);

        while (transcurrido < duracion)
        {
            transcurrido += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(transcurrido / duracion);

            // Se eleva y se desvanece al final.
            float suave = Mathf.SmoothStep(0f, 1f, t);
            rectMasUno.anchoredPosition = posicionInicialMasUno
                + Vector2.up * (desplazamientoMasUno * suave);
            grupoMasUno.alpha = 1f - Mathf.Clamp01((t - 0.30f) / 0.70f);

            yield return null;
        }

        grupoMasUno.alpha = 0f;
        rectMasUno.anchoredPosition = posicionInicialMasUno;
        animacionMasUno = null;
    }

    private void OnDestroy()
    {
        if (Instancia == this)
            Instancia = null;
    }
}
