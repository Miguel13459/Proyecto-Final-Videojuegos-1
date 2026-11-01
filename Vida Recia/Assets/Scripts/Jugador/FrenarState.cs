using System.Collections;
using UnityEngine;

public class FrenarState : IState
{
    private readonly Jugador jugador;
    private readonly StateMachine fsm;
    private float contador;

    public FrenarState (Jugador jugador, StateMachine fsm)
    {
        this.jugador = jugador;
        this.fsm = fsm;
    }

    public void Enter()
    {
        jugador.CambiarSonido(jugador.frenoSonido, false);
        contador = 0;
    }

    public void Execute()
    {
        contador += Time.deltaTime;
        if(contador > 2f)
        {
            jugador.CambiarSonido(jugador.relentiSonido, true);
        }

        if(jugador.acelerar < -0.1f)
        {
            if (jugador.velocidadActual <= 0f)
            {
                fsm.ChangeState(new RetrocederState(jugador, fsm));
                return;
            }
            jugador.velocidadActual -= jugador.desaceleracionKmH * Time.deltaTime;
            /*if (jugador.velocidadActual > 0f)
            {
                jugador.velocidadActual = Mathf.MoveTowards(
                    jugador.velocidadActual,
                    0f,
                    jugador.desaceleracionKmH * Time.deltaTime
                );
                
            }
            else
            {
                jugador.velocidadActual = 0f;
                fsm.ChangeState(new RetrocederState(jugador, fsm));
            }*/
        }
        else
        {
            fsm.ChangeState(new IdleState(jugador, fsm));
        }
    }

    public void Exit()
    {
        contador = 0f;
        //Debug.Log("Saliendo de frenar");
    }
}
