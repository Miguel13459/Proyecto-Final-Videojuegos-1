using UnityEngine;

public class MisionFallida : IState
{
    private readonly ManejadorMisiones mision;
    private readonly StateMachine fsm;
    private readonly Jugador jugador;
    private float contador;
    private readonly bool estaEnEspera;
    private string[] palabras = { "desalmado", "desquisiado", "esquizofrénico", "despiadado", "perverso", "sanguinario" };
    int palabraElegida = 0;

    public MisionFallida(ManejadorMisiones mision, StateMachine fsm, Jugador jugador)
    {
        this.mision = mision;
        this.fsm = fsm;
        this.jugador = jugador;
        contador = 0;
        estaEnEspera = true;
    }

    public MisionFallida(ManejadorMisiones mision, StateMachine fsm)
    {
        this.mision = mision;
        this.fsm = fsm;
        contador = 0;
        estaEnEspera = true;
    }

    public void Enter()
    {
        mision.EsconderCliente();
        mision.MandarCerrarVida();
        ManejadorPartida.pizzasPerdidas++;
        palabraElegida = Random.Range(0, palabras.Length);
    }

    public void Execute()
    {
        mision.AvisarSiEstaEnEspera(estaEnEspera);
        if (jugador.PedidoPerdido)
        {
            mision.StatusMision("fallida", palabras[palabraElegida]);
            contador += Time.deltaTime;
            if (contador >= 10) fsm.ChangeState(new EsperandoPedidoState(mision, fsm, jugador));
        }
    }

    public void Exit()
    {
        Debug.Log("Saliendo estado fallar mision");
    }
}
