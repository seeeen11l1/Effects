using OpenTK;
using OpenTK.Graphics;
using StorybrewCommon.Mapset;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using StorybrewCommon.Storyboarding.Util;
using StorybrewCommon.Subtitles;
using StorybrewCommon.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StorybrewScripts
{
    public class Changer : StoryboardObjectGenerator
    {
        public override void Generate()
        {
            SpamACrapTonOfThem(68628, 71099, 700, -170, 850, 240, 1800f);
            SpamACrapTonOfThem(74187, 76657, 700, 850, -170, 240, 1800f);
        }
        public void ChangeSprie(int startTime, int endTime, int changeSpeed, float inX, float endX, float yVal, float fallSpeed)
        {
            string[] sprites = {"sb/chorus/replay2.png", "sb/chorus/replay3.png", "sb/chorus/replay1.png"};
            var flipSprite = 0;
            
            for(var i = startTime; i <= endTime; i += changeSpeed)
            {
                var spriteObject = GetLayer("Sprite").CreateSprite(sprites[flipSprite]);
                var offset = Random(-100, 100);
                var timingOffset = i + Random(-500, 500);
                var endyVal = yVal + offset; 
                var scaleSpr = Random(0.1f, 0.4f);
                flipSprite += 1;

                if(flipSprite == 3)
                {
                    flipSprite = 0;
                }
                spriteObject.Fade(startTime, 0);
                spriteObject.Fade(i, i + fallSpeed, 1, 0);

                spriteObject.Scale(startTime, scaleSpr);
                spriteObject.Move(OsbEasing.Out, timingOffset, timingOffset + fallSpeed, new Vector2(inX, endyVal), new Vector2(endX + offset, endyVal + offset));
                spriteObject.Rotate(OsbEasing.None, timingOffset, timingOffset + fallSpeed, spriteObject.RotationAt(i), Random(0f, 4f));
            }
        }
        public void SpamACrapTonOfThem(int startTime, int endTime, int spammedAmount, float inX, float endX, float yVal, float fallSpeed)
        {
            int i = startTime;
            while(i <= endTime)
            {
                ChangeSprie(startTime, endTime, 300, inX, endX, yVal, fallSpeed);
                i += spammedAmount;
            }
        }

        
    }
}
