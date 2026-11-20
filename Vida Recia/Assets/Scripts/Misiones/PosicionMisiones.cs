using UnityEngine;

public class PosicionMisiones : MonoBehaviour
{
    Jugador jugador;
    public GameObject objetoEntregar;
    Vector3 posicion;
    Vector2 limitesPositivos = new Vector2(38f, 38f);
    Vector2 limitesNegativos = new Vector2(-38f, -38f);

    private void Start()
    {
        jugador = GameObject.FindGameObjectWithTag("Player").GetComponent<Jugador>();
    }

    void Update()
    {
        if (posicion.x >= limitesPositivos.x) posicion.x = limitesPositivos.x;
        else if (posicion.x <= limitesNegativos.x) posicion.x = limitesNegativos.x;

        if (posicion.z >= limitesPositivos.y) posicion.z = limitesPositivos.y;
        else if (posicion.z <= limitesNegativos.y) posicion.z = limitesNegativos.y;

        posicion = new Vector3(objetoEntregar.transform.position.x, this.transform.position.y, objetoEntregar.transform.position.z);
        this.transform.position = posicion;
    }
}
