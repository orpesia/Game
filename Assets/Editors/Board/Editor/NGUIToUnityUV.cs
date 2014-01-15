using UnityEngine;

namespace EditorUtils
{
    public class NGUIToUnityUV
    {
        public static Rect Convert( UIAtlas atlas, UISpriteData spriteData )
        {
            float texWidth = atlas.texture.width;
            float texHeight = atlas.texture.height;

            //씨발 유니티 uv가 병신이야.ㅋㅋㅋㅋㅋ.
            float left = (float)(spriteData.x) / texWidth;
            float top = (float)(texHeight - (spriteData.y + spriteData.height)) / texHeight;
            float right = (float)(spriteData.width) / texWidth;
            float bottom = (float)(spriteData.height) / texHeight;

            return new Rect(left, top, right, bottom);
        }

        public static Rect Convert(UIAtlas atlas, string name)
        {
            return Convert(atlas, atlas.GetSprite(name));
        }
    }
}
