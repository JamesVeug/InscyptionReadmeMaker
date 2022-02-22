using System.Collections.Generic;
using System.IO;
using System.Reflection;
using BepInEx;
using BepInEx.Bootstrap;
using DiskCardGame;

namespace ReadmeMaker
{
    public class LifeCost : ACost
    {
        private bool LifeAPIAvailable = false;
        private MethodInfo GetLifeCosts = null;

        public LifeCost()
        {
            bool lifeAPIInstalled = Chainloader.PluginInfos.ContainsKey("extraVoid.inscryption.LifeCost");
            if (!lifeAPIInstalled)
            {
                Plugin.Log.LogInfo("LifeCost API not detected.");
                return;
            }

            Plugin.Log.LogInfo("Detected LifeCost API!");
                
            GetLifeCosts = typeof(CardInfo).GetExtensionMethod("LifeCostz");
            LifeAPIAvailable = GetLifeCosts != null;
            if (LifeAPIAvailable)
            {
                Plugin.Log.LogInfo("LifeAPI Detected!");
            }
            else
            {
                Plugin.Log.LogInfo("LifeAPI Detected but could not get LifeCostz extension method!");
            }
            
            CostName = "Life";
            CustomIconX = "https://tinyurl.com/2s44xjen";
            IntToImage = new Dictionary<int, string>()
            {
                { 0, "https://tinyurl.com/33evecbe" },
                { 1, "https://tinyurl.com/vnu6j6t6" },
                { 2, "https://tinyurl.com/4v7mfezu" },
                { 3, "https://tinyurl.com/yr6fv3rv" },
                { 4, "https://tinyurl.com/y4dszn4t" },
                { 5, "https://tinyurl.com/mr2cxzv6" },
                { 6, "https://tinyurl.com/4ebd6hh9" },
                { 7, "https://tinyurl.com/2p97yt39" },
                { 8, "https://tinyurl.com/2p9adwxk" },
                { 9, "https://tinyurl.com/2p8bbbfm" },
            };
            CostToSingleImage = new Dictionary<int, string>()
            {
                { 1, "https://tinyurl.com/34t9pd4d" },
                { 2, "https://tinyurl.com/ym4vd6vc" },
                { 3, "https://tinyurl.com/ykmxvpmt" },
                { 4, "https://tinyurl.com/mstdm6ew" },
                { 5, "https://tinyurl.com/jrww3cd6" },
                { 6, "https://tinyurl.com/2p8z5bbp" },
                { 7, "https://tinyurl.com/bdfbdrcu" },
                { 8, "https://tinyurl.com/yckdjs9c" },
                { 9, "https://tinyurl.com/y5mhzxc9" },
                { 10, "https://tinyurl.com/4dbzjwyh" },
                { 11, "https://tinyurl.com/yc8cjfyb" },
                { 12, "https://tinyurl.com/2p9924tw" },
                { 13, "https://tinyurl.com/bdfemups" },
                { 14, "https://tinyurl.com/2p92zcxs" },
                { 15, "https://tinyurl.com/c33czhdm" },
            };
        }
        
        public override int GetCost(CardInfo cardInfo)
        {
            if (!LifeAPIAvailable)
            {
                return 0;
            }
            
            object result = GetLifeCosts.Invoke(cardInfo, new object[] { cardInfo, -1 });

            int lifeCost = (int)result;
            return lifeCost;
        }
    }
}