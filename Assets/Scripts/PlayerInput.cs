using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerInput : MonoBehaviour
{
    private Player player;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void Update()
    {
        player.Move(Input.GetAxis("Horizontal"));
        if(Input.GetKeyDown(KeyCode.Space))
            player.SwapGravity();
    }
}
