using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Jugador : MonoBehaviour
{
    //public UnityEvent OnRecogerPizza;
    //public UnityEvent OnEntregarPizza;
    public event Action<GameObject> OnMisionColisionar;

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
    public AudioClip encendido;


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
    Vector3 velocidadVertical;
    Vector3 velocidadHorizontal;
    public float fuerzaSalto;
    public float distanciaSensor;
    private bool puedeSaltar;

    [Header("Manejadores")]
    public ManejadorUI manejadorUI;
    public ManejadorMisiones manejadorMisiones;
    public ManejadorDialogos manejadorDialogos;

    private StateMachine fsm;
    public bool arranqueEnCurso;

    public bool PedidoPerdido;

    void Awake()
    {
        sonidoMoto = GetComponent<AudioSource>();
        arranqueEnCurso = true;
    }

    void Start()
    {
        playerInput = this.GetComponent<PlayerInput>();
        rb = this.GetComponent<Rigidbody>();
        //sonidoMoto = this.GetComponent<AudioSource>();

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

        velocidadVertical = Vector3.zero;
        velocidadHorizontal = Vector3.zero;
        fuerzaSalto = 1.5f;
        distanciaSensor = 0.6f;
        puedeSaltar = true;

        PedidoPerdido = false;
        //arranqueEnCurso = true;
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
        DetectarBanqueta();

        //hacer girar las llantas
        float distanciaRecorrida = (velocidadActual / 3.6f) * Time.fixedDeltaTime;
        float grados = (distanciaRecorrida / radioLlanta) * Mathf.Rad2Deg;
        gradosRotacion += grados;

        llantaDelantera.transform.localRotation = Quaternion.Euler(0, 0, gradosRotacion);
        llantaTrasera.transform.localRotation = Quaternion.Euler(0, 0, gradosRotacion);

        velocidadHorizontal = transform.forward * (velocidadActual / 3.6f);
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
    }

    internal void Acelerar()
    {
        velocidadActual += aceleracionKmH * Time.fixedDeltaTime;
        velocidadActual = Mathf.Clamp(velocidadActual, 0f, velocidadMaxima);
    }

    internal void Retroceder()
    {
        float limiteVelocidadAtras = -8f;
        velocidadActual -= desaceleracionKmH * Time.fixedDeltaTime;
        if (velocidadActual < limiteVelocidadAtras) velocidadActual = limiteVelocidadAtras;
    }

    internal void Idle()
    {
        velocidadActual = Mathf.MoveTowards(velocidadActual, 0f, desaceleracionKmH * Time.fixedDeltaTime);
    }

    internal void Frenar()
    {
        velocidadActual = Mathf.MoveTowards(
            velocidadActual,
            0f,
            desaceleracionKmH * 2f * Time.fixedDeltaTime
        );
    }

    internal void Desacelerar()
    {
        velocidadActual = Mathf.MoveTowards(
            velocidadActual,
            0f,
            desaceleracionKmH * Time.fixedDeltaTime
        );
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

    public void DetectarBanqueta()
    {
        RaycastHit hit;
        //subir banquetas
        Debug.DrawRay(sensorFrente.transform.position, this.transform.TransformDirection(Vector3.forward) * distanciaSensor, Color.red);
        if (Physics.Raycast(sensorFrente.transform.position, this.transform.TransformDirection(Vector3.forward),out hit, distanciaSensor))
        {
            Debug.Log("Objeto detectado: " + hit.collider.gameObject.name);
            Debug.Log("Etiqueta detectada: " + hit.collider.gameObject.tag);
            if (hit.collider.gameObject.CompareTag("Calles"))
            {
                Debug.Log("BanquetaDetectada");
                velocidadVertical.y = fuerzaSalto;
                rb.linearVelocity = velocidadVertical;
                puedeSaltar = false;
                Debug.Log("Puede saltar?" + puedeSaltar);
            }
        }
    }

    public void CambiarSonido(AudioClip nuevoSonido, bool loop)
    {
        if (arranqueEnCurso && nuevoSonido != encendido)
            return;

        if (sonidoMoto.clip == nuevoSonido && sonidoMoto.isPlaying)
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

    private void PerderPedido()
    {
        PedidoPerdido = true;
    }

    

    /*private void OnCollisionEnter(Collision collision)
    {
        if (esInmune) return;
        //esInmune = true;
        if (collision == null) return;
    }*/

    private void OnTriggerEnter(Collider other)
    {
        if (other == null) return;
        if (other.gameObject.CompareTag("Obstaculo"))
        {
            PerderVidaPizza();
        }
        if (other.gameObject.CompareTag("RecogerMision"))
        {
            //OnRecogerPizza?.Invoke();
            OnMisionColisionar?.Invoke(other.gameObject);
        }
        if (other.gameObject.CompareTag("EntregarMision"))
        {
            //OnEntregarPizza?.Invoke();
            OnMisionColisionar?.Invoke(other.gameObject);
        }
    }

    private void PerderVidaPizza()
    {
        if (esInmune) return;

        vidaPizza--;

        if (vidaPizza <= 0)
        {
            vidaPizza = 0;
            PerderPedido();
            return;
        }

        StartCoroutine(ActivarInmunidad());
    }

    IEnumerator ActivarInmunidad()
    {
        esInmune = true;

        float tiempoPasado = 0f;

        while (tiempoPasado < tiempoInmunidad)
        {
            modeloMoto.SetActive(!modeloMoto.activeSelf);

            yield return new WaitForSeconds(0.1f);

            tiempoPasado += 0.1f;
        }

        modeloMoto.SetActive(true);
        esInmune = false;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Suelo"))
        {
            puedeSaltar = true;
            Debug.Log("Puede saltar?" + puedeSaltar);
        }
    }
    /*
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void ReiniciarEventos()
    {
        OnPlayerCollided = null;
    }*/

    
}