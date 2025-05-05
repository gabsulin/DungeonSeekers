using UnityEngine;

public class LaserRotate : MonoBehaviour
{
    [SerializeField] GameObject laser;
    [SerializeField] Transform spawn;
    public float rotationSpeed = 90;

    private bool isRotating = false;
    private bool isActiveSecondLaser = false;
    public void StartRotating()
    {
        isRotating = true;
    }

    public void StopRotating()
    {
        isRotating = false;
    }

    void Update()
    {
        if (isRotating)
        {
            transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
        }
    }
    public void SpawnSecondLaser()
    {
        if (!isActiveSecondLaser)
        {
            var secondLaser = Instantiate(laser, spawn.position, Quaternion.identity);
            secondLaser.transform.SetParent(gameObject.transform);
            secondLaser.transform.localRotation = Quaternion.Euler(0, 0, 0);
            isActiveSecondLaser = true;
        }
    }
}
