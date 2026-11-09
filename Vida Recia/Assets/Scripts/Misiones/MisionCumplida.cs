using UnityEngine;

public class MisionCumplida : IState
{
    private readonly ManejadorMisiones mision;
    private readonly StateMachine fsm;
    private float contador;
    public MisionCumplida(ManejadorMisiones mision, StateMachine fsm)
    {
        this.mision = mision;
        this.fsm = fsm;
    }

    public void Enter()
    {
        mision.EsconderCliente();
        mision.MandarCerrarVida();
        
        contador = 0;

    }

    public void Execute()
    {
        mision.StatusMision("cumplida");
        contador += Time.deltaTime;
        if (contador >= 10) fsm.ChangeState(new EsperandoPedidoState(mision, fsm));
    }

    public void Exit()
    {
        Debug.Log("Saliendo estado cumplir mision");
    }
}
