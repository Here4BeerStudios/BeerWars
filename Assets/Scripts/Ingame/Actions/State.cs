using UnityEngine;

namespace Assets.Scripts.Ingame.Actions
{
    public struct State
    {
        public readonly int PlayerId;
        public readonly Vector2Int Origin;
        public readonly ActionCode Code;

        public State(int playerId, Vector2Int origin, ActionCode actionCode)
        {
            PlayerId = playerId;
            Origin = origin;
            Code = actionCode;
        }

        //TODO implement network transfer
    }
}
