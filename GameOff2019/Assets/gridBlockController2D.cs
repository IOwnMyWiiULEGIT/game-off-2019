﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class gridBlockController2D : MonoBehaviour
{

    [SerializeField] Tilemap tilemap;

    BoxCollider2D collider;

    RayCastOrigins rayCastOrigins;

    const float skinWidth = 0.015f;

    public GridCollisionFlags gridCollisionFlags;

    float tilelength;

    Vector3Int currentTile;

    bool moving = false;


    public float movementSpeed = 5f;


    public float maxSlopeAngle = 80f;

    public LayerMask collideableLayer;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        tilelength = tilemap.cellSize.x;
        currentTile = tilemap.WorldToCell(transform.position);

        collider = GetComponent<BoxCollider2D>();
        
        
        //move into cell centre if not already there
        transform.position = tilemap.CellToWorld(currentTile)+tilemap.tileAnchor;



    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {

        GetCollisions();


        if(!moving){


            GravityMovement();


        }

        
        
        
    }


    public void HorizontalMovement(float horizontalMovement)
    {
        if(gridCollisionFlags.below&&!moving){
           

            Vector3Int newTile = currentTile;

            if(horizontalMovement<0&&gridCollisionFlags.lslopeAngle<maxSlopeAngle){
                newTile = new Vector3Int(currentTile.x-1,currentTile.y,0);
                if(gridCollisionFlags.lslopeAngle>0){
                    newTile.y+=1;
                }
                else if(gridCollisionFlags.dslopeAngle>0){
                    newTile.y-=1;
                }
               
            }

            if(horizontalMovement>0&&gridCollisionFlags.rslopeAngle<maxSlopeAngle){
                newTile = new Vector3Int(currentTile.x+1,currentTile.y,0);
                if(gridCollisionFlags.rslopeAngle>0){
                    newTile.y+=1;
                }
                else if(gridCollisionFlags.dslopeAngle>0){
                    newTile.y-=1;
                }
            }

            //edge case for top of a slope
            if(tilemap.HasTile(newTile)){
                newTile.y+=1;
            }


            
            StartCoroutine(SmoothMove(newTile));

        }
    }

    private void GravityMovement()
    {
        if(!gridCollisionFlags.below){
            int distanceIntiles = (int)(GetDistanceToCollideAbleTile(Vector2.up*-1,rayCastOrigins.down)/tilelength);
            

            Vector3Int newTile = new Vector3Int(currentTile.x,currentTile.y-distanceIntiles,0);

            StartCoroutine(SmoothMove(newTile));
            
        }
    }

    void GetCollisions(){
        
        gridCollisionFlags.Reset();
        CalculateRayOrigins();

        float rayDistance = skinWidth+tilelength/2;
        
        //up
        RaycastHit2D hit;

        hit  = Physics2D.Raycast(rayCastOrigins.up,Vector2.up,rayDistance,collideableLayer);

        if(hit){

            gridCollisionFlags.above = true;
            gridCollisionFlags.Cabove = hit.collider;

        }


        //down
        hit  = Physics2D.Raycast(rayCastOrigins.down,-1*Vector2.up,rayDistance,collideableLayer);
        Debug.DrawRay(rayCastOrigins.down,-1*Vector2.up,Color.red);

        if(hit){

            gridCollisionFlags.below = true;
            gridCollisionFlags.Cbelow = hit.collider;
            gridCollisionFlags.dslopeAngle = Vector2.Angle(hit.normal,Vector2.up);

        }

        //left

        hit  = Physics2D.Raycast(rayCastOrigins.left,-1*Vector2.right,rayDistance,collideableLayer);

        if(hit){

            gridCollisionFlags.left = true;
            gridCollisionFlags.Cleft = hit.collider;
            gridCollisionFlags.lslopeAngle = Vector2.Angle(hit.normal,Vector2.up);

        }

        //right
        hit  = Physics2D.Raycast(rayCastOrigins.right,Vector2.right,rayDistance,collideableLayer);

        if(hit){

            gridCollisionFlags.right = true;
            gridCollisionFlags.Cright = hit.collider;
            gridCollisionFlags.rslopeAngle = Vector2.Angle(hit.normal,Vector2.up);

        }


        

    }


    void CalculateRayOrigins(){
        Bounds bounds = collider.bounds;
        bounds.Expand(-1 * skinWidth);

        rayCastOrigins.up = new Vector2(bounds.min.x+bounds.size.x/2,bounds.max.y);
        rayCastOrigins.down = new Vector2(bounds.min.x+bounds.size.x/2,bounds.min.y);
        rayCastOrigins.left = new Vector2(bounds.min.x,bounds.min.y+bounds.size.y/2);
        rayCastOrigins.right = new Vector2(bounds.max.x,bounds.min.y+bounds.size.y/2);


    }

    float GetDistanceToCollideAbleTile(Vector2 direction,Vector2 origin){

        float distance = 0;

        RaycastHit2D hit2D;

        hit2D = Physics2D.Raycast(origin,direction,float.PositiveInfinity,collideableLayer);

        if(hit2D){
            return hit2D.distance;
        }

        return distance;

    }


    IEnumerator SmoothMove(Vector3Int newTile){

        moving = true;

        Vector3 positionToMove = tilemap.CellToLocal(newTile)+tilemap.tileAnchor;
        Vector3 originPosition = tilemap.CellToLocal(currentTile)+tilemap.tileAnchor;

        while(transform.position!=positionToMove){

            float ratio = Mathf.Abs((Mathf.Abs((transform.position-originPosition).magnitude)+movementSpeed*Time.deltaTime)/(positionToMove-originPosition).magnitude);

            transform.position = Vector3.Lerp(originPosition,positionToMove,ratio);

            yield return null; 
        }
        currentTile = newTile;
        moving = false;

    }





    

    

}