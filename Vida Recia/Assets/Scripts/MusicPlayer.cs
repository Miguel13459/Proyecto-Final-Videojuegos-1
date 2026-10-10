using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Clase para agrupar el archivo de audio con la imagen del disco
[System.Serializable]
public class SongData
{
    public string songTitle;        // Nombre visible de la canción (opcional si quieres personalizarlo)
    public AudioClip audioClip;     // El archivo de audio (.mp3, .wav, etc.)
    public Sprite albumArt;         // La imagen del disco/carátula (.png, .jpg)
}

public class MusicPlayer : MonoBehaviour
{
    [Header("Componentes de UI")]
    public AudioSource audioSource;
    public TextMeshProUGUI songTitleText;
    public Slider volumeSlider;
    public Image discImage;         // El componente Image de la UI que contiene el disco

    [Header("Configuración de Rotación")]
    public float rotationSpeed = 100f; // Velocidad con la que gira el disco

    [Header("Lista de Canciones")]
    public SongData[] playlist;     // Lista estructurada de canciones con sus discos
    private int currentSongIndex = 0;
    private bool isPlayerPausedManual = false;

    void Start()
    {
        // Configurar el slider de volumen
        if (volumeSlider != null && audioSource != null)
        {
            volumeSlider.value = audioSource.volume;
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }

        // Cargar primera canción si la lista no está vacía
        if (playlist != null && playlist.Length > 0)
        {
            LoadSong(currentSongIndex);
        }
    }

    void Update()
    {
        if (audioSource == null || playlist.Length == 0) return;

        // 1. ROTACIÓN DEL DISCO: Gira solo si la música está sonando
        if (audioSource.isPlaying && discImage != null)
        {
            // Rota sobre el eje Z (hacia atrás para rotación en sentido horario)
            discImage.transform.Rotate(0, 0, -rotationSpeed * Time.deltaTime);
        }

        // 2. CAMBIO AUTOMÁTICO DE CANCIÓN
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

        // Asignar imagen del disco
        if (discImage != null && currentSong.albumArt != null)
        {
            discImage.sprite = currentSong.albumArt;
        }

        // Reiniciar la rotación del disco a 0 al cambiar de canción
        if (discImage != null)
        {
            discImage.transform.rotation = Quaternion.identity;
        }
    }
}
