using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ITCT
{
    public enum AEType
    {
        computer, wall
    }

    public class AssignmentEntity
    {
        public int aeID;
        public AEType aeType;
        public int floor;

        public Vector2 pos;
        public float radius;
        public float theta;

        public List<int> assignmentIDList = new List<int>();
    }
}
