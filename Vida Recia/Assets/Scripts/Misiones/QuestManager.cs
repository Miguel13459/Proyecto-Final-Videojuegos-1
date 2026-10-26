using UnityEngine;
using System.Collections.Generic;

public enum EstadoMision
{
    EsperandoPedido,
    DialogoJefe,
    Entregando,
    DialogoCliente
}

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;

    [Header("Archivo JSON de misiones")]
    public TextAsset archivoMisiones;

    public EstadoMision estadoActual =
        EstadoMision.EsperandoPedido;

    public MissionData misionActual;

    private List<MissionData> misiones =
        new List<MissionData>();

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        CargarMisiones();
    }

    private void CargarMisiones()
    {
        if (archivoMisiones == null)
        {
            Debug.LogError("Falta asignar el JSON de misiones.");
            return;
        }

        MissionList datos =
            JsonUtility.FromJson<MissionList>(
                archivoMisiones.text
            );

        if (datos == null || datos.misiones == null)
        {
            Debug.LogError("El JSON de misiones no tiene el formato esperado.");
            return;
        }

        misiones = new List<MissionData>(datos.misiones);

        Debug.Log("Misiones cargadas: " + misiones.Count);
    }

    public void AceptarSiguienteMision()
    {
        if (estadoActual != EstadoMision.EsperandoPedido)
            return;

        if (misiones.Count == 0)
        {
            Debug.LogWarning("No hay misiones disponibles.");
            return;
        }

        // Por ahora selecciona la primera misión disponible.
        misionActual = misiones[0];

        estadoActual = EstadoMision.DialogoJefe;

        Debug.Log("Pedido aceptado: " + misionActual.nombre);
        Debug.Log("Destino: " + misionActual.lugar);

        // Aquí iniciaremos el diálogo del jefe.
    }

    public void TerminarDialogoJefe()
    {
        if (estadoActual != EstadoMision.DialogoJefe)
            return;

        estadoActual = EstadoMision.Entregando;

        Debug.Log("Dirígete a: " + misionActual.lugar);
    }

    public void LlegarAlDestino(int misionId)
    {
        if (estadoActual != EstadoMision.Entregando)
            return;

        if (misionActual == null ||
            misionActual.misionId != misionId)
            return;

        estadoActual = EstadoMision.DialogoCliente;

        Debug.Log("Has llegado a: " + misionActual.lugar);

        // Aquí iniciaremos el diálogo del cliente.
    }

    public void TerminarDialogoCliente()
    {
        if (estadoActual != EstadoMision.DialogoCliente)
            return;

        Debug.Log("Misión completada: " + misionActual.nombre);

        misiones.Remove(misionActual);
        misionActual = null;

        estadoActual = EstadoMision.EsperandoPedido;

        Debug.Log("Puedes aceptar el siguiente pedido.");
    }
}