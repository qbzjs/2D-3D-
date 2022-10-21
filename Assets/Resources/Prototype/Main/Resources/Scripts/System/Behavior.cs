using System.Collections.Generic;
using System.Linq;

namespace KSH_Lib
{
    public class Behavior<T>
    {
        /*--- Public Fields---*/
        public int Count
        {
            get
            {
                return stateStack.Count();
            }
        }

        
        /*--- Private Fields---*/
        List<Behavior<T>> stateStack = new List<Behavior<T>>();


        /*--- Public Methods---*/
        public virtual void Update( in T actor, ref Behavior<T> state )
        {
            Behavior<T> newState = DoBehavior( actor );
            if ( newState != null )
            {
                state = newState;
                state.Activate( actor );    
            }
        }
        public void SetSuccessorStates(in List<Behavior<T>> successors)
        {
            stateStack = successors;
        }
        public void PushSuccessorState(in Behavior<T> behavior )
        {
            stateStack.Add( behavior );
        }
        public void PushSuccessorStates( in List<Behavior<T>> behaviors )
        {
            stateStack.AddRange( behaviors );
        }
        public bool HasSuccessors()
        {
            return stateStack.Count != 0;
        }
        public Behavior<T> PassState()
        {
            var ps = stateStack.Last();
            stateStack.RemoveAt( stateStack.Count - 1 );
            ps.SetSuccessorStates( stateStack );
            return ps;
        }


        /*--- Protected Methods---*/
        protected virtual void Activate( in T actor ) { }
        protected virtual Behavior<T> DoBehavior( in T actor )
        {
            if (HasSuccessors())
            {
                return PassState();
            }
            return null;
        }
    }
}