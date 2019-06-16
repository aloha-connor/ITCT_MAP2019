using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
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

        protected CompositeDisposable compDisp ;

        void Awake()
        {
            compDisp = new CompositeDisposable();
        }
        
        void Start()
        {
			transform.localScale = new Vector2(0, 0);
            editorSystem.SubjectAssignmentEntityRendererSelected_Edit.AsObservable()
                .Subscribe(entity =>
                {
                    SaveAssEntity();
                    LoadAssEntity(entity);
                    transform.localScale = new Vector2(1, 1);
                });

            // editorSystem.currentMode.AsObservable()
            //     .Subscribe(mode => {
            //         InputField[] fields = GetComponentsInChildren<InputField>();
            //         foreach(InputField f in fields)
            //         {
            //             f.readOnly = !(mode == EditorSystem.EditMode.VALUES) ;
            //         }
            //     });
        }

        protected void LoadAssEntity(AssignmentEntityRenderer entityRenderer)
        {
			Debug.Log("dd");
            compDisp.Clear();

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

            Observable
                .CombineLatest(posX.ObserveEveryValueChanged(__ => __.text), posY.ObserveEveryValueChanged(__ => __.text))
                .Where(__ => editorSystem.currentMode.Value == EditorSystem.EditMode.VALUES)
                .Subscribe(_pos => {
                    float pos0, pos1;
                    bool canParse;
                    canParse = float.TryParse(_pos[0], out pos0) ;
                    canParse = float.TryParse(_pos[1], out pos1) && canParse;
                    if(canParse) currentRenderer.transform.localPosition = new Vector2(pos0, pos1);
                }).AddTo(compDisp);

            rot.ObserveEveryValueChanged(__ => __.text)
                .Where(__ => editorSystem.currentMode.Value == EditorSystem.EditMode.VALUES)
                .Subscribe(_rot => {
                    float rotOut ;
                    if(float.TryParse(_rot, out rotOut)) currentRenderer.transform.rotation = Quaternion.Euler(0,0,rotOut);
                });
                
            rad.ObserveEveryValueChanged(__ => __.text)
                .Where(__ => currentRenderer.myType == AEType.wall)
                .Where(__ => editorSystem.currentMode.Value == EditorSystem.EditMode.VALUES)
                .Subscribe(_rad => {
                    float radOut ;
                    if(float.TryParse(_rad, out radOut))
                        currentRenderer.GetComponentInChildren<SpriteRenderer>().size = 
                            new Vector2(radOut, currentRenderer.GetComponentInChildren<SpriteRenderer>().size.y);
                });

            rad.ObserveEveryValueChanged(__ => __.text)
                .Where(__ => currentRenderer.myType == AEType.computer)
                .Where(__ => editorSystem.currentMode.Value == EditorSystem.EditMode.VALUES)
                .Subscribe(_rad => {
                    float radOut ;
                    if(float.TryParse(_rad, out radOut))
                        currentRenderer.GetComponentInChildren<SpriteRenderer>().size = 
                            new Vector2(radOut, radOut) * 2;
                });

            entityRenderer.ObserveEveryValueChanged(__ => __.transform.localPosition)
                .Where(__ => editorSystem.currentMode.Value == EditorSystem.EditMode.TRANSFORM)
                .Subscribe(pos => {
                    posX.text = pos.x.ToString();
                    posY.text = pos.y.ToString();
                }).AddTo(compDisp);
            
            entityRenderer.ObserveEveryValueChanged(__ => __.transform.localEulerAngles)
                .Where(__ => editorSystem.currentMode.Value == EditorSystem.EditMode.TRANSFORM)
                .Subscribe(_rot => {
                    rot.text = _rot.z.ToString();
                }).AddTo(compDisp);

            currentRenderer.GetComponentInChildren<SpriteRenderer>().ObserveEveryValueChanged(__ => __.size)
                .Where(__ => editorSystem.currentMode.Value == EditorSystem.EditMode.TRANSFORM)
                .Subscribe(_size => {
                    rad.text = _size.x.ToString();
                }).AddTo(compDisp);
        }

        public void SaveAssEntity()
        {
			if(!CheckValid()) 
			{
				Debug.Log("Cannot Save!");
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
        }

		protected bool CheckValid()
		{
			bool flag = false;
			int dummyInt ; float dummyFloat;

			flag = int.TryParse(id.text, out dummyInt);
			if(!flag) 
            {
                editorSystem.SendWarningMessage("ID값이 유효하지 않습니다.");
                return false;
            }
			flag = dummyInt == currentEntity.aeID || !editorSystem.mapSystem.assignmentEntityDictionary.ContainsKey(dummyInt);
			if(!flag) 
            {
                editorSystem.SendWarningMessage("ID값이 중복되었습니다.");
                return false;
            }
			flag = int.TryParse(floor.text, out dummyInt);
			if(!flag) 
            {
                editorSystem.SendWarningMessage("Floor값이 유효하지 않습니다.");
                return false;
            }
			flag = float.TryParse(rad.text, out dummyFloat);
			if(!flag) 
            {
                editorSystem.SendWarningMessage("rad값이 유효하지 않습니다.");
                return false;
            }
			flag = float.TryParse(rot.text, out dummyFloat);
			if(!flag) 
            {
                editorSystem.SendWarningMessage("rot값이 유효하지 않습니다.");
                return false;
            }
			flag = float.TryParse(posX.text, out dummyFloat);
			if(!flag) 
            {
                editorSystem.SendWarningMessage("posX값이 유효하지 않습니다.");
                return false;
            }
			flag = float.TryParse(posY.text, out dummyFloat);
			if(!flag) 
            {
                editorSystem.SendWarningMessage("posY값이 유효하지 않습니다.");
                return false;
            }
            string[] a = assignments.text.Split(' ');
            foreach (string s in a) 
			{
				if(s.Equals("")) continue;
				flag = int.TryParse(s, out dummyInt);
				if(!flag) 
                {
                    editorSystem.SendWarningMessage("Assignments ID가 유효하지 않습니다.");
                    return false;
                }
			}
			return true;
		}

        public void CloseWindow()
        {
			currentRenderer.Reinitialize(currentEntity.aeID);
            compDisp.Clear();
            transform.localScale = Vector2.zero;
            editorSystem.EditorWindowClosed();
        }

        public void SaveAndCloseWindow()
        {
			SaveAssEntity();
            CloseWindow();
        }
    }

}