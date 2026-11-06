using UnityEngine;

public class RetrocederState : IState
{
    private readonly Jugador jugador;
    private readonly StateMachine fsm;
    private readonly float limiteVelocidadAtras;

    public RetrocederState(Jugador jugador, StateMachine fsm)
    {
        this.jugador = jugador;
        this.fsm = fsm;
        limiteVelocidadAtras = -8f;
    }
    public void Enter()
    {
        //jugador.animatorDiamante.SetInteger("Estado", 2);
        jugador.CambiarSonido(jugador.relentiSonido, true);
    }

    public void Execute()
    {
        if (jugador.acelerar >= -0.1f)
        {
            fsm.ChangeState(new IdleState(jugador, fsm));
            return;
        }

        //Debug.Log("Estas en retroceder");
        jugador.velocidadActual -= jugador.desaceleracionKmH * Time.fixedDeltaTime;
        if (jugador.velocidadActual < limiteVelocidadAtras) jugador.velocidadActual = limiteVelocidadAtras;
        
        //jugador.rb.AddForce(jugador.transform.forward * jugador.velocidadActual);
        jugador.Girar(0.5f, 5, 50);
        
    }

    public void Exit()
    {
        //Debug.Log("Saliendo de retroceder");
    }
}
