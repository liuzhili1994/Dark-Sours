using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Global;

public class PlayerCtrl : MonoBehaviour
{
    public GameObject player;
    private PlayerInput playerInput;
    private Animator ani;
    private Rigidbody rig;

    private Vector3 movingVec3;
    public float rotSpeed = 0.5f;
    public float walkSpeed = 1f;
    public float runSpeed = 2f;

    // Start is called before the first frame update
    void Start()
    {
        playerInput = transform.GetComponent<PlayerInput>();
        if (!player) {
            Debug.LogError("Charactor is missing !!!",gameObject);
            return;
        }

        ani = player.GetComponent<Animator>();
        rig = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //设置动画
       // ani.SetFloat(GlobalData.ANI_action,playerInput.sqrtFlo *  (playerInput.run ? 2 : 1));
        ani.SetFloat(GlobalData.ANI_action, playerInput.sqrtFlo * Mathf.Lerp(ani.GetFloat(GlobalData.ANI_action), (playerInput.run ? 2 : 1), 0.1f));
        //角色旋转
        player.transform.forward = playerInput.dirVec3 == Vector3.zero ? player.transform.forward : Vector3.Slerp(player.transform.forward,playerInput.dirVec3,rotSpeed) ;
        //移动方向
        movingVec3 = playerInput.dirVec3  * walkSpeed * (playerInput.run ? runSpeed : 1);

    }

    private void FixedUpdate()
    {
        // rig.MovePosition(rig.position + movingVec3 * Time.fixedDeltaTime);
        rig.position += movingVec3 * Time.fixedDeltaTime;
    }
}
