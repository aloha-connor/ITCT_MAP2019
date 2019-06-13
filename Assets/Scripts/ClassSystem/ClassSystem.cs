using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace ITCT
{
    public class ClassSystem : MonoBehaviour
    {
        public List<ClassObject> classList{get;protected set;}

		public Subject<List<ClassObject>> SubjectClassListUpdated {get ; protected set;}

        private ClassXMLParser xmlParser;

        void Awake()
        {
			SubjectClassListUpdated = new Subject<List<ClassObject>>();
            classList = new List<ClassObject>();
            xmlParser = new ClassXMLParser();
            TextAsset xmlText = (TextAsset)Resources.Load("Class");
            classList = xmlParser.ParseXML(xmlText);
        }

		void Start()
		{
			SubjectClassListUpdated.OnNext(classList);
		}
    }
}