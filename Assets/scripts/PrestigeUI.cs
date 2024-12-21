using TMPro;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.UI;

public class PrestigeUI : MonoBehaviour
{
    bool hiddenUI = true;
    public Button hideUIButton;
    [SerializeField] private Button prestigeButton;
    [SerializeField] private TMP_Text prestigeLabel;

    [SerializeField] private Button echoBlocksButton;
    [SerializeField] private TMP_Text echoBlocksLabel;
    [SerializeField] private Toggle echoSquaresToggle;

    [SerializeField] private Button autoPurchaseButton;
    [SerializeField] private TMP_Text autoPurchaseLabel;
    [SerializeField] private Toggle autoPurchaseToggle;

    [SerializeField] private TMP_Text prestigeBucksLabel;
    [SerializeField] private TMP_Text prestigeBucksAmountLabel;

    [SerializeField] private TMP_Text potentialPrestigeBucksLabel;
    [SerializeField] private TMP_Text potentialPrestigeBucksAmountLabel;

    [SerializeField] private Button instrumentsButton;
    [SerializeField] private TMP_Text instrumentsLabel;

    [SerializeField] private Button scalesButton;
    [SerializeField] private TMP_Text scalesLabel;

    [SerializeField] private Button creativeModeButton;
    [SerializeField] private TMP_Text creativeModeLabel;

    private bool _prestigeIsPurchaseable = false;
    public bool prestigeIsPurchaseable
    {
        get
        {
            return _prestigeIsPurchaseable;
        }
        set
        {
            _prestigeIsPurchaseable = value;
            prestigeButton.interactable = value;
            prestigeLabel.color = value ? Color.white : Color.grey;
        }
    }
    private bool _echoBlocksIsPurchasable = false;
    public bool echoBlocksIsPurchasable
    {
        get
        {
            return _echoBlocksIsPurchasable;
        }
        set
        {
            _echoBlocksIsPurchasable = value;
            echoBlocksButton.interactable = value;
            echoBlocksLabel.color = value ? Color.white : Color.gray;
        }
    }
    private bool _autoPurchaseSquaresIsPurchasable = false;
    public bool autoPurchaseSquaresIsPurchasable
    {
        get
        {
            return _autoPurchaseSquaresIsPurchasable;
        }
        set
        {
            _autoPurchaseSquaresIsPurchasable = value;
            autoPurchaseButton.interactable = value;
            autoPurchaseLabel.color = value ? Color.white : Color.gray;
        }
    }
    private bool _instrumentsIsPurchasable = false;
    public bool instrumentsIsPurchasable
    {
        get
        {
            return _instrumentsIsPurchasable;
        }
        set
        {
            _instrumentsIsPurchasable = value;
            instrumentsButton.interactable = value;
            instrumentsLabel.color = value ? Color.white : Color.gray;
        }
    }
    private bool _scalesIsPurchasable = false;
    public bool scalesIsPurchasable
    {
        get
        {
            return _scalesIsPurchasable;
        }
        set
        {
            _scalesIsPurchasable = value;
            scalesButton.interactable = value;
            scalesLabel.color = value ? Color.white : Color.gray;
        }
    }
    private bool _creativeModeIsPurchasable = false;
    public bool creativeModeIsPurchasable
    {
        get
        {
            return _creativeModeIsPurchasable;
        }
        set
        {
            _creativeModeIsPurchasable = value;
            creativeModeButton.interactable = value;
            creativeModeLabel.color = value ? Color.white : Color.gray;
        }
    }
    private Store store;
    private Wallet wallet;

    public void setWalletAndStore(Wallet _wallet, Store _store)
    {
        wallet = _wallet;
        store = _store;
        
        wallet.prestigeBucksChanged += prestigeBucksChanged;
        prestigeBucksChanged(this, _wallet.prestigeBucks);

        wallet.potentialPrestigeBucksChanged += potentialPrestigeBucksChanged;
        potentialPrestigeBucksChanged(this, _wallet.potentialPrestigeBucks);

        store.echoSquaresPurchasedChanged += echoSquaresPurchasedChanged;
        echoSquaresPurchasedChanged(null, store.echoSquaresPurchased);

        store.autoPurchaseSquaresChanged += autoPurchaseSquaresChanged;
        autoPurchaseSquaresChanged(null, store.autoPurchaseSquaresPurchased);
        autoPurchaseToggle.isOn = _store.autoPurchaseSquaresActive;

        store.instrumentsPurchasedChanged += instrumentsPurchasedChanged;
        instrumentsPurchasedChanged(null, store.instrumentsPurchased);

        store.scalesPurchasedChanged += scalesPurchasedChanged;
        scalesPurchasedChanged(null, store.scalesPurchased);

        store.creativeModePurchasedChanged += creativeModePurchasedChanged;
        creativeModePurchasedChanged(null, store.creativeModePurchased);
    }

    private void prestigeBucksChanged(object sender, BigNumber prestigeBucks)
    {
        prestigeBucksAmountLabel.text = "" + prestigeBucks;
        checkEchoBlocksPurchasable();
        checkInstrumentsPurchasable();
        checkAutoPurchaseSquaresPurchasable();
        checkScalesPurchasable();
        checkCreativeModePurchasable();
    }

