using UnityEngine;
using Unity.Mathematics;

public class Bars : MonoBehaviour
{
    
    [SerializeField] private GameObject particleBarPrefab;
    private GameObject topBarParticle;
    private GameObject bottomBarParticle;
    public float particleDensity = 10;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        topBarParticle = Instantiate(particleBarPrefab, Vector3.zero, quaternion.identity);
        bottomBarParticle = Instantiate(particleBarPrefab, Vector3.zero, quaternion.identity);
    }

    public void sizeBars(float distanceBetweenBars, int numberOfSquares, float squareSpacingWidth, float firstSquareXPosition, GameObject rightCameraTarget)
    {
        ParticleSystem topParticleSystem = topBarParticle.GetComponent<ParticleSystem>();
        ParticleSystem bottomParticleSystem = bottomBarParticle.GetComponent<ParticleSystem>();
        var topShapeModule = topParticleSystem.shape;
        var bottomShapeModule = bottomParticleSystem.shape;
        float barHeight = topShapeModule.scale.y;
        float topBarYPosition = (barHeight/2) + distanceBetweenBars;
        float barWidth = squareSpacingWidth * numberOfSquares;
        topShapeModule.scale = new Vector3(barWidth, topShapeModule.scale.y, topShapeModule.scale.z);
        bottomShapeModule.scale = new Vector3(barWidth, bottomShapeModule.scale.y, bottomShapeModule.scale.z);
        float xPosition = firstSquareXPosition + barWidth/2;
        topBarParticle.transform.position = new Vector3(xPosition, topBarYPosition, topBarParticle.transform.position.z);
        bottomBarParticle.transform.position = new Vector3(xPosition, -barHeight/2, bottomBarParticle.transform.position.z);
        var topParticleEmission = topParticleSystem.emission;
        var bottomParticleEmission = bottomParticleSystem.emission;
        topParticleEmission.rateOverTime = barWidth * particleDensity;
        bottomParticleEmission.rateOverTime = barWidth * particleDensity;

        rightCameraTarget.GetComponent<Renderer>().transform.position = new Vector3(firstSquareXPosition + barWidth, topBarYPosition, 0);
    }

    public void flashTopBar()
    {
        topBarParticle.GetComponent<ParticleBar>().flash();
    }

    public void flashBottomBar()
    {
        bottomBarParticle.GetComponent<ParticleBar>().flash();
    }
}
