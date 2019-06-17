using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using DG.Tweening;

namespace ITCT
{
    public class SelectedMarker : MonoBehaviour
    {

		public MapSystem mapSystem;

		public float rotationSpd = 90f;

		protected float size;
		protected SpriteRenderer sprite;
		protected CompositeDisposable compDisp = new CompositeDisposable();

        // Use this for initialization
        void Start()
        {
			sprite = GetComponentInChildren<SpriteRenderer>();
			transform.position = new Vector2(5000,5000);
			mapSystem.SubjectAssignmentEntityRendererSelected.AsObservable()
				.Subscribe(r => {
					compDisp.Clear();
					sprite.color = r.queriedColor;
					r.selected.AsObservable()
						.Subscribe(flag => {
							transform.localScale = flag ? new Vector2(1,1) : Vector2.zero;
						}).AddTo(compDisp);
					transform.position = r.transform.position;
					mapSystem.currentFloor.AsObservable()
						.Delay(System.TimeSpan.FromSeconds(.5f))
						.Subscribe(f => {
							size = r.InCurrentFloor() ? r.GetComponentInChildren<SpriteRenderer>().bounds.size.x * 1.5f : 0; 
							DOTween.To(() => sprite.size, x => sprite.size = x, new Vector2(size,size), .5f);
						}).AddTo(compDisp);
				});
        }

        // Update is called once per frame
        void Update()
        {
			transform.Rotate(0,0,rotationSpd * Time.deltaTime);
        }
    }

}