using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AutoNPC_RutaSimple : MonoBehaviour
{
    [Header("Ruta: objeto con los puntos en orden")]
    [SerializeField] private Transform ruta;

    [Header("Movimiento")]
    [Min(0f)][SerializeField] private float velocidad = 6f;
    [Min(0f)][SerializeField] private float velocidadGiro = 240f;
    [Min(0.05f)][SerializeField] private float distanciaLlegada = 0.7f;

    [Header("Llantas")]
    [SerializeField] private Transform llantaDelanteraIzquierda;
    [SerializeField] private Transform llantaDelanteraDerecha;
    [SerializeField] private Transform llantaTraseraIzquierda;
    [SerializeField] private Transform llantaTraseraDerecha;
    [Min(0.01f)][SerializeField] private float radioLlanta = 0.34f;

    [Header("Baches y desniveles")]
    [SerializeField] private bool seguirSuperficie = true;
    [Tooltip("Incluye la layer Calle y la layer donde estan los baches, por ejemplo Obstaculos. No hace falta cambiar el layer del auto.")]
    [SerializeField] private LayerMask capasDeteccion = ~0;
    [Tooltip("Distancia desde el centro del auto hasta las ruedas delanteras/traseras (eje Z local).")]
    [Min(0f)][SerializeField] private float distanciaEjes = 1.05f;
    [Min(0.2f)][SerializeField] private float origenRaycastArriba = 4f;
    [Min(0.5f)][SerializeField] private float longitudRaycast = 12f;
    [Tooltip("Velocidad de elevacion/descenso del vehiculo en unidades por segundo.")]
    [Min(0.1f)][SerializeField] private float velocidadAltura = 8f;
    [Tooltip("Elevacion adicional cuando se detecta un bache; util para baches casi planos.")]
    [Min(0f)][SerializeField] private float elevacionExtraBache = 0.12f;

    private Rigidbody rb;
    private Transform[] puntos;
    private int destinoActual;
    private float alturaInicial;
    private float distanciaAlSuelo;
    private bool offsetCalculado;
    private bool rutaValida;
    private int layerCalle;

    // No asigna memoria por cada Raycast en FixedUpdate.
    private readonly RaycastHit[] impactos = new RaycastHit[16];

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        layerCalle = LayerMask.NameToLayer("Calle");
    }

    private void Start()
    {
        if (!rb.isKinematic)
            Debug.LogWarning("AutoNPC: activa Is Kinematic en su Rigidbody.", this);

        alturaInicial = rb.position.y;

        if (layerCalle < 0)
            Debug.LogWarning("AutoNPC: no existe la layer 'Calle'. Creala o verifica el nombre.", this);

        if (seguirSuperficie && EncontrarSuelo(rb.position, out float alturaSuelo, out _))
        {
            distanciaAlSuelo = alturaInicial - alturaSuelo;
            offsetCalculado = true;
        }

        if (ruta == null || ruta.childCount < 2)
        {
            Debug.LogWarning("AutoNPC: asigna una ruta con al menos 2 puntos hijos.", this);
            enabled = false;
            return;
        }

        puntos = new Transform[ruta.childCount];
        for (int i = 0; i < puntos.Length; i++)
            puntos[i] = ruta.GetChild(i);

        int cercano = 0;
        float menorDistancia = float.MaxValue;
        Vector3 posicion = rb.position;

        for (int i = 0; i < puntos.Length; i++)
        {
            Vector3 diferencia = puntos[i].position - posicion;
            diferencia.y = 0f;
            float distancia = diferencia.sqrMagnitude;
            if (distancia < menorDistancia)
            {
                menorDistancia = distancia;
                cercano = i;
            }
        }

        destinoActual = (cercano + 1) % puntos.Length;
        rutaValida = true;
    }

    private void FixedUpdate()
    {
        if (!rutaValida) return;

        Vector3 antes = rb.position;
        Vector3 destino = puntos[destinoActual].position;
        destino.y = antes.y;

        Vector3 direccion = destino - antes;
        direccion.y = 0f;

        Quaternion rotacionNueva = rb.rotation;
        if (direccion.sqrMagnitude > 0.0001f)
        {
            Quaternion rotacionDeseada = Quaternion.LookRotation(direccion.normalized, Vector3.up);
            rotacionNueva = Quaternion.RotateTowards(
                rb.rotation, rotacionDeseada, velocidadGiro * Time.fixedDeltaTime);
            rb.MoveRotation(rotacionNueva);
        }

        Vector3 siguiente = Vector3.MoveTowards(
            antes, destino, velocidad * Time.fixedDeltaTime);

        if (seguirSuperficie)
            AjustarAltura(ref siguiente, rotacionNueva);
        else
            siguiente.y = alturaInicial;

        rb.MovePosition(siguiente);

        // Gira segun el avance horizontal real, sin incluir el bote vertical.
        Vector3 desplazamiento = siguiente - antes;
        desplazamiento.y = 0f;
        float grados = desplazamiento.magnitude / radioLlanta * Mathf.Rad2Deg;
        Girar(llantaDelanteraIzquierda, grados);
        Girar(llantaDelanteraDerecha, grados);
        Girar(llantaTraseraIzquierda, grados);
        Girar(llantaTraseraDerecha, grados);

        Vector3 restante = destino - siguiente;
        restante.y = 0f;
        if (restante.sqrMagnitude <= distanciaLlegada * distanciaLlegada)
            destinoActual = (destinoActual + 1) % puntos.Length;
    }

    private void AjustarAltura(ref Vector3 siguiente, Quaternion orientacion)
    {
        // Se comprueba la superficie bajo el centro y ambos ejes del coche.
        Vector3 adelante = orientacion * Vector3.forward * distanciaEjes;
        float mayorAltura = float.NegativeInfinity;
        bool encontro = false;
        bool hayBache = false;

        Evaluar(siguiente, ref mayorAltura, ref encontro, ref hayBache);
        Evaluar(siguiente + adelante, ref mayorAltura, ref encontro, ref hayBache);
        Evaluar(siguiente - adelante, ref mayorAltura, ref encontro, ref hayBache);

        if (!encontro) return; // Si no se detecta piso, no cambiar bruscamente de altura.

        if (!offsetCalculado)
        {
            distanciaAlSuelo = rb.position.y - mayorAltura;
            offsetCalculado = true;
        }

        float alturaDeseada = mayorAltura + distanciaAlSuelo;
        if (hayBache)
            alturaDeseada += elevacionExtraBache;

        siguiente.y = Mathf.MoveTowards(
            rb.position.y, alturaDeseada, velocidadAltura * Time.fixedDeltaTime);
    }

    private void Evaluar(Vector3 punto, ref float mayorAltura, ref bool encontro, ref bool hayBache)
    {
        if (!EncontrarSuelo(punto, out float y, out bool bache)) return;
        encontro = true;
        mayorAltura = Mathf.Max(mayorAltura, y);
        hayBache |= bache;
    }

    private bool EncontrarSuelo(Vector3 punto, out float altura, out bool esBache)
    {
        altura = 0f;
        esBache = false;
        float mayorAltura = float.NegativeInfinity;
        bool valido = false;

        Vector3 origen = punto + Vector3.up * origenRaycastArriba;
        int cantidad = Physics.RaycastNonAlloc(origen, Vector3.down, impactos,
            longitudRaycast, capasDeteccion, QueryTriggerInteraction.Collide);

        for (int i = 0; i < cantidad; i++)
        {
            Collider col = impactos[i].collider;
            if (col == null || col.transform.IsChildOf(transform)) continue;

            bool bache = TieneTagBache(col.transform);
            bool calle = col.gameObject.layer == layerCalle;
            if (!bache && !calle) continue;

            valido = true;
            mayorAltura = Mathf.Max(mayorAltura, impactos[i].point.y);
            esBache |= bache;
        }

        if (valido) altura = mayorAltura;
        return valido;
    }

    private static bool TieneTagBache(Transform objeto)
    {
        // Permite que el tag este en el objeto del collider o en su padre.
        for (Transform actual = objeto; actual != null; actual = actual.parent)
        {
            if (actual.CompareTag("Bache")) return true;
        }
        return false;
    }

    private static void Girar(Transform rueda, float grados)
    {
        if (rueda != null)
            rueda.Rotate(Vector3.right, grados, Space.Self);
    }

    private void OnDrawGizmosSelected()
    {
        if (ruta == null || ruta.childCount < 2) return;
        Gizmos.color = Color.cyan;
        for (int i = 0; i < ruta.childCount; i++)
        {
            Transform a = ruta.GetChild(i);
            Transform b = ruta.GetChild((i + 1) % ruta.childCount);
            Gizmos.DrawLine(a.position, b.position);
            Gizmos.DrawSphere(a.position, 0.3f);
        }
    }
}
