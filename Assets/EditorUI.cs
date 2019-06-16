using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;

namespace ITCT
{
    public class EditorUI : MonoBehaviour
    {
		public EditorSystem editorSystem;

		public Text modeText;
		public Text description;

        // Use this for initialization
        void Start()
        {
			editorSystem.currentMode.AsObservable()
				.Subscribe(mode => {
					if(mode == EditorSystem.EditMode.NONE) 
					{
						modeText.text = "None";
						description.text = "편집할 엔티티를 선택하세요.\n 또는 F3으로 컴퓨터, F4로 벽면 엔티티를 추가하세요.";
					}
					else if(mode == EditorSystem.EditMode.VALUES) 
					{
						modeText.text = "Text Mode";
						description.text = "우측 하단 윈도우에서 편집하세요.";
					}
					else if(mode == EditorSystem.EditMode.TRANSFORM) 
					{
						modeText.text = "Transform Mode";
						description.text = "키보드를 누른 상태로 마우스를 움직이세요\n A: 마우스 위치로 이동 \n S: 회전값 변경 \n D: 길이 변경";
					}
				});
        }
    }
}