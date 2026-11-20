using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CamaraOrbital : MonoBehaviour
{
    public GameObject jugador;
    Transform componenteTransformJugador;
    public GameObject puntoCamara;
    Transform componenteTransformCamara;

    Vector2 mirar;
    Vector2 angulo;

    float sensibilidadMouse;
    float sensibilidadControl;
    float distanciaCamara;
    float velocidadRegreso;
    float contador;

    PlayerInput playerInput;

    [Header("Colisiones de cámara")]
    public float radioCamara = 0.3f;
    public float distanciaMinima = 0.5f;
    public LayerMask capasObstaculos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Application.targetFrameRate = 10;
        Cursor.lockState = CursorLockMode.Locked;

        playerInput = jugador.GetComponent<PlayerInput>();
        componenteTransformJugador = jugador.GetComponent<Transform>();
        componenteTransformCamara = puntoCamara.GetComponent<Transform>();

        angulo = new Vector2(0, 0);
        sensibilidadMouse = 0.05f;
        sensibilidadControl = 100;
        distanciaCamara = 8;
        velocidadRegreso = 5;
        contador = 0;
    }

    void Update()
    {
        mirar = (playerInput.actions["MirarMouse"].ReadValue<Vector2>() * sensibilidadMouse) + 
                (sensibilidadControl * Time.deltaTime * playerInput.actions["MirarGamepad"].ReadValue<Vector2>());

        if(mirar.sqrMagnitude < 0.1f)
        {
            contador += Time.deltaTime;
            if (contador >= 2f)
            {
                angulo.x = Mathf.Lerp(angulo.x, 0, velocidadRegreso * Time.deltaTime);
                angulo.y = Mathf.Lerp(angulo.y, 0, velocidadRegreso * Time.deltaTime);
            } 
        } else
        {
            contador = 0;
        }
        
        if(mirar != Vector2.zero) contador = 0;

        angulo.x += mirar.x;
        angulo.y += mirar.y;
        if (angulo.y > 60f) angulo.y = 60f;
        if (angulo.y < -40f) angulo.y = -40f;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Quaternion rotacionCamara = Quaternion.Euler(
            angulo.y,
            componenteTransformJugador.eulerAngles.y + angulo.x,
            0
        );

        Vector3 origen = componenteTransformCamara.position;

        Vector3 direccion = rotacionCamara *
                            new Vector3(0, 2f, -distanciaCamara);

        float distancia = direccion.magnitude;
        direccion.Normalize();

        Vector3 posicionDeseada = origen + direccion * distancia;

        RaycastHit hit;

        if (Physics.SphereCast(
            origen,
            radioCamara,
            direccion,
            out hit,
            distancia,
            capasObstaculos
        ))
        {
            posicionDeseada = origen +
                direccion * Mathf.Max(hit.distance - distanciaMinima, 0.1f);
        }

        transform.position = posicionDeseada;
        transform.LookAt(origen);
    }

}

