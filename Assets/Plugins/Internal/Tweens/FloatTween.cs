using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;

namespace Internal.Tweens
{
    public class FloatTween
    {
        private TweenerCore<float, float, FloatOptions> _tween;
        private float _value;

        public float Value => _value;

        public FloatTween(float value)
        {
            _value = value;
        }

        public void Play(float targetValue, float duration)
        {
            _tween?.Kill();
            _tween = DOTween.To(() => _value, value => _value = value, targetValue, duration);
        }
    }
}