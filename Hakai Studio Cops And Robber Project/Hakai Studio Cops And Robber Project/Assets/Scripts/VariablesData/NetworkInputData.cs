using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InputButton
{
    Jump,
}

public struct NetworkInputData : INetworkInput
{
    public Vector3 temp;
    public float gravity;
}
