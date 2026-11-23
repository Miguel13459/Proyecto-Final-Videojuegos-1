using System;
using UnityEngine;

public class Chispas : MonoBehaviour
{
    public ParticleSystem chispas;
    ParticleSystem.EmissionModule emisorChispas;
    public bool sacarChispas;
    float contador;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        emisorChispas = chispas.emission;
        chispas.Stop();
        sacarChispas = false;
        contador = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(sacarChispas)
        {
            SacarChispas();
            contador += Time.deltaTime;
            if(contador > 1f)
            {
                DetenerChispas();
                sacarChispas = false;
                contador = 0;
            }
        }
    }
    internal void SacarChispas()
    {
        chispas.Play();
        emisorChispas.rateOverTime = 120;
    }

    internal void DetenerChispas()
    {
        chispas.Stop();
    }
}
