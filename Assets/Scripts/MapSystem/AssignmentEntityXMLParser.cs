using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace ITCT
{
    public static class XmlParser
    {
        public static int IntParseNode(XmlNode parent, string nodeName)
        {
            return int.Parse(parent.SelectSingleNode(nodeName).InnerText);
        }
        public static string StringParseNode(XmlNode parent, string nodeName)
        {
            return parent.SelectSingleNode(nodeName).InnerText;
        }
        public static float FloatParseNode(XmlNode parent, string nodeName)
        {
            return float.Parse(parent.SelectSingleNode(nodeName).InnerText);
        }
    }

    public class AssignmentEntityXMLParser
    {
        public List<AssignmentEntity> ParseXML(TextAsset textAsset)
        {
            List<AssignmentEntity> result = new List<AssignmentEntity>();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(textAsset.text);

            XmlNode root = xmlDoc.SelectSingleNode("AssignmentEntities");
            XmlNodeList assignmentEntities = root.SelectNodes("AssignmentEntity");
            foreach(XmlNode node in assignmentEntities)
            {
                AssignmentEntity newAE = new AssignmentEntity();
                newAE.aeID = XmlParser.IntParseNode(node,"@id");
                newAE.aeType = XmlParser.StringParseNode(node, "@type").Equals("computer") ? AEType.computer : AEType.wall;
                newAE.floor = XmlParser.IntParseNode(node, "floor");
                if(newAE.aeType == AEType.computer)
                {
                    newAE.pos = new Vector2(XmlParser.FloatParseNode(node, "shape/point/@x"), XmlParser.FloatParseNode(node, "shape/point/@y"));
                    newAE.radius = XmlParser.FloatParseNode(node, "shape/radius/@r");
                }
                else
                {
                    XmlNodeList points = node.SelectNodes("shape/point");
                    newAE.pos = new Vector2(XmlParser.FloatParseNode(points[0], "@x"),XmlParser.FloatParseNode(points[0], "@y"));
                    newAE.pos2 = new Vector2(XmlParser.FloatParseNode(points[1], "@x"),XmlParser.FloatParseNode(points[1], "@y"));
                }
                XmlNodeList asss = node.SelectNodes("assignments/assignment");
                foreach(XmlNode ass in asss)
                {
                    newAE.assignmentIDList.Add(XmlParser.IntParseNode(ass, "@id"));
                }
                result.Add(newAE);
            }
            return result;
        }
    }
}
