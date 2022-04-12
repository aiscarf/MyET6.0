using System;
using System.Collections.Generic;
using System.IO;
using Sirenix.Serialization;
using UnityEditor;
using UnityEngine;

namespace Scarf.Moba.Timeline
{
    [CustomEditor(typeof (EventTrack))]
    public class EventTrackEditor: Editor
    {
        private EventTrack m_eventTrack;

        private void OnEnable()
        {
            m_eventTrack = this.target as EventTrack;
        }

        public override void OnInspectorGUI()
        {
            this.serializedObject.Update();

            if (GUILayout.Button("导出Animation.json"))
            {
                GenerateAnimationData();
            }
        }

        private void GenerateAnimationData()
        {
            string outputPath = Application.dataPath + "/Bundles/Moba/Config/";
            if (!Directory.Exists(outputPath))
            {
                Directory.CreateDirectory(outputPath);
            }

            var timelineClips = this.m_eventTrack.GetClips();
            var animationData = new AnimationData();
            animationData.AnimationName = $"animation_{this.m_eventTrack.parent.name}";
            animationData.AnimationEvents = new List<AnimationEventData>();
            foreach (var clip in timelineClips)
            {
                var eventShot = clip.asset as EventShot;
                if (eventShot == null)
                    continue;
                if (string.IsNullOrEmpty(eventShot.EventName) || string.IsNullOrWhiteSpace(eventShot.EventName))
                    continue;
                animationData.AnimationEvents.Add(new AnimationEventData() { Name = eventShot.EventName, Time = (int)(clip.start * 1000) });
            }

            string configPath = Path.Combine(outputPath, $"animation_{this.m_eventTrack.parent.name}.json");
            using (var fs = new FileStream(configPath, FileMode.Create))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    var bytes = Sirenix.Serialization.SerializationUtility.SerializeValue(animationData, DataFormat.JSON);
                    sw.Write(bytes);
                }
            }

            AssetDatabase.Refresh();

            Debug.Log($"动画配置{configPath}生成成功!");
        }
    }
}