using Custom.Log;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerInput : MonoBehaviour
{
    public bool inputEnable = true;                                         //  Flag  inpu模块开关
    public Axis vertical = new Axis(KeyCode.W, KeyCode.S);
    public Axis horizontal = new Axis(KeyCode.D, KeyCode.A);
    public KeyCode runKey = KeyCode.LeftShift;
    public KeyCode jumpKey = KeyCode.Space;
    public bool run = false;
    public bool jump = false;
    private bool lastJump;
    
    /// <summary>
    /// 控制animator的数值
    /// </summary>
    [Space(10)]
    public float sqrtFlo;
    /// <summary>
    /// 控制人物的方向
    /// </summary>
    public Vector3 dirVec3;

    public bool InputEnable { get => inputEnable; set {
            vertical.inputEnable = horizontal.inputEnable = inputEnable = value;
        } }

    private void Update()
    {
#if UNITY_EDITOR
        vertical.inputEnable = horizontal.inputEnable = inputEnable;
#endif
        //将正方形点转成圆形得点
        Vector2 tempVec2 = SquareToCircle(new Vector2(horizontal,vertical));
        //当前位移得长度
        sqrtFlo = Mathf.Sqrt(tempVec2.y * tempVec2.y + tempVec2.x * tempVec2.x);
        //当前位移得方向
        dirVec3 = (tempVec2.y * Vector3.forward + tempVec2.x * Vector3.right);

        run = Input.GetKey(runKey);

        bool tempJump = Input.GetKey(jumpKey);
        if (tempJump != lastJump && tempJump == true) {
            jump = true;
            //this.Log("jump trigger");
        }
        else
            jump = false;
        lastJump = tempJump;
    }

    /// <summary>
    /// 将正方体的所有点转换成球形上的点
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public Vector2 SquareToCircle(Vector2 input) {
        Vector2 output = Vector2.zero;
        output.x = input.x * Mathf.Sqrt(1 - input.y * input.y  * 0.5f);
        output.y = input.y * Mathf.Sqrt(1 - input.x * input.x * 0.5f);
        return output;
    }
}

[System.Serializable]
public class Axis {
    public  KeyCode positiveKey;
    public  KeyCode negativeKey;
    [HideInInspector]
    public bool inputEnable;

    private float smoothFlo;
    private float velocityFlo;


    public Axis(KeyCode positiveKey,KeyCode negativeKey) {
        this.positiveKey = positiveKey;
        this.negativeKey = negativeKey;
    }

    public static implicit operator float(Axis axis) {
        if (!axis.inputEnable)
            return 0;

        float tempFlo = (Input.GetKey(axis.positiveKey) ? 1 : 0) - (Input.GetKey(axis.negativeKey) ? 1 : 0);
        axis.smoothFlo =  Mathf.SmoothDamp(axis.smoothFlo, tempFlo, ref axis.velocityFlo, 0.1f);
        return axis.smoothFlo;
    }

}
