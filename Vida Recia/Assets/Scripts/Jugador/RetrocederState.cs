using UnityEngine;

public class RetrocederState : IState
{
    private readonly Jugador jugador;
    private readonly StateMachine fsm;

    public RetrocederState(Jugador jugador, StateMachine fsm)
    {
        this.jugador = jugador;
        this.fsm = fsm;
        
    }
    public void Enter()
    {
        jugador.CambiarSonido(jugador.relentiSonido, true);
    }

    public void Execute()
    {
        if (jugador.acelerar >= -0.1f)
        {
            fsm.ChangeState(new IdleState(jugador, fsm));
            return;
        }

        jugador.Retroceder();
        jugador.Girar(0.5f, 5, 50);
    }

    public void Exit()
    {
        //Debug.Log("Saliendo de retroceder");
    }
}
