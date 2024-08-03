using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace Internal.Tweens
{
    public class MaterialTween
    {
        private TweenerCore<float, float, FloatOptions> _tween;
        private Material[] _materials;
        private int _property;
        private float _value;

        public MaterialTween(int value, string shaderProperty, IEnumerable<Renderer> renderers)
        {
            _materials = renderers.SelectMany(renderer => renderer.materials).ToArray();
            _property = Shader.PropertyToID(shaderProperty);
            _value = value;
        }

        public void Play(float targetValue, float duration)
        {
            _tween?.Kill();
            _tween = DOTween.To(() => _value, SetProperty, targetValue, duration);
        }

        private void SetProperty(float value)
        {
            _value = value;

            foreach (Material material in _materials)
            {
                material.SetFloat(_property, _value);
            }
        }
    }
}