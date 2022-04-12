using System.Collections.Generic;
using Scarf.ANode.Flow.Runtime;

namespace Scarf.Moba
{
    public class BattleData
    {
        public EBattleGameMode BattleGameMode { get; set; }

        public List<PlayerData> AllPlayerDatas { get; set; }
        public int RandomSeed { get; set; }

        // DONE: 所有动画数据. {HeroId, List<AnimationDatas>}
        public Dictionary<int, List<AnimationData>> AllAnimationDatas { get; set; }

        // DONE: 所有技能图数据. {SkillId, SkillGraph}
        public Dictionary<int, FlowNodeGraph> AllSkillGraphs { get; set; }

        // DONE: 所有Buff数据.
        public Dictionary<int, BuffData> AllBuffDatas { get; set; }
    }

    public class PlayerData
    {
        public int Uid { get; set; }

        public string Nickname { get; set; }

        public int HeroId { get; set; }

        public int HeroSkinId { get; set; }

        public int HeroLevel { get; set; }
        public int Score { get; set; }
        public int ChairId { get; set; }
        public int Camp { get; set; }
        public int HeadId { get; set; }
        public int FrameId { get; set; }
        public int AILevel { get; set; }
    }

    public static class BattleDataHelper
    {
        public static List<AnimationData> GetAnimationDatas(this BattleData self, int heroId)
        {
            if (self.AllAnimationDatas == null)
            {
                return new List<AnimationData>();
            }

            if (!self.AllAnimationDatas.TryGetValue(heroId, out var result))
            {
                return new List<AnimationData>();
            }

            return result;
        }

        public static FlowNodeGraph GetFlowNodeGraph(this BattleData self, int skillId)
        {
            if (self.AllSkillGraphs == null)
            {
                return null;
            }

            if (!self.AllSkillGraphs.TryGetValue(skillId, out var result))
            {
                return null;
            }

            return result;
        }

        public static BuffData GetBuffData(this BattleData self, int buffId)
        {
            if (self.AllBuffDatas == null)
            {
                return null;
            }

            if (!self.AllBuffDatas.TryGetValue(buffId, out var result))
            {
                return null;
            }

            return result;
        }
    }
}