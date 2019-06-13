using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace ITCT
{
    public class ClassInfoCard : MonoBehaviour
    {
		protected InfoCard infoCard;
		public Text title;

		void Awake()
		{
			infoCard = GetComponent<InfoCard>();

			infoCard.id.AsObservable()
				.Where(i => i >= 0)
				.Subscribe(i => UpdateCardContents(i - 1000));
		}

		protected void UpdateCardContents(int id)
		{
			ClassObject myClass = infoCard.myViewer.classSystem.classList.Find(cl => cl.id == id);
			title.text = myClass.title ;
		}
    }
}