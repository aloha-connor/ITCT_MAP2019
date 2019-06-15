using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

namespace ITCT
{
    public class AssignmentEntityTrigger : MonoBehaviour
    {
		protected EventTrigger eventTrigger;

        protected AssignmentEntityRenderer myRenderer;

        // Use this for initialization
        void Start()
        {
            myRenderer = GetComponent<AssignmentEntityRenderer>();
			eventTrigger = GetComponent<EventTrigger>();
			
            EventTrigger.Entry pointerEnter = new EventTrigger.Entry();
            pointerEnter.eventID = EventTriggerType.PointerEnter;
            pointerEnter.callback.AddListener((eventData) => { 
                if(myRenderer.InCurrentFloor())
				    transform.DOScale(new Vector3(1.1f,1.1f,1.1f), .35f).SetEase(Ease.OutCubic);
			});

            EventTrigger.Entry pointerExit = new EventTrigger.Entry();
            pointerExit.eventID = EventTriggerType.PointerExit;
            pointerExit.callback.AddListener((eventData) => { 
                if(myRenderer.InCurrentFloor())
				    transform.DOScale(new Vector3(1f,1f,1f), .35f).SetEase(Ease.OutCubic);
			});
            
            EventTrigger.Entry pointerDown = new EventTrigger.Entry();
            pointerDown.eventID = EventTriggerType.PointerDown;
            pointerDown.callback.AddListener((eventData) => { 
                GetComponent<AssignmentEntityRenderer>().mapSystem
                    .SelectAssignmentEntityRenderer(GetComponent<AssignmentEntityRenderer>());
			});

            eventTrigger.triggers.Add(pointerEnter);
            eventTrigger.triggers.Add(pointerExit);
            eventTrigger.triggers.Add(pointerDown);
        }
    }
}