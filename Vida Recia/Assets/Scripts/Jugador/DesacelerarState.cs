using UnityEngine;

public class DesacelerarState : IState
{
    private readonly Jugador jugador;
    private readonly StateMachine fsm;

    public DesacelerarState(Jugador jugador, StateMachine fsm)
    {
        this.jugador = jugador;
        this.fsm = fsm;
    }
    public void Enter()
    {
        jugador.CambiarSonido(jugador.desaceleracionSonido, false);
    }

    public void Execute()
    {
        if (jugador.acelerar > 0.1f)
        {
            fsm.ChangeState(new AcelerarState(jugador, fsm));
            return;
        }

        if (jugador.acelerar < -0.1f)
        {
            fsm.ChangeState(new FrenarState(jugador, fsm));
            return;
        }

        if (jugador.velocidadActual <= 0.01f)
        {
            jugador.velocidadActual = 0f;
            fsm.ChangeState(new IdleState(jugador, fsm));
            return;
        }

        jugador.Desacelerar();
        jugador.Girar(1f, 20, 100);
        jugador.ActualizarAnimacion(1);
    }

    public void Exit()
    {
        //Debug.Log("Saliendo de desacelerar");
    }
}
