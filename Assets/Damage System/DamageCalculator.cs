using System.Collections.Generic;
using UnityEngine;

using CharacterSystem;
using MonsterSystem;
using SkillSystem;

namespace DamageSystem
{
    public static class DamageCalculator
    {
        public static DamageLines CharacterCalculate (Character character, Monster monster, Skill skill)
        {
            float skillMultiplier = 1 + ((skill.Base.BasePctDMG) + (skill.Level * skill.Base.PerLvlPctDMG)) / 100;
            
            var damageLines = new DamageLines();
            for (int i = 0; i < skill.Base.NumHits; i++)
            {
                (int Damage, bool IsCrit) newLine = character.RollDamage(monster.IsBoss, monster.PDR);
                newLine.Damage = (int) (newLine.Damage * skillMultiplier);

                damageLines.Add(newLine);
            }

            return damageLines;
        }

        public static DamageLines MonsterCalculate (Monster monster, Character character, Skill skill)
        {
            float skillMultiplier = 1 + ((skill.Base.BasePctDMG) + (skill.Level * skill.Base.PerLvlPctDMG)) / 100;

            var damageLines = new DamageLines();
            for (int i = 0; i < skill.Base.NumHits; i++)
            {
                (int Damage, bool IsCrit) newLine = (monster.RollDamage(), false);
                newLine.Damage = (int) (newLine.Damage * skillMultiplier);

                damageLines.Add(newLine);
            }

            return damageLines;
        }
    }
}