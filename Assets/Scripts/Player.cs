using CC2DModule;
using CC2DModule.Modules;
using UnityEngine;

[RequireComponent (typeof(CC2D))]
public class Player : MonoBehaviour
{
    private CC2D cc;
    private GravityModule gravityModule;
    private void Awake()
    {
        cc = GetComponent<CC2D>();
        gravityModule = cc.GetModule<GravityModule>();
    }

    private void Start() =>
        gravityModule.DoActivate();

    public void Move(float horizontal) =>
        cc.SetHorizontal(horizontal);

    public void SwapGravity() =>
        gravityModule.SwapGravity();
}
