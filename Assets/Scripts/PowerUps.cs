using UnityEngine;
using System.Collections;
using System;

public interface IPowerUP
{
    void DoSomeShit();
}

public class InvisibilityPowerUp : MonoBehaviour, IPowerUP
{
    public void DoSomeShit()
    {
        
    }
}

public class SpeedBoostPowerUp : MonoBehaviour, IPowerUP
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

public class TeleportPowerUp : MonoBehaviour, IPowerUP
{
    public void DoSomeShit()
    {

    }
}