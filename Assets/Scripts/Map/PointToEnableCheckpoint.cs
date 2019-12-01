using UnityEngine;

public class PointToEnableCheckpoint : MonoBehaviour
{

    [SerializeField]
    private float timeToCheck = 2f;

    [Header("Setup")]
    [SerializeField]
    private Checkpoint checkpoint = default;
    [SerializeField]
    private ParticleSystem waitingParticles = default;
    [SerializeField]
    private ParticleSystem takingParticles = default;
    [SerializeField]
    private ParticleSystem takenParticles = default;

    // ---- INTERN----
    private float currentTime = 0f;
    private bool isChecking = false;
    private bool hasBeenActivated = false;

    void Update()
    {
        if(!hasBeenActivated && isChecking)
        {
            currentTime += Time.deltaTime;

            if(currentTime >= timeToCheck)
            {
                hasBeenActivated = true;
                checkpoint.Active();
                takenParticles.Play();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(!hasBeenActivated && other.tag == "Player")
        {
            isChecking = true;
            waitingParticles.Stop();
            takingParticles.Play();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!hasBeenActivated && other.tag == "Player")
        {
            takingParticles.Stop();
            waitingParticles.Play();
            currentTime = 0f;
            isChecking = false;
        }
    }
}
