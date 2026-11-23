using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Clase para agrupar el archivo de audio con la imagen del disco
[System.Serializable]
public class SongData
{
    public string songTitle;
    public AudioClip audioClip;
}

public class MusicPlayer : MonoBehaviour
{
    [Header("Componentes de UI")]
    public AudioSource audioSource;
    public TextMeshProUGUI songTitleText;
    public Image discImage;         // El componente Image de la UI que contiene el disco
    public Slider SliderTiempoCancion;

    [Header("Lista de Canciones")]
    public SongData[] playlist;     // Lista estructurada de canciones con sus discos
    private int currentSongIndex = 0;
    private bool isPlayerPausedManual = false;

    float duracionTotal;
    float tiempoActual;
    void Start()
    {
        // Cargar primera canción si la lista no está vacía
        if (playlist != null && playlist.Length > 0)
        {
            LoadSong(currentSongIndex);
        }

        duracionTotal = audioSource.clip.length;
        
    }

    void Update()
    {
        tiempoActual = audioSource.time;
        if (SliderTiempoCancion != null && audioSource != null)
        {
            SliderTiempoCancion.value = tiempoActual / duracionTotal;
        }

        if (audioSource == null || playlist.Length == 0) return;


        // CAMBIO AUTOMÁTICO DE CANCIÓN
        if (!audioSource.isPlaying && !isPlayerPausedManual)
        {
            // Si la canción llegó al final o está al inicio sin sonar
            if (audioSource.time >= audioSource.clip.length - 0.1f || audioSource.time == 0)
            {
                NextSong();
            }
        }
    }

    public void PlayPauseToggle()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Pause();
            isPlayerPausedManual = true;
        }
        else
        {
            audioSource.Play();
            isPlayerPausedManual = false;
        }
    }

    public void NextSong()
    {
        currentSongIndex = (currentSongIndex + 1) % playlist.Length;
        LoadSong(currentSongIndex);
        audioSource.Play();
        isPlayerPausedManual = false;
    }

    public void PreviousSong()
    {
        currentSongIndex--;
        if (currentSongIndex < 0)
        {
            currentSongIndex = playlist.Length - 1;
        }
        LoadSong(currentSongIndex);
        audioSource.Play();
        isPlayerPausedManual = false;
    }

    public void SetVolume(float volume)
    {
        if (audioSource != null)
        {
            audioSource.volume = volume;
        }
    }

    private void LoadSong(int index)
    {
        if (playlist.Length == 0) return;

        SongData currentSong = playlist[index];

        // Asignar audio
        audioSource.clip = currentSong.audioClip;

        // Asignar título
        if (songTitleText != null)
        {
            // Si le pones título manual en el Inspector usa ese, si no, usa el nombre del archivo de audio
            if (!string.IsNullOrEmpty(currentSong.songTitle))
                songTitleText.text = currentSong.songTitle;
            else if (currentSong.audioClip != null)
                songTitleText.text = currentSong.audioClip.name;
        }

        // Reiniciar la rotación del disco a 0 al cambiar de canción
        if (discImage != null)
        {
            discImage.transform.rotation = Quaternion.identity;
        }
    }
}
