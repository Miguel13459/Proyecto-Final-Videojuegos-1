using UnityEngine;

public class EsperandoPedidoState : IState
{
    private readonly ManejadorMisiones mision;
    private readonly StateMachine fsm;
    private readonly Jugador jugador;
    private bool estaEnEspera;

    //GameObject jackson;
    public EsperandoPedidoState(ManejadorMisiones mision, StateMachine fsm, Jugador jugador)
    {
        this.mision = mision;
        this.fsm = fsm;
        this.jugador = jugador;
        estaEnEspera = true;
    }

    public EsperandoPedidoState(ManejadorMisiones mision, StateMachine fsm)
    {
        this.mision = mision;
        this.fsm = fsm;
        estaEnEspera = true;
    }

    public void Enter()
    {
        //Debug.Log("Entrando a EsperandoPedidoState");
        mision.AvisarSiEstaEnEspera(estaEnEspera);
        jugador.vidaPizza = 3;
        jugador.PedidoPerdido = false;
        mision.SeleccionarMision();
        mision.AparecerAJackson();
        jugador.OnMisionColisionar += CambiarAMisionEnCurso;
    }

    public void Execute()
    {
        mision.StatusMision("esperando", "");
        //Debug.Log("Ejecutando EsperandoPedidoState");
    }

    public void Exit()
    {
        jugador.OnMisionColisionar -= CambiarAMisionEnCurso;
    }

    public void CambiarAMisionEnCurso(GameObject objeto)
    {
        //jugador.Hablar(0);
        if (objeto.CompareTag("RecogerMision"))
        {
            fsm.ChangeState(new MisionEnCursoState(mision, fsm, jugador));
        }
    }
}
