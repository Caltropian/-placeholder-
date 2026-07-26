using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering;

public class InitialSetup : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField]
    private PlayerState playerState;
    [SerializeField]
    private GameObject[] startObjects;
    [SerializeField]
    private PlayableDirector initialDirector, startGameDirector;
    [SerializeField]
    private GameObject spotlightMask;

    [Header("Settings")]
    [SerializeField]
    private PlayerState.PlayerStates initialState;
    [SerializeField]
    private Transform initialCheckpoint;

    [Header("Debug")]
    [SerializeField]
    private bool skipStartScreen = false;

    private IInputReciever[] inputRecievers;
    [SerializeField]
    private GameObject tutorial1;



    void Awake()
    {
        if (playerState == null)
        {
            playerState = FindFirstObjectByType<PlayerState>();
        }
        inputRecievers ??= FindObjectsByType<IInputReciever>(FindObjectsSortMode.None).ToArray();
        spotlightMask = spotlightMask != null ? spotlightMask : GameObject.FindWithTag("Mask");
    }
    void Start()
    {
        if (skipStartScreen)
        {
            StartGame();
            return;
        }
        //Make it so you don't start counting down at the beginning.
        playerState.CurrentState = PlayerState.PlayerStates.ABOVEWATER;
        //Disable all in-game controls: Player Input and the pause button.
        foreach (IInputReciever inputReciever in inputRecievers)
        {
            inputReciever.enabled = false;
        }
        foreach (GameObject go in startObjects)
        {
            go.SetActive(true);
        }
        playerState.CurrentCheckpoint = initialCheckpoint;
        if (tutorial1 != null)
        {
            tutorial1.SetActive(false);
        }
        initialDirector.Play();
    }
    public void StartGameCutscene()
    {
        Invoke(nameof(StartGame), (float)startGameDirector.duration + 0.01f);
        startGameDirector.Play();
    }
    public void StartGame()
    {
        foreach (IInputReciever inputReciever in inputRecievers)
        {
            inputReciever.enabled = true;
        }
        foreach (GameObject go in startObjects)
        {
            if (spotlightMask.CompareTag("Mask"))
            {
                spotlightMask.GetComponent<RectTransform>().localScale = new(0.2f, 0.2f, 1);
            }
            go.SetActive(false);
        }
        if (tutorial1 != null)
        {
            tutorial1.SetActive(true);
        }
        playerState.CurrentState = initialState;
    }
}
