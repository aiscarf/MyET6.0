using DG.Tweening;
using DG.Tweening.Core;

namespace ET
{
    public static class DoTweenHelper
    {
        private static DOGetter<float> fGettter = () => 1f;
        private static DOSetter<float> fSetter = delegate(float value) {  };
        
        /// <summary>
        /// 创建延时器
        /// </summary>
        public static Tweener AddTimer(float duration, TweenCallback action)
        {
            var tween = DOTween.To(fGettter, fSetter, 0f, duration);
            tween.onComplete = action;
            return tween;
        }

        /// <summary>
        /// 创建定时器
        /// </summary>
        public static Tweener CreateIntervalTimer(float interval, TweenCallback action, int loopCount = -1)
        {
            var tween = DOTween.To(fGettter, fSetter, 0f, interval);
            tween.onStepComplete = action;
            tween.SetLoops(loopCount);
            return tween;
        }
    }
}