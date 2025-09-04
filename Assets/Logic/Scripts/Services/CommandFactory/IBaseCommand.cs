using Zenject;

namespace Logic.Scripts.Services.CommandFactory
{
    public interface IBaseCommand
    {
        void SetObjectResolver(DiContainer diContainer);
        void ResolveDependencies();
    }
}