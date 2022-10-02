using System.Collections.Generic;

public class ExampleData
{
    public string Word;
    public int Age;
    public string ImageURL;

    public ExampleData(string word, int age, string imageURL)
    {
        Word = word;
        Age = age;
        ImageURL = imageURL;
    }
}

public class ExampleCustomSection : CustomSection<ExampleData>
{
    public override string SectionName() => "Example Sections";
    public override bool Enabled() => true;
    
    /// <summary>
    /// Get all the data we need to make a section
    /// eg: This could be getting all of a specific card from the game, all totems, encounters.... etc
    /// </summary>
    public override List<ExampleData> Initialize()
    {
        return new List<ExampleData>
        {
            new ExampleData("Maker", 20, "https://i.imgur.com/H6vESv7.png"),
            new ExampleData("Readme", 40, "https://i.imgur.com/GeMgIce.png"),
            new ExampleData("From", 60, "https://i.imgur.com/C22peXt.png"),
            new ExampleData("Hello", 120, "https://i.imgur.com/WnaCjEY.png")
        };
    }

    public override void GetTableDump(out List<CustomTableHeader> headers, out List<Dictionary<string, string>> rows)
    {
        rows = BreakdownForTable(out headers, new[]
        {
            new CustomTableColumn<ExampleData>("Name", (a)=>a.Word),
            new CustomTableColumn<ExampleData>("Age", (a)=>a.Age.ToString()),
            new CustomTableColumn<ExampleData>("Image", (a)=> string.Format("<img align=\"center\" src=\"{0}\">", a.ImageURL))
        });
    }

    public override int Sort(ExampleData a, ExampleData b)
    {
        // Rows will show by highest age to lowest. Return 0 to retain order
        return b.Age - a.Age;
    }

    /// <summary>
    /// Each row can have a different GUID if it comes from a different mod
    /// Rows are filtered by GUID via the config settings
    /// </summary>
    public override string GetGUID(ExampleData row)
    {
        return "_jamesgames.inscryption.readmemaker.examplemod";
    }
}