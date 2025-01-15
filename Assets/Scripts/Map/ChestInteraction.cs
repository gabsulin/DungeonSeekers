using System.Collections;
using UnityEngine;

public class ChestInteraction : MonoBehaviour, IInteractable
{
    public Animator anim;
    [SerializeField] GameObject weaponPrefab;

    public void Interact()
    {
        anim.SetBool("ChestOpen", true);
        StartCoroutine(SpawnWeapon());
    }

    public IEnumerator SpawnWeapon()
    {
        yield return new WaitForSeconds(1);
        float angle = transform.rotation.z;
        Instantiate(weaponPrefab, transform.position, Quaternion.AngleAxis(90, Vector3.forward));
    }
}
