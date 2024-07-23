using System;
using DG.Tweening;
using UnityEngine;

namespace ProgramLab
{
    public class CollapsePanel : MonoBehaviour
    {
        [SerializeField] private Vector2 collapseDirection;
        
        private RectTransform _rectTransform;
        private bool collapsed = true;
        private Vector2 collapsedPosition;
        private Vector2 uncollapsedPosition;
        private Tweener _tweener;

        private void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
            var rect = _rectTransform.rect;
            var collapseVector = new Vector2(
                collapseDirection.x * rect.width,
                collapseDirection.y * rect.height);
            uncollapsedPosition = _rectTransform.anchoredPosition;
            collapsedPosition = uncollapsedPosition + collapseVector;

            _rectTransform.anchoredPosition = collapsedPosition;
        }

        public void Toggle()
        {
            collapsed = !collapsed;
            SetState(collapsed);
        }

        private void SetState(bool value)
        {
            if (_tweener != null && _tweener.IsActive())
            {
                _tweener.Kill();
            }

            var position = value ? collapsedPosition : uncollapsedPosition;
            _tweener = _rectTransform.DOAnchorPos(position, 1);
        }
    }
}