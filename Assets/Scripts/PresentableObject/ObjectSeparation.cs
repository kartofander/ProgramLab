using System;
using UnityEngine;

namespace ProgramLab
{
    [Serializable]
    public class ObjectSeparation
    {
        public Transform transform;
        public Vector3 targetPosition;
        public Vector3 targetRotation;
    }
}