using UnityEngine;

public class PlayerParticles : MonoBehaviour
{
    public static PlayerParticles Instance;

    public ParticleSystem ps;

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void playParticles()
    {
        ps.Play();
    }
}
