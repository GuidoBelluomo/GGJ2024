using System;
using UnityEngine;
using UnityEngine.U2D.Animation;
using Random = Unity.Mathematics.Random;

namespace Managers
{
    public class SpriteManager : MonoBehaviour
    {
        [SerializeField] private SpriteLibraryAsset[] spriteLibraryAssets;
        private SpriteLibrary _spriteLibrary;
        
        private void Awake()
        {
            _spriteLibrary = GetComponent<SpriteLibrary>();
            RandomSprite();
        }

        public bool SwitchSprite(int spriteLibraryIndex)
        {
            if (spriteLibraryIndex < spriteLibraryAssets.Length)
            {
                _spriteLibrary.spriteLibraryAsset = spriteLibraryAssets[spriteLibraryIndex];
                return true;
            }

            return false;
        }

        public bool SwitchSprite(string spriteLibraryName)
        {
            for (var i = 0; i < spriteLibraryAssets.Length; i++)
            {
                var spriteLibraryAsset = spriteLibraryAssets[i];
                if (spriteLibraryAsset.name == spriteLibraryName)
                {
                    return SwitchSprite(i);
                }
            }

            return false;
        }

        public bool RandomSprite()
        {
            return SwitchSprite(UnityEngine.Random.Range(0, spriteLibraryAssets.Length));
        }
    }
}
