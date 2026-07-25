using System.Linq;
using UnityEngine;

public class AudioContext : Singleton<AudioContext>
{

    [SerializeField]
    private IAudioEmitter[] audioEmitters;


    private PlayerAudioEmitter _playerAudioEmitter;
    private MusicAudioEmitter _musicAudioEmitter;
    public PlayerAudioEmitter PlayerAudioEmitter => _playerAudioEmitter;
    public MusicAudioEmitter MusicAudioEmitter => _musicAudioEmitter;


    protected override void Awake()
    {
        base.Awake();
        audioEmitters = FindObjectsByType<IAudioEmitter>(FindObjectsSortMode.None);
        _playerAudioEmitter = (PlayerAudioEmitter)audioEmitters.FirstOrDefault(value =>
        {
            if (typeof(PlayerAudioEmitter) == value.GetType())
            {
                return true;
            }
            return false;
        });
        _musicAudioEmitter = (MusicAudioEmitter)audioEmitters.FirstOrDefault(value =>
        {
            if (typeof(MusicAudioEmitter) == value.GetType())
            {
                return true;
            }
            return false;
        });
    }
}
