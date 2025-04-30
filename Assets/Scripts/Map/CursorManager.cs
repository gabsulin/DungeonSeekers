using UnityEngine;
using UnityEngine.InputSystem;

public class CursorManager : MonoBehaviour
{
    [SerializeField] private Texture2D cursorTexture;
    [SerializeField] private Texture2D cursorTextureCollision;

    private Vector2 cursorHotspot;
    void Start()
    {
        cursorHotspot = new Vector2(cursorTextureCollision.width / 2, cursorTextureCollision.height / 2);
        Cursor.SetCursor(cursorTexture, cursorHotspot, CursorMode.Auto);
    }
}