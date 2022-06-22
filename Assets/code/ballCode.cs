//using System.Numerics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
public class ballCode : MonoBehaviour
{
    [SerializeField] private Material material;

    public AudioSource JumpSound;
    public AudioSource CollisionSound;
    public AudioSource FinishSound;

    public float speed = 200;
    public bool isGrounded;
    /*DoubleJump is false when the double jump is already used*/
    private bool DoubleJump;
    public float xForce = 4;
    public float yForce = 90;
    private float Gravity = -30;
    private float canJump = 0f;
    private float xOrigins,yOrigins,zOrigins;
    private float red, green, blue;


    public float cubeSize = .3f;
    public int cubesInTotal = 11;

    float CubePivotDistance;
    Vector3 CubePivot;

    public float explosionForce = 650;
    public float explosionRadius = 10f;
    public float explosionUpward = 10f;

    void Start()
    {
        /*explosion*/
        CubePivotDistance = cubeSize * cubesInTotal/2;
        Vector3 CubePivot = new Vector3(CubePivotDistance, CubePivotDistance, CubePivotDistance); 
        /*explosion*/

        red = Random.Range(0, 255);
        green = Random.Range(0, 255);
        blue = Random.Range(0, 255);

        material.color = new Color(red/255f, green/255f, blue/255f);

        xOrigins = transform.position.x;
        yOrigins = transform.position.y;
        zOrigins = transform.position.z;
        Physics.gravity = new Vector3(0,Gravity,0);
    }

    // Update is called once per frame
    void Update()
    {

        if(transform.position.y < -2f){
            respawn();
        }

        // X axis movement
        float x = 0f;
        float y = 0f;
        if(isGrounded){
            if(Input.GetKey(KeyCode.D)){
            x = x + xForce* Time.deltaTime*speed;
            }
            if(Input.GetKey(KeyCode.A)){
                x = x - xForce * Time.deltaTime*speed;
            }
        }
        // Y axis movement
        if(Input.GetKeyDown(KeyCode.Space) && Time.time > canJump){
            if (isGrounded || DoubleJump){
                playJumpSound();
                if(!isGrounded && DoubleJump){
                    DoubleJump = false;
                }
                y = y + yForce * Time.deltaTime*speed;
                canJump = Time.time + .15f;//.1 seconds double jump delay
            }
        }
        if(Input.GetKeyDown(KeyCode.Escape)){
            Application.Quit();    
            Debug.Log("Game is exiting");
        }

        if(Input.GetKeyDown(KeyCode.P)){
            if(Time.timeScale == 1){
                PauseGame();
                Debug.Log("Game Paused");
            }else{
                ResumeGame();
                Debug.Log("Game Started");
            }
        }

        GetComponent<Rigidbody> ().AddForce (x,y,0f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name == "Plane"){
            respawn();
        }
        if(collision.gameObject.name == "Start" || collision.gameObject.name == "platform1" || collision.gameObject.name == "platform2" || collision.gameObject.name == "platform3" || collision.gameObject.name == "MovingPlatform"){
            isGrounded = true;
            DoubleJump = true;
            playCollisionSound();
        }        
    }
    public CameraControl cam;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "end"){
            cam.triggered = true;
            playFinishSound();
            explosion();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.name == "Start" || collision.gameObject.name == "platform1" || collision.gameObject.name == "platform2" || collision.gameObject.name == "platform3" || collision.gameObject.name == "MovingPlatform"){
            isGrounded = false;
        }
    }

    private void explosion(){
        for (int x = 0; x < cubesInTotal; x++){
            for (int y = 0; y < cubesInTotal; y++){
                for (int z = 0; z < cubesInTotal; z++){
                    createCube(x,y,z);
                }
            }
        }
        //Get explosion pos
        Vector3 explosionPos = transform.position;
        //get colliders in that pos and radius
        Collider[] colliders = Physics.OverlapSphere(explosionPos,explosionRadius);
        //add explosion force to all cubes 
        foreach(Collider hit in colliders){
            //get rigidbody from collider object
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if(rb != null){
                //add explosion force to this body with given parameters
                rb.AddExplosionForce(explosionForce,transform.position, explosionRadius, explosionUpward);
            }
        }



    }

    private void createCube(int x,int y, int z){
        //create cube
        GameObject cube;
        cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

        //Cube size
        cube.transform.position = transform.position + new Vector3(cubeSize*x, cubeSize*y, cubeSize*z) - CubePivot;
        cube.transform.localScale = new Vector3(cubeSize, cubeSize, cubeSize); 
        

        red = Random.Range(0, 255);
        green = Random.Range(0, 255);
        blue = Random.Range(0, 255);

        //adding rigid body to cube
        cube.AddComponent<Rigidbody>();
        cube.GetComponent<Rigidbody>().mass = cubeSize;
        cube.GetComponent<Renderer>().material.color = new Color(red/255f, green/255f, blue/255f);
    }




    void respawn(){
        transform.position = new Vector3(xOrigins, yOrigins, zOrigins);
    }

    void PauseGame ()
    {
        Time.timeScale = 0;
    }

    void ResumeGame ()
    {
        Time.timeScale = 1;
    }

    void playJumpSound(){
        JumpSound.Play();
        JumpSound.volume = 0.5f;
    }
    void playCollisionSound(){
        CollisionSound.Play();
    }

    void playFinishSound(){
        FinishSound.Play();
        FinishSound.volume = 0.5f;
    }
}