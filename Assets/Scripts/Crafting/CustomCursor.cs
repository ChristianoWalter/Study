using UnityEngine;

public class CustomCursor : MonoBehaviour
{
    private void Awake()
    {
        transform.position = Input.mousePosition;
    }

    private void FixedUpdate()
    {
        transform.position = Input.mousePosition;
    }
}
