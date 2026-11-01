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
        jugador.animatorDiamante.SetInteger("Estado", 1);
        jugador.CambiarSonido(jugador.aceleracionSonido, false);
    }

    public void Execute()
    {
        //Debug.Log("Estas acelerando");
        //metes frenos
        if (jugador.acelerar < -0.1f)
        {
            fsm.ChangeState(new FrenarState(jugador, fsm));
            return;
        }

        //estas acelerando
        if (jugador.acelerar > 0.1f)
        {
            jugador.velocidadActual += jugador.aceleracionKmH * Time.deltaTime;
            if(jugador.velocidadActual >= jugador.velocidadMaxima)
            {
                jugador.CambiarSonido(jugador.aceleracionConstanteSonido, true);
            }
        }
        else
        {
            //jugador.velocidadActual -= jugador.desaceleracionKmH * Time.deltaTime;
            jugador.CambiarSonido(jugador.desaceleracionSonido, false);
            jugador.velocidadActual = Mathf.MoveTowards(
                jugador.velocidadActual,
                0f,
                jugador.desaceleracionKmH * Time.deltaTime
            );
            //jugador.sonidoMoto.clip = jugador.desaceleracion;
            //jugador.sonidoMoto.Play();
        }
        jugador.velocidadActual = Mathf.Clamp(jugador.velocidadActual,0f,jugador.velocidadMaxima);
        jugador.rb.AddForce(jugador.transform.forward * jugador.velocidadActual);
        jugador.animatorDiamante.SetInteger("Estado", 1);

        jugador.Girar(1f, 20, 100);
        if(jugador.giro.x > 0.1f) jugador.animatorDiamante.SetInteger("Estado", 4);
        else if (jugador.giro.x < -0.1f) jugador.animatorDiamante.SetInteger("Estado", 5);
        else jugador.animatorDiamante.SetInteger("Estado", 1);

        if (jugador.velocidadActual <= 0.01f)
        {
            jugador.velocidadActual = 0f;
            fsm.ChangeState(new IdleState(jugador, fsm));
        }
    }

    public void Exit()
    {
        //Debug.Log("Saliendo de Acelerar");
    }
}
