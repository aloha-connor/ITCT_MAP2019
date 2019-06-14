using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;

namespace ITCT
{
    public class EntityEditorWindow : MonoBehaviour
    {
        public EditorSystem editorSystem;

        public AssignmentEntity currentEntity;
        public AssignmentEntityRenderer currentRenderer;

        public InputField id;
        public InputField floor;
        public InputField rad;
        public InputField rot;
        public InputField posX;
        public InputField posY;
        public InputField assignments;

        void Start()
        {
			transform.localScale = new Vector2(0, 0);
            editorSystem.SubjectAssignmentEntityRendererSelected_Edit.AsObservable()
                .Subscribe(entity =>
                {
                    LoadAssEntity(entity);
                    transform.localScale = new Vector2(1, 1);
                });
        }

        protected void LoadAssEntity(AssignmentEntityRenderer entityRenderer)
        {
			Debug.Log("dd");
			currentRenderer = entityRenderer;
			currentEntity = entityRenderer.mapSystem.assignmentEntityDictionary[entityRenderer.aeID] ;
            id.text = currentEntity.aeID.ToString();
            floor.text = currentEntity.floor.ToString();
            rad.text = currentEntity.radius.ToString();
            rot.text = currentEntity.theta.ToString();
            posX.text = currentEntity.pos.x.ToString();
            posY.text = currentEntity.pos.y.ToString();
            string a = "";
            foreach (int i in currentEntity.assignmentIDList) a += i + " ";
            assignments.text = a;
        }

        public void SaveAssEntity()
        {
			if(!CheckValid()) 
			{
				Debug.Log("Cannot");
				return;
			}
			int newId = int.Parse(id.text);
			if(currentEntity.aeID != newId)
			{
				editorSystem.mapSystem.assignmentEntityDictionary.Remove(currentEntity.aeID);
				editorSystem.mapSystem.assignmentEntityDictionary.Add(newId, currentEntity);
			}
            currentEntity.aeID = newId;
            currentEntity.floor = int.Parse(floor.text);
            currentEntity.radius = float.Parse(rad.text);
            currentEntity.theta = float.Parse(rot.text);
            currentEntity.pos = new Vector2(float.Parse(posX.text), float.Parse(posY.text));
            string[] a = assignments.text.Split(' ');
            List<int> newList = new List<int>();
            foreach (string s in a) 
			{
				if(!s.Equals("")) newList.Add(int.Parse(s));
			}
            currentEntity.assignmentIDList = newList;

			currentRenderer.Reinitialize(newId);
        }

		protected bool CheckValid()
		{
			bool flag = false;
			int dummyInt ; float dummyFloat;

			flag = int.TryParse(id.text, out dummyInt);
			if(!flag) return false;
			flag = dummyInt == currentEntity.aeID || !editorSystem.mapSystem.assignmentEntityDictionary.ContainsKey(dummyInt);
			if(!flag) return false;
			flag = int.TryParse(floor.text, out dummyInt);
			if(!flag) return false;
			flag = float.TryParse(rad.text, out dummyFloat);
			if(!flag) return false;
			flag = float.TryParse(rot.text, out dummyFloat);
			if(!flag) return false;
			flag = float.TryParse(posX.text, out dummyFloat);
			if(!flag) return false;
			flag = float.TryParse(posY.text, out dummyFloat);
			if(!flag) return false;
            string[] a = assignments.text.Split(' ');
            foreach (string s in a) 
			{
				if(s.Equals("")) continue;
				flag = int.TryParse(s, out dummyInt);
				if(!flag) return false;
			}
			return true;
		}

        public void CloseWindow()
        {
            transform.localScale = Vector2.zero;
        }

        public void SaveAndCloseWindow()
        {
			SaveAssEntity();
            transform.localScale = Vector2.zero;
        }
    }

}