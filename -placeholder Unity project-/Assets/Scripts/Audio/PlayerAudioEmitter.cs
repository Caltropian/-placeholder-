using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class PlayerAudioEmitter : IAudioEmitter
{
    [Serializable]
    internal class ItemCountsClass
    {
        public PlayerSFXTypes type;
        public FMODEvent fmodEvent;
        public ItemCountsClass(PlayerSFXTypes type, FMODEvent fmodEvent)
        {
            this.type = type;
            this.fmodEvent = fmodEvent;
        }
    }
    public enum PlayerSFXTypes
    {
        Breaststroke,
        Footsteps,
        Surfacing,
        Plunging,
        Ribbit,
        Wallhit
    }
    /// <summary>
    /// Plays all loopable/repeating sounds from the player. 
    /// </summary>
    [SerializeField]
    private FMODUnity.StudioEventEmitter _swimmingEmitter;
    [SerializeField]
    private FMODUnity.StudioEventEmitter _heartbeatEmitter;
    //
    private bool willStopAudioEmitter = true;
    private Dictionary<PlayerSFXTypes, FMODEvent> itemCounts = new(
    )
    {
        {PlayerSFXTypes.Breaststroke, null},  //done
        {PlayerSFXTypes.Footsteps, null}, //wait for walking cycle animation
        {PlayerSFXTypes.Surfacing, null}, //done
        {PlayerSFXTypes.Plunging, null}, //done
        {PlayerSFXTypes.Ribbit, null}, //done
        {PlayerSFXTypes.Wallhit, null}, //done
    };
    [SerializeField]
    private ItemCountsClass[] eventList =
    {
        new (PlayerSFXTypes.Breaststroke, null),
        new (PlayerSFXTypes.Footsteps, null), //wait for walking cycle animation
        new (PlayerSFXTypes.Surfacing, null), //done
        new (PlayerSFXTypes.Plunging, null), //done
        new (PlayerSFXTypes.Ribbit, null), //done
        new (PlayerSFXTypes.Wallhit, null), //done
    };

    [SerializeField]
    private FMODUnity.StudioEventEmitter _aboveWaterSwimmingEmitter;
    void Start()
    {
        //have to do it this way as Dictionaries are not serializable *grumble*
        foreach (ItemCountsClass fmodEvent in eventList)
        {
            itemCounts[fmodEvent.type] = fmodEvent.fmodEvent;
        }
    }

    void OnDisable()
    {
        _swimmingEmitter.Stop();
        _aboveWaterSwimmingEmitter.Stop();
        _heartbeatEmitter.Stop();
        willStopAudioEmitter = true;
    }
    public void PlaySwimmingWaterSfx(bool stopAudio, PlayerState.PlayerStates isAbove) //done
    {
        //make sure audio is initialized()
        //change all parameters here. 
        //play with current player location.
        FMODUnity.StudioEventEmitter emitterToChange;
        if (isAbove == PlayerState.PlayerStates.ABOVEWATER)
        {
            if (_swimmingEmitter.IsPlaying()) _swimmingEmitter.Stop();
            emitterToChange = _aboveWaterSwimmingEmitter;
        }
        else
        {
            if (_aboveWaterSwimmingEmitter.IsPlaying()) _aboveWaterSwimmingEmitter.Stop();
            emitterToChange = _swimmingEmitter;
        }
        if (stopAudio && willStopAudioEmitter == true) return;
        if (!stopAudio && willStopAudioEmitter == false) return;
        if (stopAudio)
        {
            emitterToChange.Stop();
            willStopAudioEmitter = true;
            return;
        }
        willStopAudioEmitter = false;
        emitterToChange.Play();
    }
    public void PlayHeatbeat(bool play)
    {
        if (play)
        {
            _heartbeatEmitter.Play();
        }
        else
        {
            _heartbeatEmitter.Stop();
        }
    }
    public void PlaySfx(PlayerSFXTypes type)
    {
        if (itemCounts[type] == null)
        {
            Debug.LogWarning("No FMODEvent assigned to " + type.ToString());
            return;
        }
        Debug.Log(type.ToString());
        itemCounts[type].PlayOneShot();
    }
    public void ChangeSurfacingStrength(float state)
    {
        return;
    }
    public void PlayFootstepsSfx()
    {
        if (itemCounts[PlayerSFXTypes.Footsteps] == null)
        {
            Debug.LogWarning("No FMODEvent assigned to Footsteps");
            return;
        }
        Debug.Log("Playing Footsteps Audio. (oneshot). Play from Animator.");
    }
}
