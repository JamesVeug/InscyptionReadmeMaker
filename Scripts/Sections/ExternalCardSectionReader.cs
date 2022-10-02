using System;
using System.Collections.Generic;
using System.Reflection;
using DiskCardGame;
using JamesGames.ReadmeMaker.Sections;
using UnityEngine;

namespace JamesGames.ReadmeMaker.ExternalHelpers
{
    public class ExternalCardSectionReader : ACardsSection
    {
        private static readonly BindingFlags Flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.Default | BindingFlags.NonPublic;

        public override string SectionName => CustomSectionType.GetMethod("SectionName", Flags)?.Invoke(CustomSection, null) as string;
        public override bool Enabled => (bool)CustomSectionType.GetMethod("Enabled", Flags)?.Invoke(CustomSection, null);

        private Type CustomSectionType = null;
        private Type CustomSectionBaseType = null;
        private object CustomSection = null;

        public ExternalCardSectionReader(object instance)
        {
            CustomSectionType = instance.GetType();
            CustomSectionBaseType = CustomSectionType.BaseType.BaseType;
            CustomSection = instance;

            FieldInfo field = CustomSectionBaseType.GetField("m_readmeExternalSectionReference", BindingFlags.Instance | BindingFlags.NonPublic);
            field.SetValue(instance, this);
        }

        protected override List<CardInfo> GetCards()
        {
            object data = CustomSectionType.GetMethod("Initialize", Flags).Invoke(CustomSection, null);
            return (List<CardInfo>)data;
        }

        protected override int Sort(CardInfo a, CardInfo b)
        {
            try
            {
                MethodInfo rowsField = CustomSectionType.GetMethod("Sort", Flags);
                object result = rowsField?.Invoke(CustomSection, new object[] { a, b });
                return (int)result;
            }
            catch (Exception e)
            {
                // Mod has not implemented their own sorting method
                // Use default logic
                return base.Sort(a, b);
            }
        }

        public override string GetGUID(CardInfo o)
        {
            try
            {
                return Helpers.GetExternalGUID(CustomSection, CustomSectionType, o);
            }
            catch (Exception e)
            {
                // Mod has not implemented their own GetGUID method
                // Use default logic
                return base.GetGUID(o);
            }
        }

        public override void GetTableDump(out List<TableHeader> headers, out List<Dictionary<string, string>> splitCards)
        {
            try
            {
                Helpers.GetExternalTableDump(CustomSection, CustomSectionType, out headers, out splitCards);
            }
            catch (Exception e)
            {
                // Mod has not implemented their own GetGUID method
                // Use default logic
                base.GetTableDump(out headers, out splitCards);
            }
        }
    }
    
}