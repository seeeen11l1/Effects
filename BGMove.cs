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
    public class BGMove : StoryboardObjectGenerator
    {
        [Configurable]
        public int StartTime;
        [Configurable]
        public int EndTime;
        [Configurable]
        public Vector2 bgPos;
        [Configurable]
        public Vector2 backrubPos;
        [Configurable]
        public Vector2 anyaPos;
        [Configurable]
        public Vector2 yorPos;
        [Configurable]
        public Vector2 backrub2Pos;
        [Configurable]
        public Vector2 paperPos;
        [Configurable]
        public Vector2 paper2Pos;
        [Configurable]
        public Vector2 paper3Pos; 
        [Configurable]
        public int Degrees;
        public override void Generate()
        {
            BGMOVE(58800, 72514);
            BGMOVE(127371, 141085);
        }

        public void BGMOVE(int startTime, int endTime)
        {
            List<string> assets = new List<string>() {"sb/BG/anyaID.png", "sb/BG/yorID.png", "sb/BG/backRubble.png", "sb/BG/backRubble2.png",
            "sb/BG/paper1.png", "sb/BG/paper2.png", "sb/BG/paper3.png"};

            Vector2[] locations = new Vector2[] {anyaPos, yorPos, backrubPos, backrub2Pos, paperPos, paper2Pos, paper3Pos};

            Vector2[] outsideLocations = new Vector2[] {new Vector2(840, 460), new Vector2(30, 500), new Vector2(0, 100), 
            new Vector2(180, -20), new Vector2(740, 20), new Vector2(600, -20), new Vector2(600, 300)};

            var beatDuration = Beatmap.GetTimingPointAt(startTime).BeatDuration;

		    var layer = GetLayer("BGMove");
            var bitMap = GetMapsetBitmap("sb/BG/anyaYorCut.png");
            var bgCut = layer.CreateSprite("sb/BG/anyaYorCut.png", OsbOrigin.Centre, bgPos);

            var number = 0;

            bgCut.Scale(startTime, 1080f / bitMap.Width + 0.1);

            bgCut.Fade(0,0);
            bgCut.Fade(startTime, startTime + 1, 0, 1);
            bgCut.Fade(endTime - beatDuration / 2, endTime, 1, 0);

            // It's better to seperate each sprite with their own for loop
            // Bgcut
            for(var i = startTime; i <= endTime; i += 2400)
            {
                var angle = Random(0, 2 * Math.PI);
                var radius = Math.Sqrt(Random(0, 20));

                var newX = radius * Math.Cos(angle) + bgPos.X;
                var newY = radius * Math.Sin(angle) + bgPos.Y;

                if(i < endTime - 2400)
                    bgCut.Move(OsbEasing.InOutSine, i, i + 2400, bgCut.PositionAt(i), new Vector2((float)newX, (float)newY));

            }

            foreach(var sb in assets)
            {
                var sprite = layer.CreateSprite(sb, OsbOrigin.Centre, outsideLocations[number]);
                sprite.Scale(startTime, 1080f / bitMap.Width + 0.1);

                var moveAmount = Random(1800, 2400);
                sprite.Fade(startTime, startTime + beatDuration * 4, 0, 1);
                sprite.Fade(endTime, 0);
                
                for(var i = startTime; i < endTime; i += moveAmount)
                {
                    var angle = Random(0, 2 * Math.PI);
                    var radius = Math.Sqrt(Random(0, 50));
                    var rotate = Random(RadToDeg(-Degrees), RadToDeg(Degrees));

                    var newX = radius * Math.Cos(angle) + locations[number].X;
                    var newY = radius * Math.Sin(angle) + locations[number].Y;

                    sprite.Move(OsbEasing.InOutSine, i, i + moveAmount, sprite.PositionAt(i), new Vector2((float)newX, (float)newY));
                    sprite.Rotate(i, i + moveAmount, sprite.RotationAt(i), rotate);
                }
                number++;
            }
        }
        public double RadToDeg(double angle)
        {
            double degree = angle * Math.PI/180;
            return degree;
        }
    }
}
