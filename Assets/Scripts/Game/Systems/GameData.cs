using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Systems
{
    [CreateAssetMenu(fileName = "Game Data", menuName = "Game/Game Data")]
    public class GameData : ScriptableObject
    {
        //A Tri Grade with the sanity meter pulling from the pond
        //Pond grade changes daily
        //Sanity meter depending on action
        private int PondGrade;
        private int SanityMeter; //Only goes positive
    
        public delegate void OnValueChange (int amount);

        public static OnValueChange PondGradeChange;
        public static OnValueChange SanityMeterChange;
        
        public void AddSanity(int amount)
        {
            if (amount < 0)
            {
                Debug.LogError("Can not subtract sanity from meter");
                return;
            }

            if (amount == 0)
            {
                Debug.LogWarning("Amount set as 0, does not do anything");
                return;
            }


            SanityMeter += amount;
            SanityMeterChange?.Invoke(SanityMeter);
        }

        public void SetPondGrade(int amount)
        {

            PondGrade = amount;
            PondGradeChange?.Invoke(PondGrade);
        }


    }
}