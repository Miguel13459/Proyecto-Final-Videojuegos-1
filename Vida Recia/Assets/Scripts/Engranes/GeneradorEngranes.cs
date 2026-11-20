using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneradorEngranes : MonoBehaviour
{
    [Header("Prefab y zonas")]
    [SerializeField] private EngraneColeccionable prefabEngrane;
    [SerializeField] private BoxCollider[] zonasSpawn;
    [SerializeField] private Transform jugador;

    [Header("Capas del mapa")]
    [Tooltip("Incluye Calle y todas las superficies solidas que pueden quedar por encima.")]
    [SerializeField] private LayerMask superficiesRaycast;
    [Tooltip("Solo la layer Calle.")]
    [SerializeField] private LayerMask capaCalle;
    [Tooltip("Edificios, banquetas, postes y vehiculos. No incluir Calle.")]
    [SerializeField] private LayerMask capasObstaculos;

    [Header("Generacion")]
    [Min(0)][SerializeField] private int cantidadInicial = 6;
    [Min(1)][SerializeField] private int intentosPorEngrane = 60;
    [Min(0f)][SerializeField] private float alturaSobreCalle = 0.85f;
    [Min(0.1f)][SerializeField] private float radioLibre = 0.55f;
    [Min(0f)][SerializeField] private float distanciaEntreEngranes = 6f;
    [Min(0f)][SerializeField] private float distanciaJugador = 4f;
    [Min(0f)][SerializeField] private float tiempoReaparicion = 25f;

    private readonly List<EngraneColeccionable> activos = new List<EngraneColeccionable>();
    private readonly Dictionary<EngraneColeccionable, int> zonaDeCadaEngrane =
        new Dictionary<EngraneColeccionable, int>();

    private void Start()
    {
        if (prefabEngrane == null || zonasSpawn == null || zonasSpawn.Length == 0)
        {
            Debug.LogError("GeneradorEngranes: falta Prefab Engrane o Zonas Spawn.");
            return;
        }

        List<int> zonasValidas = new List<int>();
        for (int i = 0; i < zonasSpawn.Length; i++)
        {
            if (zonasSpawn[i] != null && zonasSpawn[i].enabled &&
                zonasSpawn[i].gameObject.activeInHierarchy)
                zonasValidas.Add(i);
        }

        if (zonasValidas.Count == 0)
        {
            Debug.LogError("GeneradorEngranes: no hay zonas activas.");
            return;
        }

        // Si hay mas zonas que engranes, no todas pueden contener uno a la vez.
        for (int n = 0; n < cantidadInicial; n++)
        {
            int indiceZona = zonasValidas[n % zonasValidas.Count];
            if (!IntentarCrearEnZona(indiceZona))
                Debug.LogWarning("No encontre calle libre en " + zonasSpawn[indiceZona].name +
                                 ". Revisa Box Collider, Layers e Intentos Por Engrane.");
        }
    }

    private bool IntentarCrearEnZona(int indiceZona)
    {
        if (zonasSpawn == null || indiceZona < 0 || indiceZona >= zonasSpawn.Length)
            return false;

        BoxCollider zona = zonasSpawn[indiceZona];
        if (zona == null || !zona.enabled || !zona.gameObject.activeInHierarchy)
            return false;

        for (int intento = 0; intento < intentosPorEngrane; intento++)
        {
            // Puntos en el area X-Z de una zona. El rayo comienza por arriba.
            Vector3 local = zona.center + new Vector3(
                Random.Range(-zona.size.x * 0.5f, zona.size.x * 0.5f),
                zona.size.y * 0.5f,
                Random.Range(-zona.size.z * 0.5f, zona.size.z * 0.5f)
            );

            Vector3 inicio = zona.transform.TransformPoint(local) + Vector3.up;
            float distanciaRayo = zona.bounds.size.y + 10f;

            if (!Physics.Raycast(inicio, Vector3.down, out RaycastHit impacto,
                distanciaRayo, superficiesRaycast, QueryTriggerInteraction.Ignore))
                continue;

            if ((capaCalle.value & (1 << impacto.collider.gameObject.layer)) == 0)
                continue;

            Vector3 posicion = impacto.point + Vector3.up * alturaSobreCalle;

            if (Physics.CheckSphere(posicion, radioLibre,
                capasObstaculos, QueryTriggerInteraction.Ignore))
                continue;

            if (jugador != null &&
                DistanciaHorizontal(posicion, jugador.position) < distanciaJugador)
                continue;

            bool demasiadoCerca = false;
            foreach (EngraneColeccionable existente in activos)
            {
                if (existente != null &&
                    DistanciaHorizontal(posicion, existente.transform.position) < distanciaEntreEngranes)
                {
                    demasiadoCerca = true;
                    break;
                }
            }
            if (demasiadoCerca) continue;

            EngraneColeccionable nuevo = Instantiate(prefabEngrane, posicion, Quaternion.identity);
            nuevo.Configurar(this);
            activos.Add(nuevo);
            zonaDeCadaEngrane[nuevo] = indiceZona;
            return true;
        }

        return false;
    }

    private float DistanciaHorizontal(Vector3 a, Vector3 b)
    {
        return Vector2.Distance(new Vector2(a.x, a.z), new Vector2(b.x, b.z));
    }

    public void NotificarRecogido(EngraneColeccionable engrane)
    {
        activos.Remove(engrane);

        if (!zonaDeCadaEngrane.TryGetValue(engrane, out int indiceZona))
            return;

        zonaDeCadaEngrane.Remove(engrane);
        StartCoroutine(ReponerConRetardo(indiceZona));
    }

    private IEnumerator ReponerConRetardo(int indiceZona)
    {
        yield return new WaitForSeconds(tiempoReaparicion);

        // Repone en la misma zona para conservar el reparto equilibrado.
        if (!IntentarCrearEnZona(indiceZona))
            Debug.LogWarning("No se pudo reponer un engrane en " +
                             (zonasSpawn[indiceZona] != null ? zonasSpawn[indiceZona].name : "zona faltante") +
                             ". Reduce Distancia Entre Engranes o revisa la zona.");
    }

    private void OnDrawGizmosSelected()
    {
        if (zonasSpawn == null) return;
        Gizmos.color = new Color(0.1f, 0.8f, 1f, 0.65f);
        foreach (BoxCollider zona in zonasSpawn)
        {
            if (zona == null) continue;
            Matrix4x4 original = Gizmos.matrix;
            Gizmos.matrix = zona.transform.localToWorldMatrix;
            Gizmos.DrawWireCube(zona.center, zona.size);
            Gizmos.matrix = original;
        }
    }
}
