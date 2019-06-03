using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace ITCT
{
    public class AssignmentSystem : MonoBehaviour
    {
        public MapSystem mapSystem;

        public List<Assignment> assignmentList{get;protected set;}

        private AssignmentXMLParser xmlParser;

        public Subject<int> SubjectAssignmentUpdated{get;protected set;}
        public Subject<List<Assignment>> SubjectAssignmentListUpdated{get;protected set;}

        void Awake()
        {
            SubjectAssignmentUpdated = new Subject<int>();
            SubjectAssignmentListUpdated = new Subject<List<Assignment>>();

            assignmentList = new List<Assignment>();
            xmlParser = new AssignmentXMLParser();
            TextAsset xmlText = (TextAsset)Resources.Load("Assignment");
            assignmentList = xmlParser.ParseXML(xmlText);
        }

        void Start()
        {
            SubjectAssignmentListUpdated.OnNext(assignmentList);
        }
    }
}
