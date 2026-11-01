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
        jugador.animatorDiamante.SetInteger("Estado", 0);
        if (jugador.sonidoMoto.clip != jugador.relentiSonido)
        {
            jugador.CambiarSonido(jugador.relentiSonido, true);
        }
    }

    public void Execute()
    {
        //Debug.Log("Estas en idle");
        if (jugador.acelerar > 0.2f)
        {
            fsm.ChangeState(new AcelerarState(jugador, fsm));
            return;
        }

        if (jugador.acelerar < -0.2f)
        {
            fsm.ChangeState(new FrenarState(jugador, fsm));
            return;
        }
        jugador.velocidadActual = Mathf.MoveTowards( jugador.velocidadActual, 0f, jugador.desaceleracionKmH * Time.deltaTime );
        //jugador.Inclinarse(5, 50);
        if (jugador.giro.x > 0.1f) jugador.animatorDiamante.SetInteger("Estado", 4);
        else if (jugador.giro.x < -0.1f) jugador.animatorDiamante.SetInteger("Estado", 5);
        else jugador.animatorDiamante.SetInteger("Estado", 0);
    }

    public void Exit()
    {
        //Debug.Log("Saliendo de idle");
    }
}
