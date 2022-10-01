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

public class ExampleCustomSection : CustomSection
{
    public override string SectionName() => "Example Sections";
    public override bool Enabled() => true;

    private List<ExampleData> m_data = new List<ExampleData>();
    
    /// <summary>
    /// Get all the data we need to make a section
    /// eg: This could be getting all of a specific card from the game, all totems, encounters.... etc
    /// </summary>
    public override void Initialize()
    {
        m_data = new List<ExampleData>
        {
            new ExampleData("Hello", 20, "https://i.imgur.com/H6vESv7.png"),
            new ExampleData("From", 40, "https://i.imgur.com/GeMgIce.png"),
            new ExampleData("Readme", 60, "https://i.imgur.com/C22peXt.png"),
            new ExampleData("Maker", 120, "https://i.imgur.com/WnaCjEY.png")
        };
    }

    /// <summary>
    /// Create list of headers we want to use for a table
    /// </summary>
    public override List<CustomTableHeader> TableHeaders()
    {
        return new List<CustomTableHeader>()
        {
            new CustomTableHeader("Word", CustomAlignment.Left),
            new CustomTableHeader("Age", CustomAlignment.Middle),
            new CustomTableHeader("Image", CustomAlignment.Middle),
        };
    }

    /// <summary>
    /// Convert the data into a list of dictionaries so it can be displayed in the readme dump
    /// </summary>
    public override List<Dictionary<string, string>> GetRows()
    {
        List<Dictionary<string, string>> list = new List<Dictionary<string, string>>();
        foreach (ExampleData data in m_data)
        {
            Dictionary<string,string> dictionary = new Dictionary<string, string>();
            dictionary["Word"] = data.Word;
            dictionary["Age"] = data.Age.ToString();
            dictionary["Image"] = string.Format("<img align=\"center\" src=\"{0}\">", data.ImageURL);
            list.Add(dictionary);
        }

        return list;
    }
    
    /// <summary>
    /// Each row can have a different GUID if it comes from a different mod
    /// Rows are filtered by GUID via the config settings
    /// </summary>
    public override string GetGUID(object row)
    {
        return "_jamesgames.inscryption.readmemaker.mod1";
    }
}