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
    public class SetEnter : StoryboardObjectGenerator
    {
        [Configurable]
        public string SpritePath;
        [Configurable]
        public float RadianStrength = 180;
        [Configurable]
        public OsbEasing RotateEasing;
        [Configurable]
        public Vector2 SpriteScale;
        [Configurable]
        public Vector2 SidePosition;
        [Configurable]
        public Vector2 SidePosition2;
        [Configurable]
        public int BeatSplit;
        public override void Generate()
        {
		    TransitionEffect(57510, 68628, 1450);
            SpawnLinesTransitionEffect(77099, 78510);
            
        }

        // Seems incredibly inefficient 
        // But what can i do
        public void TransitionEffect(int startTime, int endTime, int rotateStrength)
        {
            var square = GetLayer("Transition").CreateSprite(SpritePath, OsbOrigin.CentreRight);
            var square2 = GetLayer("Transition").CreateSprite(SpritePath, OsbOrigin.CentreLeft);
            var beatDuration = Beatmap.GetTimingPointAt(startTime).BeatDuration;

            var rotation = RadianStrength * (Math.PI/180f);

            square.ColorHsb(startTime, 28, .0f, 0.4f);
            square.Fade(startTime,0);
            square.Move(startTime, SidePosition);
            square.ScaleVec(startTime, SpriteScale);

            square2.ColorHsb(startTime, 29, .0f, 0.4f);
            square2.Fade(startTime,0);
            square2.Move(startTime, SidePosition2);
            square2.ScaleVec(startTime, SpriteScale);
            
            for(var i = startTime; i <= endTime; i += rotateStrength)
            {
                square.Fade(startTime, endTime, 1, 1);
                square2.Fade(startTime, endTime, 1, 1);

                if(i <= endTime - rotateStrength)
                {
                    square.Rotate(RotateEasing, i, i + rotateStrength, square.RotationAt(i), rotation);
                    square2.Rotate(RotateEasing, i, i + rotateStrength, square2.RotationAt(i), rotation);
                    rotation *= -1f;
                }
                if(i >= endTime - rotateStrength)
                {
                    square.ScaleVec(OsbEasing.In, i, i + beatDuration, square.ScaleAt(i), new Vector2(2f, SpriteScale.Y));
                    square2.ScaleVec(OsbEasing.In, i, i + beatDuration, square2.ScaleAt(i), new Vector2(2f, SpriteScale.Y));
                }
            }
        }

        public void SpawnLinesTransitionEffect(int startTime, int endTime)
        {
            OsbOrigin spriteOrigin;
            Vector2 vector2;
            var beatDuration = (int)Beatmap.GetTimingPointAt(startTime).BeatDuration / BeatSplit;
            var num = 0;
            int xPos;

            for(var i = startTime; i <= endTime; i += beatDuration)
            {
                xPos = Random(-120, 720);

                if(num % 2 == 0)
                {
                    spriteOrigin = OsbOrigin.BottomCentre;
                    vector2 = new Vector2(xPos, 500);
                }
                else{
                    spriteOrigin = OsbOrigin.TopCentre;
                    vector2 = new Vector2(xPos, -20);
                }
                num++;
                var line = GetLayer("Transition").CreateSprite("sb/square.png", spriteOrigin);
                line.Move(i, vector2);
                line.ScaleVec(i, i + beatDuration, new Vector2(0.5f, 0), new Vector2(0.5f, 3));
                line.Fade(i, endTime, 1, 1);
                line.Fade(endTime, 0);
                line.ColorHsb(i, Random(290, 310), Random(0.3f, 0.5), 1);

                if(i + beatDuration >= endTime - beatDuration)
                break;


            }
        }
    }
}
