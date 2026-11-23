using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Clase para agrupar el archivo de audio con la imagen del disco
/*[System.Serializable]
public class SongData
{
    public string songTitle;
    public AudioClip audioClip;
}*/

public class MusicPlayer : MonoBehaviour
{
    [Header("Componentes de UI")]
    public AudioSource audioSource;
    public TextMeshProUGUI songTitleText;
    public Slider SliderTiempoCancion;

    [Header("Lista de Canciones")]
    public AudioClip[] playlist;
    private AudioClip[] playlistMezclada;
    private int currentSongIndex = 0;
    private bool isPlayerPausedManual = false;
    private bool cambiandoCancion = false;

    float duracionTotal;

    void Start()
    {
        playlistMezclada = (AudioClip[])playlist.Clone();
        MezclarLista();

        StartCoroutine(CambiarCancion(currentSongIndex));

    }

    void Update()
    {
        if (audioSource == null || playlistMezclada == null || playlistMezclada.Length == 0)
            return;

        if (SliderTiempoCancion != null && duracionTotal > 0)
        {
            SliderTiempoCancion.value = audioSource.time / duracionTotal;
        }

        if (!audioSource.isPlaying &&
            !isPlayerPausedManual &&
            !cambiandoCancion &&
            audioSource.clip != null &&
            audioSource.time >= audioSource.clip.length - 0.1f)
        {
            NextSong();
        }
    }

    private void MezclarLista()
    {
        // Algoritmo Fisher-Yates
        for (int i = playlistMezclada.Length - 1; i > 0; i--)
        {
            int indiceAleatorio = Random.Range(0, i + 1);

            (playlistMezclada[indiceAleatorio],
             playlistMezclada[i]) =
            (playlistMezclada[i],
             playlistMezclada[indiceAleatorio]);
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
        if (cambiandoCancion || playlistMezclada.Length == 0) return;

        currentSongIndex = (currentSongIndex + 1) % playlistMezclada.Length;

        StartCoroutine(CambiarCancion(currentSongIndex));
    }

    public void PreviousSong()
    {
        if (cambiandoCancion || playlistMezclada.Length == 0) return;

        currentSongIndex--;

        if (currentSongIndex < 0) currentSongIndex = playlistMezclada.Length - 1;

        StartCoroutine(CambiarCancion(currentSongIndex));
    }

    private IEnumerator CambiarCancion(int index)
    {
        cambiandoCancion = true;
        isPlayerPausedManual = false;

        audioSource.Stop();

        AudioClip nuevaCancion = playlistMezclada[index];

        if (nuevaCancion == null)
        {
            cambiandoCancion = false;
            yield break;
        }

        if (nuevaCancion.loadState != AudioDataLoadState.Loaded)
        {
            nuevaCancion.LoadAudioData();

            while (nuevaCancion.loadState == AudioDataLoadState.Loading)
                yield return null;
        }

        if (nuevaCancion.loadState == AudioDataLoadState.Failed)
        {
            cambiandoCancion = false;
            yield break;
        }

        audioSource.clip = nuevaCancion;
        duracionTotal = nuevaCancion.length;

        if (songTitleText != null) songTitleText.text = nuevaCancion.name;
        audioSource.Play();
        cambiandoCancion = false;
    }

    public void SetVolume(float volume)
    {
        if (audioSource != null) audioSource.volume = volume;
    }

}
