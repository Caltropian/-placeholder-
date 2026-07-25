using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject toSpawn;
    [SerializeField]
    private float initialDelay,
        cooldown = 5f;
    [SerializeField]
    private Transform spawnOrigin;
    private float timeToSpawn;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timeToSpawn = initialDelay;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timeToSpawn -= Time.deltaTime;
        if (timeToSpawn <= 0)
        {
            Instantiate(toSpawn, spawnOrigin.position, spawnOrigin.rotation);
            timeToSpawn = cooldown;
        }
    }
}
