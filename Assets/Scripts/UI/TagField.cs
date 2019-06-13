using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using DG.Tweening;

namespace ITCT
{
    public class TagField : MonoBehaviour
    {
		public InfoSystem infoSystem;

		void Start()
		{
			infoSystem.listByName.AsObservable()
				.Subscribe(flag => {
					Vector2 targetScale = flag ? Vector2.zero : new Vector2(1,1) ;
					transform.DOScale(targetScale, .5f)
						.SetEase(Ease.InOutCubic);
				});
		}
    }

}
