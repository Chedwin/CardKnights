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
    public interface IFactory<F>
    {
        F CreateInstance(CKGameManager.GAME_STATE_TYPE cKGameType);
        //F CreateInstance(params F[] arr);
    }


} // end namespace CKInterface