// Copyright (c) 2022 Andrei Maksimovich
// See LICENSE.md

#region

using System;
using System.Xml.Serialization;
using Amax.Navigation;
using Amax.Web;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#endregion

namespace Amax.Core.Examples
{

    public class SceneControllerExampleWebLoader : ASceneController
    {

        [Header("Image")] public Image image;
        public string imageUrl;

        [Header("JSON")] public string jsonUrl;
        public TextMeshProUGUI jsonValue;

        [Header("XML")] public string xmlUrl;
        public TextMeshProUGUI xmlValue;

        void Start()
        {

            WebLoader.Load<Sprite>(imageUrl).OnComplete += (sprite, exception) =>
            {
                image.sprite = sprite;
                if (exception != null) Debug.LogError(exception);
            };

            WebLoader.LoadFromJson<TestJson>(jsonUrl).OnComplete += (test, exception) =>
            {
                if (test != null)
                {
                    jsonValue.text = JsonUtility.ToJson(test);
                }

                if (exception != null) Debug.LogError(exception);
            };

            WebLoader.LoadFromXml<TestXml>(xmlUrl).OnComplete += (test, exception) =>
            {
                if (test != null)
                {
                    xmlValue.text = XmlUtils.Serialize(test);
                }

                if (exception != null) Debug.LogError(exception);
            };

        }

        [Serializable]

        public class TestJson
        {
            public string a;
            public string b;
            public string c;
        }

        [XmlRoot("Test")]
        public class TestXml
        {
            [XmlAttribute("id")] public string id;

            [XmlElement("String")] public string[] strings;
        }

    }

}
