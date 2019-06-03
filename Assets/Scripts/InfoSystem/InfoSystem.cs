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

		public Subject<IEnumerable<int>> SubjectQueriedAssignmentsChanged {get; protected set;}

		public InputField searchInputField;

		void Awake()
		{
			SubjectQueriedAssignmentsChanged = new Subject<IEnumerable<int>>();
		}

		public void Search()
		{
			UpdateHighlighted(QueryByName(searchInputField.text));
		}

		protected void UpdateHighlighted(IEnumerable<int> idlist)
		{
			SubjectQueriedAssignmentsChanged.OnNext(idlist);
		}

		protected IEnumerable<int> QueryByName(string _name)
		{
			return assignmentSystem.assignmentList
					.Where(a => a.teamMates.Contains(_name))
					.Select(a => a.id) ;
		}
    }
}