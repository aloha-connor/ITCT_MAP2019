using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using DG.Tweening;

namespace ITCT
{
    public class InfoCard : MonoBehaviour
    {
        public ReactiveProperty<int> id ;
		public int listOrder;
		public int classOrder;
		public bool isClass;

		public static float gap = 9.5f;

		public InfoListViewer myViewer;

		public static Vector2 defaultSize = new Vector2(403.4f,135f);
		public static Vector2 classSize = new Vector2(403.4f,55f);

		void Awake()
		{
			id = new ReactiveProperty<int>(-1);
		}

		public void InitializeInfoCard(int _id, int _listOrder, Transform parent, InfoListViewer _viewer)
		{
			transform.SetParent(parent) ;
			GetComponent<RectTransform>().anchoredPosition = 
				new Vector2(gap + defaultSize.x/2, -gap + defaultSize.y);
			GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
			listOrder = _listOrder;
			myViewer = _viewer;
			id.Value = _id;

			myViewer.SubjectRemap.AsObservable()
				.Subscribe(__ => Remap());
		}

		protected void Remap()
		{
			RectTransform t = GetComponent<RectTransform>();
			if(listOrder < 0)
			{
				Vector2 targetSize = Vector2.zero;
				t.DOScale(targetSize,.5f)
					.SetEase(Ease.InOutCubic);
			}
			else
			{
				Vector2 targetSize = new Vector2(1,1);//isClass ? classSize : defaultSize;
				t.DOScale(targetSize,.5f)
					.SetEase(Ease.InOutCubic);

				Vector2 targetPos = new Vector2(gap + defaultSize.x/2, 
					-((gap/2 + defaultSize.y) * listOrder + gap/2 + defaultSize.y/2));
				targetPos = targetPos + new Vector2(0, classSize.y * (classOrder+1));
				if(isClass) targetPos = targetPos - new Vector2(0, classSize.y/1.6f);
				t.DOAnchorPos(targetPos,.5f)
					.SetEase(Ease.InOutCubic);
			}
		}
    }
}