using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class Jugador : MonoBehaviour
{
    public GameObject modeloMoto;
    PlayerInput playerInput;
    Rigidbody rb;

    public GameObject llantaDelantera;
    public GameObject llantaTrasera;
    float radioLlanta;
    float gradosRotacion;

    public Animator animatorDiamante;

    float aceleracion;
    Vector2 giro;
    float inclinacionMoto;
    public float velocidadActual;
    public float velocidadMaxima;
    float fuerzaAceleracion;
    float fuerzaFrenado;
    float haciaAtras;
    float velocidadGiro;

    public AudioSource sonidoMoto;
    public AudioClip relentiSonido;
    public AudioClip aceleracionSonido;

    bool estaAcelerando = false;

    public int vidaPizza;
    float tiempoInmunidad;
    bool esInmune;


    void Start()
    {
        this.transform.rotation = Quaternion.Euler(0, -90, 0);

        playerInput = this.GetComponent<PlayerInput>();
        rb = this.GetComponent<Rigidbody>();
        sonidoMoto = this.GetComponent<AudioSource>();

        giro = Vector2.zero;
        inclinacionMoto = 0;
        aceleracion = 0;
        velocidadActual = 0;
        velocidadMaxima = 20;
        fuerzaAceleracion = 40;
        fuerzaFrenado = 18;
        haciaAtras = 8;
        velocidadGiro = 3;

        sonidoMoto.clip = relentiSonido;
        sonidoMoto.loop = true;
        sonidoMoto.Play();

        radioLlanta = 0.75f;
        gradosRotacion = 0;

        vidaPizza = 3;
        tiempoInmunidad = 2f;
        esInmune = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        aceleracion = playerInput.actions["Acelerar"].ReadValue<float>();
        giro = playerInput.actions["Girar"].ReadValue<Vector2>();
        float rotacion = giro.x * velocidadGiro;

        if (aceleracion > 0)
        {
            velocidadActual += fuerzaAceleracion * Time.fixedDeltaTime;
            if (!estaAcelerando)
            {
                estaAcelerando = true;
                sonidoMoto.clip = aceleracionSonido;
                sonidoMoto.loop = true;
                sonidoMoto.Play();
                animatorDiamante.SetInteger("Estado", 1);
            }

        }
        else if (aceleracion < 0)
        {
            velocidadActual -= fuerzaFrenado * Time.fixedDeltaTime;
            animatorDiamante.SetInteger("Estado", 2);
            AnimatorStateInfo stateInfo = animatorDiamante.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("Estado"))
            {
                animatorDiamante.SetInteger("Estado", 3);
            }
        }
        else
        {
            velocidadActual = Mathf.MoveTowards(velocidadActual, 0, fuerzaFrenado * Time.fixedDeltaTime);
            if (estaAcelerando)
            {
                estaAcelerando = false;
                sonidoMoto.clip = relentiSonido;
                sonidoMoto.loop = true;
                sonidoMoto.Play();
                animatorDiamante.SetInteger("Estado", 0);
            }
        }



        velocidadActual = Mathf.Clamp(velocidadActual, -fuerzaFrenado * 1.5f, velocidadMaxima);
        rb.AddForce(transform.forward * velocidadActual);
        rb.MoveRotation(rb.rotation * Quaternion.Euler(0, rotacion, 0));

        if (giro.x != 0)
        {
            inclinacionMoto += giro.x * -100 * Time.fixedDeltaTime;
            if (inclinacionMoto > 20)
            {
                inclinacionMoto = 20;
                animatorDiamante.SetInteger("Estado", 4);
            }
            if (inclinacionMoto < -20)
            {
                inclinacionMoto = -20;
                animatorDiamante.SetInteger("Estado", 5);
            }

        }
        else
        {
            inclinacionMoto = Mathf.MoveTowards(inclinacionMoto, 0, 100 * Time.fixedDeltaTime);
            animatorDiamante.SetInteger("Estado", 1);
        }
        modeloMoto.transform.localRotation = Quaternion.Euler(0, 0, inclinacionMoto);

        float distanciaRecorrida = (velocidadActual / 3) * Time.fixedDeltaTime;
        float grados = (distanciaRecorrida / radioLlanta) * Mathf.Rad2Deg;
        gradosRotacion += grados;

        llantaDelantera.transform.localRotation = Quaternion.Euler(0, 0, gradosRotacion);
        llantaTrasera.transform.localRotation = Quaternion.Euler(0, 0, gradosRotacion);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (esInmune) return;
        //esInmune = true;
        if (collision == null) return;
        if (collision.gameObject.CompareTag("Obstaculo"))
        {
            vidaPizza--;
            if (vidaPizza <= 0)
            {
                PerderPedido();
            }
            else
            {
                StartCoroutine(ActivarInmunidad());
            }
        }

    }

    private void PerderPedido()
    {
        Debug.Log("Perdiste el pedido");
    }

    IEnumerator ActivarInmunidad()
    {
        //esInmune = true;
        float tiempoPasado = 0;
        while (tiempoPasado < tiempoInmunidad)
        {
            modeloMoto.SetActive(!modeloMoto.activeSelf);
            yield return new WaitForSeconds(0.1f);
            tiempoPasado += 0.1f;
        }

        this.gameObject.SetActive(true);
        esInmune = false;
    }
}