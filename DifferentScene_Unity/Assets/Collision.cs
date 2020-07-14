using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(ParticleSystem))]

public class Collision : MonoBehaviour
{
    public float radius;
    public Vector3 center;
    public Vector3 destroyPosition;
    private ParticleSystem ps;
    private ParticleSystem.Particle[] ps_particles;
    Vector3 velocityvec;
    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        radius = 1.0f;
        center = new Vector3(0.0f, 0.0f, 0.0f);
        velocityvec = new Vector3();
    }

    // Update is called once per frame
    void Update()
    {
        InitializeIfNeeded();

        int numParticlesAlive = ps.GetParticles(ps_particles);
        for (int i = 0; i < numParticlesAlive; i++)
        {
            if (Math.Abs(ps_particles[i].position[0]) > radius)
            {
                velocityvec[0] = -ps_particles[i].velocity[0];
            }
            else
            {
                velocityvec[0] = ps_particles[i].velocity[0];
            }

            if (Math.Abs(ps_particles[i].position[1]) > radius)
            {
                velocityvec[1] = -ps_particles[i].velocity[1];
            }
            else
            {
                velocityvec[1] = ps_particles[i].velocity[1];
            }

            if (Math.Abs(ps_particles[i].position[2]) > radius)
            {
                velocityvec[2] = -ps_particles[i].velocity[2];

            }
            else
            {
                velocityvec[2] = ps_particles[i].velocity[2];
            }
            ps_particles[i].velocity = new Vector3(velocityvec[0], velocityvec[1], velocityvec[2]);

        }
        ps.SetParticles(ps_particles, numParticlesAlive);
    }
    void InitializeIfNeeded()
    {
        if (ps == null)
            ps = GetComponent<ParticleSystem>();

        if (ps_particles == null || ps_particles.Length < ps.main.maxParticles)
            ps_particles = new ParticleSystem.Particle[ps.main.maxParticles];
    }
}
