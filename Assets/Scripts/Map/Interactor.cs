using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.UI.Image;

interface IInteractable
{
    public void Interact();
}
public class Interactor : MonoBehaviour
{
    public Transform InteractorSource;
    public float interactRange = 1f;
    [SerializeField] GameObject EButton;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector2 origin = InteractorSource.position;

        Collider2D hitInfo = Physics2D.OverlapCircle(origin, interactRange, LayerMask.GetMask("Collision"));
        Collider2D hitInfoWeapon = Physics2D.OverlapCircle(origin, interactRange, LayerMask.GetMask("Interactable"));
        if ((hitInfo && hitInfo.CompareTag("Interactable") || hitInfoWeapon))
        {
            EButton.SetActive(true);
        }
        else
        {
            EButton.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
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

            if (hitInfoWeapon != null)
            {
                Debug.Log("Neni null");
                if (hitInfoWeapon.CompareTag("Interactable"))
                {
                    Debug.Log("Ma tag");
                    if (hitInfoWeapon.GetComponent<Collider2D>().gameObject.TryGetComponent(out IInteractable interactObj))
                    {
                        Debug.Log("Interakce");
                        interactObj.Interact();
                    }
                }
            }
        }
    }
}
