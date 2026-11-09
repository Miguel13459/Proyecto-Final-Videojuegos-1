using UnityEditor;
using UnityEngine;

public class MisionEnCursoState : IState
{
    private readonly ManejadorMisiones mision;
    private readonly StateMachine fsm;
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
        Jugador.OnPlayerCollided += HandlePlayerCollision;
    }

    public void Execute()
    {
        mision.StatusMision("enCurso");
    }

    public void Exit()
    {
        Jugador.OnPlayerCollided -= HandlePlayerCollision;
    }

    private void HandlePlayerCollision(GameObject objeto)
    {
        Debug.Log("jugador choco contra: " + objeto.name);
        fsm.ChangeState(new MisionCumplida(mision, fsm));
    }
}
