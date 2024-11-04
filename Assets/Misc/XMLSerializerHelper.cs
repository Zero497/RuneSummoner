using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public static class XMLSerializerHelper
{
    public static string pathRoot;
    
    private static XmlSerializer serializer;
    
    public class UnitToXML
    {
        public string UnitDataRef;

        public List<string> curActiveAbilities;
        
        public List<string> curPassiveAbilities;

        public int level;
    }

    public static void SerializeUnitToXML(UnitToXML unitToXML, string path)
    {
        serializer ??= new XmlSerializer(typeof(UnitToXML));
        TextWriter writer = new StreamWriter(pathRoot+"/Units/"+path+".xml");
    }

    public static UnitToXML DeserializeUnitToXML(string path)
    {
        serializer ??= new XmlSerializer(typeof(UnitToXML));
        FileStream fileStream = new FileStream(pathRoot+"/Units/"+path+".xml", FileMode.Open);
        return serializer.Deserialize(fileStream) as UnitToXML;
    }
}
