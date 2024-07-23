using System;
using UnityEngine;

namespace ProgramLab
{
    [Serializable]
    public class ObjectInfo
    {
        public int id;
        public Sprite sprite;
        public string title;
        [TextArea] public string description;
    }
}