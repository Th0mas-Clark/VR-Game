using UnityEngine;

public class FireplaceActivation : MonoBehaviour
{
    public ParticleSystem fireplaceParticles; 
    public AudioSource igniteSound; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Torch"))
        {
            if (fireplaceParticles != null && !fireplaceParticles.isPlaying)
            {
                fireplaceParticles.Play();
                if (igniteSound != null)
                {
                    igniteSound.Play();
                }
            }
        }
    }
}
