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
    float sensibilidad;

    float distanciaCamara;
    float alturaCamara;

    float velocidadRegreso;

    PlayerInput playerInput;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        playerInput = jugador.GetComponent<PlayerInput>();
        componenteTransformJugador = jugador.GetComponent<Transform>();
        componenteTransformCamara = puntoCamara.GetComponent<Transform>();

        angulo = new Vector2(90, 0);

        sensibilidad = 0;
        sensibilidadMouse = 0.1f;
        sensibilidadControl = 100;

        distanciaCamara = 6;
        alturaCamara = 3.5f;

        velocidadRegreso = 5;
    }

    void Update()
    {
        mirar = playerInput.actions["Mirar"].ReadValue<Vector2>();

        sensibilidad = mirar.x > 1.5f ? sensibilidadMouse : sensibilidadControl; 

        angulo.x += mirar.x * sensibilidad;
        angulo.y += mirar.y * sensibilidad;
        angulo.x = Mathf.Lerp(angulo.x, 90, velocidadRegreso * Time.deltaTime);
        angulo.y = Mathf.Lerp(angulo.y, 0, velocidadRegreso * Time.deltaTime);

        angulo.y = Math.Clamp(angulo.y, -60, 40);
    }

    // Update is called once per frame
    void LateUpdate()
    { 
        Quaternion rotacionCamara = Quaternion.Euler( angulo.y, componenteTransformJugador.eulerAngles.y + angulo.x - 90, 0 );

        Vector3 posicion = componenteTransformCamara.position - rotacionCamara * Vector3.forward * distanciaCamara;
        posicion.y = alturaCamara;
        transform.position = posicion;

        transform.LookAt(componenteTransformCamara);
    }
}

/*
        if (Mathf.Abs(mirar.x) > 0.01f)
        {
            rotacionHorizontal += mirar.x * sensibilidadMouse;
        }
        else
        {
            rotacionHorizontal = Mathf.Lerp(rotacionHorizontal, 0, velocidadRegreso * Time.deltaTime);
        }

        if (Mathf.Abs(mirar.y) > 0.01f) rotacionVertical += mirar.y * sensibilidadMouse;

        rotacionVertical = Mathf.Clamp(rotacionVertical, -10, 80);

        Quaternion rotacionJugador = Quaternion.Euler(0, componenteTransformJugador.eulerAngles.y, 0);
        Quaternion rotacionCamara = Quaternion.Euler(rotacionHorizontal, rotacionVertical, 0);

        Vector3 posicion = componenteTransformJugador.position - rotacionJugador * Vector3.forward * distanciaCamara;

        posicion.y = alturaCamara;

        transform.SetPositionAndRotation(posicion + orbita, rotacionCamara);
        transform.LookAt(componenteTransformCamara);
        Debug.Log("Mirar despues de todo" + mirar);
 */