using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class FrameRateCounter : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI display;
    [SerializeField, Range(0.1f,2f)] float sampleDuration = 1.0f;

    public enum DisplayMode { FPS, MS}
    [SerializeField] DisplayMode displayMode = DisplayMode.FPS;

    private int mFrames;
    private float mDuration;
    private float mBestDuration = float.MaxValue;
    private float mWorstDuration;

    // Update is called once per frame
    void Update()
    {
        float frameDuration = Time.unscaledDeltaTime;
        mFrames += 1;
        mDuration += frameDuration;

        if(frameDuration < mBestDuration) 
        { 
            mBestDuration = frameDuration;
        }
        if(frameDuration > mWorstDuration)
        {
            mWorstDuration = frameDuration;
        }

        if(mDuration >= sampleDuration)
        {
            if(displayMode == DisplayMode.FPS)
            {
                display.SetText("FPS\n{0:0}\n{1:0}\n{2:0}", 1f / mBestDuration, mFrames / mDuration, 1f / mWorstDuration);
                mFrames = 0;
                mDuration = 0f;
                mBestDuration = float.MaxValue;
                mWorstDuration = 0f;
            }
            else
            {
                display.SetText(
                    "MS\n{0:1}\n{1:1}\n{2:1}",
                    1000f * mBestDuration,
                    1000f * mDuration / mFrames,
                    1000f * mWorstDuration
                );
            }
        }

    }
}
