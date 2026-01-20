using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform PlayerTransform { get; private set; }

    private void Awake()
    {
        PlayerTransform = transform;
    }
}