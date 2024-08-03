using Grids;
using Internal.Dependencies.Core;
using UnityEngine;

namespace Player
{
    public class PlayerGridProxy : ADependency<IGridPointer>, IGridPointer
    {
        [SerializeField] private Transform pointer;
        [SerializeField] private Transform player;

        public Vector3 PointerPosition => pointer.position;
        public Vector3 PlayerPosition => player.position;
    }
}