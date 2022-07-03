using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;

public enum CustomAlignment
{
    Left,
    Middle,
    Right
}

public class CustomTableHeader
{
    public readonly string HeaderName;
    public readonly CustomAlignment Alignment;

    public CustomTableHeader(string headerName, CustomAlignment alignment)
    {
        HeaderName = headerName;
        Alignment = alignment;
    }
}

public abstract class CustomSection
{
    /// <summary>
    /// Appears in the readme dump and logs
    /// </summary>
    public abstract string SectionName();
    
    /// <summary>
    /// False stops the readme maker from creating a section in the dump
    /// </summary>
    public abstract bool Enabled();
    
    /// <summary>
    /// Cleanup any data used in between dumps if triggered multiple times 
    /// </summary>
    public abstract void Initialize();
    
    /// <summary>
    /// List of headers to show in a table. Order determines which order they appear in the table.
    /// </summary>
    public abstract List<CustomTableHeader> TableHeaders();
    
    /// <summary>
    /// Rows that appear on the table. Key is the same Name as the TableHeader and value is the data (displayName: Pack Rat)
    /// </summary>
    public abstract List<Dictionary<string, string>> GetRows();

    /// <summary>
    /// Attempts adding the section to the Readme Maker so it can be included in the dump.
    /// If the Readme maker mod is not enabled then this does nothing.  
    /// </summary>
    public virtual void AddSectionToReadmeMaker()
    {
        Type type = Type.GetType("JamesGames.ReadmeMaker.ReadmeDump");
        if (type == null)
        {
            return;
        }

        MethodInfo methodInfo = type.GetMethod("AddSection");
        if (methodInfo == null)
        {
            Debug.LogError("Could not add custom section. Could not find AddSection method on ReadmeDump!. Is your ExternalHelpers folder up-to-date?");
            return;
        }

        methodInfo.Invoke(null, new object[] { this });
    }

    /// <summary>
    /// Simple breakdown of what the section includes.
    /// eg: 5 New Cards
    /// </summary>
    /// <param name="stringBuilder">Current Builder for the Summary</param>
    /// <param name="rows">All Rows that will be included in the dump</param>
    public virtual void DumpSummary(StringBuilder stringBuilder, List<Dictionary<string, string>> rows)
    {
        if (rows.Count > 0)
        {
            stringBuilder.Append($"\n{rows.Count} {SectionName()}\n");
        }
    }
}
