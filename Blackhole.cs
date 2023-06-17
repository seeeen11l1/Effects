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
    public class Blackhole : StoryboardObjectGenerator
    {
        [Group("Other")]
        [Configurable]
        public Vector2 CirclePos = new Vector2(320, 240); // It is where the circle pos will be, the circle is where the spawn boundary is
        [Group("Effect Configuration")]
        [Configurable]
        public float RubbleIntensity = 100; // If this reaches values close to 0 it will break, so dont make it reach 0
        [Configurable]
        public double TotalRadius = 500;
        [Group("Spawned Assets' Data")]
        [Configurable]
        public string RubbleType = "sb/roundRect.png"; // Whatever your asset directory is
        [Configurable]
        public float FadeLength = 1000; // Time it takes for the spawned assets to fade
        [Configurable]
        public Vector2 RubbleScale;
        public override void Generate()
        {
            BlackholeEffect(47699, 71698, RubbleIntensity, TotalRadius);
        }
        
        // Start and end time is self explanatory
        // rubble amount is the RubbleIntensity it depics how intense you want the spawning to be
        // tRad represents the size of the spawn boundary of the assets, in this case, 500d is good, but play around with it
        public void BlackholeEffect(int startTime, int endTime, float rubbleAmount, double tRad)
        {
            var layer = GetLayer("Blackhole");
            var beat = Beatmap.GetTimingPointAt(0).BeatDuration;
            
            var curAngle = 0d;
            var curRadius = 0d;
        
            for(var time = (float)startTime; time <= endTime; time += rubbleAmount)
            {
                var rubble = layer.CreateSprite(RubbleType, OsbOrigin.Centre);

                // Some formula I found on a math video
                // I dont understand anything but it works so - :D
                var angle = Random(curAngle - 2d * Math.PI, curAngle + 2d * Math.PI);
                var radius = Random(curRadius - tRad, curRadius + tRad);
                
                // Puts the random angle and radius into the calculator
                var position = RubblePosition(angle, radius);

                // Gets a random fade of the spawned assets for aesthetic purposes
                // You can remove this if you want
                var randFade = Random(FadeLength - 600, FadeLength);

                // Base calling the rubble and having it fade in and out randomly
                rubble.Fade(time, time + randFade, 1, 0);
                rubble.Fade(0, 0);

                // Makes the rubble move towards the center
                rubble.Move(OsbEasing.Out, time, time + FadeLength, position, new Vector2(320, 240));

                // Scales the rubble however i want it to
                rubble.ScaleVec(0, RubbleScale);
            } 
        }

        public Vector2 RubblePosition(double angle, double radius)
        {
            // The formula uses the compass plane, and the formula corresponds to using that formula
            double posX = CirclePos.X + (radius * Math.Cos(angle));
            double posY = CirclePos.Y + ((radius * Math.Sin(angle)));
            return new Vector2((float)posX, (float)posY);
        }

    }
}
