using System.Collections;
using Character;
using UnityEngine;

public class AIManager : MonoBehaviour
{

    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private GooberWarrior goobies;
    [SerializeField] private int numGoobies;
    [SerializeField] private float initialDelay = 5;
    [SerializeField] private float spawnCycle = 1;
    public static AIManager instance { get; private set; }

    private void Awake()
    {
        if (instance && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    public Vector3 GetGoobyPoint()
    {
        return spawnPoints[Random.Range(0, spawnPoints.Length)].position;
    }

    public void Activate()
    {
        if (!enabled) return;
        enabled = false;
        StartCoroutine(SpawnCycle());
    }

    private IEnumerator SpawnCycle()
    {
        yield return new WaitForSeconds(initialDelay);
        for (int i = 0; i < numGoobies; i++)
        {
            
            Instantiate(goobies, transform);
            yield return new WaitForSeconds(spawnCycle);
        }
    }
    


}
