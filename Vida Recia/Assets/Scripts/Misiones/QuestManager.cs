using UnityEngine;
using System.Collections.Generic;
using System;

public class QuestManager : MonoBehaviour
{
    public TextAsset archivoMisiones;
    public List<MissionData> misiones = new List<MissionData>();
    StateMachine fsm;

    public GameObject objetoRecogerMision;
    public GameObject objetoEntregarMision;

    public void Awake()
    {
        CargarMisiones();
        fsm = new StateMachine();
        fsm.Initialize(new EsperandoPedidoState(this,fsm));
    }

    private void Update()
    {
        fsm.Update();
    }

    private void CargarMisiones()
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

}