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
        currentDepthLevel = Mathf.FloorToInt(currentDepthDifference / depthChunckSize);
        if (currentDepthLevel != previousDepthLevel)
        {
            Debug.Log("Call dynamic music parameter and give them: " + currentDepthLevel * -1);
        }
    }
}
