using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class ClaxonAutoNPC : MonoBehaviour
{
    [Header("Sonido")]
    [SerializeField] private AudioClip sonidoClaxon;
    [SerializeField, Range(0f, 1f)] private float volumen = 0.85f;

    [Header("Configuracion")]
    [SerializeField] private float tiempoEntreClaxones = 3f;

    private AudioSource audioSource;
    private float siguienteClaxon = 0f;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        audioSource.playOnAwake = false;
        audioSource.loop = false;
        audioSource.spatialBlend = 1f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject objeto = collision.rigidbody != null
            ? collision.rigidbody.gameObject
            : collision.gameObject;

        // Solo reacciona al jugador
        if (!objeto.CompareTag("Player"))
            return;

        // Evita sonidos repetidos
        if (Time.time < siguienteClaxon)
            return;

        if (sonidoClaxon == null)
            return;

        // Reproducir claxon
        audioSource.PlayOneShot(sonidoClaxon, volumen);

        siguienteClaxon = Time.time + tiempoEntreClaxones;
    }
}
