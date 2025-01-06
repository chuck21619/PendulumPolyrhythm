using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using Unity.Mathematics;
using System;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEditor;
using System.Linq;

public class SceneController : MonoBehaviour
{
    private Save save;
    private Wallet wallet;
    private float _volume = 1;
    private float volume
    {
        get
        {
            return _volume;
        }
        set
        {
            _volume = value;
            AudioListener.volume = value;
        }
    }

    //gameplay
    [SerializeField] private MyUI myUI;
    [SerializeField] private PrestigeUI prestigeUI;
    private Store _store;
    private Store store
    {
        get
        {
            return _store;
        }
        set
        {
            _store = value;
            _store.numberOfSquaresChanged += numberOfSquaresChanged;
            _store.didPrestige += didPrestige;
        }
    }

    //camera
    public GameObject cameraTargetObject;
    public GameObject leftCameraTarget;
    public GameObject rightCameraTarget;

    //create squares progamattically
    public GameObject squarePrefab;
    public float squareSpacingPercentage = 0.2f;
    private float firstSquareXPosition = 0;
    private float adjustedFirstSquareXPosition = 0;
    List<GameObject> squares = new List<GameObject>();
    List<GameObject> echoSquares = new List<GameObject>();
    private float squareSpacingWidth;

    [SerializeField] private Bars bars;
    public float distanceBetweenBars = 1;

    public GameObject instruments;
    public TMP_Dropdown instrumentDropdown;
    public TMP_Dropdown scaleDropdown;
    public Slider speedSlider;
    private int octaveIndex = 0;
    [SerializeField] private GameObject customScaleUI;
    [SerializeField] private GameObject noteSelectorPrefab;

    private float _speed = 1;
    private float speed
    {
        get
        {
            return _speed;
        }
        set
        {
            _speed = value;
            speedWasChanged = true;
            updateFlashDuration();
        }
    }
    private bool speedWasChanged = false;

    List<float> coefficients = new List<float>();
    List<bool?> wasGoingDown = new List<bool?>();
    List<float> positions = new List<float>();

    List<bool?> echoWasGoingDown = new List<bool?>();
    List<float> echoPositions = new List<float>();
    
    private static int[] pentatonicMajor = new int[] {0, 2, 4, 7, 9,12,14,16,19,21,24,26,28,31,33,36,38,40,43,45};
    private static int[] pentatonicMinor = new int[] {0, 3, 5, 7,10,12,15,17,19,22,24,27,29,31,34,36,39,41,43,46};
    private static int[] harmonics = new int[]       {0, 3, 4, 5, 7, 9,12,15,16,17,19,21,24,27,28,29,31,33,36,39};
    private static int[] harmonicMajor = new int[]   {0, 2, 4, 5, 7, 8,11,12,14,16,17,19,20,23,24,26,28,29,31,32};
    private static int[] harmonicMinor = new int[]   {0, 2, 3, 5, 7, 8,11,12,14,15,17,19,20,23,24,26,27,29,31,32};
    private static int[] blues = new int[]           {0, 3, 5, 6, 7,10,12,15,17,18,19,22,24,27,29,30,31,34,36,39};
    private static int[] lydian = new int[]          {0, 2, 4, 6, 7, 9,11,12,14,16,18,19,21,23,24,26,28,30,31,33};
    private static int[] ionian = new int[]          {0, 2, 4, 5, 7, 9,11,12,14,16,17,19,21,23,24,26,28,29,31,33};
    private static int[] mixolydian = new int[]      {0, 2, 4, 5, 7, 9,10,12,14,16,17,19,21,22,24,26,28,29,31,33};
    private static int[] dorian = new int[]          {0, 2, 3, 5, 7, 9,10,12,14,15,17,19,21,22,24,26,27,29,31,33};
    private static int[] aeolian = new int[]         {0, 2, 3, 5, 7, 8,10,12,14,15,17,19,20,22,24,26,27,29,31,32};
    private static int[] phrygian = new int[]        {0, 1, 3, 5, 7, 8,10,12,13,15,17,19,20,22,24,25,27,29,31,32};
    private static int[] hungarian = new int[]       {0, 3, 4, 6, 7, 9,10,12,15,16,18,19,21,22,24,27,28,30,31,33};
    private static int[] customPowers = new int[]    {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};

