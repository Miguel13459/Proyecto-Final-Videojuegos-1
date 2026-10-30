using UnityEngine;

public class AparecenDialogos : IState
{
    private readonly ManejadorDialogos dialogos;
    private readonly StateMachine fsm;
    private float contador;
    private bool tocoObjeto;
    public AparecenDialogos(ManejadorDialogos dialogos, StateMachine fsm)
    {
        this.dialogos = dialogos;
        this.fsm = fsm;
    }
    public void Enter()
    {
        contador = 0f;
        tocoObjeto = false;
        Jugador.OnPlayerCollided += HacerAparecerDialogosJackson;
        
    }
    public void Execute()
    {
        if (tocoObjeto)
        {
            contador += Time.deltaTime;
            if (contador > 5f) fsm.ChangeState(new DesaparecerDialogos(dialogos, fsm));
        }
    }

    public void Exit()
    {
        //throw new System.NotImplementedException();
    }

    private void HacerAparecerDialogosJackson(GameObject objeto)
    {
        if (objeto.CompareTag("RecogerMision"))
        {
            tocoObjeto = true;
            int dialogoIndex;
            dialogoIndex = Random.Range(0, dialogos.dialogosJackson.Count);
            dialogos.MandarInfoDialogoJackson(dialogoIndex);
        }
        if (objeto.CompareTag("EntregarMision"))
        {
            tocoObjeto = true;
            int dialogoIndex;
            dialogoIndex = Random.Range(0, dialogos.dialogoClientes.Count);
            dialogos.MandarInfoDialogosClientes(dialogoIndex);
        }
    }
}
