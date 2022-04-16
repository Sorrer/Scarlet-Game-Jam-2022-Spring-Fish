using System.Collections;
using System.Collections.Generic;
using Game.Systems.Player;
using UnityEngine;

public class LockedLookCamera : MonoBehaviour
{
    public float LerpSpeed;
    
    public Vector2 MaxRotations;
    public float DeadzonePercent;
    public AnimationCurve MovePercentage;

    public Vector3 StartAngle;

    public PlayerCursorData data;
    [Range(0,1)]
    public float LimitLookRange;
    
    // Start is called before the first frame update
    void Start()
    {
        StartAngle = this.transform.rotation.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        var centerPercent = GetPercentageFromCenter();
        Vector3 setAngle = Mathf.Abs(centerPercent.x) > 1 || Mathf.Abs(centerPercent.y) > 1
            ? this.transform.rotation.eulerAngles
            : StartAngle; 
        
        
        //Do X
        if (Mathf.Abs(centerPercent.x) > DeadzonePercent)
        {
            float delta =  (MaxRotations.x * MovePercentage.Evaluate(Mathf.Abs(centerPercent.x)));
            
            if (centerPercent.x > 0)
            {
                delta *= -1;
            }
            setAngle.x =  Mathf.Clamp(StartAngle.x + delta, -90, 90);
        }
        else
        {
            setAngle.x = StartAngle.x;
        }
        
        //Do Y
        if (Mathf.Abs(centerPercent.y) > DeadzonePercent)
        {
            float delta = (MaxRotations.y *MovePercentage.Evaluate(Mathf.Abs(centerPercent.y)));

            if (centerPercent.y < 0)
            {
                delta *= -1;
            }

            setAngle.y = StartAngle.y + delta;
        }
        else
        {
            setAngle.y = StartAngle.y;
        }
        

        this.transform.eulerAngles = Vector3LerpAngle(this.transform.eulerAngles, setAngle, LerpSpeed * Time.deltaTime);
    }

    private Vector3 Vector3LerpAngle(Vector3 a, Vector3 b, float speed)
    {
        var vec = new Vector3();
        vec.x = Mathf.LerpAngle(a.x, b.x, speed);
        vec.y = Mathf.LerpAngle(a.y, b.y, speed);
        vec.z = Mathf.LerpAngle(a.z, b.z, speed);
        return vec;
    }


    private Vector2 GetPercentageFromCenter()
    {
        Vector2 screenDimensions = new Vector2(Screen.width, Screen.height);
        Vector2 center = screenDimensions / 2.0f;

        Vector2 mousePos = Input.mousePosition;

        if (data) mousePos = data.position;
    
        // 2 - 1 = 1 , 1 + 1 = 2
        Vector2 mouseCenterVec = mousePos - center;
        

        return new Vector2(mouseCenterVec.y / center.y, mouseCenterVec.x / center.x ) * (1 - LimitLookRange);
    }
}
