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
    public ManejadorMisiones misiones;
    

    [Header("Gravedad")]
    public float gravedad;

    [Header("Inputs")]
    public float aceleracion;
    public Vector2 giro;

    [Header("Velocidades")]
    public float velocidadActual;
    public float velocidadMaxima;
    public float fuerzaAceleracion;


    [Header("Marcha atras")]
    public float fuerzaFrenado;
    public float haciaAtras;
    

    [Header("Audios")]
    public AudioSource sonidoMoto;
    public AudioClip relentiSonido;
    public AudioClip aceleracionSonido;

    [Header("Direccion")]
    public float inclinacionMoto;
    public float velocidadGiro;

    [Header("Ruedas")]
    public float radioLlanta;
    public float gradosRotacion;

    [Header("Animaciones")]
    public Animator animatorDiamante;

    [Header("Pizza")]
    public bool estaAcelerando = false;
    public int vidaPizza;
    public float tiempoInmunidad;
    public bool esInmune;


    private StateMachine fsm;   

    void Start()
    {
        fsm = new StateMachine();
        fsm.Initialize(new IdleState(this, fsm));

        this.transform.rotation = Quaternion.Euler(0, -90, 0);
        this.transform.position = Vector3.up * 10;

        playerInput = this.GetComponent<PlayerInput>();
        rb = this.GetComponent<Rigidbody>();
        sonidoMoto = this.GetComponent<AudioSource>();

        giro = Vector2.zero;

        gravedad = 20;

        velocidadMaxima = 20;
        fuerzaAceleracion = 40;
        fuerzaFrenado = 18;
        haciaAtras = 8;
        velocidadGiro = 1.2f;

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
    void Update()
    {
        fsm.Update();

        aceleracion = playerInput.actions["Acelerar"].ReadValue<float>();
        giro = playerInput.actions["Girar"].ReadValue<Vector2>();
        

        float distanciaRecorrida = (velocidadActual / 3) * Time.deltaTime;
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

    private void OnTriggerEnter(Collider other)
    {
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
        float rotacion = (giro.x * velocidadGiro) * sensibilidadGiro;
        rb.MoveRotation(rb.rotation * Quaternion.Euler(0, rotacion, 0));
        if (giro.x != 0)
        {
            inclinacionMoto += giro.x * -velocidadInclinacion * Time.deltaTime;
            if (inclinacionMoto > limiteInclinacion)
            {
                inclinacionMoto = limiteInclinacion;
            }
            if (inclinacionMoto < -limiteInclinacion)
            {
                inclinacionMoto = -limiteInclinacion;
            }

        }
        else
        {
            inclinacionMoto = Mathf.MoveTowards(inclinacionMoto, 0, velocidadInclinacion * Time.deltaTime);
        }
        modeloMoto.transform.localRotation = Quaternion.Euler(0, 0, inclinacionMoto);
    }
}