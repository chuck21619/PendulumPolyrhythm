using System.Linq;

public class BigNumber
{
    
    private static string[] Units = new string[] {"", "k", "m", "b", "t", "qd", "qt", "sx", "sp", "o", "n", "d"};

    public int unitIndex = 0;

    private float _number = 0;
    public float number
    {
        get
        {
            return _number;
        }
        set
        {
            
            float tmpNumber = value;
            while (tmpNumber >= 1000 && unitIndex != Units.Count()-1)
            {
                unitIndex++;
                tmpNumber /= 1000;
            }
            
            while (tmpNumber < 1 && unitIndex != 0)
            {
                unitIndex--;
                tmpNumber *= 1000;
            }

            _number = tmpNumber;
        }
    }

    public BigNumber(int _unitIndex = 0, float _number = 0)
    {
        unitIndex = _unitIndex;
        number = _number;
    }

    public override string ToString()
    {
        if (unitIndex == 0)
        {
            return ((int)number).ToString();
        }
        else
        {
             return number.ToString("0.0") + Units[unitIndex];
        }
    }

    public BigNumber roundedDown()
    {
        BigNumber newNumber = new BigNumber(unitIndex, (int)number);
        return newNumber;
    }

    public static BigNumber operator +(BigNumber a, BigNumber b)
    {
        BigNumber newNumber = new BigNumber(a.unitIndex, a.number);
        float bFactor = BigNumber.operandFactor(a, b);
        newNumber.number += b.number*bFactor;
        return newNumber;
    }

    public static BigNumber operator -(BigNumber a, BigNumber b)
    {
        BigNumber newNumber = new BigNumber(a.unitIndex, a.number);
        float bFactor = BigNumber.operandFactor(a, b);
        newNumber.number = newNumber.number - b.number*bFactor;
        return newNumber;
    }

    public static BigNumber operator *(BigNumber a, BigNumber b)
    {
        BigNumber newNumber = new BigNumber(a.unitIndex, a.number);
        float bFactor = BigNumber.operandFactor(a, b);
        newNumber.number *= b.number*bFactor;
        return newNumber;
    }

    public static BigNumber operator /(BigNumber a, float b)
    {
        BigNumber newNumber = new BigNumber(a.unitIndex, a.number);
        newNumber.number /= b;
        return newNumber;
    }

    public static BigNumber operator -(BigNumber a, float b)
    {
        BigNumber bNumber = new BigNumber(0, b);
        return a - bNumber;
    }

    public static BigNumber operator +(BigNumber a, float b)
    {
        BigNumber bNumber = new BigNumber(0, b);
        return a + bNumber;
    }
/*
    public static BigNumber operator *(BigNumber a, float b)
    {
        BigNumber bNumber = new BigNumber(0, b);
        return a * bNumber;
    }*/

    public static bool operator >(BigNumber a, float b)
    {
        if (a.unitIndex > 0)
        {
            return a.unitIndex*1000*a.number > b;
        }

        return a.number > b;
    }

    public static bool operator <(BigNumber a, float b)
    {
        return !(a>b);
    }

    public static bool operator ==(BigNumber a, float b)
    {
        if (a.unitIndex > 0)
        {
            return a.unitIndex*1000*a.number == b;
        }
        return a.number == b;
    }

    public static bool operator !=(BigNumber a, float b)
    {
        return !(a==b);
    }

    public static bool operator >=(BigNumber a, float b)
    {
        return a > b || a == b;
    }

    public static bool operator <=(BigNumber a, float b)
    {
        return a < b || a == b;
    }

    private static float operandFactor(BigNumber a, BigNumber b)
    {
        int unitDifference = b.unitIndex - a.unitIndex;
        float bFactor = 1;
        if (unitDifference > 0)
        {
            bFactor = 1000*unitDifference;
        }
        else if (unitDifference < 0)
        {
            bFactor = 1/(1000 * bFactor);
        }

        return bFactor;
    }
}
