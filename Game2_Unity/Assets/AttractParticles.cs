using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(ParticleSystem))]

public class partsincube : MonoBehaviour
{
    public GameObject sp;
    public float radius;
    public Vector3 center;
    private ParticleSystem ps;
    private ParticleSystem.Particle[] ps_particles;
    //private GameObject sp;
    
    float grav;
    float deprfactor;
    float force;
    Vector3 velocityvec;
    Vector3 distance;
    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        GameObject sp = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sp.transform.position = new Vector3(1.5f, 1.5f, 1.5f);
        GameObject sp2 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sp2.transform.position = new Vector3(1f, -2.5f, -2.9f);
        var main = ps.main;
        main.maxParticles = 50;
        main.startSpeed = 1;
        deprfactor = 0.95f;
        grav = 9.8f;
        center = new Vector3(0.0f, 0.0f, 0.0f);  
    }

    // Update is called once per frame
    void Update()
    {
        sp = GameObject.Find("sp");
        InitializeIfNeeded();
        var main = ps.main;
        main.startLifetime = 25;
        main.startSpeed = Random.Range(1, 10);
        int numParticlesAlive = ps.GetParticles(ps_particles);
        for (int i = 0; i < numParticlesAlive; i++)
        {   
            if (ps_particles[i].remainingLifetime > 24.98f ) {
                ps_particles[i].velocity = new Vector3(Random.Range(-10.0f, 10.0f), Random.Range(-10.0f, 10.0f), Random.Range(-10.0f, 10.0f));
            }
            if ((ps_particles[i].position - center).sqrMagnitude > 3)
            {
                if (ps_particles[i].position[0] < -3 || ps_particles[i].position[0] > 3)
                {
                    velocityvec[0] = -ps_particles[i].velocity[0] * deprfactor;
                    velocityvec[1] = ps_particles[i].velocity[1] * deprfactor;
                    velocityvec[2] = ps_particles[i].velocity[2] * deprfactor;
                }
                else if (ps_particles[i].position[1] < -3 || ps_particles[i].position[1] > 3)
                {
                    velocityvec[1] = -ps_particles[i].velocity[1] * deprfactor;
                    velocityvec[0] = ps_particles[i].velocity[0] * deprfactor;
                    velocityvec[2] = ps_particles[i].velocity[2] * deprfactor;
                }

                else if (ps_particles[i].position[2] < -3 || ps_particles[i].position[2] > 3)
                {
                    velocityvec[2] = -ps_particles[i].velocity[2] * deprfactor;
                    velocityvec[0] = ps_particles[i].velocity[0] * deprfactor;
                    velocityvec[1] = ps_particles[i].velocity[1] * deprfactor;
                }
                else
                {
                    velocityvec = ps_particles[i].velocity;
                }

                if (ps_particles[i].position[0] > 3.3f || ps_particles[i].position[1] > 3.3f || ps_particles[i].position[2] > 3.3f)
                    ps_particles[i].remainingLifetime = -1.0f;
                if (ps_particles[i].position[0] < -3.3f || ps_particles[i].position[1] < -3.3f || ps_particles[i].position[2] < -3.3f)
                    ps_particles[i].remainingLifetime = -1.0f;

                    velocityvec[1] = velocityvec[1] - grav * Time.deltaTime;

                distance = new Vector3(1.5f,1.5f,1.5f) - ps_particles[i].position;
                force = 1.0f / distance.magnitude;
                if (distance.magnitude > 3.0f)
                    force = 0;
                velocityvec = velocityvec + distance * force * Time.deltaTime;

                distance = new Vector3(1f, -2.5f, -2.9f) - ps_particles[i].position;
                force = 1.0f / distance.magnitude;
                if (distance.magnitude > 3.0f)
                    force = 0;
                velocityvec = velocityvec + distance * force * Time.deltaTime;
                ps_particles[i].velocity = velocityvec;
            }
            else
            {
                ps_particles[i].velocity = ps_particles[i].velocity;
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
