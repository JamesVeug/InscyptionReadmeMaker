using System;
using System.Collections.Generic;
using System.Reflection;
using DiskCardGame;
using UnityEngine;

public abstract class CustomCost
{
    // Shown for debugging and when there are no icons available
    protected string CostName = null; // Blood
    
    // 5 = Image of 5xICON (Most optimal way to show icons!)
    protected Dictionary<int, string> CostToSingleImageURL = null;
        
    // Icon for X when showing 5xICON
    protected string CustomIconXURL = null; // x
    
    // 5 = image of a 5
    protected Dictionary<int, string> IntToImageURL = null;
    
    /// <summary>
    /// Gets the cost attached to the CardInfo
    /// </summary>
    public abstract int GetCost(CardInfo cardInfo);
    
    /// <summary>
    /// Attempts adding the cost to the Readme Maker so it can be included in the dump.
    /// If the Readme maker mod is not enabled then this does nothing.  
    /// </summary>
    public virtual void AddCostToReadmeMaker()
    {
        Type type = Type.GetType("JamesGames.ReadmeMaker.ReadmeDump");
        if (type == null)
        {
            return;
        }

        MethodInfo methodInfo = type.GetMethod("AddCustomCost");
        if (methodInfo == null)
        {
            Debug.LogError("Could not add custom cost. Could not find AddCustomCost method on ReadmeDump!. Is your ExternalHelpers folder up-to-date?");
            return;
        }

        methodInfo.Invoke(null, new object[] { this });
    }
}
