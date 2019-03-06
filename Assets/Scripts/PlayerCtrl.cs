using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Global;
using Custom.Log;

public class PlayerCtrl : MonoBehaviour
{
    public GameObject player;
    /// <summary>
    /// input模块
    /// </summary>
    private PlayerInput playerInput;
    private Animator ani;
    private Rigidbody rig;

    /// <summary>
    /// 锁死平面移动
    /// </summary>
    private bool lockPlanner;
    private Vector3 movingVec3;
    public float jumpVelocityFlo = 0.3f;
    private Vector3 jumpVelocityVec3 = Vector3.zero;
    public float rotSpeed = 0.5f;
    public float walkSpeed = 1f;
    public float runSpeed = 2f;
    [Space(10)]
    public bool isGround;

    // Start is called before the first frame update
    void Start()
    {
        playerInput = transform.GetComponent<PlayerInput>();
        if (!player) {
            Debug.LogError("Charactor is missing !!!",gameObject);
            return;
        }
        else
        {
            this.Log("Init charactor success...");
        }

        ani = player.GetComponent<Animator>();
        rig = GetComponent<Rigidbody>();
    }//Start_End


    // Update is called once per frame
    void Update()
    {
        //设置动画
       // ani.SetFloat(GlobalData.ANI_action,playerInput.sqrtFlo *  (playerInput.run ? 2 : 1));
        ani.SetFloat(GlobalData.ANI_action, playerInput.sqrtFlo * Mathf.Lerp(ani.GetFloat(GlobalData.ANI_action), (playerInput.run ? 2 : 1), 0.2f));
        //角色旋转
        player.transform.forward = playerInput.dirVec3 == Vector3.zero ? player.transform.forward : Vector3.Slerp(player.transform.forward,playerInput.dirVec3,rotSpeed) ;
        //移动方向  这样不会受input模块的vertical和horizontal值影响
        if(!lockPlanner)
            movingVec3 = playerInput.dirVec3 * walkSpeed  * (playerInput.run ? runSpeed : 1);
        //单次触发jump逻辑
        if(playerInput.jump )
            ani.SetTrigger(GlobalData.ANI_jump);

        ani.SetBool(GlobalData.ANI_isGround,isGround);

        if (rig.velocity.magnitude > 2f)
        {
           ani.SetTrigger(GlobalData.ANI_roll);
        }
               
    }//Update_End

    private void FixedUpdate()
    {
        // rig.MovePosition(rig.position + movingVec3 * Time.fixedDeltaTime);
        //rig.position += movingVec3 * Time.fixedDeltaTime;
        // rig.velocity = Vector3.Lerp(rig.velocity, new Vector3(movingVec3.x, rig.velocity.y, movingVec3.z),0.3f) + jumpVelocityVec3 ;
        rig.velocity = new Vector3(movingVec3.x, rig.velocity.y, movingVec3.z) + jumpVelocityVec3;
        jumpVelocityVec3 = Vector3.zero;
    }

    #region AniEvents
    /// <summary>
    /// 进入jump动画事件
    /// </summary>
    public void EnterJump()
    {
        //this.Log("EnterJump");
        //关闭输入  防止跳跃中左右旋转
        playerInput.InputEnable = false;
        //开启平面锁定  一旦关闭输入 则主角的方向向量会重置未vector3.zero  主角跳不走了
        lockPlanner = true;
        //跳跃y方向  增量
        jumpVelocityVec3 = new Vector3(0,jumpVelocityFlo,0);
        
    }//EnterJump_End

    /// <summary>
    ///离开jump动画事件  
    /// </summary>
    public void ExitJump()
    {
        //开始输入
        playerInput.InputEnable = true;
        
        //this.Log("ExitJump");
    }//ExitJump_End

    public void EnterGround() {
        //关闭平面锁定   使主角方向向量随这input输入改变
        lockPlanner = false;
        playerInput.InputEnable = true;
    }

    public void EnterRoll() {
        //关闭输入  防止跳跃中左右旋转
        playerInput.InputEnable = false;
        //开启平面锁定  一旦关闭输入 则主角的方向向量会重置未vector3.zero  主角跳不走了
        lockPlanner = true;
        //跳跃y方向  增量
        //jumpVelocityVec3 = new Vector3(0, jumpVelocityFlo, 0);
    }

    #endregion

}
