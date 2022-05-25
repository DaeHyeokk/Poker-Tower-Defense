using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    ParticleSystem[] particles;

    private void Awake()
    {
        particles = GetComponentsInChildren<ParticleSystem>();
    }

    public void PlayParticle()
    {
        for (int i = 0; i < particles.Length; i++)
            particles[i].Play();
    }

    public void StopParticle()
    {
        for (int i = 0; i < particles.Length; i++)
            particles[i].Stop();
    }
}
