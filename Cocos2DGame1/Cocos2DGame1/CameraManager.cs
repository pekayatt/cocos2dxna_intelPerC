
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cocos2D;
using Microsoft.Xna.Framework.Input;


namespace Cocos2DGame1
{
    public class CameraManager: UtilMPipeline
    {
        private static CameraManager instance;
        private  CCNode parent;

        private  CCSprite hand1sprite;
        private  CCSprite hand2sprite;
        
        private  float HAND_PRECISION = 1.7f;
        private  int DEFAULT_RADIUS = 100;

        private CCPoint hand1Pos;
        private CCPoint hand2Pos;

        private readonly List<CCTouch> newTouches = new List<CCTouch>();
        private readonly List<CCTouch> newEndTouches = new List<CCTouch>();
    
        public static CameraManager Instance
        {
            get 
            {
                if (instance == null)
                {
                    instance = new CameraManager();
                }
                return instance;
            }
        }
        
        private CameraManager() { 
            if (!this.IsImageFrame()){
                this.EnableGesture();


                if (!this.Init()){
                    CCLog.Log("Camera Manager Init Failed");
                }
            }
            this.hand1sprite = null;
	        this.hand2sprite = null;


            //this.inputAreas = CCArray::createWithCapacity(50);
            //this.inputAreas.retain();
        }



        public bool isHand1Closed;

        public bool isHand2Closed;
        private CCLabelTTF labelPos;
        


        public void ResetHands()
        {
            if (this.hand1sprite != null && this.hand1sprite.Parent != null)
            {
                this.hand1sprite.RemoveFromParentAndCleanup(true);
                this.hand2sprite.RemoveFromParentAndCleanup(true);

                this.hand1sprite = null;
                this.hand2sprite = null;

                this.isHand1Closed = false;
                this.isHand2Closed = false;
            }
        }

        public bool Start(CCNode parent){
            this.parent = parent;
            
	        this.hand1sprite = new CCSprite("hand.png");
            this.hand1sprite.Opacity = 150;
            this.hand1sprite.Position = CCDirector.SharedDirector.WinSize.Center;
	        //this.hand1sprite.SetPosition(-1000,-1000); //To make it out of screen

            this.hand2sprite = new CCSprite("hand.png");
	        this.hand2sprite.Opacity =150;
            this.hand2sprite.FlipX = true;
	        this.hand2sprite.SetPosition(-1000,-1000); //To make it out of screen

	        this.parent.AddChild(this.hand1sprite, 10);
            this.parent.AddChild(this.hand2sprite, 10);

            this.labelPos = new CCLabelTTF("Starting camera", "MarkerFelt", 22);

            // position the label on the center of the screen
            this.labelPos.SetPosition(CCDirector.SharedDirector.WinSize.Center.X, CCDirector.SharedDirector.WinSize.Height - 100); ;

            // add the label as a child to this Layer
            this.parent.AddChild(this.labelPos);

            //this.inputAreas.removeAllObjects();
            this.isHand1Closed = false;
            this.isHand2Closed = false;

	        return true;
        }

        
        public void update(float dt){
            if (!this.AcquireFrame(false)){
		        return;
	        }
            
            PXCMGesture.GeoNode geoNode1 = new PXCMGesture.GeoNode();
            PXCMGesture.GeoNode geoNode2 = new PXCMGesture.GeoNode();

            QueryGesture().QueryNodeData(0, PXCMGesture.GeoNode.Label.LABEL_BODY_HAND_PRIMARY, out geoNode1);
            QueryGesture().QueryNodeData(0, PXCMGesture.GeoNode.Label.LABEL_BODY_HAND_SECONDARY, out geoNode2);


            CCSize screenSize = CCDirector.SharedDirector.WinSize;

            float cameraX = geoNode1.positionImage.x;
            float cameraY = geoNode1.positionImage.y;
            float screenWidth = (float)screenSize.Width * 1.5f;
            float screenHeight = (float)screenSize.Height * 1.5f;

            this.hand1Pos = new CCPoint(screenWidth - cameraX * (screenWidth / 320) - 100,
                                 screenHeight - cameraY * (screenHeight / 240));

            this.hand2Pos = new CCPoint((float)(screenSize.Width * 1.5 - geoNode2.positionImage.x * (screenSize.Width * HAND_PRECISION / 320) - 100),
                                 (float)(screenSize.Height * 1.5 - geoNode2.positionImage.y * (screenSize.Height * HAND_PRECISION / 240)));

            

            this.labelPos.SetString("X: " + geoNode1.positionImage.x + " Y: " + geoNode1.positionImage.y);

            if (geoNode1.opennessState == PXCMGesture.GeoNode.Openness.LABEL_CLOSE)
            {
                newTouches.Clear();
                newTouches.Add(new CCTouch(1, hand1Pos.X, hand1Pos.Y));

                CCDirector.SharedDirector.TouchDispatcher.TouchesBegan(newTouches);


                if (this.hand1sprite == null)
                {
                    return;
                }
                this.hand1sprite.RemoveFromParentAndCleanup(true);
                this.hand1sprite = new CCSprite("hand_close.png");
                this.hand1sprite.Opacity = 150;
                this.parent.AddChild(hand1sprite);
                this.isHand1Closed = true;


            }
            else if (this.isHand1Closed)
            {
                newEndTouches.Clear();
                newEndTouches.Add(new CCTouch(1, hand1Pos.X, hand1Pos.Y));

                CCDirector.SharedDirector.TouchDispatcher.TouchesEnded(newEndTouches);

                if (this.hand1sprite == null)
                {
                    return;
                }
                this.hand1sprite.RemoveFromParentAndCleanup(true);
                this.hand1sprite = new CCSprite("hand.png");
                this.hand1sprite.Opacity = 150;
                this.parent.AddChild(hand1sprite);
                this.isHand1Closed = false;
            }

	        if (hand1sprite.Parent == null || hand2sprite.Parent ==null ){
		        return;
	        }
	        this.hand1sprite.Position = this.hand1Pos;
            this.hand2sprite.Position = this.hand2Pos;


            this.ReleaseFrame();
        }
    }    
}

