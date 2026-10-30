using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class ManejadorDialogos : MonoBehaviour
{
    public TextAsset dialogosArchivo;
    public List<DialogueData> dialogosJackson = new();
    public List<DialogoClientes> dialogoClientes = new();
    public ManejadorUI manejadorUI;
    public ManejadorMisiones manejadorMisiones;
    StateMachine fsm;

    private void Awake()
    {
        CargarDialogos();
        fsm = new StateMachine();
        fsm.Initialize(new AparecenDialogos(this, fsm));
    }

    private void Update()
    {
        fsm.Update();
    }

    private void CargarDialogos()
    {
        if (dialogosArchivo == null)
        {
            Debug.Log("Falta asignar el JSON de misiones.");
            return;
        }

        DialogosArchivoData datos = JsonUtility.FromJson<DialogosArchivoData>(dialogosArchivo.text);

        if (datos.jefeJackson != null) dialogosJackson = datos.jefeJackson;

        if (datos.clientes != null) dialogoClientes = datos.clientes;

        Debug.Log(
            $"Diálogos de Jackson: {dialogosJackson.Count}\n" +
            $"Diálogos de clientes: {dialogoClientes.Count}"
        );

    }
    
    public void MandarInfoDialogoJackson(int dialogoIndex)
    {
        //Debug.Log(dialogosJackson[dialogoIndex].introduccion);
        
        manejadorUI.MostrarDialogoJackson(dialogosJackson[dialogoIndex], manejadorMisiones.misiones[manejadorMisiones.misionIndex].lugar);

    }

    public void MandarInfoDialogosClientes(int dialogoIndex)
    {
        manejadorUI.MostrarDialogoClientes(dialogoClientes[dialogoIndex], manejadorMisiones.misiones[manejadorMisiones.misionIndex].nombre);
    }

    internal void MandarCerrarDialogos()
    {
        manejadorUI.CerrarDialogos();
    }
}
