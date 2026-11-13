using UnityEngine;

public class MisionCumplida : IState
{
    private readonly ManejadorMisiones mision;
    private readonly StateMachine fsm;
    private float contador;
    private Jugador jugador;

    public MisionCumplida(ManejadorMisiones mision, StateMachine fsm)
    {
        this.mision = mision;
        this.fsm = fsm;
    }

    public MisionCumplida(ManejadorMisiones mision, StateMachine fsm, Jugador jugador)
    {
        this.mision = mision;
        this.fsm = fsm;
        this.jugador = jugador;
    }

    public void Enter()
    {
        mision.EsconderCliente();
        mision.MandarCerrarVida();
        ManejadorPartida.pizzasEntregadas++;
        contador = 0;
    }

    public void Execute()
    {
        mision.StatusMision("cumplida");
        contador += Time.deltaTime;
        if (contador >= 10) fsm.ChangeState(new EsperandoPedidoState(mision, fsm, jugador));
    }

    public void Exit()
    {
        Debug.Log("Saliendo estado cumplir mision");
    }
}
