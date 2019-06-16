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

        public Color queriedColor;
        public Color selectedColor;
        public Color defaultColor;

        private CompositeDisposable compDisp = new CompositeDisposable();

        void Start()
        {
            queried.AsObservable()
                .Subscribe(flag =>
                {
                    Color targetColor = flag ? queriedColor : defaultColor;
                    GetComponentInChildren<SpriteRenderer>()
                        .DOColor(targetColor, 0.5f);
                }).AddTo(this);

            selected.AsObservable()
                .Subscribe(flag =>
                {
                    Color targetColor = flag ? selectedColor : queried.Value ? queriedColor : defaultColor;
                    GetComponentInChildren<SpriteRenderer>()
                        .DOColor(targetColor, 0.5f);
                }).AddTo(this);
        }

        public void Initialize(int _aeID, InfoSystem _is, MapSystem _ms)
        {
            aeID = _aeID;
            infoSystem = _is;
            mapSystem = _ms;
            compDisp.Clear();

            AssignmentEntity myEntity = mapSystem.assignmentEntityDictionary[aeID];

            transform.parent = mapSystem.coordinate;//mapSystem.floorList[myEntity.floor - 1].transform;
            transform.localPosition = myEntity.pos;

            SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();

            if (myEntity.aeType == AEType.computer)
            {
                sr.size = new Vector2(myEntity.radius, myEntity.radius) * 2;
                GetComponentInChildren<CircleCollider2D>().radius = myEntity.radius;
            }
            else
            {
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, myEntity.theta));
                sr.size = new Vector2(myEntity.radius, sr.size.y);
                GetComponentInChildren<BoxCollider2D>().size = sr.size;
            }

            mapSystem.currentFloor.AsObservable()
                .Subscribe(f => {
                    Vector2 targetScale = InCurrentFloor() ? new Vector2(1,1) : Vector2.zero;
                    transform.DOScale(targetScale, .8f)
                        .SetEase(Ease.InOutElastic);
                }).AddTo(compDisp);

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

        public bool InCurrentFloor()
        {
            return mapSystem.assignmentEntityDictionary[aeID].floor == mapSystem.currentFloor.Value;
        }
    }
}
