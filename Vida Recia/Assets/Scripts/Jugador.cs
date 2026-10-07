using UnityEngine;
using UnityEngine.InputSystem;

public class Jugador : MonoBehaviour
{
    public GameObject modeloMoto;
    PlayerInput playerInput;
    Rigidbody rb;

    float aceleracion;
    Vector2 giro;
    float inclinacionMoto;
    float velocidadActual;
    float velocidadMaxima;
    float fuerzaAceleracion;
    float fuerzaFrenado;
    float velocidadGiro;

    void Start()
    {
        playerInput = this.GetComponent<PlayerInput>();
        rb = this.GetComponent<Rigidbody>();

        giro = Vector2.zero;
        inclinacionMoto = 0;
        aceleracion = 0;
        velocidadActual = 0;
        velocidadMaxima = 30;
        fuerzaAceleracion = 40;
        fuerzaFrenado = 14;
        velocidadGiro = 3;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //datos para mostrar en pantalla
        if(PasarDatos.vidaPizza == 0) Debug.Log("Perdiste");
        //Debug.Log(velocidadActual * 2);

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

        float rotacion = giro.x * velocidadGiro;

        velocidadActual = Mathf.Clamp(velocidadActual, -fuerzaFrenado * 1.5f, velocidadMaxima);
        rb.AddForce(transform.forward * velocidadActual);
        rb.MoveRotation(rb.rotation * Quaternion.Euler(0, rotacion, 0));

        if(giro.x != 0)
        {
            inclinacionMoto += giro.x * -100 * Time.fixedDeltaTime;
            if(inclinacionMoto > 45) inclinacionMoto = 45;
            if(inclinacionMoto < -45) inclinacionMoto = -45;
        }
        else
        {
            inclinacionMoto = Mathf.MoveTowards(inclinacionMoto, 0, 100 * Time.fixedDeltaTime);
        }
        modeloMoto.transform.localRotation = Quaternion.Euler(0, 0, inclinacionMoto);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision == null) return;
        if(collision.gameObject.CompareTag("Autos"))
        {
            PasarDatos.vidaPizza--;
            Debug.Log(PasarDatos.vidaPizza);
        }
        if (collision.gameObject.CompareTag("Bache"))
        {
            PasarDatos.vidaPizza--;
            Debug.Log(PasarDatos.vidaPizza);
        }
    }
}
