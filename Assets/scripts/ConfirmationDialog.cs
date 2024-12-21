using UnityEngine;
using System;

public class ConfirmationDialog : MonoBehaviour
{
    public event EventHandler<bool> confirmation;

    public void cancel()
    {
        confirmation?.Invoke(this, false);
    }

    public void confirm()
    {
        confirmation?.Invoke(this, true);
    }
}
