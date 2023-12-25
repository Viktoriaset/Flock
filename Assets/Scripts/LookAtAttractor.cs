using UnityEngine;

public class LookAtAttractor : MonoBehaviour
{
    void Update()
    {
        transform.LookAt(Attractor.POS);
    }
}
