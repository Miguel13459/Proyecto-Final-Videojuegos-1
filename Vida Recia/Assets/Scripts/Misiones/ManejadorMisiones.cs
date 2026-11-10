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


    public int misionIndex;

    private bool primeraVezRecogiendo;

    StateMachine fsm;

    string titulo;
    string descripcion;

    // Los puse aca mejor
    void Awake()
    {
        Debug.Log("Iniciando ManejadorMisiones");
        CargarMisiones();
        objetoEntregarMision.transform.position = Vector3.zero;
        objetoEntregarMision.SetActive(false);
        primeraVezRecogiendo = true;

        titulo = "";
        descripcion = "";


        fsm = new StateMachine();
        fsm.Initialize(new EsperandoPedidoState(this, fsm));
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
            Debug.Log("Falta asignar el JSON de misiones.");
            return;
        }

        MissionList datos = JsonUtility.FromJson<MissionList>(archivoMisiones.text);

        if (datos == null || datos.misiones == null)
        {
            Debug.Log("El JSON de misiones no tiene el formato esperado.");
            return;
        }

        misiones = new List<MissionData>(datos.misiones);

        Debug.Log("Misiones cargadas: " + misiones.Count);
    }

    public void AparecerAJackson()
    {
        Debug.Log("Activando procolo jackson");
        objetoRecogerMision.SetActive(true);
    }

    public void EsconderAJackson()
    {
        objetoRecogerMision.SetActive(false);
    }

    public void AparecerCLiente()
    {
        misionIndex = Random.Range(4, misiones.Count);
        float x = misiones[misionIndex].coordenadas[0];
        float z = misiones[misionIndex].coordenadas[1];
        objetoEntregarMision.transform.position = new Vector3(x, 0, z);
        objetoEntregarMision.SetActive(true);

    }

    public void EsconderCliente()
    {
        objetoEntregarMision.SetActive(false);
    }

    public void StatusMision(string status)
    {
        Debug.Log("Status solicitado: " + status);
        Debug.Log("¿Hay diálogo activo?: " + manejadorDialogos.dialogoEstaActivo);

        switch (status)
        {
            case "esperando":
                titulo = primeraVezRecogiendo ? "TOMA UNA PIZZA" : "REGRESA POR OTRA PIZZA";
                descripcion = primeraVezRecogiendo ? "Dirigete con el jefe Jackson para comenzar" : "Dirigete con el jefe Jackson, antes de que se vuelva loco";
                if (manejadorDialogos.dialogoEstaActivo == false)
                {
                    manejadorUI.CerrarStatus();
                    manejadorUI.MostrarMision(titulo, descripcion);
                }
                break;
            case "enCurso":
                titulo = "ENTREGA LA PIZZA AL CLIENTE";
                descripcion = "Entrega la pizza al cliente en el punto verde del mapa";
                if (manejadorDialogos.dialogoEstaActivo == false)
                {
                    manejadorUI.CerrarStatus();
                    manejadorUI.MostrarMision(titulo, descripcion);
                }
                break;
            case "cumplida":
                Debug.Log("Si entra a mision cumplida");
                titulo = "HAS ENTREGADO LA PIZZA";
                descripcion = "Regresa por otra pizza con el desquisiado Jackson";
                StartCoroutine(manejadorUI.MostrarStatusCorrutina(titulo, descripcion, manejadorDialogos));
                break;
            case "fallida":
                titulo = "JOLINES LA HAS LIADO";
                descripcion = "Regresa por otra pizza con el desalmado Jackson";
                StartCoroutine(manejadorUI.MostrarStatusCorrutina(titulo, descripcion, manejadorDialogos));
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
}
