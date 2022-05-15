// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

#endregion

namespace Amax {

    [Serializable]
    public class XmlSerializableStringDictionary : SerializableDictionary<string, string>, IXmlSerializable 
    {
        
        #region IXmlSerializable Members

        public XmlSchema GetSchema()
            => null;

        public void ReadXml(XmlReader reader) {
            var wasEmpty = reader.IsEmptyElement;
            reader.Read();

            if (wasEmpty)
                return;

            while (reader.NodeType != XmlNodeType.EndElement) 
            {
                var key = reader.Name;
                reader.Read();
                var value = reader.Value;
                reader.Read();
                reader.Read();
                Add(key, value);
            }
            
            reader.ReadEndElement();
        }

        public void WriteXml(XmlWriter writer) 
        {
            foreach (var key in this.Keys) 
            {
                writer.WriteStartElement(key);
                writer.WriteString(this[key]);
                writer.WriteEndElement();
            }
        }
        
        #endregion
        
    }

}
