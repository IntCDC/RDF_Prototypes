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
using System.ComponentModel;
using VDS.RDF.Ontology;


namespace BH.oM.CodeAnalysis.ConsoleApp
{
    public static partial class Tests_Diellza
    {
        private static string _fileDirectory = @"C:\temp\RDF_Prototypes_test";

       
        [Description("Code from Diellza: Tries to create owl:Classes (Structure_oM_Elements_Bar and Physical_oM_Elements_Beam) as subject and objects connectected by owl:ObjectProperties as predicate (hasElement or isPartOf). Uses owl and rdf vocabulary.")]
        public static void RoomWithBar()
        {


            IGraph g = new Graph();
           

            // Set the base uri for the ontology.
            //g.BaseUri = new Uri("https://github.com/BHoM/");

            // Set the namespace of our Ontology
          //  g.NamespaceMap.AddNamespace("bhom", UriFactory.Create("http://visualdataweb.org/newOntology/"));

            g.NamespaceMap.AddNamespace("owl", UriFactory.Create("http://www.w3.org/2002/07/owl"));

             UriNode a = (UriNode)g.CreateUriNode("rdf:type");
           //  UriNode owlClass = (UriNode)g.CreateUriNode("owl:class");

            var owlClass = g.CreateUriNode("owl:class");

             UriNode rdfProperty = (UriNode)g.CreateUriNode("rdf:Property");
             UriNode owlObjectProperty = (UriNode)g.CreateUriNode("owl:ObjectProperty");
             UriNode rdfsLabel = (UriNode)g.CreateUriNode("rdfs:label");

             var rdfsDomain = g.CreateUriNode("rdfs:domain");
             var rdfsRange = g.CreateUriNode("rdfs:range");
             var owlDatatypeProperty = g.CreateUriNode("owl:DatatypeProperty");
             var rdfsLiteral = g.CreateUriNode("rdfs:Literal");
         
             var rdfsDatatype = g.CreateUriNode("rdfs:Datatype");

             var rdfsSubclassOf = g.CreateUriNode("rdfs:subclassOf");

            //var rdfsSubPropertyOf = g.CreateUriNode("rdfs:subPropertyOf");
            // var perimeter = g.CreateUriNode("rdfs:literal");
          



            // Creates owl classes  (room x2, beam and bar)

            var Architecture_oM_Elements_Room = g.CreateUriNode(UriFactory.Create("https://github.com/BHoM/BHoM/blob/main/Architecture_oM/Elements/Room.cs"));        
            g.Assert(Architecture_oM_Elements_Room, rdfsLabel, g.CreateLiteralNode("Architecture_oM_Elements_Room", "en"));

            var Architecture_oM_Elements_LivingRoom = g.CreateUriNode(UriFactory.Create("https://github.com/BHoM/BHoM/blob/main/Architecture_oM/Elements/LivingRoom.cs"));
            g.Assert(Architecture_oM_Elements_LivingRoom, rdfsLabel, g.CreateLiteralNode("Architecture_oM_Elements_LivingRoom", "en"));

            var Physical_oM_Elements_Beam = g.CreateUriNode(UriFactory.Create("https://github.com/BHoM/BHoM/blob/main/Physical_oM/Elements/Beam.cs"));    
            g.Assert(Physical_oM_Elements_Beam, rdfsLabel, g.CreateLiteralNode("Physical_oM_Elements_Beam", "en"));

            var Structure_oM_Elements_Bar = g.CreateUriNode(UriFactory.Create("https://github.com/BHoM/BHoM/blob/main/Structure_oM/Elements/Bar.cs"));
            g.Assert(Structure_oM_Elements_Bar, rdfsLabel, g.CreateLiteralNode("Structure_oM_Elements_Bar", "en"));

           // var Structure_oM_Elements_Type = g.CreateLiteralNode("Type");
           // g.Assert(Structure_oM_Elements_Type, rdfsLabel, g.CreateLiteralNode("Structure_oM_Elements_Type", "en"));

            // var hasName = g.CreateLiteralNode("hasName");
            //  g.Assert(hasName, rdfsLabel, g.CreateLiteralNode("hasName", "en"));


            //g.Assert(Architecture_oM_Elements_LivingRoom, rdfsLiteral, Perimeter);
            //g.Assert(Perimeter,a, rdfsLiteral);


            //Makes Architecture_oM_Elements_LivingRoom subclass of Architecture_oM_Elements_Room

            //  g.Assert(Architecture_oM_Elements_LivingRoom, a, Architecture_oM_Elements_Room);
            // g.Assert(Architecture_oM_Elements_LivingRoom, rdfsLiteral, 


            // var GUID = g.CreateLiteralNode("GUID");
            //   g.Assert(GUID, a, rdfsLiteral);


            //Tries to create an OWL same as property, and make the Geometry_oM_Vector_Point and Point equal BUT IT FAILED
            //  var Geometry_oM_Vector_Point = g.CreateUriNode(UriFactory.Create("https://github.com/BHoM/BHoM/blob/main/Geometry_oM/Vector/Point.cs"));
            //  g.Assert(Geometry_oM_Vector_Point, rdfsLabel, g.CreateLiteralNode("Geometry_oM_Vector_Point", "en"));

            //var Point = g.CreateUriNode(UriFactory.Create("https://github.com/BHoM/BHoM/blob/main/Geometry_oM/Point.cs"));
            //  g.Assert(Point, rdfsLabel, g.CreateLiteralNode("Point", "en"));

            // g.Assert(Point, owlSameAs, Geometry_oM_Vector_Point);


            // Creates isPartOf Relation (Physical_oM_Elements_Beam, isPartOf, Structure_oM_Elements_Bar)
            var isPartOf = g.CreateUriNode(UriFactory.Create("https://github.com/BHoM/Predicate/isPartOf"));
            g.Assert(owlObjectProperty, a, isPartOf); // there is a problem here. It makes isPartof OWL. But if we remove the row, everything else stops working ...
            g.Assert(isPartOf, rdfsLabel, g.CreateLiteralNode("isPartOf", "en"));

            g.Assert(isPartOf, rdfsDomain, Physical_oM_Elements_Beam);
            g.Assert(isPartOf, rdfsRange, Structure_oM_Elements_Bar);


            //Relation :( Architecture_oM_Elements_Room, hasElement, Structure_oM_Elements_Bar)
            var hasElement = g.CreateUriNode(UriFactory.Create("https://github.com/BHoM/Predicate/hasElement"));
            g.Assert(hasElement, rdfsLabel, g.CreateLiteralNode("hasElement", "en"));
            g.Assert(hasElement, rdfsDomain, Architecture_oM_Elements_Room);
            g.Assert(hasElement, rdfsRange, Structure_oM_Elements_Bar);
                           
      

           //Relation 
           var hasGUID = g.CreateUriNode(UriFactory.Create("https://github.com/BHoM/Predicate/hasGUID"));
           g.Assert(hasGUID, rdfsLabel, g.CreateLiteralNode("hasGUID", "en"));
           g.Assert(hasGUID, rdfsDomain, Structure_oM_Elements_Bar);
           g.Assert(hasGUID, rdfsRange, rdfsDatatype);

            
           //Relation 
           var Material = g.CreateUriNode(UriFactory.Create("https://github.com/BHoM/Predicate/Material"));
           g.Assert(Material, rdfsLabel, g.CreateLiteralNode("hasMaterial", "en"));
           g.Assert(Material, rdfsDomain, Structure_oM_Elements_Bar);
           g.Assert(Material, rdfsRange, rdfsLiteral);
           // g.Assert(Architecture_oM_Elements_Room, hasGUID, "12");


;


            //g.Assert(hasElement, rdfsLabel, g.CreateLiteralNode("hasElement", "en"));
            //Relation :( Architecture_oM_Elements_Room, hasElement, Structure_oM_Elements_Bar)
            //var hasElement = g.CreateUriNode(UriFactory.Create("https://github.com/BHoM/Predicate/hasElement"));
            //g.Assert(hasElement, rdfsLabel, g.CreateLiteralNode("hasElement", "en"));
            //g.Assert(hasElement, rdfsDomain, Architecture_oM_Elements_Room);
            //g.Assert(hasElement, rdfsRange, Structure_oM_Elements_Bar);


            g.WriteToXMLFile(_fileDirectory, "RoomWithBar.rdf");

        


        }
    }
}
