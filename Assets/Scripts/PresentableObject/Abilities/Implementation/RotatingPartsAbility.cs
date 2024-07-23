using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace ProgramLab
{
    [Serializable]
    public class RotatingPartsAbility : AbilityBase
    {
        [SerializeField] private Transform[] parts;  
        [SerializeField] private Vector3 rotation;
        [SerializeField] private float delay = 0.2f;

        private float timeOfLastRotationStart;
        private int activeParts;

        public override void Init()
        {
        }
        
        public virtual void Reset()
        {
            activeParts = 0;
        }

        public override IEnumerator Perform()
        {
            timeOfLastRotationStart = Time.time;
            while (true)
            {
                for (var i = 0; i < activeParts; i++)
                {
                    var part = parts[i];
                    part.Rotate(rotation, 0.1f);
                }

                if (activeParts < parts.Length && Time.time - timeOfLastRotationStart >= delay)
                {
                    timeOfLastRotationStart = Time.time;
                    activeParts++;
                }
                yield return null;
            }
        }
    }
}