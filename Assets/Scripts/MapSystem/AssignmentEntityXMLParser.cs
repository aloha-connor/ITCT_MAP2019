using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;
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
        public List<AssignmentEntity> ParseXML(string text)
        {
            List<AssignmentEntity> result = new List<AssignmentEntity>();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(text);

            XmlNode root = xmlDoc.SelectSingleNode("AssignmentEntities");
            XmlNodeList assignmentEntities = root.SelectNodes("AssignmentEntity");
            foreach (XmlNode node in assignmentEntities)
            {
                AssignmentEntity newAE = new AssignmentEntity();
                newAE.aeID = XmlParser.IntParseNode(node, "@id");
                newAE.aeType = XmlParser.StringParseNode(node, "@type").Equals("computer") ? AEType.computer : AEType.wall;
                newAE.floor = XmlParser.IntParseNode(node, "floor");
                newAE.pos = new Vector2(XmlParser.FloatParseNode(node, "shape/point/@x"), XmlParser.FloatParseNode(node, "shape/point/@y"));
                newAE.radius = XmlParser.FloatParseNode(node, "shape/radius/@r");
                if(newAE.aeType == AEType.wall) newAE.theta = XmlParser.FloatParseNode(node, "shape/theta/@t");
                XmlNodeList asss = node.SelectNodes("assignments/assignment");
                foreach (XmlNode ass in asss)
                {
                    newAE.assignmentIDList.Add(XmlParser.IntParseNode(ass, "@id"));
                }
                result.Add(newAE);
            }
            return result;
        }

        public void SaveAsXML(Dictionary<int, AssignmentEntity> entityDic)
        {
            string path = Application.streamingAssetsPath + "/AssignmentEntity.xml";
            //FileStream file = File.OpenWrite(path);
            string writeData = "";
            writeData += "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n";
            writeData += "<AssignmentEntities>\n";
            foreach(KeyValuePair<int,AssignmentEntity> pair in entityDic)
            {
                writeData += "\n";
                writeData += "<AssignmentEntity id = \"" 
                    + pair.Value.aeID 
                    + "\" type = \"" 
                    + (pair.Value.aeType == AEType.computer ? "computer" : "wall")
                    + "\">\n";

                writeData += "<floor>" + pair.Value.floor + "</floor>\n" ;
                writeData += "<shape>\n" ;
                    writeData += "<point x = \""
                        + pair.Value.pos.x
                        + "\" y = \""
                        + pair.Value.pos.y
                        + "\" />\n" ;
                    writeData += "<radius r = \""
                        + pair.Value.radius
                        + "\" />\n" ;
                    writeData += "<theta t = \""
                        + pair.Value.theta
                        + "\" />\n" ;
                writeData += "</shape>\n";
                writeData += "<assignments>\n" ;
                    foreach(int id in pair.Value.assignmentIDList)
                    {
                        writeData += "<assignment id = \""
                            + id
                            + "\" />\n" ;
                    }
                writeData += "</assignments>\n" ;
                writeData += "</AssignmentEntity>\n";
            }
            writeData += "\n";
            writeData += "</AssignmentEntities>\n";
            File.WriteAllText(path, writeData);
        }
    }
}
