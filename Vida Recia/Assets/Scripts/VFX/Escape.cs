using UnityEngine;

public class Escape : MonoBehaviour
{
    public ParticleSystem chispasGrandes;
    ParticleSystem.MainModule mainChispasGrandes;
    ParticleSystem.EmissionModule emisionChispasGrandes;
    public ParticleSystem chispasPequenias;
    ParticleSystem.MainModule mainChispasPequenias;
    ParticleSystem.EmissionModule emisionChispasPequenias;
    public Light luz;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        mainChispasGrandes = chispasGrandes.main;
        mainChispasPequenias = chispasPequenias.main;
        emisionChispasGrandes = chispasGrandes.emission;
        emisionChispasPequenias = chispasPequenias.emission;
        chispasGrandes.Stop();
        chispasPequenias.Stop();
        luz.intensity = 0f;
    }

    public void Acelerarando()
    {
        luz.intensity = 0.7f;
        chispasGrandes.Play();
        chispasPequenias.Play();
        mainChispasGrandes.startSize = Random.Range(0.1f, 0.4f);
        mainChispasPequenias.startSize = Random.Range(0.1f, 0.5f);
        mainChispasGrandes.startLifetime = Random.Range(3.0f, 4.0f);
        mainChispasPequenias.startLifetime = Random.Range(0.5f, 1.5f);
        mainChispasGrandes.simulationSpeed = 10f;
        mainChispasPequenias.simulationSpeed = 10f;
        emisionChispasGrandes.rateOverTime = 60;
        emisionChispasPequenias.rateOverTime = 60;
    }

    public void Retrocediendo()
    {
        luz.intensity = 0.3f;
        chispasGrandes.Play();
        chispasPequenias.Play();
        mainChispasGrandes.startLifetime = Random.Range(1.0f, 2.0f);
        mainChispasPequenias.startLifetime = Random.Range(0.3f, 1.0f);
        mainChispasGrandes.simulationSpeed = 3f;
        mainChispasPequenias.simulationSpeed = 3f;
        emisionChispasGrandes.rateOverTime = 0;
        emisionChispasPequenias.rateOverTime = 0;
    }

    public void ModoIdle()
    {
        luz.intensity = 0.3f;
        chispasGrandes.Play();
        chispasPequenias.Play();
        mainChispasGrandes.startLifetime = Random.Range(1.0f, 2.0f);
        mainChispasPequenias.startLifetime = Random.Range(0.3f, 1.0f);
        mainChispasGrandes.simulationSpeed = 3f;
        mainChispasPequenias.simulationSpeed = 3f;
        emisionChispasGrandes.rateOverTime = 30;
        emisionChispasPequenias.rateOverTime = 30;
    }

    public void Frenando()
    {
        luz.intensity = 0.5f;
        chispasGrandes.Play();
        chispasPequenias.Play();
        mainChispasGrandes.startLifetime = Random.Range(1.0f, 2.0f);
        mainChispasPequenias.startLifetime = Random.Range(0.3f, 1.0f);
        mainChispasGrandes.simulationSpeed = 5f;
        mainChispasPequenias.simulationSpeed = 5f;
        emisionChispasGrandes.rateOverTime = 30;
        emisionChispasPequenias.rateOverTime = 30;
    }

    public void Desacelerando()
    {
        luz.intensity = 0.5f;
        chispasGrandes.Play();
        chispasPequenias.Play();
        mainChispasGrandes.startLifetime = Random.Range(2.0f, 3.0f);
        mainChispasPequenias.startLifetime = Random.Range(0.5f, 1.5f);
        mainChispasGrandes.simulationSpeed = 7f;
        mainChispasPequenias.simulationSpeed = 7f;
        emisionChispasGrandes.rateOverTime = 40;
        emisionChispasPequenias.rateOverTime = 40;
    }
}
