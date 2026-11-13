using UnityEditor;
using UnityEngine;

public class MisionEnCursoState : IState
{
    private readonly ManejadorMisiones mision;
    private readonly StateMachine fsm;
    private readonly Jugador jugador;
    public MisionEnCursoState(ManejadorMisiones mision, StateMachine fsm, Jugador jugador)
    {
        this.mision = mision;
        this.fsm = fsm;
        this.jugador = jugador;
    }

    public MisionEnCursoState(ManejadorMisiones mision, StateMachine fsm)
    {
        this.mision = mision;
        this.fsm = fsm;
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
        mision.StatusMision("enCurso");
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
