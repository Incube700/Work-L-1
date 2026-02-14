using UnityEngine;

public static class UnityLayers
{
    public static readonly int Default = LayerMask.NameToLayer("Default");
    public static readonly int TransparentFX = LayerMask.NameToLayer("TransparentFX");
    public static readonly int IgnoreRaycast = LayerMask.NameToLayer("Ignore Raycast");
    public static readonly int Water = LayerMask.NameToLayer("Water");
    public static readonly int UI = LayerMask.NameToLayer("UI");

    public static readonly int MaskDefault = 1 << Default;
    public static readonly int MaskTransparentFX = 1 << TransparentFX;
    public static readonly int MaskIgnoreRaycast = 1 << IgnoreRaycast;
    public static readonly int MaskWater = 1 << Water;
    public static readonly int MaskUI = 1 << UI;
}
