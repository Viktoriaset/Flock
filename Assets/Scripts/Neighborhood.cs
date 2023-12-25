using System.Collections.Generic;
using UnityEngine;

public class Neighborhood : MonoBehaviour
{
    [Header("Set Dynamically")]
    public List<Boid> Neighbors;
    private SphereCollider coll;

    public Vector3 avgPos
    {
        get
        {
            Vector3 avg = Vector3.zero;

            if (Neighbors.Count == 0) return avg;

            for (int i = 0; i < Neighbors.Count; i++)
            {
                avg += Neighbors[i].Pos;
            }
            avg /= Neighbors.Count;

            return avg;
        }
    }

    public Vector3 avgVel
    {
        get
        {
            Vector3 avg = Vector3.zero;

            if (Neighbors.Count == 0) return avg;

            for (int i = 0; i < Neighbors.Count; i++)
            {
                avg += Neighbors[i].rb.velocity;
            }
            avg /= Neighbors.Count;

            return avg;
        }
    }

    public Vector3 avgClosePos
    {
        get
        {
            Vector3 avg = Vector3.zero;
            Vector3 delta;
            int nearCount = 0;

            for (int i = 0; i < Neighbors.Count; i++)
            {
                delta = Neighbors[i].transform.position - transform.position;
                if (delta.magnitude <= Spawner.Instance.CollDist)
                {
                    avg += Neighbors[i].Pos;
                    nearCount++;
                }
            }

            if (nearCount == 0) return avg;

            avg /= nearCount;
            return avg;
        }
    }

    private void Start()
    {
        Neighbors = new List<Boid>();
        coll = GetComponent<SphereCollider>();
        coll.radius = Spawner.Instance.NeighborDist / 2;
    }

    private void FixedUpdate()
    {
        if (coll.radius != Spawner.Instance.NeighborDist / 2)
        {
            coll.radius = Spawner.Instance.NeighborDist / 2;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Boid b = other.GetComponent<Boid>();

        if (b != null)
        {
            if (Neighbors.IndexOf(b) == -1)
            {
                Neighbors.Add(b);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Boid b = other.GetComponent<Boid>();

        if (b != null)
        {
            if (Neighbors.IndexOf(b) != -1)
            {
                Neighbors.Remove(b);
            }
        }
    }
}
