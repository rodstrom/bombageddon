using System;
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
                int score = -1;

                if (currentState.ID == "PlayState")
                {
                    states[0] = null;
                    states[0] = new PlayState(game, "PlayState");
                    score = currentState.outputCode;
                }

                currentState.Terminate();

                currentState = SelectState(nextState);
                currentState.InputCode = score;

            }
        }

        public void Draw(GameTime gameTime)
        {
            currentState.Draw(gameTime);
        }
    }
}
