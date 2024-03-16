
namespace Otome.Core
{
    [System.Serializable]
    public class GameData
    {
        #region Game Data

            public SerializbleDictionary<string, bool> playedLevels;
            public SerializbleDictionary<string, bool> passedLevels;

        #endregion
        
        #region Heart Data

            public SerializbleDictionary<string, int> ownedHearts;

        #endregion
        
        public GameData()
        {
            playedLevels = new SerializbleDictionary<string, bool>();
            playedLevels.Add(GameManager.SceneList.Level01.ToString(), false);
            playedLevels.Add(GameManager.SceneList.Level02.ToString(), false);
            passedLevels = new SerializbleDictionary<string, bool>();
            passedLevels.Add(GameManager.SceneList.Level01.ToString(), false);
            passedLevels.Add(GameManager.SceneList.Level02.ToString(), false);
            ownedHearts = new SerializbleDictionary<string, int>();
        }
    }
}
