using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;

namespace ITCT
{
    public class AssignmentEntityRenderer : MonoBehaviour
    {
        public int aeID;
        public InfoSystem infoSystem;
        public ReactiveProperty<bool> highlighted = new ReactiveProperty<bool>();

        public AssignmentEntity myEntity;
        private CompositeDisposable compDisp = new CompositeDisposable();

        void Start()
        {
            highlighted.AsObservable()
                .Subscribe(flag => {
                    GetComponentInChildren<SpriteRenderer>().color = flag ? Color.red : Color.white ;
                });
        }

        public void Initialize(InfoSystem _is)
        {
            infoSystem = _is;
            compDisp.Clear();

            infoSystem.SubjectQueriedAssignmentsChanged.AsObservable()
                .Subscribe(l => {
                    bool flag = false;
                    foreach(int _id in l)
                    {
                        if(myEntity.assignmentIDList.Contains(_id))
                        {
                            flag = true ;
                            break;
                        }
                    }
                    highlighted.Value = flag ;
                }).AddTo(compDisp);
        }
    }
}
