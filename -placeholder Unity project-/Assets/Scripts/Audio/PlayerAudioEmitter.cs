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
        willStopAudioEmitter = true;
    }
    public void PlaySwimmingWaterSfx(bool stopAudio) //done
    {
        //make sure audio is initialized()
        //change all parameters here. 
        //play with current player location.
        if (stopAudio && willStopAudioEmitter == true) return;
        if (!stopAudio && willStopAudioEmitter == false) return;
        if (stopAudio)
        {
            _swimmingEmitter.Stop();
            willStopAudioEmitter = true;
            Debug.Log("Stopping Swimming Audio");
            return;
        }
        willStopAudioEmitter = false;
        _swimmingEmitter.Play();
        Debug.Log("Playing Swimming Audio.");
    }
    public void PlaySfx(PlayerSFXTypes type)
    {
        if (itemCounts[type] == null)
        {
            Debug.LogWarning("No FMODEvent assigned to " + type.ToString());
            return;
        }
        Debug.Log("Playing Target Audio. (oneshot)");
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
