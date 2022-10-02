using System.Collections.Generic;
using DiskCardGame;

namespace JamesGames.ReadmeMaker.ExternalHelpers
{
    public abstract class CustomCardSection : CustomSection<CardInfo>
    {
        protected override string AddSectionMethodName => "AddCardSection";

        public override void GetTableDump(out List<CustomTableHeader> headers, out List<Dictionary<string, string>> rows)
        {
            // Override with how you want the cards to be displayed by headers and rows
            // Otherwise uses default logic
            throw new System.NotImplementedException();
        }

        public override int Sort(CardInfo a, CardInfo b)
        {
            // Override with how you want the cards to be sorted
            // Otherwise uses default sorting order 
            throw new System.NotImplementedException();
        }
        
        public override string GetGUID(CardInfo row)
        {
            // Override with how the GUID is retrieved
            // Otherwise uses default logic
            throw new System.NotImplementedException();
        }
    }
}