using TMPro;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ProgramLab
{
    [RequireComponent(typeof(ButtonEditor))]
    public class SelectObjectButton : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI titleText;
        private int _objectId;
        private IPresenter _presenter;

        [Inject]
        public void Construct(IPresenter presenter)
        {
            _presenter = presenter;
        }
        
        private void Start()
        {
            var button = GetComponent<Button>();
            button.onClick.AddListener(HandleClick);
        }

        public void SetObject(PresentableObject obj)
        {
            var info = obj.GetInfo();
            image.sprite = info.sprite;
            titleText.text = info.title;
            _objectId = info.id;
        }

        private void HandleClick()
        {
            _presenter.SelectObject(_objectId);
        }
    }
}