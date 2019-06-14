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

        public GameObject editorCanvas;

        public ReactiveProperty<bool> editorOn;

				public Subject<AssignmentEntityRenderer> SubjectAssignmentEntityRendererSelected_Edit ;

        void Awake()
        {
            editorOn = new ReactiveProperty<bool>(false);
						SubjectAssignmentEntityRendererSelected_Edit = new Subject<AssignmentEntityRenderer>();
        }

        // Use this for initialization
        void Start()
        {
            this.UpdateAsObservable()
                .Where(__ => Input.GetKeyDown(KeyCode.F3))
                .Subscribe(__ =>
                {
                    editorOn.Value = !editorOn.Value;
                    editorCanvas.SetActive(editorOn.Value);
                });

						mapSystem.SubjectAssignmentEntityRendererSelected.AsObservable()
								.Where(__ => editorOn.Value)
								.Subscribe(__ => {
										SubjectAssignmentEntityRendererSelected_Edit.OnNext(__);
								});
							
        }
    }
}