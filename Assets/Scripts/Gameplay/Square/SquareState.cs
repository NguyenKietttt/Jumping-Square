using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SquareState : StateBase
{
    private Action<object> _onTitleRef, _onTitleToGameplayRef, _onGameplayRef, _onGameplayToGameoverRef;


    private Transform _transform;
    private Rigidbody2D _squareRb;
    private bool _isAllowJump, _isFirstJump;


    private void Awake()
    {
        CacheComponents();
        CacheCallbacks();
    }

    private void OnEnable()
    {
        EventDispatcher.RegisterListener(EventsID.TITLE_STATE, _onTitleRef);
        EventDispatcher.RegisterListener(EventsID.TITLE_TO_GAMEPLAY_STATE, _onTitleToGameplayRef);
        EventDispatcher.RegisterListener(EventsID.GAMEPLAY_STATE, _onGameplayRef);
        EventDispatcher.RegisterListener(EventsID.GAMEPLAY_TO_GAMEOVER_STATE, _onGameplayToGameoverRef);
    }

    private void OnDisable()
    {
        EventDispatcher.RemoveListener(EventsID.TITLE_STATE, _onTitleRef);
        EventDispatcher.RemoveListener(EventsID.TITLE_TO_GAMEPLAY_STATE, _onTitleToGameplayRef);
        EventDispatcher.RemoveListener(EventsID.GAMEPLAY_STATE, _onGameplayRef);
        EventDispatcher.RemoveListener(EventsID.GAMEPLAY_TO_GAMEOVER_STATE, _onGameplayToGameoverRef);
    }


    public override void OnTitle()
    {
        _isAllowJump = false;
        EventDispatcher.PostEvent(EventsID.ALLOW_JUMP_SQUARE, _isAllowJump);

        _squareRb.bodyType = RigidbodyType2D.Kinematic;
    }

    public override void OnTitleToGameplay()
    {
        _isFirstJump = true;
        EventDispatcher.PostEvent(EventsID.FIRST_JUMP_SQUARE, _isFirstJump);
    }

    public override void OnGameplay()
    {
        _squareRb.bodyType = RigidbodyType2D.Dynamic;
        EventDispatcher.PostEvent(EventsID.FIRST_COLLIDED_SQUARE);
    }

    public override void OnGameplayToGameover()
    {
        EventDispatcher.PostEvent(EventsID.HIDE_SQUARE);
    }


    private void CacheComponents()
    {
        _transform = transform;
        _squareRb = GetComponent<Rigidbody2D>();
    }

    private void CacheCallbacks()
    {
        _onTitleRef = (param) => OnTitle();
        _onTitleToGameplayRef = (param) => OnTitleToGameplay();
        _onGameplayRef = (param) => OnGameplay();
        _onGameplayToGameoverRef = (param) => OnGameplayToGameover();
    }
}
