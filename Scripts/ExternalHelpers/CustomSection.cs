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
    public abstract string SectionName();
    public abstract bool Enabled();
    public abstract void Initialize();
    public abstract List<CustomTableHeader> TableHeaders();
    public abstract List<Dictionary<string, string>> GetRows();

    public virtual void AddSectionToReadmeMaker()
    {
        Type type = Type.GetType("JamesGames.ReadmeMaker.ReadmeDump");
        if (type == null)
        {
            Debug.Log("Could not find ReadmeMaker!");
            return;
        }

        MethodInfo methodInfo = type.GetMethod("AddSection");
        if (methodInfo == null)
        {
            Debug.Log("Could not find AddSection method on ReadmeDump!");
            return;
        }

        methodInfo.Invoke(null, new object[] { this });
        Debug.Log("Added custom section!");
    }

    public virtual void DumpSummary(StringBuilder stringBuilder, List<Dictionary<string, string>> rows)
    {
        if (rows.Count > 0)
        {
            stringBuilder.Append($"\n{rows.Count} {SectionName()}\n");
        }
    }
}
