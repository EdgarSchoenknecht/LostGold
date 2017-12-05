using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canonball : MonoBehaviour {

    public GameObject ParticleExplosion;
    public GameObject ParticleMiss;

    private bool _isDestroyed = false;


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Monster"))
        {
            if (!_isDestroyed)
            {
                _isDestroyed = true;

                GameObject Temporary_Particle_Handler;
                Temporary_Particle_Handler = Instantiate(ParticleExplosion, transform.position, transform.rotation) as GameObject;
                Destroy(Temporary_Particle_Handler, 2.0f);

                Destroy(gameObject);
            }
        }

        if (other.CompareTag("Water"))
        {

            if (!_isDestroyed)
            {
                _isDestroyed = true;

                GameObject Temporary_Particle_Handler;
                Temporary_Particle_Handler = Instantiate(ParticleMiss, transform.position, transform.rotation) as GameObject;
                Destroy(Temporary_Particle_Handler, 2.0f);

                Destroy(gameObject);
            }


        }

    }
}
