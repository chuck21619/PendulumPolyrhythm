using UnityEngine;
using System.Collections;
using Unity.VisualScripting;


public class squareScript : MonoBehaviour
{
    private AudioSource audioSource;
    private bool flashing = false;
    public float flashDuration = 0.5f;
    private float flashStart;
    [SerializeField] private GameObject creationParticleObject;
    [SerializeField] private ParticleSystem topParticleSystem;
    [SerializeField] private ParticleSystem bottomParticleSystem;
    [SerializeField] private ParticleSystem staticParticleSystem;
    [SerializeField] private ParticleSystem explosionParticleSystem;
    ParticleSystem.MainModule staticMainModule;
    public bool isEcho = false;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        staticMainModule = staticParticleSystem.main;
        //updateFlashDuration(1);
    }

    // Update is called once per frame
    void Update()
    {
        if (flashing && !isEcho)
        {
            if (Time.time - flashStart >= flashDuration)
            {
                flashing = false;
            }
            else
            {
                Color tmpColor = isEcho ? Color.red : Color.white;
                float alphaValue = Mathf.Lerp(0, 1f, (Time.time - flashStart) / flashDuration);
                ParticleSystem.ColorOverLifetimeModule col = staticParticleSystem.colorOverLifetime;

                Gradient gradient = new Gradient();
                GradientColorKey[] colorKeys = new GradientColorKey[1];
                GradientAlphaKey[] alphaKeys = new GradientAlphaKey[3];

                colorKeys[0].color = tmpColor;
                colorKeys[0].time = 0;

                alphaKeys[0].alpha = 0;
                alphaKeys[0].time = 0;
                alphaKeys[1].alpha = alphaValue;
                alphaKeys[1].time = 0.5f;
                alphaKeys[2].alpha = 0;
                alphaKeys[2].time = 1;

                gradient.SetKeys(colorKeys, alphaKeys);

                col.color = gradient;


                alphaValue = 1 - Mathf.Lerp(0, 1f, (Time.time - flashStart) / flashDuration);
                col = creationParticleObject.GetComponent<ParticleSystem>().colorOverLifetime;

                gradient = new Gradient();
                colorKeys = new GradientColorKey[1];
                alphaKeys = new GradientAlphaKey[3];

                colorKeys[0].color = tmpColor;
                colorKeys[0].time = 0;

                alphaKeys[0].alpha = 0;
                alphaKeys[0].time = 0;
                alphaKeys[1].alpha = alphaValue;
                alphaKeys[1].time = 0.5f;
                alphaKeys[2].alpha = 0;
                alphaKeys[2].time = 1;

                gradient.SetKeys(colorKeys, alphaKeys);

                col.color = gradient;
            }
        }
    }
    
    public void flash(bool playExplosionParticle = true, bool playCreationParticle = true)
    {
        if (playExplosionParticle)
        {
            explosionParticleSystem.Play();
        }
        if (playCreationParticle && !creationParticleObject.GetComponent<ParticleSystem>().isPlaying)
        {
            creationParticleObject.GetComponent<ParticleSystem>().Play();
        }
        flashing = true;
        flashStart = Time.time;
        staticParticleSystem.Stop();
        staticParticleSystem.Clear();
        staticParticleSystem.Play();
    }

    public void playTopParticle()
    {
        topParticleSystem.Play();
    }

    public void playBottomParticle()
    {
        bottomParticleSystem.Play();
    }

    public void updateFlashDuration(float gameSpeed)
    {
        flashDuration = 5/gameSpeed;
        staticMainModule.startDelay = 0/gameSpeed;
        creationParticleObject.GetComponent<CreationParticle>().updateFlashDuration(gameSpeed);
    }

    public void setupEchoAnimation()
    {
        ParticleSystem.ColorOverLifetimeModule col = creationParticleObject.GetComponent<ParticleSystem>().colorOverLifetime;

        Gradient gradient = new Gradient();
        GradientColorKey[] colorKeys = new GradientColorKey[1];
        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[3];

        colorKeys[0].color = Color.red;
        colorKeys[0].time = 0;

        alphaKeys[0].alpha = 0;
        alphaKeys[0].time = 0;
        alphaKeys[1].alpha = 0.5f;
        alphaKeys[1].time = 0.5f;
        alphaKeys[2].alpha = 0;
        alphaKeys[2].time = 1;

        gradient.SetKeys(colorKeys, alphaKeys);

        col.color = gradient;


        col = explosionParticleSystem.colorOverLifetime;

        gradient = new Gradient();
        colorKeys = new GradientColorKey[1];
        alphaKeys = new GradientAlphaKey[3];

        colorKeys[0].color = Color.red;
        colorKeys[0].time = 0;

        alphaKeys[0].alpha = 0;
        alphaKeys[0].time = 0;
        alphaKeys[1].alpha = 0.5f;
        alphaKeys[1].time = 0.5f;
        alphaKeys[2].alpha = 0;
        alphaKeys[2].time = 1;

        gradient.SetKeys(colorKeys, alphaKeys);

        col.color = gradient;


        col = staticParticleSystem.colorOverLifetime;

        gradient = new Gradient();
        colorKeys = new GradientColorKey[1];
        alphaKeys = new GradientAlphaKey[3];

        colorKeys[0].color = Color.red;
        colorKeys[0].time = 0;

        alphaKeys[0].alpha = 0;
        alphaKeys[0].time = 0;
        alphaKeys[1].alpha = 0.125f;
        alphaKeys[1].time = 0.125f;
        alphaKeys[2].alpha = 0;
        alphaKeys[2].time = 1;

        gradient.SetKeys(colorKeys, alphaKeys);

        col.color = gradient;
    }

    public void echoAnimation()
    {
        //staticParticleSystem.Stop();
        flash();
        //creationParticleObject.GetComponent<ParticleSystem>().Play();
    }

    public void stopEchoAnimation()
    {
        explosionParticleSystem.Play();
        staticParticleSystem.Stop();
        staticParticleSystem.Clear();
        creationParticleObject.GetComponent<ParticleSystem>().Stop();
        creationParticleObject.GetComponent<ParticleSystem>().Clear();
    }
    
}
