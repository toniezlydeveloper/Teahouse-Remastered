using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace Internal.Tweens
{
    public class ColorTween
    {
        private TweenerCore<Color, Color, ColorOptions> _tween;
        private Color _value;

        public Color Value => _value;

        public ColorTween(Color value)
        {
            _value = value;
        }

        public void Play(Color targetValue, float duration)
        {
            _tween?.Kill();
            _tween = DOTween.To(() => _value, value => _value = value, targetValue, duration);
        }
    }
}