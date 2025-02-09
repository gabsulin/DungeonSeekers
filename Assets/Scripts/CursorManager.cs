using UnityEngine;
using UnityEngine.InputSystem;

public class CursorManager : MonoBehaviour
{
    [SerializeField] private Texture2D cursorTexture;
    [SerializeField] private Texture2D cursorTextureCollision;

    private Vector2 cursorHotspot;
    void Start()
    {
        cursorHotspot = new Vector2(cursorTexture.width / 2, cursorTexture.height / 2);
        Cursor.SetCursor(cursorTexture, cursorHotspot, CursorMode.Auto);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 worldCursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Collider2D hit = Physics2D.OverlapPoint(worldCursorPos, LayerMask.GetMask("Collision"));

        if (hit)
        {
            Cursor.SetCursor(cursorTextureCollision, cursorHotspot, CursorMode.Auto);
        }
        else
        {
            Cursor.SetCursor(cursorTexture, cursorHotspot, CursorMode.Auto);
        }
    }
}