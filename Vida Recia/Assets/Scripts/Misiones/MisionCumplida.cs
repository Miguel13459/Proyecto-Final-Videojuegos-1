using UnityEngine;

public class MisionCumplida : MonoBehaviour, IState
{
    private readonly QuestManager mision;
    private readonly StateMachine fsm;
    private readonly GameObject objeto;
    public MisionCumplida(QuestManager mision, StateMachine fsm, GameObject objeto)
    {
        this.mision = mision;
        this.fsm = fsm;
        this.objeto = objeto;
    }

    public void Enter()
    {
        Destroy(objeto);
    }

    public void Execute()
    {
        throw new System.NotImplementedException();
    }

    public void Exit()
    {
        fsm.ChangeState(new EsperandoPedidoState(mision, fsm));
    }
}
