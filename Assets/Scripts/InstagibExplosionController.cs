using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstagibExplosionController : MonoBehaviour {
    ParticleSystem myParticleSystem;

	void Start () {
        myParticleSystem = GetComponent<ParticleSystem>();
	}
	
	void Update () {
        if (!myParticleSystem.IsAlive())
        {
            // Destory automatically after the system is finished
            Destroy(gameObject);
        }
	}
}
