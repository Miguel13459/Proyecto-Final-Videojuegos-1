using UnityEditor;
using UnityEngine;

public class MisionEnCursoState : IState
{
    private readonly ManejadorMisiones mision;
    private readonly StateMachine fsm;
    private readonly Jugador jugador;
    private bool estaEnEspera;
    public MisionEnCursoState(ManejadorMisiones mision, StateMachine fsm, Jugador jugador)
    {
        this.mision = mision;
        this.fsm = fsm;
        this.jugador = jugador;
        estaEnEspera = false;
    }

    public MisionEnCursoState(ManejadorMisiones mision, StateMachine fsm)
    {
        this.mision = mision;
        this.fsm = fsm;
        estaEnEspera = false;
    }

    public void Enter()
    {
        mision.EsconderAJackson();
        mision.AparecerCLiente();
        mision.MandarAparecerVida();
        jugador.OnMisionColisionar += CambiarAMisionCumplida;
    }

    public void Execute()
    {
        mision.AvisarSiEstaEnEspera(estaEnEspera);
        mision.StatusMision("enCurso", "");
        if(jugador.PedidoPerdido) fsm.ChangeState(new MisionFallida(mision, fsm, jugador));
    }

    public void Exit()
    {
        jugador.OnMisionColisionar -= CambiarAMisionCumplida;
    }

    public void CambiarAMisionCumplida(GameObject objeto)
    {
        if (objeto.CompareTag("EntregarMision"))
        {
            fsm.ChangeState(new MisionCumplida(mision, fsm, jugador));
        }
    }
}
