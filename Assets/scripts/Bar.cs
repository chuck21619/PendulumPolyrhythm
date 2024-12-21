using UnityEngine;

public class Bar : MonoBehaviour
{
    private bool flashing = false;
    public float flashDuration = 0.5f;
    private float flashStart;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (flashing && Time.time - flashStart >= flashDuration)
        {
            flashing = false;
            //set color to white
        }
        else //if (flashing) // untested
        {
            Color tmpColor = Color.Lerp(Color.white, Color.red, (Time.time - flashStart) / flashDuration);
            GetComponent<SpriteRenderer>().color = tmpColor;
        }
    }

    public void flash()
    {
        flashing = true;
        flashStart = Time.time;
    }
}
