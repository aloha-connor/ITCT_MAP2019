using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UnityEngine.EventSystems;
using DG.Tweening;

namespace ITCT
{
    public class AssignmentInfoCard : MonoBehaviour
    {
        protected InfoCard infoCard;

        public Text title;
        public Text teammates;

        public EventTrigger myButtonTrigger;

        public RectTransform tags;

        public GameObject posterTag, videoTag, protoTag, installTag, gameTag, webTag;

        protected static float gap = 5f;

        void Awake()
        {
            infoCard = GetComponent<InfoCard>();

            infoCard.id.AsObservable()
                .Where(i => i >= 0)
                .Subscribe(i => UpdateCardContents(i));
        }

        void Start()
        {
            EventTrigger.Entry pointerEnter = new EventTrigger.Entry();
            pointerEnter.eventID = EventTriggerType.PointerEnter;
            pointerEnter.callback.AddListener((eventData) => { transform.DOScale(new Vector3(1.02f,1.02f,1.02f), .2f).SetEase(Ease.InOutCubic);});

            EventTrigger.Entry pointerExit = new EventTrigger.Entry();
            pointerExit.eventID = EventTriggerType.PointerExit;
            pointerExit.callback.AddListener((eventData) => { transform.DOScale(new Vector3(1f,1f,1f), .2f).SetEase(Ease.InOutCubic);});

            myButtonTrigger.triggers.Add(pointerEnter);
            myButtonTrigger.triggers.Add(pointerExit);

			GetComponent<Button>().onClick
				.AddListener(() => infoCard.myViewer.infoSystem.InfoCardSelected(infoCard.id.Value));
        }

        protected void UpdateCardContents(int id)
        {
            Assignment myAss = infoCard.myViewer.assignmentSystem.assignmentList.Find(ass => ass.id == id);
            title.text = myAss.title;
            string _teammates = "";
            foreach (string n in myAss.teamMates)
            {
                _teammates += n + " ";
            }
            teammates.text = _teammates;
            AddTags(myAss.assignmentTagMask);
        }

        protected void AddTags(int tagMask)
        {
            float offset = 0;
            List<GameObject> tagList = new List<GameObject>();
            if (((1 << (int)AssignmentTag.poster) & tagMask) > 0) tagList.Add(GameObject.Instantiate(posterTag, tags));
            if (((1 << (int)AssignmentTag.game) & tagMask) > 0) tagList.Add(GameObject.Instantiate(gameTag, tags));
            if (((1 << (int)AssignmentTag.prototype) & tagMask) > 0) tagList.Add(GameObject.Instantiate(protoTag, tags));
            if (((1 << (int)AssignmentTag.video) & tagMask) > 0) tagList.Add(GameObject.Instantiate(videoTag, tags));
            if (((1 << (int)AssignmentTag.web) & tagMask) > 0) tagList.Add(GameObject.Instantiate(webTag, tags));
            if (((1 << (int)AssignmentTag.installation) & tagMask) > 0) tagList.Add(GameObject.Instantiate(installTag, tags));
            for (int i = 0; i < tagList.Count; i++)
            {
                Debug.Log(offset);
                tagList[i].transform.localPosition = new Vector2(offset, 0);
                offset += gap + ((RectTransform)tagList[i].transform).sizeDelta.x;
            }
        }
    }

}