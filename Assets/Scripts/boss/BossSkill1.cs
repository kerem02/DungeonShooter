using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSkill1 : MonoBehaviour
{
    private float timeTrack =1;
    public float triggerTime =1f;
    public float damage = 10f;
    public player player;
    public Transform targetTransform;
    public float lifeTime = 5;
    // Start is called before the first frame update
    void Start()
    {
        player = FindAnyObjectByType<player>();
        Destroy(gameObject,lifeTime);
        targetTransform = player.transform;

    }

    // Update is called once per frame
    void Update()
    {   
        if(player != null || targetTransform != null){           
            if(timeTrack <=0f&& Vector3.Distance(targetTransform.position,transform.position) <2 ){
                
                player.takeDamage(damage,Vector2.zero,0);
                
                timeTrack = triggerTime;
            }else{
                timeTrack -= Time.deltaTime;
            }
        }
    }
}
