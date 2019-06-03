using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using DG.Tweening;

namespace ITCT
{
    public class InfoCard : MonoBehaviour
    {
        public int id ;
		public int listOrder;

		public static float gap = 9.5f;

		protected Vector2 defaultSize ;

		public void InitializeInfoCard(int _id, int _listOrder, Transform parent, InfoListViewer viewer)
		{
			transform.SetParent(parent) ;
			id = _id;
			listOrder = _listOrder;
			defaultSize = GetComponent<RectTransform>().sizeDelta;

			viewer.SubjectRemap.AsObservable()
				.Subscribe(__ => Remap());
		}

		protected void Remap()
		{
			RectTransform t = GetComponent<RectTransform>();
			if(listOrder < 0)
			{
				Vector2 targetSize = Vector2.zero;
				t.DOSizeDelta(targetSize,.5f)
					.SetEase(Ease.InOutCubic);
			}
			else
			{
				t.DOSizeDelta(defaultSize,.5f)
					.SetEase(Ease.InOutCubic);
				Vector2 targetPos = new Vector2(gap + defaultSize.x/2, -((gap + defaultSize.y) * listOrder + gap + defaultSize.y/2));
				t.DOAnchorPos(targetPos,.5f)
					.SetEase(Ease.InOutCubic);
			}
		}
    }
}