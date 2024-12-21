using TMPro;
using UnityEngine;
using System;

public class NoteSelector : MonoBehaviour
{
    [SerializeField] private TMP_Text pitchIndexLabel;
    
    public int squareIndex = 0;

    public event EventHandler<int> changePitchUp;
    public event EventHandler<int> changePitchDown;

    public void upPressed()
    {
        changePitchUp?.Invoke(this, squareIndex);
    }

    public void downPressed()
    {
        changePitchDown?.Invoke(this, squareIndex);
    }

    public void updatePitchIndexLabel(object sender, int[] pitches)
    {
        pitchIndexLabel.text = "" + pitches[squareIndex];
    }
}
