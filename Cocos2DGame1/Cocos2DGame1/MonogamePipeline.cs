using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Perceptual.Faces;
//using FarseerPhysics.Samples.ScreenSystem.Perceptual.Hands;
using System.Threading.Tasks;


namespace Perceptual
{
    public class MonogamePipeline : UtilMPipeline
    {
        public MonogamePipeline()
        {
            // Enabling image capture
            EnableImage(PXCMImage.ColorFormat.COLOR_FORMAT_DEPTH);
            EnableImage(PXCMImage.ColorFormat.COLOR_FORMAT_RGB32);
            EnableImage(PXCMImage.ColorFormat.COLOR_FORMAT_VERTICES);

            // Enabling gesture
            EnableGesture();

            // Enabling Face Detection and landmark
            EnableFaceLocation();
            EnableFaceLandmark();

        }

        public Texture2D QueryCameraImage()
        {
            return null;
        }

        public List<Face> QueryFaces()
        {
            List<Face> faces = new List<Face>();

            PXCMFaceAnalysis faceAnalysis = QueryFace();
            PXCMFaceAnalysis.Detection faceLocation = (PXCMFaceAnalysis.Detection)faceAnalysis.DynamicCast(PXCMFaceAnalysis.Detection.CUID);
            PXCMFaceAnalysis.Landmark faceLandmark = (PXCMFaceAnalysis.Landmark)faceAnalysis.DynamicCast(PXCMFaceAnalysis.Landmark.CUID);

            for (uint fidx = 0; ; fidx++)
            {
                int faceId = 0;
                ulong timeStamp;


                pxcmStatus sts = faceAnalysis.QueryFace(fidx, out faceId, out timeStamp);
                if (sts < pxcmStatus.PXCM_STATUS_NO_ERROR) break; // no more faces

                PXCMFaceAnalysis.Detection.Data faceLocationData;
                pxcmStatus locationStatus = faceLocation.QueryData(faceId, out faceLocationData);
                uint detectionConfidence = faceLocationData.confidence;


                if (locationStatus >= pxcmStatus.PXCM_STATUS_NO_ERROR && faceLocationData.fid != 0)
                {
                    Face face = new Face(faceLocationData);
                    // Face landmarks

                    PXCMFaceAnalysis.Landmark.ProfileInfo landmarkProfile;
                    faceLandmark.QueryProfile(1, out landmarkProfile);
                    faceLandmark.SetProfile(ref landmarkProfile);

                    PXCMFaceAnalysis.Landmark.LandmarkData[] faceLandmarkData = new PXCMFaceAnalysis.Landmark.LandmarkData[7];
                    pxcmStatus landmarkStatus = faceLandmark.QueryLandmarkData(faceId, PXCMFaceAnalysis.Landmark.Label.LABEL_7POINTS, faceLandmarkData);
                    face.AddLandmarks(faceLandmarkData);
                    faces.Add(face);
                }
            }

            return faces;
        }
        }
#if USE_HANDS
        public Hand QueryHand(Hand.HandType handType)
        {
            Hand hand = null;

            PXCMGesture.GeoNode geoNode = new PXCMGesture.GeoNode();
            QueryGesture().QueryNodeData(0, Hand.GetLabel(handType), out geoNode);

            hand = new Hand(geoNode, handType);

            return hand;
        }

        public List<Hand> QueryHands()


        {
            List<Hand> hands = new List<Hand>();

            foreach (Hand.HandType type in Enum.GetValues(typeof(Hand.HandType)))
            {
                Hand hand = QueryHand(type);

                if (hand.IsPresent)
                {
                    hands.Add(hand);
                }
            }

            return hands;
        }
#endif

    }
}

