using UnityEngine;
using UnityEngine.U2D;

namespace ET
{
    public static class AtlasHelper
    {
        public const string ATLAS_DUNGEON = "Atlas_Dungeon";
        public const string ATLAS_EQUIP = "Atlas_Equip";
        public const string ATLAS_HEAD = "Atlas_Head";
        public const string ATLAS_PROP = "Atlas_Prop";

        /// <summary>
        /// 异步加载图集.
        /// </summary>
        public static async ETTask LoadAtlasAsync(string atlasName)
        {
            await ResourcesComponent.Instance.LoadBundleAsync(atlasName.ToLower().StringToAB());
        }

        /// <summary>
        /// 同步卸载图集
        /// </summary>
        public static void UnLoadAtlas(string atlasName)
        {
            ResourcesComponent.Instance.UnloadBundle(atlasName.ToLower().StringToAB());
        }

        /// <summary>
        /// 同步加载精灵. (不保证一定能加载到)
        /// 使用此Api前需要确认图集是否已经加载.
        /// </summary>
        public static Sprite LoadSprite(string atlasName, string spriteName)
        {
            var atlas = (SpriteAtlas)ResourcesComponent.Instance.GetAsset(atlasName.ToLower().StringToAB(), atlasName);
            return atlas == null ? null : atlas.GetSprite(spriteName);
        }
    }
}