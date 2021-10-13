public enum EventsID
{
	// State
	TITLE_STATE,
	TITLE_TO_GAMEPLAY_STATE,
	GAMEPLAY_STATE,
	GAMEPLAY_TO_GAMEOVER_STATE,
	GAMEOVER_STATE,


	// Square
	SHOW_SQUARE,
	HIDE_SQUARE,
	ALLOW_JUMP_SQUARE,
	FIRST_JUMP_SQUARE,
	FIRST_COLLIDED_SQUARE,
	COLLIDED_SQUARE,


	// Score
	DISPLAY_SCORE,
	RESET_SCORE,


	// Holder
	SHOW_HOLDER,
	HIDE_HOLDER,

	// Spike
	GET_SPIKE_CHILD,
	SET_SPIKE_TO_SPAWN,
	HIDE_SPIKE,


	// VFX
	SQUARE_JUMP_VFX,
	SQUARE_COLLIDED_VFX,
	SQUARE_DEAD_VFX,


	// SFX
	SQUARE_MOVEMENT_SFX,
	SQUARE_COLLIDE_SFX,
	SQUARE_TRIGGER_SFX,
	HOLDER_SFX,
	SPIKE_SFX,
	PANEL_SFX,
	BUTTON_PLAY_SFX,
	BUTTON_SOUND_SFX
}