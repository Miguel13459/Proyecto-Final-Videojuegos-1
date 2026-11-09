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
        jugador.CambiarSonido(jugador.frenoSonido, false);
    }

    public void Execute()
    {
        if (jugador.acelerar >= -0.1f)
        {
            fsm.ChangeState(new IdleState(jugador, fsm));
            return;
        }

        if (jugador.velocidadActual <= 0f)
        {
            fsm.ChangeState(new RetrocederState(jugador, fsm));
            return;
        }

        /*contador += Time.deltaTime;
        if(contador > 2f)
        {
            jugador.CambiarSonido(jugador.relentiSonido, true);
        }*/

        jugador.velocidadActual = Mathf.MoveTowards(
            jugador.velocidadActual,
            0f,
            jugador.desaceleracionKmH * 2f * Time.fixedDeltaTime
        );
    }

    public void Exit()
    {
        //Debug.Log("Saliendo de frenar");
    }
}
