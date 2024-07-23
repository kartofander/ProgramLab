using System;
using System.Collections.Generic;
using DG.Tweening;
using ProgramLab.Models;
using UnityEngine;

namespace ProgramLab
{
    public class PresentableObject : MonoBehaviour
    {
        [SerializeField] private ObjectInfo info;
        [SerializeField] private GameObject[] hideableParts;
        [SerializeField] private ObjectSeparation[] separableParts;
        [SerializeReference] private AbilityBase[] abilities;

        private Coroutine[] _abilitiesRoutines;
        private Sequence _separationSequence;
        private List<Tuple<Transform, TransformDescription>> _childrenInitialTransforms;

        public bool cutActive { get; private set; }
        public bool separationActive { get; private set; }
        public bool abilitiesActive { get; private set; }

        private void Start()
        {
            _abilitiesRoutines = new Coroutine[abilities.Length];

            _separationSequence = DOTween.Sequence();
            _separationSequence.Pause();
            _separationSequence.SetAutoKill(false);
            foreach (var part in separableParts)
            {
                _separationSequence.Join(part.transform.DOMove(part.targetPosition, 1));
                _separationSequence.Join(part.transform.DORotate(part.targetRotation, 1));
            }

            foreach (var ability in abilities)
            {
                ability.Init();
            }

            SetInitialTransforms();
        }

        private void SetInitialTransforms()
        {
            _childrenInitialTransforms = new List<Tuple<Transform, TransformDescription>>();
            var queue = new Queue<Transform>();
            for (int j = 0; j < transform.childCount; j++)
            {
                queue.Enqueue(transform.GetChild(j));
            }

            while (queue.Count > 0)
            {
                var childTransform = queue.Dequeue();
                var tDescription = new TransformDescription()
                {
                    position = childTransform.position,
                    rotation = childTransform.rotation.eulerAngles,
                    scale = childTransform.localScale,
                };
                _childrenInitialTransforms.Add(new Tuple<Transform, TransformDescription>(childTransform, tDescription));
                
                for (int j = 0; j < childTransform.childCount; j++)
                {
                    queue.Enqueue(childTransform.GetChild(j));
                }
            }
        } 

        public ObjectInfo GetInfo()
        {
            return info;
        }
        
        public void Reset()
        {
            foreach (var (tr, inf) in _childrenInitialTransforms)
            {
                tr.position = inf.position;
                tr.rotation = Quaternion.Euler(inf.rotation);
                tr.localScale = inf.scale;
            }

            separationActive = false;
            _separationSequence.isBackwards = false;
            _separationSequence.Rewind();
            
            abilitiesActive = false;
            StopAbilities();
            foreach (var ability in abilities)
            {
                ability.Reset();
            }
        }

        public void ToggleCut()
        {
            cutActive = !cutActive;

            foreach (var part in hideableParts)
            {
                part.SetActive(!cutActive);
            }
        }

        public void ToggleSeparation()
        {
            if (!separationActive)
            {
                Reset();
            }
            
            separationActive = !separationActive;

            _separationSequence.isBackwards = !separationActive;
            _separationSequence.Play();
        }

        public void ToggleAbilityPerformance()
        {
            if (!abilitiesActive)
            {
                Reset();
            }

            abilitiesActive = !abilitiesActive;
            
            if (abilitiesActive)
            {
                ActivateAbilities();
            }
            else
            {
                StopAbilities();
            }
        }

        private void ActivateAbilities()
        {
            for (var i = 0; i < abilities.Length; i++)
            {
                var ability = abilities[i];
                _abilitiesRoutines[i] = StartCoroutine(ability.Perform());
            }
        }
        
        private void StopAbilities()
        {
            foreach (var abilityRoutine in _abilitiesRoutines)
            {
                if (abilityRoutine != null)
                {
                    StopCoroutine(abilityRoutine);
                }
            }

            foreach (var ability in abilities)
            {
                ability.OnStop();
            }
        }
    }
}