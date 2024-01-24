using System;
using UnityEngine;

namespace Obstacles
{
    public class ElectricWall : MonoBehaviour
    {
        public PlayerManager player1;
        public PlayerManager player2;
        public bool isAbove = true;
        public bool bluePolarity = true;
        [ColorUsage(showAlpha:true, hdr:true)]
        public Color blueColor, redColor;

        public GameObject[] emitters;
        public ElectricWallBeam wall;

        private Rigidbody2D _player1RB, _player2RB;
        private SpriteRenderer[] _emitterRenderers;

        private void Start()
        {
            _player1RB = player1.GetComponent<Rigidbody2D>();
            _player2RB = player2.GetComponent<Rigidbody2D>();

            _emitterRenderers = new SpriteRenderer[emitters.Length];

            for (int i = 0; i < emitters.Length; i++)
            {
                _emitterRenderers[i] = emitters[i].GetComponent<SpriteRenderer>();
            }
            
            UpdateColor();
            UpdateTarget();

            wall.bluePolarity = bluePolarity;
            wall.blueColor = blueColor;
            wall.redColor = redColor;
        }

        private void Update()
        {
            //Update current target and shader params
            UpdateTarget();
        }

        private void UpdateColor()
        {
            Color newColor = blueColor;
            if (!bluePolarity)
            {
                newColor = redColor;
            }
            
            foreach (SpriteRenderer emitterRenderer in _emitterRenderers)
            {
                emitterRenderer.color = newColor;
            }
        }

        private void UpdateTarget()
        {
            PlayerManager tgt;
            if (isAbove)
            {
                if (_player1RB.gravityScale > 0)
                {
                    tgt = player1;
                }
                else
                {
                    tgt = player2;
                }
            }
            else
            {
                if (_player1RB.gravityScale < 0)
                {
                    tgt = player1;
                }
                else
                {
                    tgt = player2;
                }
            }
            
            wall.target = tgt;
        }
    }
}