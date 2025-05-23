using Unity.Cinemachine;
using UnityEngine;

public class GetCameraBoundary : MonoBehaviour
{
    private GameObject gameObjectCameraBoundary;
    public CompositeCollider2D cameraBoundary;
    [SerializeField]
    private CinemachineConfiner2D confiner;


    void Start()
    {
        gameObjectCameraBoundary = GameObject.FindGameObjectWithTag("CameraBoundary");
        cameraBoundary = gameObjectCameraBoundary.GetComponent<CompositeCollider2D>();
        confiner = FindAnyObjectByType<CinemachineConfiner2D>();
        confiner.BoundingShape2D = cameraBoundary;
    }

    private void Update()
    {
        if (gameObjectCameraBoundary == null)
        {
            gameObjectCameraBoundary = GameObject.FindGameObjectWithTag("CameraBoundary");
            cameraBoundary = gameObjectCameraBoundary.GetComponent<CompositeCollider2D>();

            confiner.BoundingShape2D = cameraBoundary;
        }
    }
}