    public event EventHandler<int[]> customPowersChanged;

    private int[][] scalePowers = {pentatonicMajor, pentatonicMinor, harmonics, harmonicMajor, harmonicMinor, blues, lydian, ionian, mixolydian, dorian, aeolian, phrygian, hungarian, customPowers};
    private List<GameObject> noteSelectors = new List<GameObject>();

    private float timeSinceStart;
    private float speedChangeTimeDifference = 0;
    private float distanceTraveled = 0;
    private float echoDistanceTraveled = 0;
    private float firstSquareDistanceTraveled = 0;
    private float remainder;
    private float echoRemainder;
    private bool goingDown;
    private bool echoGoingDown;
    public float echoAmount = 0.05f;

    void Start()
    {
        Application.targetFrameRate = 100;
        save = new Save();
        volume = save.volume;
        wallet = new Wallet(save);
        store = new Store(wallet, save);
        
        store.echoSquaresActiveChanged += echoSquaresToggleChanged;

        //resetSave();
        myUI.save = save;
        myUI.store = store;
        myUI.wallet = wallet;
        myUI.resetButtonPressed += resetPressed;
        myUI.squareSliderChanged += store.squareSliderChanged;
        myUI.updateVolume += updateVolume;
        prestigeUI.setWalletAndStore(wallet, store);

        instrumentDropdown.value = save.instrument;
        scaleDropdown.value = save.scale;
        int[] savedScale = save.customScale;
        for (int i = 0; i < customPowers.Length; i++)
        {
            customPowers[i] = savedScale[i];
        }
        customScaleUI.SetActive(scaleDropdown.value == 13);
        octaveIndex = save.octave;
        myUI.updateButtons(wallet.bonkBucks, store.squareCost);
        createSquares();
        createNoteSelectors();
        hideExcessNoteSelectors();
        bars.sizeBars(distanceBetweenBars, store.numberOfSquares, squareSpacingWidth, firstSquareXPosition, rightCameraTarget);
        
        //wallet.bonkBucks = new BigNumber(0, 80000);
        //wallet.prestigeBucks = new BigNumber(0, 90000);
    }

    private void OnApplicationQuit()
    {
        save.numberOfSquares = store.numberOfSquares;
        save.instrument = instrumentDropdown.value;
        save.scale = scaleDropdown.value;
        save.octave = octaveIndex;
        save.bonkBucks = wallet.bonkBucks;
        save.prestigeBucks = wallet.prestigeBucks;
        save.volume = volume;

        save.echoSquares = store.echoSquaresPurchased;
        save.instruments = store.instrumentsPurchased;
        save.autoPurchaseSquares = store.autoPurchaseSquaresPurchased;
        save.autoPurchaseSquaresActive = store.autoPurchaseSquaresActive;
        save.scales = store.scalesPurchased;
        save.creativeMode = store.creativeModePurchased;

        save.customScale = customPowers;
    }

    private void resetPressed(object sender, EventArgs _)
    {
        resetBoard();
        resetSave();
    }

    private void updateVolume(object sender, float value)
    {
        volume = value;
    }

    private void resetSave()
    {
        save.resetSave();
        wallet.empty();
        store.resetStore();
        instrumentDropdown.value = 0;
        scaleDropdown.value = 0;
        speedSlider.value = 1;
    }

    private void didPrestige(object sender, EventArgs _)
    {
        resetBoard();
    }

    private void numberOfSquaresChanged(object sender, int numberOfSquares)
    {
        int difference = numberOfSquares - squares.Count;
        if (difference < 0)
        {
            resetBoard(false);
        }
        int squaresToAdd = difference < 0 ? numberOfSquares : difference;

        for (int i = 0; i < squaresToAdd; i++)
        {
            addSquareGracefully();
        }

        bars.sizeBars(distanceBetweenBars, store.numberOfSquares, squareSpacingWidth, firstSquareXPosition, rightCameraTarget);
        hideExcessNoteSelectors();
    }

