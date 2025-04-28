using System;


namespace TowerDefense
{
    [Serializable]
    public class SaveData 
    {
        public string lastSelectedLevel = "demo";
        public bool fullscreen = true;
        public float volume = .8f;
        public string statisticsFileId;
        public Achievements achievements;
        
        [Serializable]
        public class Achievements
        {
            public bool kills;
            public bool damage;
            public bool placed;
        }
    }
    
}
