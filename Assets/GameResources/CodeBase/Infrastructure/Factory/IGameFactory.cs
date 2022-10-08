using CodeBase.Infrastructure.Services;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
    public interface IGameFactory : IService
    {
        public GameObject CreateHero(GameObject at);
        public void CreateHud();
    }
}