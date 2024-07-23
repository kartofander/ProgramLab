using UnityEngine;
using Zenject;

namespace ProgramLab
{
    public class ObjectsListPanel : MonoBehaviour, IObjectsListDisplay
    {
        [SerializeField] private GameObject buttonTemplate;
        private DiContainer _diContainer;
        
        [Inject]
        public void Construct(DiContainer diContainer)
        {
            _diContainer = diContainer;
        }
        
        public void AddObject(PresentableObject presentableObject)
        {
            var buttonObject = _diContainer.InstantiatePrefab(buttonTemplate, transform);
            var selectObjectButton = buttonObject.GetComponent<SelectObjectButton>();
            selectObjectButton.SetObject(presentableObject);
        }
    }
}