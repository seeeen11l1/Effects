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
    public class Moon : StoryboardObjectGenerator
    {
        [Configurable]
        public float ScaleOut;
        [Configurable]
        public Color4 ColorMan;
        [Configurable]
        public Vector2 MainMoonPos;

        public override void Generate()
        {
		var layer = GetLayer("Moon");
  
            ChorusEffects(86228, 99943, layer);
            BeatJumper(86228, 99943, layer);

            ChorusEffects(154800, 182228, layer);
            BeatJumper(168514, 182228, layer);
            Rings(154800, 168514, 4, layer, 35, 1, 120);
            
        }
        public void ChorusEffects(int startTime, int endTime, StoryboardLayer layer)
        {
            var mainMoon = layer.CreateSprite("sb/MOON/mainMoon.png", OsbOrigin.Centre, MainMoonPos);
            var beatration = Beatmap.GetTimingPointAt(startTime).BeatDuration;
            double j = startTime;
            var randomRot = DegToRad(15);

            while(j <= endTime)
            {   
                mainMoon.Rotate(OsbEasing.InOutSine, j, j + beatration * 4, mainMoon.RotationAt(j), randomRot);
                j += beatration * 4;
                randomRot *= -1;
            }
            mainMoon.Fade(startTime - beatration * 2, startTime, 0, 1);
            mainMoon.Fade(endTime, 0);

            mainMoon.Move(OsbEasing.OutBack, startTime - beatration * 2, startTime, new Vector2(320, -100), MainMoonPos);
            mainMoon.Scale(startTime, 0.3);

            mainMoon.Color(startTime, ColorMan);
            mainMoon.Color(startTime - beatration * 2, Color4.Black);

            mainMoon.Rotate(startTime - beatration * 2, startTime, DegToRad(-15), DegToRad(0));
        }
	
        public void BeatJumper(int startTime, int endTime, StoryboardLayer layer)
        {   
            var moonOut = layer.CreateSprite("sb/MOON/moonOut.png", OsbOrigin.Centre, new Vector2(320, 240));
            var angle = 8;
            var beatration = Beatmap.GetTimingPointAt(startTime).BeatDuration;

            moonOut.Fade(startTime, startTime + beatration, 0, 1);
            moonOut.Fade(endTime, 0);

            for(double i = startTime; i <= endTime; i += beatration)
            {
                moonOut.Scale(i, i + 150, ScaleOut, ScaleOut + .03);
                moonOut.Scale(i + 150, i + 225, ScaleOut + .03, ScaleOut);

                moonOut.Rotate(i, i + beatration, moonOut.RotationAt(i), DegToRad(angle));
                angle += 8;
            }
        }

        public void Rings(int startTime, int endTime, int rings, StoryboardLayer layer, int numberOfBalls, int beatMult, int rad)
        {
            var beatration = Beatmap.GetTimingPointAt(startTime).BeatDuration;
            var multiplier = 1;
            for(var i = 1; i <= rings; i++)
            {
                var radius = rad * i;
                var number = 360d / numberOfBalls;
                for(double j = 0; j < numberOfBalls; j++)
                {   
                    var trueAngle = number * j;
                    var posX = radius * Math.Cos(DegToRad(trueAngle)) * multiplier + 320;
                    var posY = radius * Math.Sin(DegToRad(trueAngle)) + 240;

                    var sprite = layer.CreateSprite("sb/ball.png", OsbOrigin.Centre, new Vector2((float)posX, (float)posY));

                    sprite.Fade(startTime, startTime + beatration, 0, 1);
                    sprite.Scale(0, 0.01 * i);

                    var angle = 6;
                    var moveMult = beatration * beatMult;

                    for(double k = startTime; k < endTime; k += moveMult)
                    {
                        var newX = radius * Math.Cos(DegToRad(trueAngle - angle)) * multiplier + 320;
                        var newY = radius * Math.Sin(DegToRad(trueAngle - angle)) + 240;

                        if(k < endTime - moveMult)
                        {
                            sprite.Move(OsbEasing.InOutSine, k, k + moveMult, sprite.PositionAt(k), new Vector2((float)newX, (float)newY));
                            angle += 6;
                        } else{
                            newX = 600 * Math.Cos(DegToRad(trueAngle - angle)) * multiplier + 320;
                            newY = 600 * Math.Sin(DegToRad(trueAngle - angle)) + 240;
                            sprite.Move(OsbEasing.InBack, endTime - moveMult, endTime, sprite.PositionAt(endTime), new Vector2((float)newX, (float)newY));
                        }
                    }
                }
                multiplier *= -1;
            }
        }
        public double DegToRad(double angle)
        {
            double degree = angle * Math.PI/180; 
            return degree;
        }
    }
}
 
