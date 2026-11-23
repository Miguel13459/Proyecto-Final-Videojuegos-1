using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class SonidoBotones : MonoBehaviour, 
    IPointerClickHandler, 
    IPointerEnterHandler, 
    IPointerExitHandler
{
    public AudioSource fuenteDeAudio;
    public AudioClip sonidoSeleccion;
    public AudioClip sonidoSubmit;

    RectTransform propiedadesBoton;
    float width;
    float height;
    Vector2 escalaInicial;
    Vector2 escalaFinal;
    bool isHovered;
    /*bool isSelected;
    bool primeraSeleccion;*/
    void Awake()
    {
        propiedadesBoton = GetComponent<RectTransform>();
        width = propiedadesBoton.rect.width;
        height = propiedadesBoton.rect.height;
        escalaInicial = new Vector2(width, height);
        escalaFinal = new Vector2(width + 12, height + 8);
        //primeraSeleccion = true;
    }

    void Update()
    {
        if (isHovered)
        {
            EscalarBoton();
        }
        else
        {
            RegresarEscalaBoton();
        }
    }
    /*public void OnSelect(BaseEventData eventData)
    {
        isSelected = true;

        if (primeraSeleccion)
        {
            primeraSeleccion = false;
            return;
        }

        ReproducirSonido(sonidoSeleccion);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        isSelected = false;
    }*/

    public void OnPointerClick(PointerEventData eventData)
    {
        ReproducirSonido(sonidoSubmit);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
        ReproducirSonido(sonidoSeleccion);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
    }
    void ReproducirSonido(AudioClip audio)
    {
        if (fuenteDeAudio != null && sonidoSeleccion != null)
        {
            fuenteDeAudio.PlayOneShot(audio);
        }
    }

    private void EscalarBoton()
    {
        width += Time.deltaTime * 60;
        height += Time.deltaTime * 60;
        if(width >= escalaFinal.x) width = escalaFinal.x;
        if(height >= escalaFinal.y) height = escalaFinal.y;
        propiedadesBoton.sizeDelta = new Vector3(width, height);

    }

    private void RegresarEscalaBoton()
    {
        width -= Time.deltaTime * 60;
        height -= Time.deltaTime * 60;
        if (width <= escalaInicial.x) width = escalaInicial.x;
        if (height <= escalaInicial.y) height = escalaInicial.y;
        propiedadesBoton.sizeDelta = new Vector3(width, height);
    }

    
}
