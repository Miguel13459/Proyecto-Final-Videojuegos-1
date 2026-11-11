using UnityEngine;

public class DesaparecerDialogos : IState
{
    private readonly ManejadorDialogos dialogos;
    private readonly StateMachine fsm;
    private readonly Jugador jugador;
    bool dialogoEstaActivo;
    public DesaparecerDialogos(ManejadorDialogos dialogos, StateMachine fsm, Jugador jugador)
    {
        this.dialogos = dialogos;
        this.fsm = fsm;
        this.jugador = jugador;
        
    }
    public DesaparecerDialogos(ManejadorDialogos dialogos, StateMachine fsm)
    {
        this.dialogos = dialogos;
        this.fsm = fsm;
    }
    public void Enter()
    {
        dialogoEstaActivo = false;
        dialogos.MandarCerrarDialogos(dialogoEstaActivo);
        fsm.ChangeState(new AparecenDialogos(dialogos, fsm));
    }

    public void Execute()
    {

    }

    public void Exit()
    {
        //throw new System.NotImplementedException();
    }
}
