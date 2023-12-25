using UnityEngine;

public class Boid : MonoBehaviour
{
    public Vector3 Pos
    {
        get { return transform.position; }
        set { transform.position = value; }
    }

    public Rigidbody rb;

    private Neighborhood neighborhood;

    private void Awake()
    {
        neighborhood = GetComponent<Neighborhood>();
        rb = GetComponent<Rigidbody>();

        Pos = Random.insideUnitSphere * Spawner.Instance.SpawnRadius;

        Vector3 vel = Random.onUnitSphere * Spawner.Instance.Velocity;
        rb.velocity = vel;

        LookAhead();

        Color randColor = Color.black;
        while (randColor.g + randColor.b + randColor.r <= 1.0f)
        {
            randColor = new Color(Random.value, Random.value, Random.value);
        }

        Renderer[] rends = gameObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer r in rends)
        {
            r.material.color = randColor;
        }

        TrailRenderer tRend = GetComponent<TrailRenderer>();
        tRend.material.SetColor("_TintColor", randColor);
    }

    private void LookAhead()
    {
        transform.LookAt(Pos + rb.velocity);
    }

    private void FixedUpdate()
    {
        DefinitionVelocity();
        LookAhead();
    }

    private void DefinitionVelocity()
    {
        Vector3 vel = rb.velocity;
        Spawner s = Spawner.Instance;

        Vector3 velAwoid = CountVelAwoid(s);
        Vector3 velAlign = CountVelAlign(s);
        Vector3 velCenter = CountVelCenter(s);

        float fdt = Time.fixedDeltaTime;

        if (velAwoid != Vector3.zero)
        {
            vel = Vector3.Lerp(vel, velAwoid, s.CollAvoid * fdt);
        }
        else
        {
            vel = CountVelWhenAwoidNotZero(vel, velAlign, velCenter, s);
        }

        vel = vel.normalized * s.Velocity;
        rb.velocity = vel;
    }

    private Vector3 CountVelAwoid(Spawner s)
    {
        Vector3 velAwoid = Vector3.zero;
        Vector3 tooClosePos = neighborhood.avgClosePos;
        if (tooClosePos != Vector3.zero)
        {
            velAwoid = Pos - tooClosePos;
            velAwoid.Normalize();
            velAwoid *= s.Velocity;
        }

        return velAwoid;
    }

    private Vector3 CountVelAlign(Spawner s)
    {
        Vector3 velAlign = neighborhood.avgVel;
        if (velAlign != Vector3.zero)
        {
            velAlign.Normalize();
            velAlign *= s.Velocity;
        }

        return velAlign;
    }

    private Vector3 CountVelCenter(Spawner s)
    {
        Vector3 velCenter = neighborhood.avgPos;
        if (velCenter != Vector3.zero)
        {
            velCenter -= transform.position;
            velCenter.Normalize();
            velCenter *= s.Velocity;
        }

        return velCenter;
    }

    private Vector3 CountVelWhenAwoidNotZero(Vector3 vel, Vector3 velAlign, Vector3 velCenter, Spawner s)
    {
        float fdt = Time.fixedDeltaTime;

        Vector3 delta = Attractor.POS - Pos;
        bool attracted = (delta.magnitude > s.AttractPushDist);
        Vector3 velAttract = delta.normalized * s.Velocity;

        if (velAlign != Vector3.zero)
        {
            vel = Vector3.Lerp(vel, velAlign, s.VelMatching * fdt);
        }
        if (velCenter != Vector3.zero)
        {
            vel = Vector3.Lerp(vel, velAlign, s.FlockCentering * fdt);
        }
        if (velAttract != Vector3.zero)
        {
            if (attracted)
            {
                vel = Vector3.Lerp(vel, velAttract, s.AttractPull * fdt);
            }
            else
            {
                vel = Vector3.Lerp(vel, -velAttract, s.AttractPush * fdt);
            }
        }

        return vel;
    }
}
