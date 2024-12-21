using UnityEngine;

public class CreationParticle : MonoBehaviour
{
    ParticleSystem.MainModule mainModule;
    bool shouldUpdateFlashDuration = false;
    float cachedGameSpeed;

    void Start()
    {
        mainModule = GetComponent<ParticleSystem>().main;
        mainModule.stopAction = ParticleSystemStopAction.Callback;
        updateFlashDuration(1);
    }

    public void updateFlashDuration(float gameSpeed)
    {
        if (!GetComponent<ParticleSystem>().isPlaying)
        {
            mainModule.duration = 1/gameSpeed;
            mainModule.startDelay = 0.25f/gameSpeed;
            shouldUpdateFlashDuration = false;
        }
        else
        {
            shouldUpdateFlashDuration = true;
            cachedGameSpeed = gameSpeed;
        }
    }

    private void OnParticleSystemStopped()
    {
        if (shouldUpdateFlashDuration)
        {
            updateFlashDuration(cachedGameSpeed);
        }
    }
}
