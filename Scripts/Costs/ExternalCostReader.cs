using System;
using System.Collections.Generic;
using DiskCardGame;
using BindingFlags = System.Reflection.BindingFlags;

namespace JamesGames.ReadmeMaker
{
    public class ExternalCostReader : ACost
    {
        public static readonly BindingFlags Flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.Default | BindingFlags.NonPublic;
        
        private Type type = null;
        private object instance = null;
        public ExternalCostReader(object o)
        {
            type = o.GetType();
            instance = o;
            
            CostName = (string)type.GetField("CostName", Flags).GetValue(instance);
            CustomIconX = (string)type.GetField("CustomIconXURL", Flags).GetValue(instance);
            IntToImage = (Dictionary<int, string>)type.GetField("IntToImageURL", Flags).GetValue(instance);
            CostToSingleImage = (Dictionary<int, string>)type.GetField("CostToSingleImageURL", Flags).GetValue(instance);
        }
        
        public override int GetCost(CardInfo cardInfo)
        {
            return (int)type.GetMethod("GetCost").Invoke(instance, new object[]{cardInfo});
        }
    }
}
