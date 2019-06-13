using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;
using DG.Tweening;

namespace ITCT
{
    public class InfoListViewer : MonoBehaviour
    {
        public AssignmentSystem assignmentSystem;
        public ClassSystem classSystem;
        public InfoSystem infoSystem;

        public InfoCard infoCardPrefab;
        public InfoCard classInfoCardPrefab;

        public GameObject content;

        //public List<InfoCard> infoCardList;
        public Dictionary<int, InfoCard> infoCardTable;

        public Subject<RemapData> SubjectRemap { get; protected set; }

        public struct RemapData {public int num; public int classNum;}

        // Use this for initialization
        void Awake()
        {
            SubjectRemap = new Subject<RemapData>();
            infoCardTable = new Dictionary<int, InfoCard>();

            assignmentSystem.SubjectAssignmentListUpdated.AsObservable()
                .Subscribe(__ => InstantiateInfoTable(__));

            SubjectRemap.AsObservable()
                .Subscribe(rmdata =>
                {
                    Vector2 targetSize = new Vector2(
                        content.GetComponent<RectTransform>().sizeDelta.x,
                        rmdata.num * (InfoCard.gap + infoCardPrefab.GetComponent<RectTransform>().sizeDelta.y) + InfoCard.gap
                        );
                        
				    targetSize = targetSize - new Vector2(0, InfoCard.classSize.y * (rmdata.classNum+1));

                    content.GetComponent<RectTransform>().DOSizeDelta(targetSize, .5f)
                        .SetEase(Ease.InOutCubic);
                });

            infoSystem.SubjectQueriedAssignmentsChanged.AsObservable()
                .Subscribe(l => SetRemappedOrder(l));
        }

        private void InstantiateInfoTable(List<Assignment> assList)
        {
            int i = 0;
            int currentClass = -1;

            foreach (Assignment ass in assList)
            {
                currentClass = ass.classID;
                
                if(!infoCardTable.ContainsKey(currentClass + 1000))
                {
                    InfoCard classCard = GameObject.Instantiate(classInfoCardPrefab);
                    classCard.InitializeInfoCard(currentClass + 1000, i++, content.transform, this);
                    infoCardTable.Add(currentClass + 1000, classCard);   
                }

                InfoCard newCard = GameObject.Instantiate(infoCardPrefab);
                newCard.InitializeInfoCard(ass.id, i++, content.transform, this);
                infoCardTable.Add(ass.id, newCard);
            }
        }

        private void SetRemappedOrder(ICollection<int> queried)
        {
            int order = 0;
            int classOrder = -1;

            foreach(KeyValuePair<int,InfoCard> pair in infoCardTable)
            {
                pair.Value.listOrder = -1;
            }

            IEnumerator<int> e = queried.GetEnumerator();
            while(e.MoveNext())
            {
                int i = e.Current;
                if(i >= 1000) classOrder++;
                infoCardTable[i].listOrder = order++;
                infoCardTable[i].classOrder = classOrder;
            }
            
            RemapInfoCards(order, classOrder);
        }

        private void RemapInfoCards(int numberOfCards, int classNum)
        {
            RemapData rmdata ;
            rmdata.num = numberOfCards;
            rmdata.classNum = classNum;
            SubjectRemap.OnNext(rmdata);
        }

#if UNITY_EDITOR
        int it;
        [ContextMenu("RemapTest")]
        public void RemapTest()
        {
            // for (int i = 0; i < infoCardList.Count; i++)
            // {
            //     if (it == 0) infoCardList[i].listOrder = infoCardList.Count - 1 - i;
            //     else if (it == 1) infoCardList[i].listOrder = -1;
            //     else infoCardList[i].listOrder = i;
            // }
            // RemapInfoCards(infoCardList.Count);
            it++;
            it %= 3;
        }
#endif
    }
}
