using UnityEngine;

namespace ProgramLab
{
    public class ToolsButtonsAccessor : MonoBehaviour, IToolsButtonsAccessor
    {
        [SerializeField] private ToggleButton cutButton; 
        [SerializeField] private ToggleButton separationButton; 
        [SerializeField] private ToggleButton abilitiesButton;

        public void SetCutButtonState(bool cut)
        {
            cutButton.SetState(cut);
        }

        public void SetSeparationButtonState(bool separated)
        {
            separationButton.SetState(separated);
        }

        public void SetAbilitiesButtonState(bool active)
        {
            abilitiesButton.SetState(active);
        }
    }
}