    private void checkEchoBlocksPurchasable()
    {
        echoBlocksIsPurchasable = !store.echoSquaresPurchased && wallet.prestigeBucks >= 100;
    }

    private void checkInstrumentsPurchasable()
    {
        instrumentsIsPurchasable = !store.instrumentsPurchased && wallet.prestigeBucks >= 5;
    }

    private void checkScalesPurchasable()
    {
        scalesIsPurchasable = !store.scalesPurchased && wallet.prestigeBucks >= 10;
    }

    private void checkAutoPurchaseSquaresPurchasable()
    {
        autoPurchaseSquaresIsPurchasable = !store.autoPurchaseSquaresPurchased && wallet.prestigeBucks >= 1;
    }

    private void checkCreativeModePurchasable()
    {
        creativeModeIsPurchasable = !store.creativeModePurchased && wallet.prestigeBucks >= 10000;
    }

    private void potentialPrestigeBucksChanged(object sender, BigNumber potentialPrestigeBucks)
    {
        potentialPrestigeBucksAmountLabel.text = "" + potentialPrestigeBucks;//.roundedDown();
        prestigeIsPurchaseable = potentialPrestigeBucks >= 1;
        hideUIButton.image.color = potentialPrestigeBucks >= 1 ? Color.green : Color.white;
        bool somethingIsPurchasableAfterPrestige = instrumentsIsPurchasable || scalesIsPurchasable || autoPurchaseSquaresIsPurchasable || echoBlocksIsPurchasable || creativeModeIsPurchasable;
        hideUIButton.image.color = somethingIsPurchasableAfterPrestige ? Color.magenta : hideUIButton.image.color;
    }

    public void ToggleUI()
    {
        float xPosition = hiddenUI ? 0 : 4000;
        transform.localPosition = new Vector3(xPosition, 0, 0);
        hideUIButton.GetComponent<RectTransform>().Rotate(0, 0, 180);
        hiddenUI = !hiddenUI;
    }
    
    public void prestigeButtonPressed()
    {
        store.prestige();
    }

    public void autoPurchaseSquaresPressed()
    {
        store.autoPurchaseSquaresPurchased = true;
        wallet.prestigeBucks -= 1;
    }

    public void instrumentsPressed()
    {
        wallet.prestigeBucks -= 5;
        store.instrumentsPurchased = true;
    }

    public void scalesPressed()
    {
        wallet.prestigeBucks -= 10;
        store.scalesPurchased = true;
    }

    public void creativeModePressed()
    {
        wallet.prestigeBucks -= 10000;
        store.creativeModePurchased = true;
    }

    private void autoPurchaseSquaresChanged(object sender, bool autoPurchaseSquares)
    {
        checkAutoPurchaseSquaresPurchasable();
        autoPurchaseToggle.gameObject.SetActive(autoPurchaseSquares);
        autoPurchaseLabel.text = autoPurchaseSquares ? "Auto Purchase Purchased" : "Auto Purchase Squares (1x)";
    }

    private void echoSquaresPurchasedChanged(object sender, bool autoPurchaseSquares)
    {
        checkEchoBlocksPurchasable();
        echoBlocksLabel.text = autoPurchaseSquares ? "Echoes Purchased" : "Echo Blocks (100x)";
    }

    private void instrumentsPurchasedChanged(object sender, bool unlocked)
    {
        checkInstrumentsPurchasable();
        instrumentsLabel.text = unlocked ? "Instruments Purchased" : "Instruments (5x)";
    }

    private void scalesPurchasedChanged(object sender, bool unlocked)
    {
        checkScalesPurchasable();
        scalesLabel.text = unlocked ? "Scales Purchased" : "Scales (10x)";
    }

    private void creativeModePurchasedChanged(object sender, bool unlocked)
    {
        checkCreativeModePurchasable();
        prestigeBucksLabel.gameObject.SetActive(!unlocked);
        prestigeBucksAmountLabel.gameObject.SetActive(!unlocked);
        potentialPrestigeBucksLabel.gameObject.SetActive(!unlocked);
        potentialPrestigeBucksAmountLabel.gameObject.SetActive(!unlocked);
        autoPurchaseButton.gameObject.SetActive(!unlocked);
        instrumentsButton.gameObject.SetActive(!unlocked);
        scalesButton.gameObject.SetActive(!unlocked);
        creativeModeButton.gameObject.SetActive(!unlocked);
        prestigeButton.gameObject.SetActive(!unlocked);
        echoSquaresToggle.gameObject.SetActive(unlocked);
        if (unlocked)
        {
            echoBlocksLabel.text = "Echo Squares";
            store.echoSquaresPurchased = true;
            store.autoPurchaseSquaresPurchased = true;
            store.instrumentsPurchased = true;
            store.scalesPurchased = true;
            store.numberOfSquares = 0;
            autoPurchaseToggle.gameObject.SetActive(false);
        }

        creativeModeLabel.text = unlocked ? "Creative Mode Purchased" : "Creative Mode (10k x)";
    }

    public void autoPurchaseToggleChanged()
    {
        store.autoPurchaseSquaresActive = autoPurchaseToggle.isOn;
    }

    public void echoSquaresToggleChanged()
    {
        store.echoSquaresActive = echoSquaresToggle.isOn;
    }
}
