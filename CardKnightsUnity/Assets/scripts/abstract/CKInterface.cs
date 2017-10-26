using System;
using System.Collections;
using System.Collections.Generic;

namespace CKInterface
{
    /// ISTATE
    public interface IState<T>
    {
        string StateName { get; set; }

        void EnterState(T _owner);
        void ExitState(T _owner);
        void UpdateState(T _owner);
    }

    /// IFACTORY
    public interface ICKFactory<F>
    {
        F CreateGameState(CKGameManager.GAME_STATE_TYPE cKGameType);
    }


} // end namespace CKInterface