using UnityEngine;

public class DesaparecerDialogos : IState
{
    private readonly ManejadorDialogos dialogos;
    private readonly StateMachine fsm;
    private float contador;
    public DesaparecerDialogos(ManejadorDialogos dialogos, StateMachine fsm)
    {
        this.dialogos = dialogos;
        this.fsm = fsm;
    }
    public void Enter()
    {
        dialogos.MandarCerrarDialogos();
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
