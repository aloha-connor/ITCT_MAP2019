using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace ITCT
{
    public class AssignmentXMLParser
    {
        public List<Assignment> ParseXML(TextAsset textAsset)
        {
            List<Assignment> result = new List<Assignment>();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(textAsset.text);

            XmlNode root = xmlDoc.SelectSingleNode("Assignments");
            XmlNodeList assignments = root.SelectNodes("Assignment");
            foreach(XmlNode node in assignments)
            {
                Assignment newAss = new Assignment();
                newAss.id = IntParseNode(node,"@id");
                newAss.floor = int.Parse(node.SelectSingleNode("floor").InnerText);
                newAss.room = int.Parse(node.SelectSingleNode("room").InnerText);
                // newAss.pos = 
                //     new Vector2(
                //         float.Parse(node.SelectSingleNode("room/@x").InnerText),
                //         float.Parse(node.SelectSingleNode("room/@y").InnerText)
                //     );
                newAss.classID = int.Parse(node.SelectSingleNode("classID").InnerText);
                newAss.assignmentEntityID = IntParseNode(node,"entityID");
                newAss.title = node.SelectSingleNode("title").InnerText;
                newAss.comment = node.SelectSingleNode("comment").InnerText;
                newAss.conceptComment = node.SelectSingleNode("conceptComment").InnerText;
                newAss.teamMates = new List<string>();
                newAss.assignmentTagMask = GetAssignmentTagMask(node);
                XmlNodeList teamMatesList = node.SelectNodes("teammates/person");
                foreach(XmlNode mate in teamMatesList) newAss.teamMates.Add(mate.InnerText);
                result.Add(newAss);
            }

            return result;
        }

        private int IntParseNode(XmlNode parent, string nodeName)
        {
            return int.Parse(parent.SelectSingleNode(nodeName).InnerText);
        }

        private int GetAssignmentTagMask(XmlNode parent)
        {
            int result = 0;
            XmlNodeList list = parent.SelectNodes("tag");
            foreach(XmlNode n in list)
            {
                string inner = n.InnerText;
                int shift =
                    inner.Equals("poster") ? (int)AssignmentTag.poster :
                    inner.Equals("video") ? (int)AssignmentTag.video :
                    inner.Equals("prototype") ? (int)AssignmentTag.prototype :
                    inner.Equals("game") ? (int)AssignmentTag.game :
                    inner.Equals("web") ?(int) AssignmentTag.web :
                    inner.Equals("installation") ? (int)AssignmentTag.installation : 32;
                result |= 1 << shift;
            }
            return result;
        }
    }
}
