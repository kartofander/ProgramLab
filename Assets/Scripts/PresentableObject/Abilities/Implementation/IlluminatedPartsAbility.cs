using System;
using System.Collections;
using UnityEngine;

namespace ProgramLab
{
    [Serializable]
    public class IlluminatedPartsAbility : AbilityBase
    {
        [SerializeField] private Renderer[] parts;  
        [SerializeField] private Color[] colors;
        private Color[] initColors;

        private const float EmissionIntensity = 1.2f;

        public override void Init()
        {
            initColors = new Color[parts.Length];
            for (var i = 0; i < parts.Length; i++)
            {
                initColors[i] = parts[i].material.GetColor("_BaseColor");
            }
        }

        public override IEnumerator Perform()
        {
            for (var i = 0; i < parts.Length; i++)
            {
                var part = parts[i];
                var color = colors[i];

                var factor = Mathf.Pow(2,EmissionIntensity);
                color = new Color(color.r * factor,color.g * factor,color.b * factor);
            
                part.material.EnableKeyword("_EMISSION");
                part.material.SetColor("_BaseColor", color);
                part.material.SetColor("_EmissionColor", color);
            }

            yield return null;
        }

        public override void OnStop()
        {
            for (var i = 0; i < parts.Length; i++)
            {
                var part = parts[i];
                var color = initColors[i];
                part.material.EnableKeyword("_EMISSION");
                part.material.SetColor("_BaseColor", color);
                part.material.SetColor("_EmissionColor", color);
            }
        }
    }
}