using UnityEngine;
using System.Collections;

public class PixelSprite {
	public bool isAnimating = true; 
	public bool isFacingLeft = false;

	private Texture2D originalTexture;
	private Color32[] pixelBuffer;

	private int sourceTopLeftX, sourceTopLeftY;
	private int eachWid, eachHei;
	private int animFrames;

	public PixelSprite(Texture2D useTexture,
	                   int perPicWid=-1, int perPicHei=-1,
	                   int srcX = 0,int srcY = 0) {
		originalTexture = useTexture;
		pixelBuffer = originalTexture.GetPixels32();

		sourceTopLeftX = srcX;
		sourceTopLeftY = srcY;

		if(perPicWid < 0) {
			eachWid = originalTexture.width;
			eachHei = originalTexture.height;
			isAnimating = false; // single frame, no need to animate it
		} else {
			eachWid = perPicWid;
			eachHei = perPicHei;
		}

		animFrames = originalTexture.width / eachWid;
	}

	public void drawImage(PixelScreenLib toSurface,
	                      int xDest, int yDest) {
		toSurface.copyBitmapFromToColorArray(sourceTopLeftX, sourceTopLeftY,
		                                     eachWid, eachHei,
		                                     xDest,yDest,
		                                     pixelBuffer,originalTexture.width,
		                                     (isAnimating ? GameManager.animFrameStep % animFrames : 0),
		                                     isFacingLeft);
	}

}