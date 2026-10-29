using System.Collections;
using UnityEngine;

public class FrenarState : IState
{
    private readonly Jugador jugador;
    private readonly StateMachine fsm;

    public FrenarState (Jugador jugador, StateMachine fsm)
    {
        this.jugador = jugador;
        this.fsm = fsm;
    }

    public void Enter()
    {
        
    }

    public void Execute()
    {
        if(jugador.aceleracion < -0.1f)
        {
            if(jugador.velocidadActual <= 0f)
            {
                fsm.ChangeState(new RetrocederState(jugador, fsm));
                return;
            }
            jugador.velocidadActual -= jugador.fuerzaFrenado * Time.deltaTime;
        }
        else
        {
            fsm.ChangeState(new IdleState(jugador, fsm));
        }
    }

    public void Exit()
    {
        //Debug.Log("Saliendo de frenar");
    }
}
