using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace ITCT
{
    public class TagSelector : MonoBehaviour
    {
		public List<Toggle> toggles;

		public ReactiveProperty<int> tagMask = new ReactiveProperty<int>();

        // Use this for initialization
        void Start()
        {
			foreach(Toggle t in toggles)
			{
				t.OnValueChangedAsObservable()
					.Subscribe(__ => UpdateTagMask());
			}
			UpdateTagMask();
        }

        // Update is called once per frame
        protected void UpdateTagMask()
        {
			int result = 0;
			for(int i = 0 ; i < toggles.Count ; i++)
			{
				if(toggles[i].isOn)
				{
					result |= 1 << i;
				}
			}
			tagMask.Value = result;
        }
    }
}