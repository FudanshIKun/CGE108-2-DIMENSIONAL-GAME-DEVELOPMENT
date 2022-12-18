
namespace Otome.Core
{
    [System.Serializable]
    public class GameData
    {
        #region Game Data

            public SerializbleDictionary<string, bool> playedLevels;

        #endregion
        
        #region Heart Data

            public SerializbleDictionary<string, int> ownedHearts;

        #endregion
        
        public GameData()
        {
            playedLevels = new SerializbleDictionary<string, bool>();
            ownedHearts = new SerializbleDictionary<string, int>();
        }
    }
}
