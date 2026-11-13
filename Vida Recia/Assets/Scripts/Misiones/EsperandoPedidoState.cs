using UnityEngine;

public class EsperandoPedidoState : IState
{
    private readonly ManejadorMisiones mision;
    private readonly StateMachine fsm;
    private readonly Jugador jugador;

    //GameObject jackson;
    public EsperandoPedidoState(ManejadorMisiones mision, StateMachine fsm, Jugador jugador)
    {
        this.mision = mision;
        this.fsm = fsm;
        this.jugador = jugador;
    }

    public EsperandoPedidoState(ManejadorMisiones mision, StateMachine fsm)
    {
        this.mision = mision;
        this.fsm = fsm;
    }

    public void Enter()
    {
        Debug.Log("Entrando a EsperandoPedidoState");
        mision.AparecerAJackson();
        jugador.OnMisionColisionar += CambiarAMisionEnCurso;
    }

    public void Execute()
    {
        mision.StatusMision("esperando");
        //Debug.Log("Ejecutando EsperandoPedidoState");
    }

    public void Exit()
    {
        jugador.OnMisionColisionar -= CambiarAMisionEnCurso;
    }

    public void CambiarAMisionEnCurso(GameObject objeto)
    {
        if (objeto.CompareTag("RecogerMision"))
        {
            fsm.ChangeState(new MisionEnCursoState(mision, fsm, jugador));
        }
    }
}
