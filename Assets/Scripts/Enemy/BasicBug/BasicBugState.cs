namespace Enemy.BasicBug
{
    public abstract class BasicBugState : State
    {
        protected StateController StateController;
        protected BasicBug Enemy;

        protected BasicBugState(StateController stateController, BasicBug enemy)
        {
            this.StateController = stateController;
            this.Enemy = enemy;
        }
    }
}