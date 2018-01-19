using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;


namespace TeaspoonTools.TextboxSystem.Utils
{

    [System.Serializable]
    public class TextSettings
    {

        public AudioClip audioSample = null;
        public TextSpeed textSpeed = TextSpeed.medium;

        [HideInInspector]
        public TextSpeed higherTextSpeed;
        [HideInInspector]
        public TextSpeed effectiveTextSpeed = TextSpeed.medium;

        [Range(1, 1000)]
        public int linesPerTextbox = 3;
        public string textToDisplay { get { return textboxController.textToDisplay; } }
        public Font font;
        public int fontSize = 12;
        public bool autoFontSize = true;

		public TextboxController textboxController { get; set; } 
		TextboxText textboxText { get { return textboxController.text; } }

		AutoFontScaler fontScaler = new AutoFontScaler();

		/// <summary>
		/// Copy constructor.
		/// </summary>
		/// <param name="toCopy">To copy.</param>
		public TextSettings(TextSettings toCopy)
		{
			this.textboxController = toCopy.textboxController;
			this.audioSample = toCopy.audioSample;
			this.textSpeed = toCopy.textSpeed;
			SetHigherSpeed ();
			this.linesPerTextbox = toCopy.linesPerTextbox;
			this.font = toCopy.font;

			if (font == null)
				LoadDefaultFont ();

			this.fontSize = toCopy.fontSize;
		}

        public void Initialize(TextboxController tbText)
        {
            this.textboxController = tbText;

			if (font == null)
				LoadDefaultFont ();

            SetHigherSpeed();

			if (fontScaler == null)
				fontScaler = new AutoFontScaler ();
			
			if (autoFontSize)
				SetAutoFontSize ();

        }
			
        public void Initialize(TextboxController textboxController,
			TextSpeed textSpeed, int linesPerTextbox)
        {
			this.textboxController = textboxController;

			if (font == null)
				LoadDefaultFont ();

            this.linesPerTextbox = linesPerTextbox;
            this.textSpeed = textSpeed;

			if (fontScaler == null)
				fontScaler = new AutoFontScaler ();
			
			if (autoFontSize)
				SetAutoFontSize ();
			
            SetHigherSpeed();

        }

        public void SetHigherSpeed()
        {
            switch (textSpeed)
            {
                case TextSpeed.verySlow:
                    higherTextSpeed = TextSpeed.slow;
                    break;
                case TextSpeed.slow:
                    higherTextSpeed = TextSpeed.medium;
                    break;
                case TextSpeed.medium:
                    higherTextSpeed = TextSpeed.fast;
                    break;
                case TextSpeed.fast:
                    higherTextSpeed = TextSpeed.instant;
                    break;
                case TextSpeed.instant:
                    higherTextSpeed = TextSpeed.instant;
                    break;

                default:
                    throw new System.NotImplementedException("Text speed not accounted for in text settings.");

            }

            //Debug.Log("Higher speed is:" + higherTextSpeed);

        }
        
        /// <summary>
        /// Automatically calculates the font size, so that it's just as big (or small) as it needs to be to fill the 
		/// textbox with a given number of lines at a time.
        /// </summary>
        public void SetAutoFontSize()
        {
            if (fontScaler == null)
                fontScaler = new AutoFontScaler();

			fontSize = fontScaler.ScaleFontSizeToText (textboxText, font, linesPerTextbox);
      
        }
        
		void LoadDefaultFont()
		{
			Debug.Log("Font was null! Loading default!");
			font = Resources.GetBuiltinResource<Font>("Arial.ttf");
		}

	}

}