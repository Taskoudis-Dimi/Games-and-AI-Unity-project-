using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(ParticleSystem))]

public class Particles : MonoBehaviour
{
    public GameObject sp;
    public float radius;
    public Vector3 center;
    private ParticleSystem ps;
    private ParticleSystem.Particle[] ps_particles;
    //private GameObject sp;

    float grav;
    float deprfactor;
    float Radius;
    Vector3 velocityvec;
    Vector3 distance;
    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        var main = ps.main;
        main.maxParticles = 10;
        main.startSpeed = 1;
        deprfactor = 0.98f;
        grav = 9.8f;
        Radius = 4.0f;
        center = new Vector3(0.0f, 0.0f, 0.0f);
    }

    // Update is called once per frame
    [Obsolete]
    void Update()
    {
        InitializeIfNeeded();
        var main = ps.main;
        main.startLifetime = 15;
        main.startSpeed = UnityEngine.Random.RandomRange(1, 10);
        int numParticlesAlive = ps.GetParticles(ps_particles);
        for (int i = 0; i < numParticlesAlive; i++)
        {

            if (ps_particles[i].position[0] < -4 || ps_particles[i].position[0] > 4)
            {
                velocityvec[0] = -ps_particles[i].velocity[0] * deprfactor;
                velocityvec[1] = ps_particles[i].velocity[1] * deprfactor;
                velocityvec[2] = ps_particles[i].velocity[2] * deprfactor;
            }
            else if (ps_particles[i].position[1] < -4|| ps_particles[i].position[1] > 4)
            {
                velocityvec[1] = -ps_particles[i].velocity[1] * deprfactor;
                velocityvec[0] = ps_particles[i].velocity[0] * deprfactor;
                velocityvec[2] = ps_particles[i].velocity[2] * deprfactor;
            }

            else if (ps_particles[i].position[2] < -4 || ps_particles[i].position[2] > 4)
            {
                velocityvec[2] = -ps_particles[i].velocity[2] * deprfactor;
                velocityvec[0] = ps_particles[i].velocity[0] * deprfactor;
                velocityvec[1] = ps_particles[i].velocity[1] * deprfactor;
            }
            else
            {
                velocityvec = ps_particles[i].velocity;
            }
        
            velocityvec[2] = velocityvec[2] - grav * Time.deltaTime;
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
