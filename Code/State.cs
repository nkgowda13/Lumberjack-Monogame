﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumberjack
{
    public abstract class State
    {
        private List<Transition> m_Transitions = new List<Transition>();
        public List<Transition> Transitions
        {
            get { return m_Transitions; }
        }

        public void AddTransition(Transition transition)
        {
            m_Transitions.Add(transition);
        }

        public abstract void Enter(object owner);
        public abstract void Exit(object owner);
        public abstract void Execute(object owner, GameTime gameTime);

        public string Name
        {
            get;
            set;
        }
    }

    public class Transition
    {
        public readonly State NextState;
        public readonly Func<bool> Condition;

        public Transition(State nextState, Func<bool> condition)
        {
            NextState = nextState;
            Condition = condition;
        }
    }

}
