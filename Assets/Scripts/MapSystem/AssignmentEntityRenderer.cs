using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;
using DG.Tweening;

namespace ITCT
{
    public class AssignmentEntityRenderer : MonoBehaviour
    {
        public int aeID;
        public InfoSystem infoSystem;
        public MapSystem mapSystem;
        public ReactiveProperty<bool> queried = new ReactiveProperty<bool>(false);
        public ReactiveProperty<bool> selected = new ReactiveProperty<bool>(false);
        public AEType myType;

        private CompositeDisposable compDisp = new CompositeDisposable();

        void Start()
        {
            queried.AsObservable()
                .Subscribe(flag => {
                    Color targetColor = flag ? Color.red : Color.black;
                    if(myType == AEType.computer) 
                    {
                        GetComponentInChildren<SpriteRenderer>()
                            .DOColor(targetColor, 0.5f);
                    }
                    else 
                    {
                        Color currentcolor = GetComponentInChildren<LineRenderer>().startColor;
                        GetComponentInChildren<LineRenderer>()
                            .DOColor(new Color2(currentcolor, currentcolor), new Color2(targetColor, targetColor), 0.5f);
                    }
                });

            selected.AsObservable()
                .Subscribe(flag => {
                    Color targetColor = flag ? Color.yellow : queried.Value ? Color.red : Color.black;
                    if(myType == AEType.computer) 
                    {
                        GetComponentInChildren<SpriteRenderer>()
                            .DOColor(targetColor, 0.5f);
                    }
                    else 
                    {
                        Color currentcolor = GetComponentInChildren<LineRenderer>().startColor;
                        GetComponentInChildren<LineRenderer>()
                            .DOColor(new Color2(currentcolor, currentcolor), new Color2(targetColor, targetColor), 0.5f);
                    }
                });
        }

        public void Initialize(int _aeID, InfoSystem _is, MapSystem _ms)
        {
            aeID = _aeID;
            infoSystem = _is ;
            mapSystem = _ms ;
            compDisp.Clear();

            AssignmentEntity myEntity = mapSystem.assignmentEntityDictionary[aeID];

            transform.parent = mapSystem.floorList[myEntity.floor - 1].transform ;

            if(myEntity.aeType == AEType.computer)
            {
                transform.localPosition = myEntity.pos;
            }
            else
            {
                LineRenderer lr = GetComponentInChildren<LineRenderer>();
                transform.localPosition = Vector2.zero;
                lr.SetPosition(0, myEntity.pos);
                lr.SetPosition(1, myEntity.pos2);
            }

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
                    queried.Value = flag ;
                }).AddTo(compDisp);

            infoSystem.SubjectSelectedInfoCardChanged.AsObservable()
                .Subscribe(_id => {
                    selected.Value = myEntity.assignmentIDList.Contains(_id) ;
                }) ;
        }
    }
}
