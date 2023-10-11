using System.Collections.Generic;

namespace DamageSystem
{
    public class DamageLines
    {
        public (int Damage, bool IsCrit) this[int idx] {get => Lines[idx];}

        public List<(int Damage, bool IsCrit)> Lines;
        public int Sum => GetSum();
        public int Size => Lines.Count;

        public DamageLines ()
        {
            Lines = new List<(int Damage, bool IsCrit)>();
        }

        public void Add (int damage, bool isCrit)
        {
            Lines.Add((damage, isCrit));
        }

        public void Add ((int, bool) newLine)
        {
            Lines.Add(newLine);
        }

        public void RemoveAt (int idx)
        {
            Lines.RemoveAt(idx);
        }

        private int GetSum ()
        {
            int sum = 0;
            for (int i = 0; i < Lines.Count; i++)
                sum += Lines[i].Damage;

            return sum;
        }
    }
}