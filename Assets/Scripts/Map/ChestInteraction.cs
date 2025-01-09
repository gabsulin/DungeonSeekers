using UnityEngine;

public class ChestInteraction : MonoBehaviour, IInteractable
{
    public Animator anim;

    public void Interact()
    {
        anim.SetBool("ChestOpen", true);
        Debug.Log("Open chest");
    }
}
