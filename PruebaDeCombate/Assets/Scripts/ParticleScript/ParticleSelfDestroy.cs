using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSelfDestroy : MonoBehaviour
{

    private ParticleSystem particula;

    void Start()
    {
        particula = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!particula.IsAlive())
        {
            Destroy(gameObject);
        }
    }
}
