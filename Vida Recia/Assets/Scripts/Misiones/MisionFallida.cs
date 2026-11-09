using UnityEngine;

public class MisionFallida : IState
{
    private readonly ManejadorMisiones mision;
    private readonly StateMachine fsm;
    private float contador;
    public MisionFallida(ManejadorMisiones mision, StateMachine fsm)
    {
        this.mision = mision;
        this.fsm = fsm;
        contador = 0;
    }

    public void Enter()
    {
        mision.EsconderCliente();
        mision.MandarCerrarVida();
        

    }

    public void Execute()
    {
        mision.StatusMision("fallida");
        contador += Time.deltaTime;
        if(contador >= 10) fsm.ChangeState(new EsperandoPedidoState(mision, fsm));
    }

    public void Exit()
    {
        Debug.Log("Saliendo estado fallar mision");

    }
}
