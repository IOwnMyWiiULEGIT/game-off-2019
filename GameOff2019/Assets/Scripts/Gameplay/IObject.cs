﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum ObjectState{
    On,
    Off
}

public enum ObjectColor {
    Red,
    Blue,
    Green
}


public abstract class IObject : MonoBehaviour
{
    public ObjectColor objectColor;

    public ObjectState objectState = ObjectState.Off;

    public bool persistent = false;

    [HideInInspector]
    public Vector3Int cellPos;

    Tilemap tilemap;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        tilemap = GetComponentInParent<Tilemap>();
        if(tilemap == null){
            cellPos = new Vector3Int(0,0,0);
            Debug.LogError("No tilemap in parent");
            return;
        }
      
        cellPos = tilemap.WorldToCell(transform.position);
    }


    public void ToggleState(){

        if(this.objectState == ObjectState.Off){
            objectState = ObjectState.On;
            TurnOn();
        }
        else{
            objectState = ObjectState.Off;
            TurnOff();
        }

    }

    public virtual void TurnOn(){}

    public virtual void TurnOff(){}


    
}