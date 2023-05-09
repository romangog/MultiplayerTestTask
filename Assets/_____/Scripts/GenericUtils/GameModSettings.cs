using System;
using Sirenix.OdinInspector;

[Serializable]
public class GameModSettings
{
    public AllowedInt StartMoney;

    public AllowedInt StartLevel;

    public bool ClearSaves;

    public bool Screencast;

    [Serializable]
    public class AllowedInt
    {
        [HorizontalGroup]
        [HideLabel]
        public bool Allowed;
        [HorizontalGroup]
        [HideLabel]
        [EnableIf("Allowed")]
        public int Num;
    }
}
