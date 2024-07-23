using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace ProgramLab
{
    public class Presenter : MonoBehaviour, IPresenter
    {
        [SerializeField] private Transform pedestal;

        private readonly Dictionary<int, GameObject> _prefabsDictionary = new();
        private readonly Dictionary<int, GameObject> _instancesDictionary = new();
        
        private IObjectInfoDisplay _objectInfoDisplay;
        private IObjectsListDisplay _objectsListDisplay;
        private IToolsButtonsAccessor _toolsButtonsAccessor;
        
        private PresentableObject _currentObject;

        [Inject]
        public void Construct(IObjectInfoDisplay objectInfoDisplay, 
            IToolsButtonsAccessor toolsButtonsAccessor,
            IObjectsListDisplay objectsListDisplay)
        {
            _objectInfoDisplay = objectInfoDisplay;
            _objectsListDisplay = objectsListDisplay;
            _toolsButtonsAccessor = toolsButtonsAccessor;
        }
        
        private void Start()
        {
            var allPresentableObjects = Resources.LoadAll<GameObject>("PresentedObjects");
            foreach (var obj in allPresentableObjects)
            {
                var presentableObj = obj.GetComponent<PresentableObject>();
                var id = presentableObj.GetInfo().id;
                _prefabsDictionary.Add(id, obj);
                _objectsListDisplay.AddObject(presentableObj);
            }

            var first = _prefabsDictionary.First().Key;
            SelectObject(first);
            UpdateToolsButtonsState();
        }

        public void ToggleHide()
        {
            _currentObject.ToggleCut();
            UpdateToolsButtonsState();
        }
        
        public void ToggleSeparation()
        {
            _currentObject.ToggleSeparation();
            UpdateToolsButtonsState();
        }
        
        public void ToggleAbilities()
        {
            _currentObject.ToggleAbilityPerformance();
            UpdateToolsButtonsState();
        }

        private void UpdateToolsButtonsState()
        {
            _toolsButtonsAccessor.SetCutButtonState(_currentObject.cutActive);
            _toolsButtonsAccessor.SetSeparationButtonState(_currentObject.separationActive);
            _toolsButtonsAccessor.SetAbilitiesButtonState(_currentObject.abilitiesActive);
        }

        public void SelectObject(int objectId)
        {
            if (_currentObject)
            {
                _currentObject.gameObject.SetActive(false);
            }
            
            if (!_instancesDictionary.ContainsKey(objectId))
            {
                var newInstance = Instantiate(_prefabsDictionary[objectId], pedestal);
                _instancesDictionary.Add(objectId, newInstance);
            }

            _currentObject = _instancesDictionary[objectId].GetComponent<PresentableObject>();
            _currentObject.gameObject.SetActive(true);
            _objectInfoDisplay.SetInfo(_currentObject.GetInfo());
        }
    }
}