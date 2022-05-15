// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using System.IO;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

#endregion

namespace Amax {

    public static class XmlUtils  
    {
	    
		public static T DeserializeResource<T>(string path)
			=> Deserialize<T>(Resources.Load<TextAsset>(path).text);

		public static T Deserialize<T>(TextAsset textAsset)
			=> Deserialize<T>(textAsset.text);
		
		public static T Deserialize<T>(string xml) 
		{
			var xmlSerializer = new XmlSerializer(typeof(T));
			var xmlReaderSettings = new XmlReaderSettings
			{
				IgnoreWhitespace = true
			};
			var stringReader = new StringReader(xml);
			var xmlReader = XmlReader.Create(stringReader, xmlReaderSettings);
			return (T) xmlSerializer.Deserialize(xmlReader);
		}
		
		public static string Serialize<T>(T obj) 
		{
			var serializer = new XmlSerializer (typeof(T));
			var stringWriter = new StringWriter ();
			var xmlWriterSettings = new XmlWriterSettings
			{
				OmitXmlDeclaration = true
			};
			var xmlWriter = XmlWriter.Create (stringWriter, xmlWriterSettings);
			var namespaces = new XmlSerializerNamespaces();
			namespaces.Add("","");
			serializer.Serialize (xmlWriter, obj, namespaces);
			return stringWriter.ToString ();
		}
		
	}

}