    private void hideExcessNoteSelectors()
    {
        for (int i = 0; i < noteSelectors.Count; i++)
        {
            noteSelectors[i].SetActive(i < store.numberOfSquares);
        }
    }

    public void buyEchoSquaresButtonPressed()
    {
        //switch this method to the store and add a callback here
        int tmpNumberOfSquares = store.numberOfSquares;
        resetBoard();
        store.echoSquaresPurchased = true;
        wallet.prestigeBucks -= 100;
        for (int i = 0; i < tmpNumberOfSquares; i++)
        {
            store.buySquare();
        }
        //store.numberOfSquares = tmpNumberOfSquares;
        //createSquares();
        bars.sizeBars(distanceBetweenBars, store.numberOfSquares, squareSpacingWidth, firstSquareXPosition, rightCameraTarget);
    }

    void updateFlashDuration()
    {
        foreach (GameObject square in squares)
        {
            square.GetComponent<squareScript>().updateFlashDuration(speed);
        }
    }

    void resetBoard(bool updateStore = true)
    {
        foreach (GameObject square in squares)
        {
            Destroy(square);
        }
        foreach (GameObject echoSquare in echoSquares)
        {
            Destroy(echoSquare);
        }
        squares.Clear();
        coefficients.Clear();
        wasGoingDown.Clear();
        positions.Clear();
        echoSquares.Clear();
        echoWasGoingDown.Clear();
        echoPositions.Clear();
        if (updateStore)
        {
            store.numberOfSquares = 0;
        }
    }

    void createNoteSelectors()
    {
        for (int i = 0; i < store.maxNumberOfSquares; i++)
        {
            GameObject noteSelector = Instantiate(noteSelectorPrefab, new Vector3(adjustedFirstSquareXPosition + (i * squareSpacingWidth), -2, 0), quaternion.identity);
            noteSelectors.Add(noteSelector);
            noteSelector.transform.SetParent(customScaleUI.transform);
            noteSelector.GetComponent<NoteSelector>().squareIndex = i;
            noteSelector.GetComponent<NoteSelector>().changePitchUp += changePitchUp;
            noteSelector.GetComponent<NoteSelector>().changePitchDown += changePitchDown;
            customPowersChanged += noteSelector.GetComponent<NoteSelector>().updatePitchIndexLabel; 
        }
        customPowersChanged?.Invoke(this, customPowers);
    }

    void changePitchUp(object sender, int squareIndex)
    {
        customPowers[squareIndex]++;
        customPowersChanged?.Invoke(this, customPowers);
        setAudioPitches(squareIndex);
    }

    void changePitchDown(object sender, int squareIndex)
    {
        customPowers[squareIndex]--;
        customPowersChanged?.Invoke(this, customPowers);
        setAudioPitches(squareIndex);
    }

    void createSquares()
    {
        for(int i = 0; i < store.numberOfSquares; i++)
        {
            coefficients.Add(1-i*0.01f);
            wasGoingDown.Add(null);
            positions.Add(0);
        }
        Vector3 squareSize = squarePrefab.transform.GetComponent<Renderer>().bounds.size;
        float squareWidth = squareSize.x;
        float squareHeight = squareSize.y;
        float yPosition = squareHeight/2;
        adjustedFirstSquareXPosition = firstSquareXPosition + squareWidth/2;
        float squareGapWidth = squareWidth*squareSpacingPercentage;
        squareSpacingWidth = squareWidth + squareWidth*squareGapWidth;
        for (int i = 0; i < store.numberOfSquares; i++)
        {
            GameObject newSquare = Instantiate(squarePrefab, new Vector3(adjustedFirstSquareXPosition + (i * squareSpacingWidth), yPosition, 0), quaternion.identity);
            squares.Add(newSquare);
            newSquare.GetComponent<squareScript>().flash(false);
        
            if (store.echoSquaresPurchased)
            {
                echoWasGoingDown.Add(null);
                echoPositions.Add(0);
                GameObject newEchoSquare = Instantiate(squarePrefab, new Vector3(adjustedFirstSquareXPosition + (i * squareSpacingWidth), yPosition, 0), quaternion.identity);
                echoSquares.Add(newEchoSquare);
                newEchoSquare.GetComponent<squareScript>().isEcho = true;
                newEchoSquare.GetComponent<squareScript>().setupEchoAnimation();
            }
        }

        setAudioPitches();
        setAudioSources();
    }

