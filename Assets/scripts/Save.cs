using System;
using UnityEngine;

public class Save
{
    const string bonkBucksKey = "bonkBucks";
    const string bonkBucksUnitKey = "bonkBucksUnit";
    const string prestigeBucksKey = "prestigeBucks";
    const string prestigeBucksUnitKey = "prestigeBucksUnit";
    const string numberOfSquaresKey = "numberOfSquaresKey";
    const string instrumentIndexKey = "instrumentIndex";
    const string scaleKey = "scale";
    const string octaveKey = "octave";
    const string volumeKey = "volume";

    const string echoSquaresKey = "echoSquares";
    const string autoPurchaseSquaresKey = "autoPurchaseSquares";
    const string autoPurchaseSquaresActiveKey = "autoPurchaseSquaresActive";
    const string instrumentsKey = "instruments";
    const string scalesKey = "scales";
    const string creativeKey = "creative";
    const string customScaleKey = "customScale";

    public int octave
    {
        get
        {
            return PlayerPrefs.GetInt(octaveKey);
        }
        set
        {
            PlayerPrefs.SetInt(octaveKey, value);
        }
    }

    public bool echoSquares
    {
        get
        {
            return PlayerPrefs.GetInt(echoSquaresKey) != 0;
        }
        set
        {
            PlayerPrefs.SetInt(echoSquaresKey, value == false ? 0 : 1);
        }
    }

    public int numberOfSquares
    {
        get
        {
            return PlayerPrefs.GetInt(numberOfSquaresKey);
        }
        set
        {   
            PlayerPrefs.SetInt(numberOfSquaresKey, value);
        }
    }

    public float volume
    {
        get
        {
            return PlayerPrefs.GetFloat(volumeKey, 1);
        }
        set
        {
            PlayerPrefs.SetFloat(volumeKey, value);
        }
    }

    public bool autoPurchaseSquares
    {
        get
        {
            return PlayerPrefs.GetInt(autoPurchaseSquaresKey) != 0;
        }
        set
        {
            PlayerPrefs.SetInt(autoPurchaseSquaresKey, value == false ? 0 : 1);
        }
    }

    public bool autoPurchaseSquaresActive
    {
        get
        {
            return PlayerPrefs.GetInt(autoPurchaseSquaresActiveKey) != 0;
        }
        set
        {
            PlayerPrefs.SetInt(autoPurchaseSquaresActiveKey, value == false ? 0 : 1);
        }
    }

    public bool instruments
    {
        get
        {
            return PlayerPrefs.GetInt(instrumentsKey) != 0;
        }
        set
        {
            PlayerPrefs.SetInt(instrumentsKey, value == false ? 0 : 1);
        }
    }

    public bool scales
    {
        get
        {
            return PlayerPrefs.GetInt(scalesKey) != 0;
        }
        set
        {
            PlayerPrefs.SetInt(scalesKey, value == false ? 0 : 1);
        }
    }

    public bool creativeMode
    {
        get
        {
            return PlayerPrefs.GetInt(creativeKey) != 0;
        }
        set
        {
            PlayerPrefs.SetInt(creativeKey, value == false ? 0 : 1);
        }
    }

    public int scale
    {
        get
        {
            return PlayerPrefs.GetInt(scaleKey);
        }
        set
        {
            PlayerPrefs.SetInt(scaleKey, value);
        }
    }

    public BigNumber prestigeBucks
    {
        get
        {
            BigNumber newNumber = new BigNumber();
            newNumber.number = PlayerPrefs.GetFloat(prestigeBucksKey);
            newNumber.unitIndex = PlayerPrefs.GetInt(prestigeBucksUnitKey);
            return newNumber;
        }
        set
        {
            PlayerPrefs.SetFloat(prestigeBucksKey, value.number);
            PlayerPrefs.SetInt(prestigeBucksUnitKey, value.unitIndex);
        }
    }

    public BigNumber bonkBucks
    {
        get
        {
            BigNumber newNumber = new BigNumber();
            newNumber.number = PlayerPrefs.GetFloat(bonkBucksKey);
            newNumber.unitIndex = PlayerPrefs.GetInt(bonkBucksUnitKey);
            return newNumber;
        }
        set
        {
            PlayerPrefs.SetFloat(bonkBucksKey, value.number);
            PlayerPrefs.SetInt(bonkBucksUnitKey, value.unitIndex);
        }
    }

    public int instrument
    {
        get
        {
            return PlayerPrefs.GetInt(instrumentIndexKey);
        }
        set
        {
            PlayerPrefs.SetInt(instrumentIndexKey, value);
        }
    }

    public int[] customScale
    {
        get
        {
            string scaleString = PlayerPrefs.GetString(customScaleKey, "0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0");
            int[] scale = Array.ConvertAll(scaleString.Split(','), int.Parse);
            return scale;
        }
        set
        {
            PlayerPrefs.SetString(customScaleKey, string.Join(",", value));
        }
    }
    
    public void resetSave()
    {
        prestigeBucks = new BigNumber();
        bonkBucks = new BigNumber();
        numberOfSquares = 0;
        instrument = 0;
        
        echoSquares = false;
        autoPurchaseSquares = false;
        instruments = false;
        scales = false;
        creativeMode = false;
    }
}
