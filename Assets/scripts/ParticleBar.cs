using UnityEngine;

public class ParticleBar : MonoBehaviour
{
    private bool flashing = false;
    public float flashDuration = 0.5f;
    private float flashStart;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (flashing)
        {
            if (Time.time - flashStart >= flashDuration)
            {
                flashing = false;
            
                ParticleSystem.ColorOverLifetimeModule col = GetComponent<ParticleSystem>().colorOverLifetime;

                Gradient gradient = new Gradient();
                GradientColorKey[] colorKeys = new GradientColorKey[1];
                GradientAlphaKey[] alphaKeys = new GradientAlphaKey[3];

                colorKeys[0].color = Color.red;
                colorKeys[0].time = 0;

                alphaKeys[0].alpha = 0;
                alphaKeys[0].time = 0;
                alphaKeys[1].alpha = 1;
                alphaKeys[1].time = 0.5f;
                alphaKeys[2].alpha = 0;
                alphaKeys[2].time = 1;

                gradient.SetKeys(colorKeys, alphaKeys);

                col.color = gradient;
            }
            else
            {
                Color tmpColor = Color.Lerp(Color.white, Color.red, (Time.time - flashStart) / flashDuration);

                ParticleSystem.ColorOverLifetimeModule col = GetComponent<ParticleSystem>().colorOverLifetime;

                Gradient gradient = new Gradient();
                GradientColorKey[] colorKeys = new GradientColorKey[1];
                GradientAlphaKey[] alphaKeys = new GradientAlphaKey[3];

                colorKeys[0].color = tmpColor;
                colorKeys[0].time = 0;

                alphaKeys[0].alpha = 0;
                alphaKeys[0].time = 0;
                alphaKeys[1].alpha = 1;
                alphaKeys[1].time = 0.5f;
                alphaKeys[2].alpha = 0;
                alphaKeys[2].time = 1;

                gradient.SetKeys(colorKeys, alphaKeys);

                col.color = gradient;
            }
        }
    }

    public void flash()
    {
        flashing = true;
        flashStart = Time.time;
    }
}
