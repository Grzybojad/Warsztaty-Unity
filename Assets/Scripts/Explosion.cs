using UnityEngine;

public class Explosion : MonoBehaviour
{
    private ParticleSystem ps;

    public Transform target;
    public float force;

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if( ps )
        {
            if( !ps.IsAlive() )
            {
                Destroy( gameObject );
            }
        }
    }

    private void LateUpdate()
    {
        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[ ps.particleCount ];

        ps.GetParticles( particles );

        for( int i = 0; i < particles.Length; ++i )
        {
            ParticleSystem.Particle p = particles[ i ];

            if( p.remainingLifetime < p.startLifetime * .5f )
            {
                Vector3 directionToTarget = (target.position - p.position).normalized;

                Vector3 seekForce = directionToTarget * force * Time.deltaTime;

                p.velocity += seekForce;

                particles[ i ] = p;
            }
        }

        ps.SetParticles( particles, particles.Length );
    }
}
