using Game.Character;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace LifetimeScopes
{
    public class UICharacterLifetimeScope : LifetimeScope
    {
        // キャラクター選択用
        [SerializeField] private CharacterCore characterCore;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(characterCore);
        }
    }
}