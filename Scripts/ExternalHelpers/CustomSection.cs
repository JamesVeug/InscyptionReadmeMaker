using System;
using System.Collections;
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

public class CustomTableColumn<T>
{
    public readonly string HeaderName;
    public readonly CustomAlignment Alignment;
    public readonly Func<object, string> Getter;
    public readonly bool Enabled;

    public CustomTableColumn(string headerName, Func<T, string> getter, bool enabled=true, CustomAlignment alignment=CustomAlignment.Left)
    {
        HeaderName = headerName;
        Getter = (a)=>getter((T)a);
        Alignment = alignment;
        Enabled = enabled;
    }
}

public abstract class CustomSection<T>
{
    private static readonly BindingFlags Flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.Default | BindingFlags.NonPublic;
    
    /// <summary>
    /// Appears in the readme dump and logs
    /// </summary>
    public abstract string SectionName();
    
    /// <summary>
    /// Returns the guid of your mod so it can be sorted and filtered.
    /// </summary>
    public abstract string GetGUID(object row);
    
    /// <summary>
    /// False stops the readme maker from creating a section in the dump
    /// </summary>
    public abstract bool Enabled();
    
    /// <summary>
    /// Get all the data required to display the rows
    /// </summary>
    /// <returns>List of all rows data</returns>
    public abstract List<ExampleData> Initialize();
    
    /// <summary>
    /// Convert data returned from Initialize() into a list of dictionaries so it can be displayed in the readme dump.
    /// </summary>
    public abstract void GetTableDump(out List<CustomTableHeader> headers, out List<Dictionary<string, string>> rows);
    
    /// <summary>
    /// Sort the rows to appear in the correct order 
    /// </summary>
    public abstract int Sort(T a, T b);

    private object m_readmeExternalSectionReference = null;

    protected List<Dictionary<string, string>> BreakdownForTable(out List<CustomTableHeader> headers, params CustomTableColumn<T>[] grouping)
    {
        MethodInfo method = m_readmeExternalSectionReference.GetType().GetMethod("BreakdownForTableExternal", Flags);
        object[] args = new object[]{null, grouping};
        object result = method.Invoke(m_readmeExternalSectionReference, args);
        headers = new List<CustomTableHeader>();
        
        // Convert grouping to CustomTableHeader<object>[]
        IEnumerable givenHeaders = (IEnumerable)args[0];
        foreach (object group in givenHeaders)
        {
            Type groupType = group.GetType();
                
            // Misc
            string HeaderName = (string)groupType.GetField("HeaderName", Flags).GetValue(group);

            // Alignment
            object alignmentData = groupType.GetField("Alignment", Flags).GetValue(group);
            Enum.TryParse(alignmentData.ToString(), out CustomAlignment alignment);
            
            CustomTableHeader column = new CustomTableHeader(HeaderName, alignment);
            headers.Add(column);
        }
        
        return result as List<Dictionary<string, string>>;
    }
    
    /// <summary>
    /// Attempts adding the section to the Readme Maker so it can be included in the dump.
    /// If the Readme maker mod is not enabled then this does nothing.  
    /// </summary>
    public virtual void AddSectionToReadmeMaker()
    {
        Type type = Type.GetType("JamesGames.ReadmeMaker.ReadmeDump, ReadmeMaker");
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
        Debug.Log($"Registered custom section {SectionName()} to Readme Maker!");
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
