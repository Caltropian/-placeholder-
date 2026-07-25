using UnityEngine;

public class DepthTracker : MonoBehaviour
{
    [SerializeField]
    private float depthChunckSize = 500f;
    [SerializeField]
    private float onDeeperChunkOxygenChange = 3f;
    [SerializeField]
    private int currentDepthLevel = 0;
    [SerializeField]
    private int maxDepthLevels = 4;
    [SerializeField]
    private Transform playerPositon;
    [SerializeField]
    private Transform startPosition;

    void Awake()
    {
        if (playerPositon == null)
        {
            playerPositon = GameObject.FindWithTag("Player").transform;
        }
    }

    private void Update()
    {
        float previousDepthLevel = currentDepthLevel;
        float currentDepthDifference = startPosition.position.y - playerPositon.position.y;
        if (currentDepthDifference < 0) currentDepthDifference = 0;
        currentDepthLevel = Mathf.FloorToInt(currentDepthDifference / depthChunckSize);
        if (currentDepthLevel != previousDepthLevel)
        {
            //Clamp the value. idk why im not using Mathf clamp don't look at me! i got no brain power left!
            if (currentDepthLevel > maxDepthLevels) currentDepthLevel = maxDepthLevels;
            if (currentDepthLevel < 0) currentDepthLevel = 0;
            AudioContext.Instance.MusicAudioEmitter.ChangeIntensity(currentDepthLevel, maxDepthLevels);
        }
    }
}
