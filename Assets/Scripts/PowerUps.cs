using UnityEngine;
using System.Collections;
using System;

public interface IPowerUP
{
    void DoSomeShit();
    //string GetType();
}

public class InvisibilityPowerUp : MonoBehaviour, IPowerUP
{
    public void DoSomeShit()
    {
           
    }
}

public class StunPowerUp : MonoBehaviour, IPowerUP
{
    public void DoSomeShit()
    {
        
    }
}