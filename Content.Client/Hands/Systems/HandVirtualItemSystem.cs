﻿using Content.Client.Items;
using Content.Shared.Hands.Components;
using JetBrains.Annotations;
using Robust.Shared.GameObjects;

namespace Content.Client.Hands.Systems
{
    [UsedImplicitly]
    public sealed class HandVirtualItemSystem : EntitySystem
    {
        public override void Initialize()
        {
            base.Initialize();

            Subs.ItemStatus<HandVirtualItemComponent>(_ => new HandVirtualItemStatus());
        }
    }
}
