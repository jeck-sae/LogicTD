using Sirenix.OdinInspector;
using UnityEngine;


namespace TowerDefense
{
    public class TDColors : Singleton<TDColors>
    {
        [ColorPalette] public Color affordableColor;
        [ColorPalette] public Color unaffordableColor;
        public static Color AffordableColor => TDColors.Instance.affordableColor;
        public static Color UnaffordableColor => TDColors.Instance.unaffordableColor;
    
    
    }
}
