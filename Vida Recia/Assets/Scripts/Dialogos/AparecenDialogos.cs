using UnityEngine;

public class AparecenDialogos : IState
{
    private readonly ManejadorDialogos dialogos;
    private readonly StateMachine fsm;
    private readonly Jugador jugador;
    private float contador;
    private bool tocoObjeto;
    private bool dialogoEstaActivo;
    public AparecenDialogos(ManejadorDialogos dialogos, StateMachine fsm, Jugador jugador)
    {
        this.dialogos = dialogos;
        this.fsm = fsm;
        this.jugador = jugador;
    }

    public AparecenDialogos(ManejadorDialogos dialogos, StateMachine fsm)
    {
        this.dialogos = dialogos;
        this.fsm = fsm;
    }
    public void Enter()
    {
        contador = 0f;
        tocoObjeto = false;
        dialogoEstaActivo = false;
        Debug.Log("JUGADOOOOOOR: " + jugador);
        jugador.OnPlayerCollided += HacerAparecerDialogos;

    }
    public void Execute()
    {
        if (tocoObjeto)
        {
            contador += Time.deltaTime;
            if (contador > 6.5f)
            {
                fsm.ChangeState(new DesaparecerDialogos(dialogos, fsm));
            }
        }
    }

    public void Exit()
    {
        contador = 0f;
        jugador.OnPlayerCollided -= HacerAparecerDialogos;
    }

    private void HacerAparecerDialogos(GameObject objeto)
    {
        dialogoEstaActivo = true;
        if (objeto.CompareTag("RecogerMision"))
        {
            tocoObjeto = true;
            int dialogoIndex;
            dialogoIndex = Random.Range(0, dialogos.dialogosJackson.Count);
            dialogos.MandarInfoDialogoJackson(dialogoIndex, dialogoEstaActivo);
        }
        if (objeto.CompareTag("EntregarMision"))
        {
            tocoObjeto = true;
            int dialogoIndex;
            dialogoIndex = Random.Range(0, dialogos.dialogoClientes.Count);
            dialogos.MandarInfoDialogosClientes(dialogoIndex, dialogoEstaActivo);
        }
    }
}
