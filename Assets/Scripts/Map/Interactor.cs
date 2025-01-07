using UnityEngine;
using UnityEngine.UIElements;

interface IInteractable
{
    public void Interact();
}
public class Interactor : MonoBehaviour
{
    public Transform InteractorSource;
    public float InteractRange = 0.5f;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Vector2 origin = InteractorSource.position;
            Vector2 direction = InteractorSource.up;

            RaycastHit2D hitInfo = Physics2D.Raycast(origin, direction, InteractRange, LayerMask.GetMask("Collision"));

            if(hitInfo.collider != null)
            {
                if (hitInfo.collider.CompareTag("Interactable"))
                {
                    if (hitInfo.distance <= InteractRange)
                    {
                        if (hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObj))
                        {
                            interactObj.Interact();
                        }
                    }
                }
            }
        }
    }
}
