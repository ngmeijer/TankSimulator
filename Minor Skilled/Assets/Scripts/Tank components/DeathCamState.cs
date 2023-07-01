namespace FSM.CameraStates
{
    public class DeathCamState : CameraState
    {
        public override void Enter()
        {
            base.Enter();

            OnDeathActions();
        }



        private void OnDeathActions()
        {

        }
    }
}