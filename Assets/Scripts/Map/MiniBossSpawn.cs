using System.Collections;
using UnityEngine;

public class MiniBossSpawn : MonoBehaviour
{
    [SerializeField] GameObject miniBossPrefab;
    [SerializeField] GameObject areaExit;
    void Start()
    {
        areaExit.SetActive(true);
        StartCoroutine(WaitForClosingExitAndSpawnMiniBoss());
        
    }

    private IEnumerator WaitForClosingExitAndSpawnMiniBoss()
    {
        yield return new WaitForSeconds(0.1f);
        areaExit.SetActive(false);
        yield return new WaitForSeconds(1.5f);
        Instantiate(miniBossPrefab, Vector2.zero, Quaternion.identity);

    }
}
