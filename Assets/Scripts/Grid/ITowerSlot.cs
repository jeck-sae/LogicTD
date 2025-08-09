namespace TowerDefense
{
    public interface ITowerSlot
    {
        public Tower Tower { get; }
        public bool CanPlace(Tower t);
        public void PlaceTower(Tower tower);
        public void RemoveTower();
    }
}