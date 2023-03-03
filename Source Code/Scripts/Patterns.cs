using Godot;
using System.Collections.Generic;

namespace Scripts.Patterns
{
    public class State<T>
    {
        public T node;
        public StateManager<T> stateManager;

        public virtual int Process(float delta)
        {
            return 0;
        }
        public virtual int PhyicsProcess(float delta)
        {
            return 0;
        }

        public virtual void Enter()
        {
            return;
        }

        public virtual void Exit()
        {
            return;
        }
    }

    public class StateManager<T>
    {
        //States of the state manager are stored in this dictionary

        protected Dictionary<int, State<T>> states = new Dictionary<int, State<T>>();


        //Active state
        protected State<T> currentState;

        //The parent node that use the StateManager
        protected T node;


        //Empty Constructor
        public StateManager() { }


        //Constructor passing the parent node
        public StateManager(T user)
        {
            this.node = user;

        }



        //Add a state to the state manager
        public void AddState(int name, State<T> state)
        {
            //Reference to the node and the state manager
            state.node = node;
            state.stateManager = this;
            //Add the state to the dictionary
            states.Add(name, state);
        }

        //Set the active state
        public void SetCurrentState(int newState)
        {
            if (states[newState] == currentState)
            {
                return;
            }

            if (currentState != null)
            {
                currentState.Exit();
            }

            currentState = states[newState];

            currentState.Enter();


        }

        //Get the active state
        public State<T> GetCurrentState() => currentState;

        //Process the active state
        public void executeStateProcess(float delta)
        {
            currentState.Process(delta);
        }
        public void executeStatePhysics(float delta)
        {
            int newState = currentState.PhyicsProcess(delta);
            SetCurrentState(newState);


        }
    }
}