    void addSquareGracefully()
    {
        bars.sizeBars(distanceBetweenBars, store.numberOfSquares, squareSpacingWidth, firstSquareXPosition, rightCameraTarget);
        int i = squares.Count;
        coefficients.Add(1-i*0.01f);
        wasGoingDown.Add(null);
        positions.Add(0);
        Vector3 squareSize = squarePrefab.transform.GetComponent<Renderer>().bounds.size;
        float squareWidth = squareSize.x;
        float squareHeight = squareSize.y;
        float yPosition = squareHeight/2;
        float adjustedFirstSquareXPosition = firstSquareXPosition + squareWidth/2;
        float squareGapWidth = squareWidth*squareSpacingPercentage;
        squareSpacingWidth = squareWidth + squareWidth*squareGapWidth;
        GameObject newSquare = Instantiate(squarePrefab, new Vector3(adjustedFirstSquareXPosition + (i * squareSpacingWidth), yPosition, 0), quaternion.identity);
        squares.Add(newSquare);
        newSquare.GetComponent<squareScript>().flash(false, true);
        
        if (store.echoSquaresPurchased)
        {
            echoWasGoingDown.Add(null);
            echoPositions.Add(0);
            GameObject newEchoSquare = Instantiate(squarePrefab, new Vector3(adjustedFirstSquareXPosition + (i * squareSpacingWidth), yPosition, 0), quaternion.identity);
            newEchoSquare.SetActive(store.echoSquaresActive);
            echoSquares.Add(newEchoSquare);
            newEchoSquare.GetComponent<squareScript>().isEcho = true;
            newEchoSquare.GetComponent<squareScript>().setupEchoAnimation();
        }
        
        setAudioPitches(i);
        setAudioSources(i);
    }
    
    void Update()
    {
        timeSinceStart = Time.realtimeSinceStartup - speedChangeTimeDifference;
        for(int i = 0; i < store.numberOfSquares; i++)
        {
            Transform square = squares[i].transform;

            //gracefully handle speed change
            if (speedWasChanged)
            {
                speedWasChanged = false;
                float adjustedTime = (firstSquareDistanceTraveled / speed) / coefficients[i];
                speedChangeTimeDifference = Time.realtimeSinceStartup - adjustedTime;
                timeSinceStart = adjustedTime;
            }
            
            distanceTraveled = timeSinceStart * coefficients[i] * speed;
            remainder = distanceTraveled % distanceBetweenBars;
            goingDown = (int)(distanceTraveled / distanceBetweenBars) % 2 != 0;

            echoDistanceTraveled = timeSinceStart * (coefficients[i] - echoAmount) * speed;
            echoRemainder = echoDistanceTraveled % distanceBetweenBars;
            echoGoingDown = (int)(echoDistanceTraveled / distanceBetweenBars) % 2 != 0;

            if (i == 0)
            {
                firstSquareDistanceTraveled = distanceTraveled;
            }

            if (wasGoingDown[i] != null && wasGoingDown[i] != goingDown)
            {
                square.GetComponent<AudioSource>().Play();
                if (!store.creativeModePurchased)
                {
                    wallet.bonkBucks += wallet.prestigeBucks+1;
                }
                squareScript tmpSquareScript = square.GetComponent<squareScript>();
                tmpSquareScript.flash();

                if (store.echoSquaresPurchased)
                {
                    squareScript echoSquareScript = echoSquares[i].transform.GetComponent<squareScript>();
                    echoSquareScript.echoAnimation(false, true);
                }
                
                if (goingDown)
                {
                    tmpSquareScript.playTopParticle();
                    bars.flashTopBar();
                }
                else
                {
                    tmpSquareScript.playBottomParticle();
                    bars.flashBottomBar();
                }
            }

            if (store.echoSquaresPurchased && store.echoSquaresActive)
            {
                Transform echoSquare = echoSquares[i].transform;

                if (echoWasGoingDown[i] != null && echoWasGoingDown[i] != echoGoingDown)
                {
                    echoSquare.GetComponent<AudioSource>().Play();
                    if (!store.creativeModePurchased)
                    {
                        wallet.bonkBucks += wallet.prestigeBucks+1;
                    }
                    squareScript tmpSquareScript = echoSquare.GetComponent<squareScript>();
                    tmpSquareScript.stopEchoAnimation();

                    if (echoGoingDown)
                    {
                        tmpSquareScript.playTopParticle();
                        bars.flashTopBar();
                    }
                    else
                    {
                        tmpSquareScript.playBottomParticle();
                        bars.flashBottomBar();
                    }
                }
            }

            wasGoingDown[i] = goingDown;
            positions[i] = goingDown ? distanceBetweenBars - remainder : remainder;
            square.position = new Vector2(square.position.x, positions[i]);
            
            if (store.echoSquaresPurchased)
            {
                echoWasGoingDown[i] = echoGoingDown;
                echoPositions[i] = echoGoingDown ? distanceBetweenBars - echoRemainder : echoRemainder;
                Transform echoSquare = echoSquares[i].transform;
                echoSquare.position = new Vector2(echoSquare.position.x, echoPositions[i]);
            }
        }
    }

