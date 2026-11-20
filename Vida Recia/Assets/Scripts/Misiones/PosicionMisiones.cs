using UnityEngine;

public class PosicionMisiones : MonoBehaviour
{
    Jugador jugador;
    public GameObject objetoMision;
    public Material materialSpriteMision;
    public Sprite Ubicacion;
    public Sprite UbicacionPunto;
    Vector3 posicion;
    float radio = 38f;

    private void Start()
    {
        jugador = GameObject.FindGameObjectWithTag("Player")
            .GetComponent<Jugador>();
    }

    void Update()
    {
        Vector3 direccion = objetoMision.transform.position -
                            jugador.transform.position;

        direccion.y = 0;

        if (direccion.magnitude > radio)
        {
            direccion = direccion.normalized * radio;
            materialSpriteMision.mainTexture = UbicacionPunto.texture;
            this.transform.localScale = new Vector3(6,6,6);
        }
        else
        {
            materialSpriteMision.mainTexture = Ubicacion.texture;
            this.transform.localScale = new Vector3(6, 8, 6);
        }

        posicion = jugador.transform.position + direccion;

        transform.position = new Vector3(
            posicion.x,
            transform.position.y,
            posicion.z
        );
    }
}
