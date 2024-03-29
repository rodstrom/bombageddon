﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Bombageddon.Code.States
{
    class StateManager
    {
        Bombageddon game;

        List<State> states;
        State currentState;

        public PlayState level
        {
            get
            {
                return (PlayState)states[0];
            }
        }

        public StateManager(Bombageddon game)
        {
            this.game = game;

            states = new List<State>();
            states.Add(new PlayState(game, "PlayState"));
            states.Add(new HighScoreState(game, "HighScoreState"));
            states.Add(new NameInputState(game, "NameInputState"));

            currentState = SelectState(game.config.getValue("General", "StartState"));
        }

        public State SelectState(String name)
        {
            foreach (State s in states)
            {
                if (s.ID == name)
                {
                    return s;
                }
            }
            return null;
        }

        public void Update(GameTime gameTime)
        {
            currentState.Update(gameTime);

            if (currentState.changeState)
            {
                String nextState = currentState.nextState;
                String input = "";

                if (currentState.ID == "PlayState")
                {
                    states[0] = null;
                    states[0] = new PlayState(game, "PlayState");
                } 
                if (currentState.ID == "NameInputState")
                {
                    states[2] = null;
                    states[2] = new NameInputState(game, "NameInputState");
                }
                input = currentState.outputCode;
                currentState.Terminate();

                currentState = SelectState(nextState);
                currentState.InputCode = input;

            }
        }

        public void Draw(GameTime gameTime)
        {
            currentState.Draw(gameTime);
        }
    }
}
