using UnityEngine;

public interface IPushable
{
    Vector2 CurrentGridPosition { get; }
    bool Push(float x, float y);
}
