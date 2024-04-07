namespace Enemy.BasicBug
{
    public class StateController : BaseStateController
    {
        private BasicBug _enemy;
        public ChaseState ChaseState;
        public IdleState IdleState;
        public AttackState AttackState;
        public SearchState SearchState;
        
        public StateController(BasicBug enemy)
        {
            this._enemy = enemy;
        }

        public override void Initialize()
        {
            ChaseState = new ChaseState(this, _enemy);
            IdleState = new IdleState(this, _enemy);
            AttackState = new AttackState(this, _enemy);
            SearchState = new SearchState(this, _enemy);
            
            ChangeState(IdleState);
        }
    }
}