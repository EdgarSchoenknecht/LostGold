using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootCannon : MonoBehaviour {

    //Drag in the Bullet Emitter from the Component Inspector.
    private GameObject Bullet_Emitter;
    private GameObject Misfire_Emitter;

    //Drag in the Bullet Prefab from the Component Inspector.
    public GameObject Bullet_Prefab;
    public GameObject FireCannon_Particle;
    public GameObject Misfire_Particle;


    //Enter the Speed of the Bullet from the Component Inspector.
    public float Bullet_Forward_Force;
    public float Bullet_Up_Force;
    

    private AudioSource _cannonShootAudioSource;
    private AudioClip[] _audioClipsArrayShoot;

    private Animator _animator;

    void Awake()
    {
        Bullet_Emitter = GameObject.Find("Cannon/Bullet Emitter");
        Misfire_Emitter = GameObject.Find("Cannon/Misfire Emitter");

        //sound
        _cannonShootAudioSource = GameObject.Find("GameController/Sound/Cannon").GetComponent<AudioSource>();
        _audioClipsArrayShoot = GameObject.Find("GameController/Sound/Cannon/Shoot").GetComponent<SoundClips>().AudioClipsArray;

        _animator = GetComponent<Animator>();
    }

    public void Shoot()
    {
        
        //The Bullet instantiation happens here.
        GameObject Temporary_Bullet_Handler;
        GameObject Temporary_Particle_Handler;

        Temporary_Bullet_Handler = Instantiate(Bullet_Prefab, Bullet_Emitter.transform.position, Bullet_Emitter.transform.rotation) as GameObject;

        Temporary_Particle_Handler = Instantiate(FireCannon_Particle, Bullet_Emitter.transform.position, Bullet_Emitter.transform.rotation) as GameObject;


        //Sometimes bullets may appear rotated incorrectly due to the way its pivot was set from the original modeling package.
        //This is EASILY corrected here, you might have to rotate it from a different axis and or angle based on your particular mesh.
        Temporary_Bullet_Handler.transform.Rotate(Vector3.left * 90);

        //Retrieve the Rigidbody component from the instantiated Bullet and control it.
        Rigidbody Temporary_RigidBody;
        Temporary_RigidBody = Temporary_Bullet_Handler.GetComponent<Rigidbody>();

        //animation
        _animator.SetTrigger("Fire Cannon");

        //sound
        SoundController.RandomizeSfx(.95f, 1.05f, _cannonShootAudioSource, _audioClipsArrayShoot);

        //Tell the bullet to be "pushed" forward by an amount set by Bullet_Forward_Force.
        Temporary_RigidBody.AddForce(transform.forward * Bullet_Forward_Force);
        Temporary_RigidBody.AddForce(transform.up * Bullet_Up_Force);

        //Basic Clean Up, set the Bullets to self destruct after 10 Seconds, I am being VERY generous here, normally 3 seconds is plenty.
        Destroy(Temporary_Bullet_Handler, 2.0f);
        Destroy(Temporary_Particle_Handler, 2.0f);

        _animator.SetBool("Reload Cannon", false);
    }

    public void Misfire()
    {
        GameObject Temporary_Particle_Handler;
        Temporary_Particle_Handler = Instantiate(Misfire_Particle, Misfire_Emitter.transform.position, Misfire_Emitter.transform.rotation) as GameObject;
    }



}
