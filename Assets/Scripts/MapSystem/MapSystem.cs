using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ITCT
{
    public class MapSystem : MonoBehaviour
    {
			public List<Floor> floorList;
			public AssignmentEntity assCompPrefab;
			public AssignmentEntity assWallPrefab;
			public InfoSystem infoSystem;
			protected List<AssignmentEntity> assignmentEntityList;

			protected AssignmentEntityXMLParser xmlParser;

			void Awake()
			{
				xmlParser = new AssignmentEntityXMLParser();
				TextAsset xmlText = (TextAsset)Resources.Load("AssignmentEntity");
				assignmentEntityList = xmlParser.ParseXML(xmlText);
			}

			void Start()
			{
				InstEntityRenderers();
			}

			public void InstEntityRenderers()
			{
				for(int i = 0; i < assignmentEntityList.Count ; i++)
				{
					// AssignmentEntity newAssDot ;
					// if(i < assignmentEntityList.Count) 
					// {
					// 	newAssDot = assignmentEntityList[i];
					// } 
					// else
					// {
					// 	newAssDot = GameObject.Instantiate(assDotPrefab);
					// 	assignmentEntityList.Add(newAssDot);
					// }
					// newAssDot.Initialize(_as, this, infoSystem);
				}
			}
    }
}