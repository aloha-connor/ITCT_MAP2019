using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using DG.Tweening;

namespace ITCT
{
    public class Detail : MonoBehaviour
    {
		public InfoSystem infoSystem;

		public Text className;
		public Text title;
		public Text teammates;
		public Text comment;
		public Text conceptComment;

        // Use this for initialization
        void Start()
        {
			infoSystem.showDetail.AsObservable()
				.Subscribe(flag => {
					Vector2 targetSize = flag ? new Vector2(1,1) : new Vector2(1,0);
					transform.DOScale(targetSize, .4f)
						.SetEase(Ease.InOutCubic);
				});

			infoSystem.SelectedAssignmentID.AsObservable()
				.Where(id => id >= 0)
				.Subscribe(id => {
					Assignment ass = infoSystem.assignmentSystem.assignmentList.Find(a => a.id == id);
					className.text = infoSystem.classSystem.classList.Find(c => c.id == ass.classID).title ;
					title.text = ass.title;
					string tm = "";
					foreach(string s in ass.teamMates) tm += s + " " ;
					tm = tm.Remove(tm.Length - 1);
					teammates.text = tm;
					comment.text = ass.comment;
					conceptComment.text = ass.conceptComment;
				});
        }
    }

}