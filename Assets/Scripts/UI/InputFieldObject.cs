using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using DG.Tweening;

namespace ITCT
{
    public class InputFieldObject : MonoBehaviour
    {
		public InfoSystem infoSystem;

		void Start()
		{
			infoSystem.listByName.AsObservable()
				.Subscribe(flag => {
					Vector2 targetScale = flag ?  new Vector2(1,1) : Vector2.zero ;
					transform.DOScale(targetScale, .5f)
						.SetEase(Ease.InOutCubic);
				});
		}
    }

}
