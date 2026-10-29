using UnityEngine;

public class RetrocederState : IState
{
    private readonly Jugador jugador;
    private readonly StateMachine fsm;
    private float limiteVelocidadAtras;

    public RetrocederState(Jugador jugador, StateMachine fsm)
    {
        this.jugador = jugador;
        this.fsm = fsm;
        limiteVelocidadAtras = -8f;
    }
    public void Enter()
    {
        jugador.animatorDiamante.SetInteger("Estado", 2);
    }

    public void Execute()
    {
        //Debug.Log("Estas en retroceder");
        jugador.animatorDiamante.SetInteger("Estado", 2);
        jugador.velocidadActual -= jugador.fuerzaFrenado * Time.deltaTime;
        if (jugador.velocidadActual < limiteVelocidadAtras) jugador.velocidadActual = -8f;
        
        jugador.rb.AddForce(jugador.transform.forward * jugador.velocidadActual);

        jugador.Girar(0.5f, 5, 50);

        if (jugador.aceleracion >= -0.1f) fsm.ChangeState(new IdleState(jugador, fsm));
    }

    public void Exit()
    {
        //Debug.Log("Saliendo de retroceder");
    }
}
