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
        jugador.OnPlayerCollided += HandlePlayerCollision;
    }

    public void Execute()
    {
        mision.StatusMision("enCurso");
    }

    public void Exit()
    {
        jugador.OnPlayerCollided -= HandlePlayerCollision;
    }

    private void HandlePlayerCollision(GameObject objeto)
    {
        Debug.Log("jugador choco contra: " + objeto.name);
        fsm.ChangeState(new MisionCumplida(mision, fsm));
    }
}
