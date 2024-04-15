using Godot;
using System;
using System.Threading.Tasks;

// Author : Baptiste Simon

public static class UtilsCoroutine
{
    public static async Task WaitForSeconds(float pSecs)
    {
        await Task.Delay(TimeSpan.FromSeconds(pSecs));
    }
}
