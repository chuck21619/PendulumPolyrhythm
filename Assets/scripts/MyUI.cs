using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MyUI : MonoBehaviour
{
    private Save _save;
    public Save save
    {
        get
        {
            return _save;
        }
        set
        {
            _save = value;
            octaveSlider.value = save.octave;
        }
    }

    private Wallet _wallet;
    public Wallet wallet
    {
        get
        {
            return _wallet;
        }
        set
        {
            _wallet = value;
            _wallet.bonkBucksChanged += bonkBucksChanged;
            bonkBucksChanged(this, _wallet.bonkBucks);
        }
    }
    private Store _store;
    public Store store
    {
        get
        {
            return _store;
        }
        set
        {
            _store = value;
            
            _store.numberOfSquaresChanged += numberOfSquaresChanged;

            _store.squaresMaxxedChanged += squaresMaxxedChanged;

            _store.instrumentsPurchasedChanged += instrumentsPurchasedChanged;
            instrumentsPurchasedChanged(null, _store.instrumentsPurchased);

            _store.scalesPurchasedChanged += scalesPurchasedChanged;
            scalesPurchasedChanged(null, _store.scalesPurchased);

            _store.creativeModePurchasedChanged += creativeModePurchasedChanged;
            creativeModePurchasedChanged(null, _store.creativeModePurchased);
        }
    }
    public TMP_Dropdown instrumentDropdown;
    [SerializeField] TMP_Dropdown scalesDropdown;
    [SerializeField] private Slider speedSlider;
    [SerializeField] private Slider octaveSlider;
    [SerializeField] private Slider numberOfSqauresSlider;
    [SerializeField] private TMP_Text bonkBucksLabel;
    [SerializeField] private TMP_Text bonkBucksAmountLabel;
    bool hiddenUI = true;
    public GameObject mainUI;
    private bool _squareIsPurchasable = false;
    public bool squareIsPurchasable
    {
        get
        {
            return _squareIsPurchasable;
        }
        set
        {
            _squareIsPurchasable = value;
            buySquareButton.interactable = value;
            buySquareLabel.color = value ? Color.white : Color.grey;
        }
    }
    public Button hideUIButton;
    [SerializeField] private Button buySquareButton;
    [SerializeField] private TMP_Text buySquareLabel;
    
    public event EventHandler resetButtonPressed;
    public event EventHandler<int> squareSliderChanged;
    [SerializeField] private GameObject confirmationPrefab;
    GameObject confirmationDialog;

    void Start()
    {
        confirmationDialog = Instantiate(confirmationPrefab);
        confirmationDialog.SetActive(false);
        confirmationDialog.GetComponent<ConfirmationDialog>().confirmation += resetAction;
        confirmationDialog.transform.SetParent(this.transform, false);
    }

    private void bonkBucksChanged(object sender, BigNumber bonkBucks)
    {
        bonkBucksAmountLabel.text = "" + bonkBucks;
        updateButtons(bonkBucks, store.squareCost);
    }

    private void instrumentsPurchasedChanged(object sender, bool unlocked)
    {
        instrumentDropdown.gameObject.SetActive(unlocked);
    }

    private void scalesPurchasedChanged(object sender, bool unlocked)
    {
        scalesDropdown.gameObject.SetActive(unlocked);
    }

    private void creativeModePurchasedChanged(object sender, bool unlocked)
    {
        speedSlider.gameObject.SetActive(unlocked);
        buySquareButton.gameObject.SetActive(!unlocked);
        bonkBucksAmountLabel.gameObject.SetActive(!unlocked);
        bonkBucksLabel.gameObject.SetActive(!unlocked);
        numberOfSqauresSlider.gameObject.SetActive(unlocked);
        if (unlocked && scalesDropdown.options.Count != 14 )
        {
            scalesDropdown.options.Add(new TMP_Dropdown.OptionData() {text="Custom"});
        }
        else if (!unlocked && scalesDropdown.options.Count == 14)
        {
            scalesDropdown.options.RemoveAt(13);
        }
    }

    private void squaresMaxxedChanged(object sender, bool maxxed)
    {
        checkSquareButton(wallet.bonkBucks, store.squareCost);
    }

    public void ToggleUI()
    {
        float xPosition = hiddenUI ? 0 : -1000;
        mainUI.transform.localPosition = new Vector3(xPosition, 0, 0);
        hideUIButton.GetComponent<RectTransform>().Rotate(0, 0, 180);
        hiddenUI = !hiddenUI;
    }

    public void updateButtons(BigNumber bonkBucks, int squareCost)
    {
        checkSquareButton(bonkBucks, squareCost);
        Color arrowColor = squareIsPurchasable ? Color.magenta : Color.white;
        hideUIButton.GetComponent<Image>().color = arrowColor;
    }

    private void checkSquareButton(BigNumber bonkBucks, int squareCost)
    {
        squareIsPurchasable = !store.squaresMaxxed && bonkBucks >= squareCost;
        buySquareLabel.text = store.squaresMaxxed ? "maxed" : "Square (" + squareCost + ")";
    }

    private void numberOfSquaresChanged(object sender, int numberOfSquares)
    {
        updateButtons(wallet.bonkBucks, store.squareCost);
    }
    
    public void numberOfSquaresSliderChanged(int value)
    {
        squareSliderChanged?.Invoke(this, value);
    }

    public void buySquarePressed()
    {
        store.buySquare();
    }

    public void resetPressed()
    {
        confirmationDialog.SetActive(true);
        //bigNumberTest();
    }

    public void resetAction(object sender,  bool confirmed)
    {
        confirmationDialog.SetActive(false);
        if (confirmed)
        {
            resetButtonPressed?.Invoke(this, EventArgs.Empty);
        }
    }
    
    public void quit()
    {
        Application.Quit();
    }

    public void bigNumberTest()
    {
        BigNumber testNumber = new BigNumber();
        testNumber.number = 100;
        Debug.Assert(testNumber.ToString() == "100", "100 failed");

        BigNumber testNumber2 = new BigNumber();
        testNumber2.number = 101;
        Debug.Assert(testNumber2.ToString() == "101", "101 failed");
        
        BigNumber testNumber3 = new BigNumber();
        testNumber3.number = 111111111;// 111,111,111
        Debug.Assert(testNumber3.ToString() == "111.1m", "100m failed");

        BigNumber resultNumber = testNumber + testNumber2;
        Debug.Assert(resultNumber.ToString() == "201", "201 failed");

        resultNumber = testNumber * testNumber2;
        Debug.Assert(resultNumber.ToString() == "10.1k", "10.1k failed");

        testNumber.number = 2000;
        resultNumber = testNumber - testNumber2;
        Debug.Assert(resultNumber.ToString() == "1.9k", "1.9k failed");

        resultNumber = testNumber + testNumber2;
        Debug.Assert(resultNumber.ToString() == "2.1k", "2.1k failed");

        resultNumber = testNumber2/100;
        Debug.Assert(resultNumber.ToString() == "1", "1 failed");

        resultNumber = testNumber3/100;
        Debug.Assert(resultNumber.ToString() == "1.1m", "1.1m failed");

        BigNumber testNumber4 = new BigNumber();
        testNumber4 += 1;
        Debug.Assert(!(testNumber4 >= 100), "greater than equals failed");

        BigNumber testNumber5 = new BigNumber(0, 1200);
        testNumber5 /= 100;
        Debug.Assert(testNumber5.ToString() == "12", "dividing to less than 1 failed");
        
        Debug.Log("tests finished");
    }
}
