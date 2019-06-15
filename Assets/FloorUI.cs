using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace ITCT
{
    public class FloorUI : MonoBehaviour
    {
		
		public MapSystem mapSystem;

		public Text currentFloor;
		public Button upstair;
		public Button downStair;

        // Use this for initialization
        void Start()
        {
			mapSystem.currentFloor.AsObservable()
				.Subscribe(f => {
					currentFloor.text = f.ToString();
					upstair.interactable = f != 3;
					downStair.interactable = f != 1;
				});
        }
    }
}