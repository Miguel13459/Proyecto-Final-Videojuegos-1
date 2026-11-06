using UnityEngine;

public class IdleState : IState
{
    private readonly Jugador jugador;
    private readonly StateMachine fsm;
    public IdleState(Jugador jugador, StateMachine fsm)
    {
        this.jugador = jugador;
        this.fsm = fsm;
    }
    public void Enter()
    {
        //jugador.animatorDiamante.SetInteger("Estado", 0);
        jugador.CambiarSonido(jugador.relentiSonido, true);
    }

    public void Execute()
    {
        //Debug.Log("Estas en idle");
        if (jugador.acelerar > 0.1f)
        {
            fsm.ChangeState(new AcelerarState(jugador, fsm));
            return;
        }

        if (jugador.acelerar < -0.1f)
        {
            fsm.ChangeState(new RetrocederState(jugador, fsm));
            return;
        }
        jugador.velocidadActual = Mathf.MoveTowards(jugador.velocidadActual, 0f, jugador.desaceleracionKmH * Time.fixedDeltaTime);
        jugador.ActualizarAnimacion(0);
    }

    public void Exit()
    {
        //Debug.Log("Saliendo de idle");
    }
}
