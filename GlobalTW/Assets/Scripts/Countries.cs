using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using WPMF;


namespace GTW
{

    public struct cdetail
    {
        public int[] a;
    }

    public struct gevent
    {
        public int key;
        public int status;
        public int size;
    }
    
    public class Countries : Admin
    {
        public int player { get; set; }
        public int WorldTension { get; set; }
        public int Atax { get; set; }
        public int Ctax { get; set; }        
        public int turnsN { get; set; }
        public int AP { get; set; }
        public bool ready = false;
        public List<gevent> gevents { get; set; }
        //player's country details : 

        public float[] production  {get; set;}

        public float[] population { get; set; }

        public float[] needs { get; set; }

        public float[,] FactionPN { get; set; }        

        public float unemployment { get; set; }
        
        //all other countries details :

        public int size { get; set; }

        public int[] index { get; set; }

        public string[] names { get; set; }

        public float[] standing { get; set; }

        public cdetail[] details { get; set; } 

                
        // Use this for initialization
        public Countries( int setting)
        {
            player = setting;
        }




    }
}