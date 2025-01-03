using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SliderDrag : MonoBehaviour, IPointerUpHandler
{
    public SceneController sceneController;
    public MyUI myUI;

    public void OnPointerUp(PointerEventData eventData)
    { 
        if (transform.name == "speed")
        {
            Debug.Log("change speed");
            sceneController.speedSliderChanged(gameObject.GetComponent<Slider>().value);
        }
        else if (transform.name == "pitch")
        {
            Debug.Log("change octave");
            sceneController.octaveSliderChanged((int)GetComponent<Slider>().value);
        }
        else if (transform.name == "numberOfSquares")
        {
            Debug.Log("change # of squares");
            myUI.numberOfSquaresSliderChanged((int)GetComponent<Slider>().value);
        }
        else if (transform.name == "volume")
        {
            Debug.Log("volume");
            myUI.volumeSliderChanged(GetComponent<Slider>().value);
        }
    }
}
