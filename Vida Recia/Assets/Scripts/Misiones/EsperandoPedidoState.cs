using UnityEngine;

public class EsperandoPedidoState : IState
{
    private readonly ManejadorMisiones mision;
    private readonly StateMachine fsm;
    //GameObject jackson;
    public EsperandoPedidoState(ManejadorMisiones mision, StateMachine fsm)
    {
        this.mision = mision;
        this.fsm = fsm;
    }

    public void Enter()
    {
        Debug.Log("Entrando a EsperandoPedidoState");
        mision.AparecerAJackson();
        Jugador.OnPlayerCollided += HandlePlayerCollision;
    }

    public void Execute()
    {
        Debug.Log("Ejecutando EsperandoPedidoState");
    }

    public void Exit()
    {
        Jugador.OnPlayerCollided -= HandlePlayerCollision;
    }

    private void HandlePlayerCollision(GameObject objeto)
    {
        Debug.Log("jugador choco contra: " + objeto.name);

        fsm.ChangeState(new MisionEnCursoState(mision, fsm));
    }
}
