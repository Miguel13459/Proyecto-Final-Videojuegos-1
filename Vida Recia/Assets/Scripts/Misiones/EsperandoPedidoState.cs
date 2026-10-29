using UnityEngine;

public class EsperandoPedidoState : MonoBehaviour, IState
{
    private readonly QuestManager mision;
    private readonly StateMachine fsm;
    public EsperandoPedidoState(QuestManager mision, StateMachine fsm)
    {
        this.mision = mision;
        this.fsm = fsm;
    }

    public void Enter()
    {
        Instantiate(mision.objetoRecogerMision, new Vector3(-2.5f, 0, 3.5f), Quaternion.identity);
        Jugador.OnPlayerCollided += HandlePlayerCollision;
    }

    public void Execute()
    {
        
    }

    public void Exit()
    {
        Jugador.OnPlayerCollided -= HandlePlayerCollision;
    }

    private void HandlePlayerCollision(GameObject objeto)
    {
        Debug.Log("jugador choco contra: " + objeto.name);

        fsm.ChangeState(new MisionEnCursoState(mision, fsm, objeto));
    }
}
