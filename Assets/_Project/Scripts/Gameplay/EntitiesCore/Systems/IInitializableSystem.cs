namespace Assets._Project.Scripts.Gameplay.EntitiesCore.Systems
{
    public interface IInitializableSystem: IEntitySystem
    {
        void OnInit(Entity entity);
    }
}
