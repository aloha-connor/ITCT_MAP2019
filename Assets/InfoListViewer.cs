﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using DG.Tweening;

namespace ITCT
{
    public class InfoListViewer : MonoBehaviour
    {
		public AssignmentSystem assignmentSystem;
		public InfoSystem infoSystem;

		public InfoCard infoCardPrefab;

		public GameObject content;

		public List<InfoCard> infoCardList;

		public Subject<int> SubjectRemap {get; protected set;}

        // Use this for initialization
        void Awake()
        {
			SubjectRemap = new Subject<int>();

			assignmentSystem.SubjectAssignmentListUpdated.AsObservable()
				.Subscribe(__ => InstantiateInfoList(__));

			SubjectRemap.AsObservable()
				.Subscribe(num => {
					Vector2 targetSize = new Vector2(
						content.GetComponent<RectTransform>().sizeDelta.x,
						num * (InfoCard.gap + infoCardPrefab.GetComponent<RectTransform>().sizeDelta.y) + InfoCard.gap
						);
					
					content.GetComponent<RectTransform>().DOSizeDelta(targetSize,.5f)
						.SetEase(Ease.InOutCubic);
				});
        }

		private void InstantiateInfoList(List<Assignment> assList)
		{
			int i = 0;
			foreach(Assignment ass in assList)
			{
				InfoCard newCard = GameObject.Instantiate(infoCardPrefab);
				newCard.InitializeInfoCard(ass.id, i++, content.transform, this);
				infoCardList.Add(newCard);
			}
			RemapInfoCards(infoCardList.Count);
		}

		int it;
		[ContextMenu("RemapTest")]
		public void RemapTest()
		{
			for(int i = 0 ; i < infoCardList.Count ; i++)
			{
				if(it == 0) infoCardList[i].listOrder = infoCardList.Count - 1 - i ;
				else if(it == 1) infoCardList[i].listOrder = -1;
				else infoCardList[i].listOrder = i ;
			}
			RemapInfoCards(infoCardList.Count);
			it++;
			it %= 3;
		}

		public void RemapInfoCards(int numberOfCards)
		{
			SubjectRemap.OnNext(numberOfCards);
		}
    }
}
