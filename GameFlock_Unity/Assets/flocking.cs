using UnityEngine;
using System.Collections;

// The velocity along the y axis is 10 units per second.  If the GameObject starts at (0,0,0) then
// it will reach (0,100,0) units after 10 seconds.

public class flocking : MonoBehaviour
{
    private ParticleSystem ps;
    private ParticleSystem.Particle[] ps_particles;
    public Vector3 goal;
    public Vector3 distance;
    public Vector3 distancebetween;
    public Vector3 velocityvec;
    public Vector3 v;
    public Vector3 align;
    public Vector3 cohend;
    public Vector3 separate;
    public float force;
    public int count;

    void Start()
    {
        //Initiation of PSystem
        ps = GetComponent<ParticleSystem>();
        var main = ps.main;
        main.maxParticles = 100;
        main.startLifetime = 10;
        main.startSpeed = 0;
    }

    void Update()
    {

        InitializeIfNeeded();
        int numParticlesAlive = ps.GetParticles(ps_particles);
        for (int i = 0; i < numParticlesAlive; i++)
        {
            //find a goal for 1st particle to go
            v = new Vector3(0, 0, 0);
            if (Time.time % 20 < 5)
                goal = new Vector3(100.0f, 0.0f, 100.0f);
            if (Time.time % 20 < 10 && Time.time % 20 > 5)
                goal = new Vector3(0.0f, -100.0f, 100.0f);
            if (Time.time % 20 < 15 && Time.time % 20 > 10)
                goal = new Vector3(0.0f, -100.0f, 0.0f);
            if (Time.time % 20 < 20 && Time.time % 20 > 15)
                goal = new Vector3(100.0f, -100.0f, 0.0f);

            //send 1st particle to goal
            //ps_particles[0].velocity = velocityvec;

            //Set particles starting position
            if (ps_particles[i].remainingLifetime == 10)
            {
                ps_particles[i].position = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
            }
            //tutsplus
            //alignment
            v = new Vector3(0, 0, 0);
            count = 0;
            for (int j = 0; j < numParticlesAlive; j++)
            {
                if (j == i)
                    continue;
                distance = ps_particles[j].position - ps_particles[i].position;
                if (distance.magnitude < 50)
                {
                    v += ps_particles[j].velocity;
                    count += 1;
                }
                if (count == 0)
                    v = new Vector3(0, 0, 0);
                else
                {
                    v /= count;
                    v = v.normalized;
                }
                align = v;
            }
            //cohension
            v = new Vector3(0, 0, 0);
            count = 0;
            for (int j = 0; j < numParticlesAlive; j++)
            {
                if (j == i)
                    continue;
                distance = ps_particles[i].position - ps_particles[j].position;
                if (distance.magnitude < 50)
                {
                    v += ps_particles[j].position;
                    count += 1;
                }
                if (count == 0)
                    v = new Vector3(0, 0, 0);
                else
                {
                    v /= count;
                    v = v - ps_particles[i].position;
                    v = v.normalized;
                }
                cohend = v;
            }
            //Separation
            v = new Vector3(0, 0, 0);
            count = 0;
            for (int j = 0; j < numParticlesAlive; j++)
            {
                if (j == i)
                    continue;
                distance = ps_particles[i].position - ps_particles[j].position;
                if (distance.magnitude < 50)
                {
                    v += ps_particles[j].position - ps_particles[i].position;
                    count += 1;
                }
                if (count == 0)
                    v = new Vector3(0, 0, 0);
                else
                {
                    v /= count;
                    v = v * -1;
                    v = v.normalized;
                }
                separate = v;
            }
            v = new Vector3(0, 0, 0);
            count = 0;
            ps_particles[i].velocity += 0.9f * align + 0.9f * cohend + 0.9f * separate;
            //ps_particles[i].velocity = ps_particles[i].velocity.normalized;
            distance = goal - ps_particles[i].position;
            velocityvec = ps_particles[i].velocity + distance.normalized * 5 * Time.deltaTime;
            ps_particles[i].velocity = velocityvec;
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