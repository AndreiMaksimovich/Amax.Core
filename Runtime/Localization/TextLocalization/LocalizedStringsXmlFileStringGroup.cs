// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using System.Xml.Serialization;

#endregion

namespace Amax.Localization
{

    public class LocalizedStringsXmlFileStringGroup 
    {

		[XmlElement("Prefix")]
		public string idPrefix;

		[XmlElement("String")]
		public LocalizedStringsXmlFileString[] strings;
		
		[XmlElement("Group")]
		public LocalizedStringsXmlFileStringGroup[] groups;
		
    }
	
}