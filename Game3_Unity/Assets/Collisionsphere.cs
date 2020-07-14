using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(ParticleSystem))]

public class collisionsphere : MonoBehaviour
{
    public float radius;
    public Vector3 center;
    public Vector3 destroyPosition;
    private ParticleSystem ps;
    private ParticleSystem.Particle[] ps_particles;
    Vector3 velocityvec;
    float grav;
    float force;
    Vector3 oldvelocity;
    Vector3 distance;
    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        var main = ps.main;
        main.maxParticles = 25;
        main.startLifetime = 15;
        main.startSpeed = 0;
        grav = -9.8f;
        radius = 20.0f;
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
            // Initialize particles position
            if (Time.time < 2)
            {
                ps_particles[i].position = new Vector3(5, 0, 0);
            }
            // Collision and direction of velocity change according to vector pointing to the center of the sphere
            if ((ps_particles[i].position - center).sqrMagnitude > radius * radius)
            {
                oldvelocity = ps_particles[i].velocity;
                oldvelocity = oldvelocity * 0.9f;
                ps_particles[i].position = center + (ps_particles[i].position - center).normalized * radius * 0.999f;
                ps_particles[i].velocity = Vector3.Reflect(oldvelocity, ((ps_particles[i].position / radius) - center));
            }
            else
            {
                ps_particles[i].velocity = ps_particles[i].velocity;
            }
            // Apply Gravitational force
            velocityvec = ps_particles[i].velocity;
            velocityvec[1] = velocityvec[1] + grav * Time.deltaTime;
            ps_particles[i].velocity = velocityvec;
            // Apply Repulsion force
            for (int j = 0; j < numParticlesAlive; j++)
            {
                if (i == j || Time.time < 2)
                    continue;
                distance = ps_particles[i].position - ps_particles[j].position;
                //We let the force be applied even if the particles are far away from each other
                if (distance.magnitude < 0.1f)
                    force = 0;
                else
                    force = 5.0f / distance.magnitude + 0.01f;
                velocityvec = ps_particles[i].velocity + distance * force * Time.deltaTime;
                ps_particles[i].velocity = velocityvec;
            }
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
