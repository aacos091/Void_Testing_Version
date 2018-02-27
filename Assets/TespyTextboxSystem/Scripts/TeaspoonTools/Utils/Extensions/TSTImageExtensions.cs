using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace TeaspoonTools.Utils
{
    public static class TSTImageExtensions
    {
        /// <summary>
        /// Changes the opacity of the image to the percentage you pass it.
        /// </summary>
        public static void SetOpacity(this Image img, float opacity)
        {
            Color newCol = img.color;
            newCol.a = opacity / 100f;
            img.color = newCol;
        }
    }
}
