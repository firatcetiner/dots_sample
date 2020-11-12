using System;
using System.ComponentModel;
using Unity.Entities;

[Serializable]
[GenerateAuthoringComponent]
public struct BallComponent : IComponentData
{
    public float MoveSpeed;
    public int Tag;
}
