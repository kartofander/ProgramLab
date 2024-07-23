using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ProgramLab
{
    public class ToggleButton : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private Sprite activateImage;
        [SerializeField] private Sprite disableImage;
        [SerializeField] private UnityEvent eventHandler;
        
        private Button _button;
        private bool _showActivateImage = true;

        private void Start()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(Toggle);
        }

        private void Toggle()
        {
            eventHandler.Invoke();
        }

        public void SetState(bool value)
        {
            _showActivateImage = !value;
            image.sprite = _showActivateImage ? activateImage : disableImage;
        }
    }
}