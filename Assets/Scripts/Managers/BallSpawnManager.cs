using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawnManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public int maxNumBalls = 6;
    private HashSet<GameObject> Balls = new HashSet<GameObject>();

    public GameObject prefabToSpawn;
    public float spawnInterval = 2f;

    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            Balls.RemoveWhere(item => item == null);
            if(Balls.Count < maxNumBalls)
            {
                GameObject newBall = Instantiate(prefabToSpawn, transform.position, Quaternion.identity);
                Balls.Add(newBall);

                if(newBall.gameObject.TryGetComponent<PinBall>(out PinBall ball))
                {
                    Vector3 RandomDir = Vector3.ProjectOnPlane(Random.onUnitSphere, Vector3.up).normalized;
                    ball.AddImpulse(RandomDir * 20.0f);
                }
            }

        }
    }
}
