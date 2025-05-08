using UnityEngine;

public class LaserRotate : MonoBehaviour
{
    CameraShake shake;

    [SerializeField] GameObject laser;
    [SerializeField] Transform spawn;
    public float rotationSpeed = 90;

    private bool isRotating = false;
    private bool isActiveSecondLaser = false;
    private void Start()
    {
        shake = GetComponent<CameraShake>();
    }
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
            shake.StartShake(1, 0.05f, 0.25f);
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
