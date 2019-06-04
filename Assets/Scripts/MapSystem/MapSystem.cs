using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ITCT
{
    public class MapSystem : MonoBehaviour
    {
        public List<Floor> floorList;
        public AssignmentEntityRenderer assCompPrefab;
        public AssignmentEntityRenderer assWallPrefab;
        public InfoSystem infoSystem;
        public Dictionary<int, AssignmentEntity> assignmentEntityDictionary;

        protected AssignmentEntityXMLParser xmlParser;

        void Awake()
        {
            xmlParser = new AssignmentEntityXMLParser();
            TextAsset xmlText = (TextAsset)Resources.Load("AssignmentEntity");
            assignmentEntityDictionary = MakeDictionary(xmlParser.ParseXML(xmlText));
        }

        void Start()
        {
            InstEntityRenderers();
        }

        public void InstEntityRenderers()
        {
            AssignmentEntityRenderer newAERenderer;
            foreach (KeyValuePair<int, AssignmentEntity> kv in assignmentEntityDictionary)
            {
                AssignmentEntity ae = kv.Value;

                if (ae.aeType == AEType.computer)
                {
                    newAERenderer = GameObject.Instantiate(assCompPrefab);
                }
                else
                {
                    newAERenderer = GameObject.Instantiate(assWallPrefab);
                }
                newAERenderer.Initialize(kv.Key, infoSystem, this);
            }
        }

        protected Dictionary<int, AssignmentEntity> MakeDictionary(List<AssignmentEntity> list)
        {
            Dictionary<int, AssignmentEntity> result = new Dictionary<int, AssignmentEntity>();

            foreach (AssignmentEntity ae in list)
            {
                result.Add(ae.aeID, ae);
            }

            return result;
        }
    }
}