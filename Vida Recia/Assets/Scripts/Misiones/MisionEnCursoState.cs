using UnityEditor;
using UnityEngine;

public class MisionEnCursoState : MonoBehaviour, IState
{
    private readonly QuestManager mision;
    private readonly StateMachine fsm;
    private readonly GameObject objetoRecogerMision;
    private int misionIndex;
    public MisionEnCursoState(QuestManager mision, StateMachine fsm, GameObject objetoRecogerMision)
    {
        this.mision = mision;
        this.fsm = fsm;
        this.objetoRecogerMision = objetoRecogerMision;
        misionIndex = -1;
    }

    public void Enter()
    {
        Destroy(objetoRecogerMision);
        misionIndex = Random.Range(0, mision.misiones.Count);
        float x = mision.misiones[misionIndex].coordenadas[0];
        float z = mision.misiones[misionIndex].coordenadas[1];
        Instantiate(
            mision.objetoEntregarMision,
            new Vector3(x, 0, z),
            Quaternion.identity
            );
        Jugador.OnPlayerCollided += HandlePlayerCollision;
    }

    public void Execute()
    {
        //throw new System.NotImplementedException();
    }

    public void Exit()
    {
        Jugador.OnPlayerCollided -= HandlePlayerCollision;
    }

    private void HandlePlayerCollision(GameObject objeto)
    {
        Debug.Log("jugador choco contra: " + objeto.name);
        fsm.ChangeState(new MisionCumplida(mision, fsm, objeto));
    }
}
