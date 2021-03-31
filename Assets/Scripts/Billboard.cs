using UnityEngine;

public class Billboard : MonoBehaviour
{
    void Update()
    {
        transform.LookAt(GameObject.Find("PlayerCamera").transform);
    }
}
