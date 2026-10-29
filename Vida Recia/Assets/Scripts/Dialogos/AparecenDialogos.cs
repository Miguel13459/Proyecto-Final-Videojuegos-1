using UnityEngine;

public class AparecenDialogos : IState
{
    private readonly ManejadorDialogos dialogos;
    private readonly StateMachine fsm;
    public AparecenDialogos(ManejadorDialogos dialogos, StateMachine fsm)
    {
        this.dialogos = dialogos;
        this.fsm = fsm;
    }
    public void Enter()
    {
        int dialogoIndex;
        dialogoIndex = Random.Range(0, dialogos.dialogosJackson.Count);
        dialogos.HacerAparecerDialogosJackson(dialogoIndex);
    }

    public void Execute()
    {
        //throw new System.NotImplementedException();
    }

    public void Exit()
    {
        //throw new System.NotImplementedException();
    }
}
