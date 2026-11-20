using UnityEngine;

public class EngraneColeccionable : MonoBehaviour
{
    [Header("Movimiento visual")]
    [SerializeField] private Transform modeloVisual;
    [SerializeField] private float velocidadGiro = 150f;
    [SerializeField] private float amplitudFlotacion = 0.12f;
    [SerializeField] private float velocidadFlotacion = 2.5f;

    private Vector3 posicionVisualInicial;
    private bool recogido;
    private GeneradorEngranes generador;

    public void Configurar(GeneradorEngranes origen)
    {
        generador = origen;
    }

    private void Awake()
    {
        // Busca el pivote por su nombre si aun no se asigno en el Inspector.
        if (modeloVisual == null)
            modeloVisual = transform.Find("VisualGiratorio");

        if (modeloVisual == null)
            Debug.LogWarning($"{name}: asigna VisualGiratorio a Modelo Visual en el prefab.");
    }

    private void Start()
    {
        if (modeloVisual != null)
            posicionVisualInicial = modeloVisual.localPosition;
    }

    private void Update()
    {
        if (recogido || modeloVisual == null) return;

        // Girar el objeto
        // El engrane permanece de pie
        modeloVisual.Rotate(Vector3.up, velocidadGiro * Time.deltaTime, Space.World);

        // Flotar suavemente: el Collider y el Rigidbody del objeto raiz no se mueven.
        float desplazamiento = Mathf.Sin(Time.time * velocidadFlotacion) * amplitudFlotacion;
        modeloVisual.localPosition = posicionVisualInicial + Vector3.up * desplazamiento;
    }

    private void OnTriggerEnter(Collider otro)
    {
        if (recogido) return;

        if (!otro.transform.root.CompareTag("Player")) return;

        recogido = true;

        // El contador reproduce el sonido y muestra +1 en el HUD.
        if (ContadorEngranes.Instancia != null)
            ContadorEngranes.Instancia.AgregarEngrane();
        else
            Debug.LogWarning("No se encontro un ContadorEngranes en la escena.");

        if (generador != null)
            generador.NotificarRecogido(this);

        gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
