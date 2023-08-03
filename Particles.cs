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
    public class Particles : StoryboardObjectGenerator
    {
        [Configurable]
        public string BreezeParticle;
        [Configurable]
        public float BreezeScale;
        public override void Generate()
        {
            // So i dont have to keep calling this
            var layer = GetLayer("Particle");

            // Breeze effect the first verse
		    BreezeEffect(58800, 72514, 1, layer);

            // Chorus particles
            MoonEffect(86228, 99943, 6, layer);
            BreezeEffect(86228, 99943, 4, layer);

            // At the slow part
            StarEffect(99943, 113657, 4, layer);

            // Breeze effect at the other verse later
            BreezeEffect(127371, 141085, 1, layer);

            // Chorus particles
            MoonEffect(168514, 182228, 7, layer);
            BreezeEffect(168514, 182228, 4, layer);

            // Final section
            StarEffect(182228, 195942, 4, layer);
        }

        public void BreezeEffect(int startTime, int endTime, int beatDivisor, StoryboardLayer layer)
        {
            
            var beatration = Beatmap.GetTimingPointAt(startTime).BeatDuration / beatDivisor;
            
            var XPos = -138;
            var radius = 1000;

            for(double i = startTime; i < endTime; i += beatration)
            {
                var scaleR = Random(0.001, BreezeScale);
                var moveTime = i + beatration * beatDivisor * 2;
                var randomY = Random(0, 480);
                var angle = Random(0, Math.PI);

                var breezePar = layer.CreateSprite(BreezeParticle, OsbOrigin.Centre);
            
                var posX = radius * Math.Cos(DegToRad(5)) + XPos;
                var posY = radius * Math.Sin(DegToRad(5)) + randomY;

                breezePar.Scale(i, scaleR);
                breezePar.Move(i, moveTime, new Vector2(XPos, randomY), new Vector2((float)posX, (float)posY));
            }
        }

        public void MoonEffect(int startTime, int endTime, int parAmount, StoryboardLayer layer)
        {
            var beatration = Beatmap.GetTimingPointAt(startTime).BeatDuration / parAmount;
            
            Vector2 moonPos = new Vector2(320, 240);

            for(double i = startTime; i <= endTime; i += beatration)
            {
                var particle = layer.CreateSprite(BreezeParticle, OsbOrigin.Centre);
                var randRadius = 30 * Math.Sqrt(Random(100, 400));
                var angle = Random(0, Math.PI * 2);

                var scaleR = Random(0.001, BreezeScale);
                var randEasing = OsbEasing.None;
                var moveTime = i + beatration * parAmount * 4;

                var newX = randRadius * Math.Cos(angle) + moonPos.X;
                var newY = randRadius * Math.Sin(angle) + moonPos.Y;

                particle.Fade(i + beatration * 2, i + beatration * 4, 0, 1);
                particle.Scale(i, scaleR);
                particle.Move(randEasing, i, moveTime, moonPos, new Vector2((float)newX, (float)newY));
            }
        }

        public void StarEffect(int startTime, int endTime, int rate, StoryboardLayer layer)
        {
            var beatration = Beatmap.GetTimingPointAt(startTime).BeatDuration;
            var radius = 800;
            for(double i = startTime; i < endTime; i += beatration / rate)
            {
                var randomX = Random(-148, 788);
                var randomY = Random(-20, 500);

                var posX = radius * Math.Cos(DegToRad(5)) + randomX;
                var posY = radius * Math.Sin(DegToRad(5)) + randomY;
                
                var particle = layer.CreateSprite(BreezeParticle, OsbOrigin.Centre, new Vector2(randomX, randomY));

                particle.Scale(i, 0.003);

                particle.Move(i, i + beatration * 32, particle.PositionAt(i), new Vector2(320, 240));
                particle.Fade(i, i + beatration, 0, 1);
                particle.Fade(i + beatration * 2, i + beatration * 4, 1, 0);
            }
        }

        public double DegToRad(double angle)
        {
            double degree = angle * Math.PI/180;
            return degree;
        }
    }
}
