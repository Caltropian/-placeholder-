using UnityEngine;

public class RoughCamLeveler : MonoBehaviour
{
    Transform parent;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        parent = transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(0, 0, -parent.rotation.z);
    }
}
