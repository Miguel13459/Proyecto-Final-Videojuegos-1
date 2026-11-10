using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ManejadorResultados : MonoBehaviour
{
    public GameObject pantallaNegra;
    Image imagen;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        imagen = pantallaNegra.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Jugar()
    {
        SceneManager.LoadScene("Juego");
    }

    public void Salir()
    {
        Application.Quit();
    }
}
