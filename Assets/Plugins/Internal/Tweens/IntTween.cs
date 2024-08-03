using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;

namespace Internal.Tweens
{
    public class IntTween
    {
        private TweenerCore<int, int, NoOptions> _tween;
        private int _value;

        public int Value => _value;

        public IntTween(int value)
        {
            _value = value;
        }

        public void Play(int targetValue, float duration)
        {
            _tween?.Kill();
            _tween = DOTween.To(() => _value, value => _value = value, targetValue, duration);
        }
    }
}