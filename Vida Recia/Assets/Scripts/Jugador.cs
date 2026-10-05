using UnityEngine;
using UnityEngine.InputSystem;

public class Jugador : MonoBehaviour
{
    public GameObject modeloJugador;
    PlayerInput playerInput;
    Rigidbody rb;

    float aceleracion;
    Vector2 giro;
    float velocidadInclinacion;
    Vector3 inclinacion;
    float velocidadActual;
    float velocidadMaxima;
    float fuerzaAceleracion;
    float fuerzaFrenado;
    float velocidadGiro;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerInput = this.GetComponent<PlayerInput>();
        rb = this.GetComponent<Rigidbody>();

        giro = Vector2.zero;
        velocidadInclinacion = 30;
        inclinacion = Vector3.zero;
        aceleracion = 0;
        velocidadActual = 0;
        velocidadMaxima = 20;
        fuerzaAceleracion = 40;
        fuerzaFrenado = 36;
        velocidadGiro = 3;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        aceleracion = playerInput.actions["Acelerar"].ReadValue<float>();
        giro = playerInput.actions["Girar"].ReadValue<Vector2>();

        
        if (aceleracion > 0)
        {
            velocidadActual += fuerzaAceleracion * Time.fixedDeltaTime;
        }
        else if (aceleracion < 0)
        {
            velocidadActual -= fuerzaFrenado * Time.fixedDeltaTime;
        }
        else
        {
            velocidadActual = Mathf.MoveTowards( velocidadActual,0,fuerzaFrenado * Time.fixedDeltaTime);
        }
        //Debug.Log("arriba " + velocidadActual);

        
        float rotacion = giro.x * velocidadGiro;

        velocidadActual = Mathf.Clamp(velocidadActual, -fuerzaFrenado * 1.5f, velocidadMaxima);
        //Debug.Log(direccion);
        //Debug.Log("abajo " + velocidadActual);
        rb.AddForce(transform.forward * velocidadActual);
        rb.MoveRotation(rb.rotation * Quaternion.Euler(0, rotacion, 0));

    }
}
