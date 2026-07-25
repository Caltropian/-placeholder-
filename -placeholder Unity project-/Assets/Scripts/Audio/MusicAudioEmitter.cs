using UnityEngine;

public class MusicAudioEmitter : IAudioEmitter
{
    [SerializeField]
    private FMODUnity.StudioEventEmitter _musicEmitter;
    [SerializeField]
    private FMODUnity.StudioEventEmitter _ambienceEmitter;

    [SerializeField]
    private FMODEvent music;
    [SerializeField]
    private FMODEvent caveAmbiance;

    void Start()
    {
        music.InitialiseParameters();
        caveAmbiance.InitialiseParameters();
        if (_musicEmitter.EventReference.Path == "") _musicEmitter.EventReference = music.EventReference;
        if (_ambienceEmitter.EventReference.Path == "") _ambienceEmitter.EventReference = caveAmbiance.EventReference;
    }
    /// <summary>
    /// Should be assigned to the OnPlayerStateChange event in PlayerState
    /// </summary>
    /// <param name="playerState"></param>
    public void OnChangePlayerState(PlayerState.PlayerStates playerState)
    {
        if (playerState == PlayerState.PlayerStates.UNDERWATER)
        {
            if (_ambienceEmitter.IsPlaying()) _ambienceEmitter.Stop();
            if (!_musicEmitter.IsPlaying())
            {
                _musicEmitter.Play();
            }
            //Change the parameter: in_air_pocket = false;
            _musicEmitter.SetParameter(music.ParamList[0].name, 1f, false);
        }
        else
        {
            if (_musicEmitter.IsPlaying())
            {
                //Change the parameter: in_air_pocket = true;
                _musicEmitter.SetParameter(music.ParamList[0].name, 0f, false);
            }
            if (!_ambienceEmitter.IsPlaying()) _ambienceEmitter.Play();
        }
    }
    public void ChangeIntensity(int currIntensityLevel, int maxIntensityLevel)
    {
        //normalize value
        Debug.Log(currIntensityLevel);
        Debug.Log(maxIntensityLevel);
        float normalizedIntensityLevel = (float)currIntensityLevel / maxIntensityLevel;
        Debug.Log(normalizedIntensityLevel);
        //change the parameter: Intensity = normalizedIntensityLevel
        _musicEmitter.SetParameter(music.ParamList[1].name, normalizedIntensityLevel, false);
    }
}
