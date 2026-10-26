using UnityEngine;

public class MissionDestination : MonoBehaviour
{
    [Header("ID de la misión que se entrega aquí")]
    public int misionId;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        QuestManager.instance.LlegarAlDestino(misionId);
    }
}
