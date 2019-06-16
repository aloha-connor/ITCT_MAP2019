using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UnityEngine.UI;

namespace ITCT
{
    public class InfoSystem : MonoBehaviour
    {
        public AssignmentSystem assignmentSystem;
        public ClassSystem classSystem;
        public MapSystem mapSystem;

        public Subject<ICollection<int>> SubjectQueriedAssignmentsChanged { get; protected set; }

        public ReactiveProperty<int> SelectedAssignmentID { get; protected set; }

		public ReactiveProperty<bool> listByName;

		public ReactiveProperty<bool> showDetail;

        public TagSelector tagSelector;

        public InputField searchInputField;

        void Awake()
        {
            SubjectQueriedAssignmentsChanged = new Subject<ICollection<int>>();
            SelectedAssignmentID = new ReactiveProperty<int>();
			listByName = new ReactiveProperty<bool>(false);
            showDetail = new ReactiveProperty<bool>(false);
        }

        void Start()
        {
            SelectedAssignmentID.AsObservable()
                .Subscribe(id => {
                    showDetail.Value = id >= 0 ;
                });
            InfoCardSelected(-1);

            tagSelector.tagMask.AsObservable()
                .Subscribe(mask => UpdateQueried(QueryDefault(mask)));

			listByName.AsObservable()
				.Subscribe(byName => {
					if(!byName) UpdateQueried(QueryDefault(tagSelector.tagMask.Value));
					else Search();
				});

            searchInputField.onEndEdit
                .AddListener((s) => Search());

            mapSystem.SubjectAssignmentEntityRendererSelected.AsObservable()
                .Subscribe(ent => {
                    AssignmentEntity entity = mapSystem.assignmentEntityDictionary[ent.aeID];
                    UpdateQueried(QueryByList(entity.assignmentIDList));
                });
        }

		public void ListAll()
		{
			listByName.Value = false;
		}

		public void ListName()
		{
			listByName.Value = true;
		}

        public void Search()
        {
            UpdateQueried(QueryByName(searchInputField.text));
        }

        public void InfoCardSelected(int id)
        {
            SelectedAssignmentID.Value = id;
        }

        protected void UpdateQueried(ICollection<int> idlist)
        {
            SubjectQueriedAssignmentsChanged.OnNext(idlist);
        }

        protected ICollection<int> QueryDefault(int tagMask = 0)
        {
            List<Assignment> queried = assignmentSystem.assignmentList;
            if (tagMask != 0)
            {
                queried = queried.Where(ass => (ass.assignmentTagMask & tagMask) != 0).ToList();
            }
            return ListQueried(queried);
        }

        protected ICollection<int> QueryByName(string _name)
        {
            List<Assignment> queriedByName =
                                     assignmentSystem.assignmentList
                                            .Where(a => a.teamMates.Contains(_name))
                                            .ToList();

            return ListQueried(queriedByName);
        }

        protected ICollection<int> QueryByList(List<int> list)
        {
            List<Assignment> queriedByList =
                                assignmentSystem.assignmentList
                                    .Where(a => list.Contains(a.id))
                                    .ToList();

            return ListQueried(queriedByList);
        }

        protected ICollection<int> ListQueried(ICollection<Assignment> queried)
        {
            IEnumerator<Assignment> orderedQueried
                    = queried.OrderBy(ass => ass.classID).GetEnumerator();

            List<int> result = new List<int>();

            int currentClass = -1;
            bool currentSelectedQueried = false;

            while (orderedQueried.MoveNext())
            {
                if (orderedQueried.Current.classID != currentClass)
                {
                    result.Add(orderedQueried.Current.classID + 1000);
                    currentClass = orderedQueried.Current.classID;
                }
                result.Add(orderedQueried.Current.id);
                if(SelectedAssignmentID.Value == orderedQueried.Current.id) currentSelectedQueried = true;
            }

            if(!currentSelectedQueried) InfoCardSelected(-1);

            return result;
        }
    }
}