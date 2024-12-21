using UnityEngine;
using System;

public class Store
{
    private int _numberOfSquares = 0;
    public int numberOfSquares
    {
        get
        {
            return _numberOfSquares;
        }
        set
        {
            squaresMaxxed = value >= maxNumberOfSquares;
            bool didChange = value != _numberOfSquares;
            _numberOfSquares = value;

            if (didChange)
            {
                numberOfSquaresChanged?.Invoke(this, value);
            }
        }
    }
    
    public int maxNumberOfSquares = 20;
    private bool _squaresMaxxed = false;
    public bool squaresMaxxed
    {
        get
        {
            return _squaresMaxxed;
        }
        set
        {
            if (value != _squaresMaxxed)
            {
                _squaresMaxxed = value;
                squaresMaxxedChanged?.Invoke(null, value);
            }
        }
    }
    
    public event EventHandler<int> numberOfSquaresChanged;
    public event EventHandler<bool> squaresMaxxedChanged;
    public event EventHandler didPrestige;
    public event EventHandler<bool> autoPurchaseSquaresChanged;
    public event EventHandler<bool> echoSquaresPurchasedChanged;
    public event EventHandler<bool> instrumentsPurchasedChanged; 
    public event EventHandler<bool> scalesPurchasedChanged;
    public event EventHandler<bool> creativeModePurchasedChanged;
    public event EventHandler<bool> echoSquaresActiveChanged;
    
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
    private int squareCostFactor = 10;
    public int squareCost
    {
        get
        {
            return numberOfSquares*squareCostFactor;
        }
        set
        {
            Debug.LogError("setting squareCost is invalid");
        }
    }

    private bool _echoSquaresPurchased = false;
    public bool echoSquaresPurchased
    {
        get
        {
            return _echoSquaresPurchased;
        }
        set
        {
            _echoSquaresPurchased = value;
            echoSquaresPurchasedChanged?.Invoke(this, value);
        }
    }

    private bool _echoSquaresActive = true;
    public bool echoSquaresActive
    {
        get
        {
            return _echoSquaresActive;
        }
        set
        {
            _echoSquaresActive = value;
            echoSquaresActiveChanged?.Invoke(this, value);
        }
    }

    private bool _autoPurchaseSquaresPurchased = false;
    public bool autoPurchaseSquaresPurchased
    {
        get
        {
            return _autoPurchaseSquaresPurchased;
        }
        set
        {
            _autoPurchaseSquaresPurchased = value;
            autoPurchaseSquaresChanged?.Invoke(this, value);
        }
    }

    private bool _autoPurchaseSquaresActive = true;
    public bool autoPurchaseSquaresActive
    {
        get
        {
            return _autoPurchaseSquaresActive;
        }
        set
        {
            _autoPurchaseSquaresActive = value;
            buySquareIfAutoPurchasable();
        }
    }

    private bool _instrumentsPurchased = false;
    public bool instrumentsPurchased
    {
        get
        {
            return _instrumentsPurchased;
        }
        set
        {
            _instrumentsPurchased = value;
            instrumentsPurchasedChanged?.Invoke(this, value);
        }
    }

    private bool _scalesPurchased = false;
    public bool scalesPurchased
    {
        get
        {
            return _scalesPurchased;
        }
        set
        {
            _scalesPurchased = value;
            scalesPurchasedChanged?.Invoke(this, value);
        }
    }

    private bool _customScalesPurchased = false;
    public bool customScalesPurchased
    {
        get
        {
            return _customScalesPurchased;
        }
        set
        {
            _customScalesPurchased = value;
        }
    }

    private bool _creativeModePurchased = false;
    public bool creativeModePurchased
    {
        get
        {
            return _creativeModePurchased;
        }
        set
        {
            _creativeModePurchased = value;
            creativeModePurchasedChanged?.Invoke(this, value);
        }
    }
    
    public Store(Wallet _wallet, Save save)
    {
        wallet = _wallet;
        numberOfSquares = save.numberOfSquares;

        echoSquaresPurchased = save.echoSquares;
        autoPurchaseSquaresPurchased = save.autoPurchaseSquares;
        autoPurchaseSquaresActive = save.autoPurchaseSquaresActive;
        instrumentsPurchased = save.instruments;
        scalesPurchased = save.scales;
        creativeModePurchased = save.creativeMode;
    }

    public void resetStore()
    {
        echoSquaresPurchased = false;
        autoPurchaseSquaresPurchased = false;
        instrumentsPurchased = false;
        scalesPurchased = false;
        creativeModePurchased = false;
    }

    private void bonkBucksChanged(object sender, BigNumber bonkBucks)
    {
        buySquareIfAutoPurchasable();
    }

    private void buySquareIfAutoPurchasable()
    {
        if (autoPurchaseSquaresPurchased && autoPurchaseSquaresActive && wallet.bonkBucks >= squareCost && squareCost > 0 && !squaresMaxxed)
        {
            buySquare();
        }
    }

    public void buySquare()
    {
        int tmpSquareCost = squareCost;
        numberOfSquares++;
        wallet.bonkBucks -= tmpSquareCost;
    }

    public void prestige()
    {
        wallet.prestigeBucks += wallet.potentialPrestigeBucks;
        didPrestige?.Invoke(this, EventArgs.Empty);
        numberOfSquares = 0;
        wallet.bonkBucks = new BigNumber();
    }    

    public void squareSliderChanged(object sender, int value)
    {
        numberOfSquares = value;
    }
}
