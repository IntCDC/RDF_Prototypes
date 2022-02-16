﻿using BH.oM.Analytical.Results;
using BH.oM.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BH.Engine.RDF;
using VDS.RDF;
using VDS.RDF.Writing;
using BH.oM.Architecture.Elements;
using BH.oM.Physical.Elements;
using BH.oM.RDF;
using Newtonsoft.Json.Linq;
using BH.Engine.Base;
using log = BH.oM.RDF.Log;

namespace BH.oM.CodeAnalysis.ConsoleApp
{
    public static class Program
    {
        public static void Main(string[] args = null)
        {
            List<Assembly> oMassemblies = BH.Engine.RDF.Compute.LoadAssembliesInDirectory(true);

            // Remove duplicate classes in the same file, e.g. `BH.oM.Base.Output` which has many generics replicas.
            List<TypeInfo> oMTypes = new List<TypeInfo>(oMassemblies
                .SelectMany(a => a.DefinedTypes)
                .Where(t => t.NameValidChars().Count() > 1) // removes those 'c<>' generics types that appear as duplicates
                .GroupBy(t => t.FullName.OnlyAlphabeticAndDots())
                .Select(g => g.First()));

            // Take a subset of the types avaialble to reduce the size of the output graph. This can become a Filter function.
            //IEnumerable<TypeInfo> onlyBaseOmTypes = oMTypes.Where(t => t != null && t.Namespace != null && t.Namespace.EndsWith("BH.oM.Base")).ToList();
            //onlyBaseOmTypes = onlyBaseOmTypes.Where(t => t.Name == "NamedNumericTolerance" || t.Name == "IObject");
            //onlyBaseOmTypes = onlyBaseOmTypes.Where(t => t.Name.Contains("Output"));
            //onlyBaseOmTypes = onlyBaseOmTypes.Where(t => t.Name.Contains("ComparisonConfig"));

            SortedDictionary<string, string> webVOWLJsonsPerNamespace = WebVOWLJsonPerNamespace(oMTypes);

            string generatedOntologiesDirectoryName = "WebVOWLOntology";

            // Save all generated ontologies to file
            foreach (var kv in webVOWLJsonsPerNamespace)
                kv.Value.WriteToJsonFile($"{kv.Key}.json", $"..\\..\\..\\{generatedOntologiesDirectoryName}");

            // Save the URLS to the ontologies. These are links to the WebVOWL website with a parameter passed that links directly the Github URL of the ontology.
            string allWebOWLOntologyURL = $"..\\..\\..\\{generatedOntologiesDirectoryName}\\_allWebOWLOntologyURL.txt";

            File.WriteAllText(allWebOWLOntologyURL, ""); // empty the file
            foreach (var kv in webVOWLJsonsPerNamespace)
            {
                string WebVOWLOntologyURL = $"https://service.tib.eu/webvowl/#url=https://raw.githubusercontent.com/BHoM/RDF_Prototypes/main/{generatedOntologiesDirectoryName}/{kv.Key}.json";
                File.AppendAllText(allWebOWLOntologyURL, "\n" + WebVOWLOntologyURL);
            }

            //List<Type> 
            //Dictionary<Type, List<IRelation>> dictionaryGraph = group.DictionaryGraphFromTypes();
            //string webVOWLJson = Engine.RDF.Convert.ToWebVOWLJson(dictionaryGraph);

            //result[group.Key] = webVOWLJson;


            // Invoke all static methods in `Tests_Alessio` class
            //typeof(Tests_Alessio).GetMethods().Where(mi => mi.IsStatic).ToList().ForEach(mi => mi.Invoke(null, null));

            // Invoke all static methods in `Tests_Diellza` class
            //typeof(Tests_Diellza).GetMethods().Where(mi => mi.IsStatic).ToList().ForEach(mi => mi.Invoke(null, null));

            Console.WriteLine("\nPress any key to exit.");
            Console.ReadKey();
            log.SaveLogToDisk("..\\..\\..\\log.txt");
        }

        public static SortedDictionary<string, string> WebVOWLJsonPerNamespace(List<TypeInfo> oMTypes, List<string> namespaceToConsider = null, List<string> typeNamesToConsider = null, int namespaceGroupDepth = 3)
        {
            SortedDictionary<string, string> result = new SortedDictionary<string, string>(new NaturalSortComparer<string>());

            // Null check
            oMTypes = oMTypes.Where(t => t != null && t.Namespace != null).ToList();

            // Filters
            if (namespaceToConsider != null && namespaceToConsider.All(f => !string.IsNullOrWhiteSpace(f)))
                oMTypes = oMTypes.Where(t => namespaceToConsider.Any(nsf =>
                {
                    if (nsf.StartsWith("BH."))
                        return t.Namespace.StartsWith(nsf);
                    else
                        return t.Namespace.Contains(nsf);
                })).ToList();


            if (typeNamesToConsider != null && typeNamesToConsider.All(f => !string.IsNullOrWhiteSpace(f)))
                oMTypes = oMTypes.Where(t => typeNamesToConsider.Any(tn =>
                {
                    if (tn.StartsWith("BH."))
                        return t.Name == tn;
                    else
                        return t.Name.Contains(tn);
                })).ToList();

            // Group by namespace
            if (namespaceGroupDepth < 3)
                namespaceGroupDepth = 3; // at least group per BH.oM.Something

            var oMTypesGroupsPerNamespace = oMTypes.GroupBy(t => string.Join(".", t.Namespace.Split('.').Take(namespaceGroupDepth)));

            foreach (var group in oMTypesGroupsPerNamespace)
            {
                // Extract a dictionary representation of the BHoM Ontology Graph
                Dictionary<TypeInfo, List<IRelation>> dictionaryGraph = group.DictionaryGraphFromTypes();
                string webVOWLJson = Engine.RDF.Convert.ToWebVOWLJson(dictionaryGraph);

                result[group.Key] = webVOWLJson;
            }

            return result;
        }
    }
}
