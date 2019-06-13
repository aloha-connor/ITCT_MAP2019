using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace ITCT
{
    public class ClassXMLParser
    {

        public List<ClassObject> ParseXML(TextAsset textAsset)
        {
            List<ClassObject> result = new List<ClassObject>();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(textAsset.text);

            XmlNode root = xmlDoc.SelectSingleNode("Classes");
            XmlNodeList assignments = root.SelectNodes("Class");
            foreach (XmlNode node in assignments)
            {
                ClassObject newAss = new ClassObject();
                newAss.id = IntParseNode(node, "@id");
				newAss.title = node.InnerText;
				result.Add(newAss);
            }

            return result;
        }

        private int IntParseNode(XmlNode parent, string nodeName)
        {
            return int.Parse(parent.SelectSingleNode(nodeName).InnerText);
        }
    }
}