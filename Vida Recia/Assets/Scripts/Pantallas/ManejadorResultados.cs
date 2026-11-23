using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ManejadorResultados : MonoBehaviour
{
    public GameObject pantallaNegra;

    public TMP_Text pizzasEntregadas;
    public TMP_Text pizzasPerdidas;
    public TMP_Text mayorPizzasEntregadas;
    Image imagen;
    Color color;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        imagen = pantallaNegra.GetComponent<Image>();
        pizzasEntregadas.text = ManejadorPartida.pizzasEntregadas.ToString();
        pizzasPerdidas.text = ManejadorPartida.pizzasPerdidas.ToString();
        mayorPizzasEntregadas.text = PlayerPrefs.GetInt("mayorPizzasEntregadas", 0).ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if(imagen != null)
        {
            color = imagen.color;
            color.a -= Time.deltaTime;
            if (color.a <= 0) color.a = 0;
            imagen.color = color;
        }
    }


    public void Jugar()
    {
        SceneManager.LoadScene("Juego");
    }

    public void Salir()
    {
        SceneManager.LoadScene("MenuInicio");
    }
}
