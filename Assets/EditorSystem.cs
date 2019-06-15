using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace ITCT
{
    public class EditorSystem : MonoBehaviour
    {

        public MapSystem mapSystem;
        public InfoSystem infoSystem;

        public GameObject editorCanvas;

        public ReactiveProperty<bool> editorOn;

        public AssignmentEntityRenderer selectedEntity;

        public ReactiveProperty<EditMode> currentMode { get; protected set; }

        public Subject<AssignmentEntityRenderer> SubjectAssignmentEntityRendererSelected_Edit;

        public AssignmentEntityRenderer assCompPrefab;
        public AssignmentEntityRenderer assWallPrefab;

        private Vector3 mousePos;

        public enum EditMode
        {
            NONE, VALUES, TRANSFORM
        }

        void Awake()
        {
            editorOn = new ReactiveProperty<bool>(false);
            currentMode = new ReactiveProperty<EditMode>(EditMode.NONE);
            SubjectAssignmentEntityRendererSelected_Edit = new Subject<AssignmentEntityRenderer>();
        }

        // Use this for initialization
        void Start()
        {
            this.UpdateAsObservable()
                .Where(__ => Input.GetKeyDown(KeyCode.F2))
                .Subscribe(__ =>
                {
                    editorOn.Value = !editorOn.Value;
                });

            this.UpdateAsObservable()
                .Where(__ => editorOn.Value)
                .Where(__ => Input.GetKeyDown(KeyCode.F3))
                .Subscribe(__ =>
                {
                    CreateNew(AEType.computer);
                });

            this.UpdateAsObservable()
                .Where(__ => editorOn.Value)
                .Where(__ => Input.GetKeyDown(KeyCode.F4))
                .Subscribe(__ =>
                {
                    CreateNew(AEType.wall);
                });

            this.UpdateAsObservable()
                .Where(__ => this.currentMode.Value == EditMode.TRANSFORM)
                .Subscribe(__ =>
                {
                    if (Input.GetKey(KeyCode.Alpha1))
                    {
                        Vector3 screenToWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        selectedEntity.transform.position = new Vector3(screenToWorld.x, screenToWorld.y, selectedEntity.transform.position.z);
                    }
                    if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Alpha3))
                    {
                        mousePos = Input.mousePosition;
                    }
                    else if (Input.GetKey(KeyCode.Alpha2))
                    {
                        float deltaY = Input.mousePosition.y - mousePos.y;
                        selectedEntity.transform.Rotate(0, 0, deltaY * Time.deltaTime * 10);
                        mousePos = Input.mousePosition;
                    }
                    else if (Input.GetKey(KeyCode.Alpha3))
                    {
                        float deltaX = Input.mousePosition.x - mousePos.x;
                        Vector2 size = selectedEntity.GetComponentInChildren<SpriteRenderer>().size;
                        selectedEntity.GetComponentInChildren<SpriteRenderer>().size = new Vector2(size.x + deltaX * Time.deltaTime, size.y);
                        mousePos = Input.mousePosition;
                    }
                });

            editorOn.AsObservable()
                .Subscribe(flag =>
                {
                    editorCanvas.SetActive(flag);
                });

            mapSystem.SubjectAssignmentEntityRendererSelected.AsObservable()
                    .Where(__ => editorOn.Value)
                    .Subscribe(ent =>
                    {
                        if (currentMode.Value == EditMode.NONE || selectedEntity != ent)
                        {
                            currentMode.Value = EditMode.VALUES;
                            selectedEntity = ent;
                            SubjectAssignmentEntityRendererSelected_Edit.OnNext(ent);
                        }
                        else
                        {
                            if (currentMode.Value == EditMode.TRANSFORM)
                            {
                                Debug.Log("current mode :: values");
                                currentMode.Value = EditMode.VALUES;
                            }
                            else
                            {
                                Debug.Log("current mode :: transform");
                                currentMode.Value = EditMode.TRANSFORM;
                            }
                        }
                    });

        }

        public void CreateNew(AEType _type)
        {
            AssignmentEntity newEntity = new AssignmentEntity();
            int newID = 1000;
            while(mapSystem.assignmentEntityDictionary.ContainsKey(newID)) newID++ ;
            newEntity.aeID = newID;
            newEntity.aeType = _type;
            newEntity.floor = mapSystem.currentFloor.Value;
            newEntity.pos = Vector2.zero;
            newEntity.radius = 3;
            newEntity.theta = 0;

            mapSystem.assignmentEntityDictionary.Add(newEntity.aeID, newEntity);

            AssignmentEntityRenderer newRenderer = _type == AEType.computer ?
                GameObject.Instantiate(assCompPrefab, mapSystem.coordinate) :
                GameObject.Instantiate(assWallPrefab, mapSystem.coordinate);

            newRenderer.Initialize(newEntity.aeID, infoSystem, mapSystem);
        }

        public void EditorWindowClosed()
        {
            currentMode.Value = EditMode.NONE;
            selectedEntity = null;
        }
    }
}