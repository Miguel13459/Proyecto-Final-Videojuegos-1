using System.Collections.Generic;
using UnityEngine;

public class ManejadorMisiones : MonoBehaviour
{
    public TextAsset archivoMisiones;
    public List<MissionData> misiones = new();

    public GameObject objetoRecogerMision;
    public GameObject objetoEntregarMision;
    public ManejadorUI manejadorUI;
    public ManejadorDialogos manejadorDialogos;
    Jugador jugador;

    public int misionIndex;

    private bool primeraVezRecogiendo;

    StateMachine fsm;

    string titulo;
    string descripcion;

    // Los puse aca mejor
    void Awake()
    {
        CargarMisiones();
        objetoEntregarMision.transform.position = Vector3.zero;
        objetoEntregarMision.SetActive(false);
        primeraVezRecogiendo = true;
        jugador = GameObject.FindGameObjectWithTag("Player").GetComponent<Jugador>();

        titulo = "";
        descripcion = "";


        fsm = new StateMachine();
        fsm.Initialize(new EsperandoPedidoState(this, fsm, jugador));
    }

    // Update is called once per frame
    void Update()
    {
        fsm.Update();
    }

    public void CargarMisiones()
    {
        if (archivoMisiones == null)
        {
            return;
        }

        MissionList datos = JsonUtility.FromJson<MissionList>(archivoMisiones.text);

        if (datos == null || datos.misiones == null)
        {
            return;
        }

        misiones = new List<MissionData>(datos.misiones);
    }

    public void AparecerAJackson()
    {
        objetoRecogerMision.SetActive(true);
    }

    public void EsconderAJackson()
    {
        objetoRecogerMision.SetActive(false);
    }

    public void SeleccionarMision()
    {
        if (misiones == null || misiones.Count == 0)
        {
            Debug.LogError("No hay misiones cargadas.");
            return;
        }
        misionIndex = Random.Range(0, misiones.Count);

        Debug.Log("Misión seleccionada: " + misiones[misionIndex].lugar);
    }

    public void AparecerCLiente()
    {
        float x = misiones[misionIndex].coordenadas[0];
        float z = misiones[misionIndex].coordenadas[1];
        objetoEntregarMision.transform.position = new Vector3(x, 0, z);
        objetoEntregarMision.SetActive(true);

    }

    public void EsconderCliente()
    {
        objetoEntregarMision.SetActive(false);
    }

    public void StatusMision(string status, string palabraElegida)
    {
        
        switch (status)
        {
            case "esperando":
                titulo = primeraVezRecogiendo ? "TOMA UNA PIZZA" : "REGRESA POR OTRA PIZZA";
                descripcion = primeraVezRecogiendo ? "Dirigete con el jefe Jackson para comenzar, ve al punto amarillo del mapa." : "Dirigete con el jefe Jackson, antes de que se vuelva loco, ve al punto amarillo del mapa.";
                primeraVezRecogiendo = false;
                if (manejadorDialogos.dialogoEstaActivo == false)
                {
                    manejadorUI.CerrarStatus();
                    manejadorUI.MostrarMision(titulo, descripcion);
                }
                break;
            case "enCurso":
                titulo = "ENTREGA LA PIZZA AL CLIENTE";
                descripcion = "Entrega la pizza al cliente. Dirigete al punto verde del mapa";
                if (manejadorDialogos.dialogoEstaActivo == false)
                {
                    manejadorUI.CerrarStatus();
                    manejadorUI.MostrarMision(titulo, descripcion);
                }
                break;
            case "cumplida":
                Debug.Log("Si entra a mision cumplida");
                titulo = "HAS ENTREGADO LA PIZZA";
                descripcion = "Regresa por otra pizza con el " + palabraElegida + " Jackson";
                //StartCoroutine(manejadorUI.MostrarStatusCorrutina(titulo, descripcion, manejadorDialogos));
                manejadorUI.MostrarStatus(titulo, descripcion, manejadorDialogos);
                break;
            case "fallida":
                titulo = "JOLINES LA HAS LIADO";
                descripcion = "Regresa por otra pizza con el " + palabraElegida + " Jackson";
                manejadorUI.MostrarStatus(titulo, descripcion, manejadorDialogos);
                //StartCoroutine(manejadorUI.MostrarStatusCorrutina(titulo, descripcion, manejadorDialogos));
                break;
        }
    }

    public void CerrarCelularMisiones()
    {
        manejadorUI.CerrarMision();
    }

    public void MandarAparecerVida()
    {
        manejadorUI.AparecerVidaPizza();
    }

    public void MandarCerrarVida()
    {
        manejadorUI.CerrarVidaPizza();
    }

    internal void AvisarSiEstaEnEspera(bool estaEnEspera)
    {
        jugador.estaEnEspera = estaEnEspera;
    }
}
