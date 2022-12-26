
namespace Otome.Core
{
    public interface IDataManager
    {
        void LoadData(GameData gameData);
        void SaveData(ref GameData data);
    }
}
