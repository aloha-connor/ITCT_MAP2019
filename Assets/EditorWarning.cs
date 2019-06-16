using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;
using DG.Tweening;

namespace ITCT
{
    public class EditorWarning : MonoBehaviour
    {
		public EditorSystem editorSystem;

		public Text message;

        // Use this for initialization
        void Start()
        {
			editorSystem.SubjectWarningMessage.AsObservable()
				.Subscribe(s => {
					transform.localScale = new Vector2(1,1);
					message.text = s;
					Observable.Timer(System.TimeSpan.FromSeconds(2f))
						.Subscribe(__ => transform.DOScale(Vector3.zero, .5f))
						.AddTo(this);
				});
        }
    }
}