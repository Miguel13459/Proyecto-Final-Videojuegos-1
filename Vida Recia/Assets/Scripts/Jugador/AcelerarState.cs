using UnityEngine;
using UnityEngine.UIElements;

public class AcelerarState : IState
{
    private readonly Jugador jugador;
    private readonly StateMachine fsm;


    public AcelerarState(Jugador jugador, StateMachine fsm)
    {
        this.jugador = jugador;
        this.fsm = fsm;
    }

    public void Enter()
    {
        //jugador.animatorDiamante.SetInteger("Estado", 1);
        jugador.CambiarSonido(jugador.aceleracionSonido, false);
    }

    public void Execute()
    {
        //metes frenos
        if (jugador.acelerar < -0.1f)
        {
            fsm.ChangeState(new FrenarState(jugador, fsm));
            return;
        }

        if (jugador.acelerar <= 0.1f)
        {
            fsm.ChangeState(new DesacelerarState(jugador, fsm));
            return;
        }

        jugador.Acelerar();
        if (jugador.velocidadActual >= jugador.velocidadMaxima)
        {
            jugador.CambiarSonido(jugador.aceleracionConstanteSonido, true);
        }
        jugador.Girar(1f, 20, 100);
        jugador.ActualizarAnimacion(1);
    }

    public void Exit()
    {
        //Debug.Log("Saliendo de Acelerar");
    }
}
