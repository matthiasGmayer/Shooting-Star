using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkPlayer : Photon.MonoBehaviour
{

    public Camera cam;
    public GameObject aim, bullet;
    private new Rigidbody2D rigidbody;
    PlayerController playerController;
    int id;
    // Use this for initialization
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        if (photonView.isMine)
        {
            Camera.main.enabled = false;
            cam.enabled = true;
            aim.SetActive(true);
            playerController = GetComponent<PlayerController>();
            playerController.enabled = true;
            cam.transform.parent = null;
            photonView.RPC("SetId", PhotonTargets.All, PhotonNetwork.player.ID);
        }
        else
        {
            //cam.enabled = false;
            //aim.SetActive(false);
            //transform.parent.GetComponent<PlayerController>().enabled = false;
        }
    }

    private Vector3 correctPlayerPos;
    private float correctPlayerRot;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!photonView.isMine)
        {
            //float speed = Time.deltaTime * 5;
            //transform.position = Vector3.Lerp(transform.position, this.correctPlayerPos, Time.deltaTime * 5);
            //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0,0,correctPlayerRot), Time.deltaTime * 5);

            //float speed = Time.deltaTime * 10;

            //rigidbody.MovePosition(Vector3.Lerp(transform.position, this.correctPlayerPos, speed));
            //rigidbody.MoveRotation(Mathf.Lerp(transform.rotation.eulerAngles.z, this.correctPlayerRot, speed));
        }
    }

    //void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    //{
    //    if (stream.isWriting)
    //    {
    //        // We own this player: send the others our data
    //        stream.SendNext(transform.position);
    //        stream.SendNext(transform.rotation.z);
    //    }
    //    else
    //    {
    //        // Network player, receive data
    //        this.correctPlayerPos = (Vector3)stream.ReceiveNext();
    //        this.correctPlayerRot = (float)stream.ReceiveNext();
    //    }
    //}
    public void FireBullet(int id, Vector2 start, Vector2 target, float speed)
    {
        photonView.RPC("GenerateBullet", PhotonTargets.All, id, start, target, speed);
    }

    [PunRPC]
    void GenerateBullet(int shooter, Vector2 start, Vector2 target, float speed)
    {
        GameObject bullet = Instantiate(this.bullet);

        Vector2 position = start;
        bullet.transform.position = position;
        Vector2 targetDirection = (target - position).normalized;
        Vector2 diff = target - position;
        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.Euler(0f, 0f, rot_z);

        bullet.GetComponent<Rigidbody2D>().velocity = targetDirection * speed;

        Destroy(bullet, 10);
    }
    [PunRPC]
    void SetId(int id)
    {
        this.id = id;
    }
}