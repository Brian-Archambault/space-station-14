using Content.Shared.Storage;
using JetBrains.Annotations;
using Robust.Client.GameObjects;
using Robust.Shared.GameObjects;
using Robust.Shared.IoC;
using Robust.Shared.Serialization.Manager.Attributes;

namespace Content.Client.Storage.Visualizers
{
    [UsedImplicitly]
    public sealed class StorageVisualizer : AppearanceVisualizer
    {
        [DataField("state")]
        private string? _stateBase;
        [DataField("state_open")]
        private string? _stateOpen;
        [DataField("state_closed")]
        private string? _stateClosed;

        public override void InitializeEntity(EntityUid entity)
        {
            if (!IoCManager.Resolve<IEntityManager>().TryGetComponent(entity, out ISpriteComponent? sprite))
            {
                return;
            }

            if (_stateBase != null)
            {
                sprite.LayerSetState(0, _stateBase);
            }
        }

        public override void OnChangeData(AppearanceComponent component)
        {
            base.OnChangeData(component);

            var entities = IoCManager.Resolve<IEntityManager>();
            if (!entities.TryGetComponent(component.Owner, out ISpriteComponent? sprite))
            {
                return;
            }

            component.TryGetData(StorageVisuals.Open, out bool open);
            var state = open ? _stateOpen ?? $"{_stateBase}_open" : _stateClosed ?? $"{_stateBase}_door";

            sprite.LayerSetState(StorageVisualLayers.Door, state);

            if (component.TryGetData(StorageVisuals.CanLock, out bool canLock) && canLock)
            {
                if (!component.TryGetData(StorageVisuals.Locked, out bool locked))
                {
                    locked = true;
                }

                sprite.LayerSetVisible(StorageVisualLayers.Lock, !open);
                if (!open)
                {
                    sprite.LayerSetState(StorageVisualLayers.Lock, locked ? "locked" : "unlocked");
                }
            }

            if (component.TryGetData(StorageVisuals.CanWeld, out bool canWeld) && canWeld)
            {
                if (component.TryGetData(StorageVisuals.Welded, out bool weldedVal))
                {
                    sprite.LayerSetVisible(StorageVisualLayers.Welded, weldedVal);
                }
            }
        }
    }

    public enum StorageVisualLayers : byte
    {
        Door,
        Welded,
        Lock
    }
}
