using System;
using Cocos2D;
using Microsoft.Xna.Framework;

namespace Cocos2DGame1
{
    public class IntroLayer : CCLayerColor
    {
        public IntroLayer()
        {
            
            // setup our color for the background
            Color = new CCColor3B(Microsoft.Xna.Framework.Color.Blue);
            Opacity = 255;

            CameraManager.Instance.Start(this);

            //this.addSprite();
            this.TouchEnabled = true;
            this.ScheduleUpdate();

        }

        public override void Update(float dt)
        {
            CameraManager.Instance.update(dt);
            base.Update(dt);
        }


        public static CCScene Scene
        {
            get
            {
                // 'scene' is an autorelease object.
                var scene = new CCScene();

                // 'layer' is an autorelease object.
                var layer = new IntroLayer();

                // add layer as a child to scene
                scene.AddChild(layer);

                // return the scene
                return scene;

            }

        }

        public void addSprite()
        {
 
            // adicionando o sprite ao lazyLayer, centralizado e usando o arquivo .png como  fonte.
            CCSprite sprite = new CCSprite("hand.png");
            sprite.Position = CCDirector.SharedDirector.WinSize.Center;
            // escalona o Sprite para ter metade de seu tamanho nominal
            sprite.Scale = 0.5f;
            // gira o Sprite em 180 graus.
            sprite.Rotation= 180;
 
            this.AddChild(sprite, 0);

            var rotateToA = new CCRotateTo(1, 360);
            var scaleToA = new CCScaleTo(2, 1, 1);

            // executa uma sequência de ações diretamente no this.sprite
            sprite.RunAction(new CCSequence(rotateToA, scaleToA));


        }

        public override void TouchesBegan(System.Collections.Generic.List<CCTouch> touches)
        {
            base.TouchesBegan(touches);
        }


        public override void TouchesMoved(System.Collections.Generic.List<CCTouch> touches)
        {
            base.TouchesMoved(touches);
        }

        public override void TouchesEnded(System.Collections.Generic.List<CCTouch> touches)
        {
            base.TouchesEnded(touches);
        }

        public override void KeyPressed(Microsoft.Xna.Framework.Input.Keys key)
        {
            base.KeyPressed(key);
        }

        public override void KeyReleased(Microsoft.Xna.Framework.Input.Keys key)
        {
            base.KeyReleased(key);
            switch (key)
            {
                case Microsoft.Xna.Framework.Input.Keys.W:
                    //Hero.WalkFoward();
                    CCLog.Log("W apertado");
                    break;

            }
        }

        public override void DidAccelerate(CCAcceleration pAccelerationValue)
        {
            base.DidAccelerate(pAccelerationValue);

            //hero.move(pAccelerationValue.X);
        }

        protected override void OnGamePadButtonUpdate(CCGamePadButtonStatus backButton, CCGamePadButtonStatus startButton, CCGamePadButtonStatus systemButton, CCGamePadButtonStatus aButton, CCGamePadButtonStatus bButton, CCGamePadButtonStatus xButton, CCGamePadButtonStatus yButton, CCGamePadButtonStatus leftShoulder, CCGamePadButtonStatus rightShoulder, PlayerIndex player)
        {
            base.OnGamePadButtonUpdate(backButton, startButton, systemButton, aButton, bButton, xButton, yButton, leftShoulder, rightShoulder, player);
        }
    }
}

