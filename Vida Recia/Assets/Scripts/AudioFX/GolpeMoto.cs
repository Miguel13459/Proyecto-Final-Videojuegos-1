using UnityEngine;

public class GolpeMoto : MonoBehaviour
{
    AudioSource motoGolpeada;
    public AudioClip sonidoMotoGolpeada;

    private void Awake()
    {
        motoGolpeada = GetComponent<AudioSource>();
    }

    public void ReproducirSonidoMotoGolpeada()
    {
        motoGolpeada.PlayOneShot(sonidoMotoGolpeada);
    }
}
