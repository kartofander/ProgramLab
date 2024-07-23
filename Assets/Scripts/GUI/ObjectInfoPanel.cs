using TMPro;
using UnityEngine;

namespace ProgramLab
{
    public class ObjectInfoPanel : MonoBehaviour, IObjectInfoDisplay
    {
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        
        public void SetInfo(ObjectInfo info)
        {
            titleText.text = info.title;
            descriptionText.text = info.description;
        }
    }
}