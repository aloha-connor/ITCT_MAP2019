using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using DG.Tweening;

namespace ITCT 
{
	public class Floor : MonoBehaviour 
	{
		public int number;
		public List<Room> roomList;
		public MapSystem mapSystem;

		protected SpriteRenderer sprite;

		void Start()
		{
			sprite = GetComponentInChildren<SpriteRenderer>();
			mapSystem.currentFloor.AsObservable()
				.Subscribe(f => {
					ShowMap(number == f);
				});
		}

		protected void ShowMap(bool show)
		{
			float targetAlpha = show ? 1 : 0;
			Color targetColor = new Color(1,1,1,targetAlpha);
			sprite.DOColor(targetColor, .5f);
		}
	}
}