    public void instrumentChanged()
    {
        setAudioSources();
    }

    public void scaleChanged()
    {
        customScaleUI.SetActive(scaleDropdown.value == 13);
        setAudioPitches();
    }

    private void setAudioSources(int index = -1)
    {
        int instrumentIndex = instrumentDropdown.value;
        AudioClip tmpAudioClip = instruments.transform.GetChild(instrumentIndex).GetComponent<AudioSource>().clip;

        if (index == -1)
        {
            foreach (GameObject square in squares)
            {
                square.GetComponent<AudioSource>().clip = tmpAudioClip;
            }

            foreach (GameObject square in echoSquares)
            {
                square.GetComponent<AudioSource>().clip = tmpAudioClip;
                square.GetComponent<AudioSource>().volume = 0.1f;
            }
        }
        else
        {
            squares[index].GetComponent<AudioSource>().clip = tmpAudioClip;

            if (store.echoSquaresPurchased)
            {
                echoSquares[index].GetComponent<AudioSource>().clip = tmpAudioClip;
                echoSquares[index].GetComponent<AudioSource>().volume = 0.1f;
            }
        }
    }

    private void setAudioPitches(int index = -1)
    {
        int scaleIndex = scaleDropdown.value;
        int[] powers = scalePowers[scaleIndex];

        if (index == -1)
        {
            for(int i = 0; i < squares.Count; i++)
            {            
                Transform square = squares[i].transform;
                var audioSource = square.GetComponent<AudioSource>();
                audioSource.pitch = Mathf.Pow(2, (powers[i]+octaveIndex)/12.0f);

                if (store.echoSquaresPurchased)
                {
                    Transform echoSquare = echoSquares[i].transform;
                    var echoAudioSource = echoSquare.GetComponent<AudioSource>();
                    echoAudioSource.pitch = Mathf.Pow(2, (powers[i]+octaveIndex)/12.0f);
                }
            }
        }
        else
        {
            int i = index;
            Transform square = squares[i].transform;
            var audioSource = square.GetComponent<AudioSource>();
            audioSource.pitch = Mathf.Pow(2, (powers[i]+octaveIndex)/12.0f);

            if (store.echoSquaresPurchased)
            {
                Transform echoSquare = echoSquares[i].transform;
                var echoAudioSource = echoSquare.GetComponent<AudioSource>();
                echoAudioSource.pitch = Mathf.Pow(2, (powers[i]+octaveIndex)/12.0f);
            }
        }
    }

    public void speedSliderChanged(float value)
    {
        speed = value;
    }

    public void octaveSliderChanged(int value)
    {
        octaveIndex = value;
        setAudioPitches();
    }

    private void echoSquaresToggleChanged(object sender, bool echoSquaresActive)
    {
        foreach (GameObject echoSquare in echoSquares)
        {
            echoSquare.SetActive(echoSquaresActive);
        }
    }
}
