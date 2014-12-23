using UnityEngine;
using System.Collections;

public class ToggleAutoBuild : MonoBehaviour
{
		public GUITexture guiTexture;
		public Texture texture1;
		public Texture texture2;

		void Start ()
		{
				guiTexture.texture = texture1;
		}

		public void Toggle ()
		{
				if (guiTexture.texture == texture1) {
						guiTexture.texture = texture2;
				} else {
						guiTexture.texture = texture1;
				}
		}
}
