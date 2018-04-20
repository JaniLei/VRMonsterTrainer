using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleOnCollide : MonoBehaviour {

    ParticleSystem particleSys;

    public int maxParticles = 100;

    float delay;
    public float maxDelay = 0.1f;

    ParticleSystem ps;
    public MonsterStats mStats;
    //ParticleSystem.EmitParams emitParams;



    void Start()
    {
        ps = GameObject.Find("PoopSplatParticles").GetComponent<ParticleSystem>();
        //ps = Instantiate(particleSys, Vector3.zero, Quaternion.identity);
        //ps.startColor = transform.GetComponent<MeshRenderer>().material.color;
        //emitParams = new ParticleSystem.EmitParams();
    }


	void Update ()
    {
        delay += Time.deltaTime;
    }

    void OnCollisionStay(Collision col)
    {
        if (delay > maxDelay)
        {
            delay = 0;
            ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams();
            ps.startColor = transform.GetComponent<MeshRenderer>().material.color;
            foreach (ContactPoint c in col.contacts)
            {
                emitParams.position = c.point;
                emitParams.applyShapeToPosition = true;

                //Destroy(ps, 10);
                //ps.transform.rotation = Quaternion.LookRotation(c.normal);
                /*if (particles.Count > maxParticles)
                {
                    Destroy(particles[0]);
                }*/
            }
            ps.Emit(emitParams, 2);
        }
        
    }

    void CreateSplatter(Collision col)
    {
    }
}
