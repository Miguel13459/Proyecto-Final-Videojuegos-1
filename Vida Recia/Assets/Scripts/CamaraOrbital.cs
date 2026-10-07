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
        distanciaCamara = 6;
        velocidadRegreso = 5;
        contador = 0;
    }

    void Update()
    {
        mirar = (playerInput.actions["MirarMouse"].ReadValue<Vector2>() * sensibilidadMouse) + (sensibilidadControl * Time.deltaTime * playerInput.actions["MirarGamepad"].ReadValue<Vector2>());

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
        angulo.y = Math.Clamp(angulo.y, -20, 40);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Quaternion rotacionJugador = Quaternion.Euler(0, componenteTransformJugador.eulerAngles.y, 0 );
        Vector3 posicion = componenteTransformCamara.position - rotacionJugador * Vector3.forward * distanciaCamara;
        posicion.y = componenteTransformCamara.position.y + 2;
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