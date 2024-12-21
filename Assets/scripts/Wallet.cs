using System;
using UnityEngine;

public class Wallet
{
    private Save save;

    private BigNumber _bonkBucks = new BigNumber();
    public BigNumber bonkBucks
    {
        get
        {
            return _bonkBucks;
        }
        set
        {
            _bonkBucks = value;
            bonkBucksChanged?.Invoke(this, value);
            potentialPrestigeBucks = bonkBucks/100;
        }
    }

    private BigNumber _potentialPrestigeBucks = new BigNumber();
    public BigNumber potentialPrestigeBucks
    {
        get
        {
            return _potentialPrestigeBucks;
        }
        set
        {
            _potentialPrestigeBucks = value;
            potentialPrestigeBucksChanged?.Invoke(this, value);
        }
    }

    private BigNumber _prestigeBucks = new BigNumber();
    public BigNumber prestigeBucks
    {
        get
        {
            return _prestigeBucks;
        }
        set
        {
            _prestigeBucks = value;
            prestigeBucksChanged?.Invoke(this, value);
        }
    }

    public event EventHandler<BigNumber> bonkBucksChanged;
    public event EventHandler<BigNumber> prestigeBucksChanged;
    public event EventHandler<BigNumber> potentialPrestigeBucksChanged;

    public Wallet(Save _save)
    {
        save = _save;
        bonkBucks = save.bonkBucks;
        prestigeBucks = save.prestigeBucks;
    }

    public void empty()
    {
        bonkBucks = new BigNumber();
        prestigeBucks = new BigNumber();
    }
}
