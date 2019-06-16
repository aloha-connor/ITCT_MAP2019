using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;
using DG.Tweening;

namespace ITCT
{
    public class ScrollView : MonoBehaviour
    {
		public InfoSystem infoSystem;

		public RectTransform detail;

		public float gap = 5;

		protected float defaultHeight;

		protected ScrollRect scrollRect;
		public RectTransform content;

        // Use this for initialization
        void Start()
        {
			scrollRect = GetComponent<ScrollRect>();
			RectTransform t = GetComponent<RectTransform>();
			defaultHeight = t.sizeDelta.y;
			infoSystem.showDetail.AsObservable()
				.Subscribe(flag => {
					float targetY = flag ? defaultHeight/2 : defaultHeight;
					Vector2 targetSize = new Vector2(GetComponent<RectTransform>().sizeDelta.x, targetY);
					GetComponent<RectTransform>().DOSizeDelta(targetSize, .5f)
						.SetEase(Ease.InOutCubic);
				});
        }

		void Update()
		{
			scrollRect.verticalNormalizedPosition += 100 * (Input.mouseScrollDelta.y)/content.sizeDelta.y ;
		}
    }
}