using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.IO;

namespace ITCT
{
    public class MapSystem : MonoBehaviour
    {
        public Transform coordinate;
        public AssignmentEntityRenderer assCompPrefab;
        public AssignmentEntityRenderer assWallPrefab;
        public InfoSystem infoSystem;
        public Dictionary<int, AssignmentEntity> assignmentEntityDictionary { get; protected set; }

        public ReactiveProperty<int> currentFloor { get; protected set; }

        public Subject<AssignmentEntityRenderer> SubjectAssignmentEntityRendererSelected { get; protected set; }
        public Subject<int> SubjectAssignmentEntityModified { get; protected set; }

        protected AssignmentEntityXMLParser xmlParser;

        void Awake()
        {
            xmlParser = new AssignmentEntityXMLParser();
            string xmlText = LoadXMLFile();
            assignmentEntityDictionary = MakeDictionary(xmlParser.ParseXML(xmlText));
            SubjectAssignmentEntityRendererSelected = new Subject<AssignmentEntityRenderer>();
            SubjectAssignmentEntityModified = new Subject<int>();
            currentFloor = new ReactiveProperty<int>(1);
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

        public void SelectAssignmentEntityRenderer(AssignmentEntityRenderer r)
        {
            SubjectAssignmentEntityRendererSelected.OnNext(r);
        }

        public void SetCurrentFloor(int f)
        {
            if (f >= 1 && f <= 3)
            {
                currentFloor.Value = f;
            }
        }

        public void DownStair()
        {
            SetCurrentFloor(currentFloor.Value - 1);
        }

        public void UpStair()
        {
            SetCurrentFloor(currentFloor.Value + 1);
        }

        public void SaveCurrentEntityData()
        {
            xmlParser.SaveAsXML(assignmentEntityDictionary);
        }

        protected string LoadXMLFile()
        {
            string result = "";
            string path = Application.streamingAssetsPath + "/AssignmentEntity.xml";

            Debug.Log(Application.streamingAssetsPath);

            if (!File.Exists(path))
            {
                string origin = ((TextAsset)Resources.Load("AssignmentEntity")).text;
                FileStream file = File.Create(path);
                file.Close();
                File.WriteAllText(path, origin);
            }
            using (StreamReader reader = new StreamReader(path))
            {
                string line;
                do
                {
                    line = reader.ReadLine();
                    result += line + "\n";
                }
                while(line != null);
            }
            return result;
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