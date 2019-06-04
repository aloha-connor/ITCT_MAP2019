using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;
using UnityEngine.UI;

namespace ITCT
{
    public class InfoSystem : MonoBehaviour
    {
		public AssignmentSystem assignmentSystem;

		public Subject<ICollection<int>> SubjectQueriedAssignmentsChanged {get; protected set;}
		
		public Subject<int> SubjectSelectedInfoCardChanged {get; protected set;}

		public InputField searchInputField;

		void Awake()
		{
			SubjectQueriedAssignmentsChanged = new Subject<ICollection<int>>();
			SubjectSelectedInfoCardChanged = new Subject<int>();
		}

		void Start()
		{
			InfoCardSelected(-1);
		}

		public void Search()
		{
			UpdateQueried(QueryByName(searchInputField.text));
		}

		public void InfoCardSelected(int id)
		{
			SubjectSelectedInfoCardChanged.OnNext(id);
		}

		protected void UpdateQueried(ICollection<int> idlist)
		{
			SubjectQueriedAssignmentsChanged.OnNext(idlist);
		}

		protected ICollection<int> QueryByName(string _name)
		{
			return assignmentSystem.assignmentList
					.Where(a => a.teamMates.Contains(_name))
					.Select(a => a.id)
					.ToList() ;
		}
    }
}