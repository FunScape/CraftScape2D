using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameThrower : MonoBehaviour {

    ParticleSystem m_particleSystem;

	// Use this for initialization
	void Start () {
        m_particleSystem = GetComponent<ParticleSystem>();
	}

    private void OnParticleCollision(GameObject other)
    {
        
    }


}
