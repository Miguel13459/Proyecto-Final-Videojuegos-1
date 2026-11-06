using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Jugador : MonoBehaviour
{
    public static event Action<GameObject> OnPlayerCollided;

    [Header("Referencias")]
    public GameObject modeloMoto;
    PlayerInput playerInput;
    public Rigidbody rb;
    public GameObject llantaDelantera;
    public GameObject llantaTrasera;

    [Header("Inputs")]
    public float acelerar;
    public Vector2 giro;

    [Header("Velocidades")]
    public float velocidadActual;
    public float velocidadMaxima;
    public float aceleracionKmH;
    public float desaceleracionKmH;


    [Header("Audios")]
    public AudioSource sonidoMoto;
    public AudioClip relentiSonido;
    public AudioClip aceleracionSonido;
    public AudioClip aceleracionConstanteSonido;
    public AudioClip desaceleracionSonido;
    public AudioClip frenoSonido;


    [Header("Direccion")]
    public float inclinacionMoto;
    public float velocidadGiro;

    [Header("Ruedas")]
    public float radioLlanta;
    public float gradosRotacion;

    [Header("Animaciones")]
    public Animator animatorDiamante;

    [Header("Pizza")]
    public int vidaPizza;
    public float tiempoInmunidad;
    public bool esInmune;

    [Header("Sensores")]
    public GameObject sensorFrente;
    public GameObject sensorAtras;
    float alturaMaxima;
    Vector3 inclinacionActualFrente;
    Vector3 angulosSubir;


    private StateMachine fsm;   

    void Start()
    {
        this.transform.rotation = Quaternion.Euler(0, -90, 0);
        this.transform.position = Vector3.up * 10;

        playerInput = this.GetComponent<PlayerInput>();
        rb = this.GetComponent<Rigidbody>();
        sonidoMoto = this.GetComponent<AudioSource>();

        giro = Vector2.zero;

        velocidadActual = 0;
        velocidadMaxima = 100f * 2;
        aceleracionKmH = 16.67f * 2;
        desaceleracionKmH = 25f * 2;

        velocidadGiro = 120f;

        radioLlanta = 0.75f;
        gradosRotacion = 0;

        vidaPizza = 3;
        tiempoInmunidad = 2f;
        esInmune = false;

        alturaMaxima = 20f;
        angulosSubir = Vector3.zero;

        sonidoMoto.clip = relentiSonido;
        sonidoMoto.loop = true;
        sonidoMoto.Play();

        fsm = new StateMachine();
        fsm.Initialize(new IdleState(this, fsm));
    }

    // Update is called once per frame
    void Update()
    {
        LeerControles();
    }

    void FixedUpdate()
    {
        fsm.Update();
        //Subir();

        //hacer girar las llantas
        float distanciaRecorrida = (velocidadActual / 3.6f) * Time.fixedDeltaTime;
        float grados = (distanciaRecorrida / radioLlanta) * Mathf.Rad2Deg;
        gradosRotacion += grados;

        llantaDelantera.transform.localRotation = Quaternion.Euler(0, 0, gradosRotacion);
        llantaTrasera.transform.localRotation = Quaternion.Euler(0, 0, gradosRotacion);

        //subir banquetas
        /*Debug.DrawRay(sensorFrente.transform.position, this.transform.TransformDirection(Vector3.forward) * 0.4f, Color.red);
        if (Physics.Raycast(sensorFrente.transform.position, this.transform.TransformDirection(Vector3.forward), 0.4f))
        {
            Debug.Log("Estoy pegandole a algo");
            //rb.AddForce(transform.up * velocidadActual);
            //angulosSubir.z -= Time.deltaTime;
        }*/

        Vector3 velocidadVertical = Vector3.up * rb.linearVelocity.y;
        Vector3 velocidadHorizontal = transform.forward * (velocidadActual / 3.6f);
        rb.linearVelocity = new Vector3(
            velocidadHorizontal.x,
            rb.linearVelocity.y,
            velocidadHorizontal.z
        );
    }

    private void LeerControles()
    {
        acelerar = playerInput.actions["Acelerar"].ReadValue<float>();
        giro = playerInput.actions["Girar"].ReadValue<Vector2>();
        if (giro.x != 0)
        {
            Debug.Log("Acelerador: " + acelerar + " | Giro: " + giro.x);
        }
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

    private void OnTriggerEnter(Collider other)
    {
        if (other == null) return;
        if (other.gameObject.CompareTag("Obstaculo"))
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
        if (other.gameObject.CompareTag("RecogerMision"))
        {
            OnPlayerCollided?.Invoke(other.gameObject);
        }
        if (other.gameObject.CompareTag("EntregarMision"))
        {
            OnPlayerCollided?.Invoke(other.gameObject);
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

    public void Girar(float sensibilidadGiro, float limiteInclinacion, float velocidadInclinacion)
    {
        float rotacion = giro.x * velocidadGiro * sensibilidadGiro * Time.fixedDeltaTime;
        rb.MoveRotation(rb.rotation * Quaternion.Euler(0, rotacion, 0));
        if (giro.x != 0)
        {
            inclinacionMoto += giro.x * -velocidadInclinacion * Time.fixedDeltaTime;
            if (inclinacionMoto > limiteInclinacion) inclinacionMoto = limiteInclinacion;
            if (inclinacionMoto < -limiteInclinacion) inclinacionMoto = -limiteInclinacion;
        }
        else
        {
            inclinacionMoto = Mathf.MoveTowards(inclinacionMoto, 0, velocidadInclinacion * Time.fixedDeltaTime);
        }
        modeloMoto.transform.localRotation = Quaternion.Euler(0, 0, inclinacionMoto);
    }

    /*public void Subir()
    {
        
        //this.transform.localRotation = Quaternion.Euler(angulosSubir);
    }*/

    public void CambiarSonido(AudioClip nuevoSonido, bool loop)
    {
        if (sonidoMoto.clip == nuevoSonido)
            return;

        sonidoMoto.clip = nuevoSonido;
        sonidoMoto.loop = loop;
        sonidoMoto.Play();
    }

    internal void ActualizarAnimacion(int estadoNormal)
    {
        if (giro.x > 0.1f) animatorDiamante.SetInteger("Estado", 4);
        else if (giro.x < -0.1f) animatorDiamante.SetInteger("Estado", 5);
        else animatorDiamante.SetInteger("Estado", estadoNormal);
    }
}