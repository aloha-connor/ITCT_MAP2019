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
                .Subscribe(flag =>
                {
                    Color targetColor = flag ? Color.red : Color.black;
                    GetComponentInChildren<SpriteRenderer>()
                        .DOColor(targetColor, 0.5f);
                    // if (myType == AEType.computer)
                    // {
                    //     GetComponentInChildren<SpriteRenderer>()
                    //         .DOColor(targetColor, 0.5f);
                    // }
                    // else
                    // {
                    //     Color currentcolor = GetComponentInChildren<LineRenderer>().startColor;
                    //     GetComponentInChildren<LineRenderer>()
                    //         .DOColor(new Color2(currentcolor, currentcolor), new Color2(targetColor, targetColor), 0.5f);
                    // }
                });

            selected.AsObservable()
                .Subscribe(flag =>
                {
                    Color targetColor = flag ? Color.yellow : queried.Value ? Color.red : Color.black;
                    GetComponentInChildren<SpriteRenderer>()
                        .DOColor(targetColor, 0.5f);
                    // if (myType == AEType.computer)
                    // {
                    //     GetComponentInChildren<SpriteRenderer>()
                    //         .DOColor(targetColor, 0.5f);
                    // }
                    // else
                    // {
                    //     Color currentcolor = GetComponentInChildren<LineRenderer>().startColor;
                    //     GetComponentInChildren<LineRenderer>()
                    //         .DOColor(new Color2(currentcolor, currentcolor), new Color2(targetColor, targetColor), 0.5f);
                    // }
                });
        }

        public void Initialize(int _aeID, InfoSystem _is, MapSystem _ms)
        {
            aeID = _aeID;
            infoSystem = _is;
            mapSystem = _ms;
            compDisp.Clear();

            AssignmentEntity myEntity = mapSystem.assignmentEntityDictionary[aeID];

            transform.parent = mapSystem.floorList[myEntity.floor - 1].transform;
            transform.localPosition = myEntity.pos;

            if (myEntity.aeType == AEType.computer)
            {
            }
            else
            {
                SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, myEntity.theta));
                sr.size = new Vector2(myEntity.radius, sr.size.y);
                GetComponentInChildren<BoxCollider2D>().size = sr.size;
            }

            mapSystem.SubjectAssignmentEntityModified.AsObservable()
                .Where(id => id == aeID)
                .Subscribe(id => {
                    Initialize(id, infoSystem, mapSystem);
                }).AddTo(compDisp);

            infoSystem.SubjectQueriedAssignmentsChanged.AsObservable()
                .Subscribe(l =>
                {
                    bool flag = false;
                    foreach (int _id in l)
                    {
                        if (myEntity.assignmentIDList.Contains(_id))
                        {
                            flag = true;
                            break;
                        }
                    }
                    queried.Value = flag;
                }).AddTo(compDisp);

            infoSystem.SelectedAssignmentID.AsObservable()
                .Subscribe(_id =>
                {
                    selected.Value = myEntity.assignmentIDList.Contains(_id);
                }).AddTo(compDisp);
        }

        public void Reinitialize(int id)
        {
            Initialize(id, infoSystem, mapSystem);
        }
    }
}
