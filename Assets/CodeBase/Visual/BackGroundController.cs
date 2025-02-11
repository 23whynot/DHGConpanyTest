using UnityEngine;

namespace CodeBase.Visual
{
    public class BackGroundController : MonoBehaviour
    {
        [SerializeField] private Transform sprite1;
        [SerializeField] private Transform sprite2;
        
        private float _scrollSpeed = 5f;
        private float _spriteHeight;

        private void Start()
        {
            _spriteHeight = sprite1.GetComponent<SpriteRenderer>().bounds.size.y;
        }

        private void Update()
        {
            sprite1.Translate(Vector3.down * _scrollSpeed * Time.deltaTime);
            sprite2.Translate(Vector3.down * _scrollSpeed * Time.deltaTime);
            
            if (sprite1.position.y <= -_spriteHeight)
            {
                sprite1.position = new Vector3(sprite1.position.x, sprite2.position.y + _spriteHeight, sprite1.position.z);
            }

            if (sprite2.position.y <= -_spriteHeight)
            {
                sprite2.position = new Vector3(sprite2.position.x, sprite1.position.y + _spriteHeight, sprite2.position.z);
            }
        }
    }
}