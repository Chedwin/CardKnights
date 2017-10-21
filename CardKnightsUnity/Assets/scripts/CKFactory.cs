﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CKInterface;

namespace CKFactory
{
    // GAMESTATE FACTORY
    public class GameStateFactory : IFactory<IState<CKGameManager>>
    {
        public GameStateFactory() { }

        public IState<CKGameManager> CreateInstance(CKGameManager.GAME_STATE_TYPE cKGameType)
        {
            IState<CKGameManager> gameState = null;
            switch (cKGameType)
            {
                case CKGameManager.GAME_STATE_TYPE.MAIN_MENU:
                    gameState = new MainMenuState();
                    break;
                case CKGameManager.GAME_STATE_TYPE.WORLD_MAP:
                    gameState = new WorldMapState();
                    break;
                case CKGameManager.GAME_STATE_TYPE.LOCAL_MAP:
                    gameState = new LocalMapState();
                    break;
                case CKGameManager.GAME_STATE_TYPE.BATTLE:
                    gameState = new BattleState();
                    break;
                case CKGameManager.GAME_STATE_TYPE.INGAME_MENU:
                    gameState = new InGameMenuState();
                    break;
            }
            return gameState;
        }
    }

} // end namespace CKFactory