using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Scarf.Moba.Timeline
{
    [Serializable]
    [TrackClipType(typeof (EventShot))]
    [TrackColor(0.7f, 0.7f, 0.00f)]
    public class EventTrack: TrackAsset
    {
        protected override void OnCreateClip(TimelineClip clip)
        {
            clip.duration = 0.05f;
        }

        protected override Playable CreatePlayable(PlayableGraph graph, GameObject gameObject, TimelineClip clip)
        {
            return base.CreatePlayable(graph, gameObject, clip);
        }
    }
}