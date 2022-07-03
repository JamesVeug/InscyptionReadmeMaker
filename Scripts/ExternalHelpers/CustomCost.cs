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
    
    public abstract int GetCost(CardInfo cardInfo);
    
    public virtual void AddCostToReadmeMaker()
    {
        Type type = Type.GetType("JamesGames.ReadmeMaker.ReadmeDump");
        if (type == null)
        {
            Debug.Log("Could not find ReadmeMaker!");
            return;
        }

        MethodInfo methodInfo = type.GetMethod("AddCustomCost");
        if (methodInfo == null)
        {
            Debug.Log("Could not find AddCustomCost method on ReadmeDump!");
            return;
        }

        methodInfo.Invoke(null, new object[] { this });
        Debug.Log("Added custom Cost!");
    }
}
