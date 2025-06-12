using UnityEngine;

public class LevitationAnimation : MonoBehaviour
{
    public float floatSpeed;
    public float floatHeight;
    Vector2 startPos;
    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    { 
        float newY = Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.position = new Vector2(startPos.x, newY);
    }
}
