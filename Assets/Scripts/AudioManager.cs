// ---------------------------------------------------------------------------------
// SCRIPT PARA LA GESTIÓN DE AUDIO (vinculado a un GameObject vacío "AudioManager")
// ---------------------------------------------------------------------------------
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    public static AudioManager instance;
    public AudioSource sfxSource;
    public AudioSource musicSource;
    public Dictionary<string, AudioClip> sfxClips = new Dictionary<string, AudioClip>();
    public Dictionary<string, AudioClip> musicClips = new Dictionary<string, AudioClip>();
    private void Awake() {
        if (instance == null) instance = this;
        else { Destroy(gameObject); return; }
        DontDestroyOnLoad(gameObject);
        LoadSFXClips();
        LoadMusicClips();

    }

    private void LoadSFXClips() {
        // Los recursos (ASSETS) que se cargan en TIEMPO DE EJECUCIÓN DEBEN ESTAR DENTRO de una carpeta denominada /Assets/Resources/SFX
        sfxClips["Jump"] = Resources.Load<AudioClip>("SFX/JumpSFX");
        sfxClips["CollectCoin"] = Resources.Load<AudioClip>("SFX/CoinSFX");
        sfxClips["CollectKey"] = Resources.Load<AudioClip>("SFX/KeySFX");
        sfxClips["Hit"] = Resources.Load<AudioClip>("SFX/HitSFX");
        sfxClips["Lose"] = Resources.Load<AudioClip>("SFX/LoseSFX");
        sfxClips["Heart"] = Resources.Load<AudioClip>("SFX/1upSFX");
    }

    private void LoadMusicClips() {
        musicClips["MainTheme"] = Resources.Load<AudioClip>("Music/mainTheme");
    }
    public void PlaySFX(string clipName) {
        if (sfxClips.ContainsKey(clipName)) {
            sfxSource.clip = sfxClips[clipName];
            sfxSource.Play();
        } else Debug.LogWarning("El AudioClip " + clipName + " no se encontró en el diccionario de sfxClips.");
    }
    public void PlayMusic(string clipName) {
        if (musicClips.ContainsKey(clipName)) {
            musicSource.clip = musicClips[clipName];
            musicSource.Play();
        } else Debug.LogWarning("El AudioClip " + clipName + " no se encontró en el diccionario de musicClips.");
    }

}

