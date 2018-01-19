using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TeaspoonTools.TextboxSystem.Utils
{
	public class AutoFontScaler
	{
		Font font;

		void LoadDefaultFont()
		{
			Debug.Log("Font was null! Loading default!");
			font = Resources.GetBuiltinResource<Font>("Arial.ttf");
		}
			

		/// <summary>
		/// Calculates the best font size for the passed text field to have, considering the 
		/// passed font and how many lines the client wants the text field to be able to hold
		/// at once.
		/// </summary>
		/// <returns></returns>
		public int ScaleFontSizeToText(Text textField, Font font = null, int linesPerTextbox = 3)
		{
			if (font == null)
				LoadDefaultFont ();
			else
				this.font = font;

            Debug.Log("Scaling font size with font " + font.name);

			// set up a label prefab as a measuring stick
			GameObject testLabel = 		CreateTestLabel();
			Text labelText = 			testLabel.GetComponent<Text> ();
			RectTransform labelRect = 	testLabel.GetComponent<RectTransform> ();

			labelRect.SetParent (textField.transform.parent.parent, false);

			// Using a small font size and seeing how tall the label gets with one line of
			// text, use that to figure out a good font size for the main text field to use
			int baseFontSize = 5;
			labelText.fontSize = 		baseFontSize;
			labelText.text = "A";

			Canvas.ForceUpdateCanvases();
			float baseHeight = 			labelRect.rect.height;

			//  further preparations to calculate the aforementioned good size
			RectTransform textRect = 	textField.rectTransform;
			float heightLimit = 		textRect.rect.height;
			float targetHeightPerLine = heightLimit / linesPerTextbox;

			float linesFittable = 		Mathf.Ceil(targetHeightPerLine / baseHeight); // with the current font size
			int resultSize = 			Mathf.FloorToInt (baseFontSize * (targetHeightPerLine / baseHeight));

			// now test that good size...
			labelText.text = 			"";
			labelText.fontSize = 		resultSize;
			for (int i = 0; i < linesPerTextbox - 1; i++)
				labelText.text += "A\nA";

			Canvas.ForceUpdateCanvases();

			float diffInHeight = 		labelRect.rect.height - heightLimit;
			float diffInLines = 		Mathf.Abs(diffInHeight / targetHeightPerLine); 
			// ^lines taken up

			int alterationAmount;
			int passes = 0; // just for debugging
			float differenceLimit = 0.1f;

			while (diffInLines >= differenceLimit) // ... and change it as necessary.
			{
				alterationAmount = Mathf.CeilToInt(diffInLines);

				// Raise the size? Lower it?
				if (diffInHeight > 0)
					resultSize -= alterationAmount;

				else if (diffInHeight < 0)
					resultSize += alterationAmount;

				// compare the text heights again for a more accurate result
				labelText.fontSize = resultSize;
				Canvas.ForceUpdateCanvases();
				diffInHeight = labelRect.rect.height - heightLimit;

				diffInLines = Mathf.Abs(diffInHeight / baseHeight);

				passes++;

				// avoid an infinite loop, cutting our losses
				if (passes >= linesPerTextbox * 2)
					break;

			}

			Debug.Log ("Adjusted the font size to best fit the textbox with " + passes + " extra passes.");
			Debug.Log("Using the simpler, better algorithm, the font size chosen for font " + font.name + " is: " + resultSize);

			// won't need this anymore!
			MonoBehaviour.Destroy(labelText.gameObject);

			return resultSize;

		}

		GameObject CreateTestLabel()
		{
			// to help with the auto-font-sizing

			GameObject label = new GameObject ("PrefabTestingLabel");
			label.AddComponent<RectTransform> ();

			Text labelText = label.AddComponent<Text> ();
			labelText.text = "";
			labelText.font = font;
			labelText.fontSize = 5;

			ContentSizeFitter sizeFitter = label.AddComponent<ContentSizeFitter> ();
			sizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
			sizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;

			// make the label invisible
			CanvasGroup canvasGroup = label.AddComponent<CanvasGroup> ();
			canvasGroup.alpha = 0;
			canvasGroup.interactable = false;
			canvasGroup.blocksRaycasts = false;

			return label;
		}
	}

}

