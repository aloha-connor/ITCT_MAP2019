using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ITCT
{
    [System.Serializable]
    public class Assignment
    {
        public int id;
        public int floor;
        public int room;
        public int classID;
        public int assignmentEntityID;
        public Vector2 pos;
        public string title;
        public List<string> teamMates;
        public string comment;
        public string conceptComment;
        public int assignmentTagMask;
    }

    public enum AssignmentTag
    {
        poster = 0, video = 1, prototype = 2, game = 3, web = 4, installation = 5
    }
}