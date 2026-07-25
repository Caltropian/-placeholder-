using System.Linq;
using UnityEngine;

public class AudioContext : Singleton<AudioContext>
{

    [SerializeField]
    private IAudioEmitter[] audioEmitters;


    private PlayerAudioEmitter _playerAudioEmitter;
    public PlayerAudioEmitter PlayerAudioEmitter => _playerAudioEmitter;


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
    }
}
