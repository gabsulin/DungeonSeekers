using UnityEngine;
using UnityEngine.UIElements;

interface IInteractable
{
    public void Interact();
}
public class Interactor : MonoBehaviour
{
    public Transform InteractorSource;
    public float interactRange = 1f;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Vector2 origin = InteractorSource.position;

            Collider2D hitInfo = Physics2D.OverlapCircle(origin, interactRange, LayerMask.GetMask("Collision"));

            if (hitInfo != null)
            {
                if (hitInfo.CompareTag("Interactable"))
                {
                    if (hitInfo.GetComponent<Collider2D>().gameObject.TryGetComponent(out IInteractable interactObj))
                    {
                        interactObj.Interact();
                    }
                }
            }
        }
    }
}
