using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ManejadorMenu : MonoBehaviour
{
    public GameObject creditos;
    public GameObject botones;
    public GameObject titulo;

    public Image pantallaNegra;
    public GameObject logoAnimado;
    Animator animator;
    Color color;
    private bool comenzarPartida;
    private bool cargandoPartida;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        comenzarPartida = false;
        cargandoPartida = false;

        animator = logoAnimado.GetComponent<Animator>();

        color = pantallaNegra.color;
        color.a = 0;
        pantallaNegra.color = color;

        logoAnimado.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (comenzarPartida && !cargandoPartida)
        {
            color = pantallaNegra.color;
            color.a += Time.deltaTime;
            color.a = Mathf.Clamp01(color.a);

            pantallaNegra.color = color;

            if (color.a >= 1)
            {
                cargandoPartida = true;
                StartCoroutine(ComenzarJuego());
            }
        }
    }

    public void Jugar()
    {
        Debug.Log("Comenzando");
        comenzarPartida = true;
    }

    IEnumerator ComenzarJuego()
    {
        logoAnimado.SetActive(true);
        animator.SetBool("PartidaIniciada", true);
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("Juego");
    }

    public void Salir()
    {
        Application.Quit();
    }

    public void MostrarCreditos()
    {
        botones.SetActive(false);
        titulo.SetActive(false);
        creditos.SetActive(true);
    }

    public void CerrarCreditos()
    {
        creditos.SetActive(false);
        botones.SetActive(true);
        titulo.SetActive(true);
    }
}
