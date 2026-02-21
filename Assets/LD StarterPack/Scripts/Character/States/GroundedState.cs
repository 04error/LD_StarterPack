using UnityEngine;

public class GroundedState : ICharacterState
{
    PlayerCharacter player;

    public GroundedState(PlayerCharacter player)
    {
        this.player = player;
    }

    public void Enter()
    {
        Debug.Log("Grounded");
        player.Motor.SetOnLadder(false);
    }

    public void Tick()
    {
        Vector2 input = new Vector2(
            Input.GetAxis("Horizontal"),
            Input.GetAxis("Vertical")
        );
        
        if (Input.GetKeyDown(player.jumpKey))
            player.Motor.Jump();

        
        if (!player.IsGrounded)
            player.StateMachine.SetState(new FallState(player));

        if (player.Interactor.TryInteract())
            return;

        if (player.Interactor.TryEnterLadder(out Ladder ladder))
            player.StateMachine.SetState(new LadderState(player, ladder));

        
        if (player.TryGetGround(out RaycastHit hit))
        {
            Vector3 forward = player.CameraController.cameraPivot.forward;
            Vector3 right = player.CameraController.cameraPivot.right;

            forward.y = 0f;
            right.y = 0f;

            forward.Normalize();
            right.Normalize();

            Vector3 move = forward * input.y + right * input.x;

            if (move.sqrMagnitude > 1f)
                move.Normalize();
            
            if (player.CameraController.currentModeType == CameraModeType.ThirdPerson)
                player.Motor.RotateCharacter(move, player.transform);

            if (player.IsOnSteepSlope(out _))
            {
                player.Motor.ApplySlide(hit.normal);
                return;
            }

            Vector3 surfaceMove = player.Motor.ProjectOnGround(move, hit.normal);
            float speed = player.stats.moveSpeed;

            if (player.SprintHeld())
                speed = player.stats.sprintSpeed;

            if (player.CrouchHeld())
                speed = player.stats.crouchSpeed;

            player.Motor.MoveSurface(surfaceMove, speed);

        }

    }
    

    public void Exit() { }
}
