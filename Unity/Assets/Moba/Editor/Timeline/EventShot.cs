using UnityEngine;
using UnityEngine.Playables;

namespace Scarf.Moba.Timeline
{
    internal sealed class EventShotPlayable: PlayableBehaviour
    {
        public string EventName;

        public override void OnBehaviourPlay(Playable playable, FrameData info)
        {
            if (string.IsNullOrEmpty(EventName) || string.IsNullOrWhiteSpace(this.EventName))
            {
                return;
            }
            
            // TODO 需要判断是那种模式.
            // TODO 使用UnityUpdate驱动发送事件.
        }
    }

    public sealed class EventShot: PlayableAsset
    {
        public string EventName;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<EventShotPlayable>.Create(graph);
            playable.GetBehaviour().EventName = this.EventName;
            return playable; 
        }
    }
}