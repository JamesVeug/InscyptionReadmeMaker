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
            Debug.Log("Could not add custom section. ReadmeMaker not found. Is it installed and enabled?");
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

    public virtual void DumpSummary(StringBuilder stringBuilder, List<Dictionary<string, string>> rows)
    {
        if (rows.Count > 0)
        {
            stringBuilder.Append($"\n{rows.Count} {SectionName()}\n");
        }
    }
}
