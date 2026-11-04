using UnityEngine;

public class MisionCumplida : IState
{
    private readonly ManejadorMisiones mision;
    private readonly StateMachine fsm;
    public MisionCumplida(ManejadorMisiones mision, StateMachine fsm)
    {
        this.mision = mision;
        this.fsm = fsm;
    }

    public void Enter()
    {
        mision.EsconderCliente();
    }

    public void Execute()
    {
        fsm.ChangeState(new EsperandoPedidoState(mision, fsm));
    }

    public void Exit()
    {
        Debug.Log("Saliendo estado cumplir mision");
        
    }
}
