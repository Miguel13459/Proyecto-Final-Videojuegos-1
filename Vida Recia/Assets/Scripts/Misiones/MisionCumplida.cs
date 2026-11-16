using UnityEngine;

public class MisionCumplida : IState
{
    private readonly ManejadorMisiones mision;
    private readonly StateMachine fsm;
    private float contador;
    private Jugador jugador;
    private bool estaEnEspera;

    public MisionCumplida(ManejadorMisiones mision, StateMachine fsm)
    {
        this.mision = mision;
        this.fsm = fsm;
        estaEnEspera = true;
    }

    public MisionCumplida(ManejadorMisiones mision, StateMachine fsm, Jugador jugador)
    {
        this.mision = mision;
        this.fsm = fsm;
        this.jugador = jugador;
        estaEnEspera = true;
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
        mision.AvisarSiEstaEnEspera(estaEnEspera);
        mision.StatusMision("cumplida");
        contador += Time.deltaTime;
        if (contador >= 10) fsm.ChangeState(new EsperandoPedidoState(mision, fsm, jugador));
    }

    public void Exit()
    {
        Debug.Log("Saliendo estado cumplir mision");
    }
}
