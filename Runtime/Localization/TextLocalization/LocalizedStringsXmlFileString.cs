// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

#endregion

namespace Amax.Localization
{

    public class LocalizedStringsXmlFileString : IXmlSerializable {

		[XmlAttribute("id")]
		public String id;

        [XmlAttribute("description")]
        public string description;

		[XmlIgnore]
		public Dictionary<string, string> values = new Dictionary<string, string>();

		#region IXmlSerializable Members
		public XmlSchema GetSchema() {
			return null;
		}

		public void ReadXml(XmlReader reader) {
			
			bool wasEmpty = reader.IsEmptyElement;
			id = reader.GetAttribute ("id");
			reader.Read();

			if (wasEmpty)
				return;

			while (reader.NodeType != XmlNodeType.EndElement) {
				string key = reader.Name;
				reader.Read();
				string value = reader.Value;
				reader.Read();
				reader.Read();
				values.Add(key, value);
			}
			reader.ReadEndElement();
		}

		public void WriteXml(XmlWriter writer) {
		}
		#endregion
		
	}
	
}
