// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
using UnityEditor.Rendering;
using UnityEngine;

[CreateAssetMenu(fileName = "FMOD Event", menuName = "jpix/Audio/FMOD/FMOD Event")]
public class FMODEvent : ScriptableObject
{
    [SerializeField]
    private FMODUnity.EventReference _eventReference;
    [SerializeField]
    private string[] _paramNames;

    private FMOD.Studio.PARAMETER_DESCRIPTION[] _paramList;
    public FMODUnity.EventReference EventReference => _eventReference;
    public FMOD.Studio.PARAMETER_DESCRIPTION[] ParamList
    {
        private set
        {
            _paramList = value;
        }
        get
        {
            if (_paramList == null)
            {
                InitialiseParameters();
            }
            return _paramList;
        }
    }


    /// <summary>
    /// 
    /// </summary>
    public void InitialiseParameters()
    {
        _paramList = new FMOD.Studio.PARAMETER_DESCRIPTION[_paramNames.Length];
        int _counter = 0;
        foreach (string _paramName in _paramNames)
        {
            FMOD.Studio.EventDescription eventDescription = FMODUnity.RuntimeManager.GetEventDescription(_eventReference);
            eventDescription.getParameterDescriptionByName(_paramName, out FMOD.Studio.PARAMETER_DESCRIPTION paramDesc);
            _paramList[_counter] = paramDesc;
            _counter++;
        }
    }

    /// <summary>
    /// Play the FMOD event from position (0, 0, 0)
    /// </summary>
    public void PlayOneShot() => FMODUnity.RuntimeManager.PlayOneShot(_eventReference);

    /// <summary>
    /// Play the FMOD event from specific position
    /// </summary>
    /// <param name="origin">Origin of the sounds</param>
    public void PlayOneShot(Vector3 origin) => FMODUnity.RuntimeManager.PlayOneShot(_eventReference, origin);

    /// <summary>
    /// Play the FMOD event from a MonoBehavior's position
    /// </summary>
    /// <param name="origin">Origin of the sounds</param>
    public void PlayOneShot(MonoBehaviour origin) => PlayOneShot(origin.transform.position);

    /// <summary>
    /// Play the FMOD event from a GameObject's position
    /// </summary>
    /// <param name="origin">Origin of the sounds</param>
    public void PlayOneShot(GameObject origin) => PlayOneShot(origin.transform.position);
}

