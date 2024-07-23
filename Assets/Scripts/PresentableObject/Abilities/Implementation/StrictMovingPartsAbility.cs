using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace ProgramLab
{
    [Serializable]
    public class StrictMovingPartsAbility : AbilityBase
    {
        [SerializeField] private Transform[] parts;  
        [SerializeField] private Vector3 targetOffset;
        [SerializeField] private float delay = 0.2f;
        [SerializeField] private float speed = 1f;

        private Vector3[] initPositions;
        private Vector3[] targetPositions;
        private bool[] reverses;
        private float timeOfLastRotationStart;
        private int activeParts;

        public override void Init()
        {
            initPositions = new Vector3[parts.Length];
            reverses = new bool[parts.Length];
            targetPositions = new Vector3[parts.Length];
            
            for (var i = 0; i < parts.Length; i++)
            {
                var initPos = parts[i].position;
                initPositions[i] = initPos;
                targetPositions[i] = initPos + targetOffset;
            }
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
                    var reversed = reverses[i];
                    var target = reversed
                        ? initPositions[i]
                        : targetPositions[i];
                    
                    if (Vector3.Distance(part.position, target) < 0.01f)
                    {
                        reverses[i] = !reversed;
                    }
                    
                    part.position = Vector3.MoveTowards(part.position, target, Time.deltaTime * speed);
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