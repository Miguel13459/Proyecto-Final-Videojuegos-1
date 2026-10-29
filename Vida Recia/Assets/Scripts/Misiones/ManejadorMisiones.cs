using System.Collections.Generic;
using UnityEngine;

public class ManejadorMisiones : MonoBehaviour
{
    public TextAsset archivoMisiones;
    public List<MissionData> misiones = new();

    public GameObject objetoRecogerMision;
    public GameObject objetoEntregarMision;

    public int misionIndex;

    StateMachine fsm;

    // Los puse aca mejor
    void Awake()
    {
        Debug.Log("Iniciando ManejadorMisiones");
        CargarMisiones();
        fsm = new StateMachine();
        fsm.Initialize(new EsperandoPedidoState(this, fsm));
        objetoRecogerMision.transform.position = new Vector3(-2.5f, 0, 3.5f);
        objetoEntregarMision.transform.position = Vector3.zero;
        objetoEntregarMision.SetActive(false);
        
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
        objetoEntregarMision.transform.position = new Vector3(x,0,z);
        objetoEntregarMision.SetActive(true);

    }

    public void EsconderCliente()
    {
        objetoEntregarMision.SetActive(false);
    }
}
