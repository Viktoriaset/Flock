using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    static public Spawner Instance;
    static public List<Boid> Boids;

    [Header("Set in Inspector: Spawning")]
    public GameObject BoidPrefab;
    public Transform BoidAnchor;
    public int NumBoinds = 100;
    public float SpawnRadius = 100f;
    public float SpawnDelay = 0.1f;

    [Space(5)]
    [Header("Set in Inspector: Boids")]
    public float Velocity = 30f;
    public float NeighborDist = 30f;
    public float CollDist = 4f;
    public float VelMatching = 0.25f;
    public float FlockCentering = 0.25f;
    public float CollAvoid = 2f;
    public float AttractPull = 2f;
    public float AttractPush = 2f;
    public float AttractPushDist = 5f;

    private void Awake()
    {
        Instance = this;

        Boids = new List<Boid>();
        InstantiateBoid();
    }

    private void InstantiateBoid()
    {
        GameObject go = Instantiate(BoidPrefab);
        Boid b = go.GetComponent<Boid>();
        b.transform.SetParent(BoidAnchor);
        Boids.Add(b);

        if (Boids.Count < NumBoinds)
        {
            Invoke("InstantiateBoid", SpawnDelay);
        }
    }
}
