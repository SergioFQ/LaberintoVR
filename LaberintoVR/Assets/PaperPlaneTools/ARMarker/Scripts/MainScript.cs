namespace PaperPlaneTools.AR {
	using OpenCvSharp;

	using UnityEngine;
	using System.Collections;
	using System.Runtime.InteropServices;
	using System;
	using System.Collections.Generic;
	using UnityEngine.UI;
	
	public class MainScript: WebCamera {
        //CODIGO SERGIO
        //public GameObject posObject;
        public GameObject cameraPlayer;
        private GameObject objeto;
        public float dist;//cuanto queremos que se acerque el objeto (no excederse con los valores)
        //CODIGO SERGIO
		[Serializable]
		public class MarkerObject
		{
			public int markerId;
			public GameObject markerPrefab;
		}

		public class MarkerOnScene
		{
			public int bestMatchIndex = -1;
			public float destroyAt = -1f;
			public GameObject gameObject = null;
		}

		/// <summary>
		/// List of possible markers
		/// The list is set in Unity Inspector
		/// </summary>
		public List<MarkerObject> markers;

		/// <summary>
		/// The marker detector
		/// </summary>
		private MarkerDetector markerDetector;


		/// <summary>
		/// Objects on scene
		/// </summary>
		private Dictionary<int, List<MarkerOnScene>> gameObjects = new Dictionary<int, List<MarkerOnScene>>();

		void Start () {
			markerDetector = new MarkerDetector ();

			foreach (MarkerObject markerObject in markers) {
				gameObjects.Add(markerObject.markerId, new List<MarkerOnScene>());
			}

		}


		protected override void Awake() {
			int cameraIndex = -1;
			for (int i = 0; i < WebCamTexture.devices.Length; i++) {
				WebCamDevice webCamDevice = WebCamTexture.devices [i];
				if (webCamDevice.isFrontFacing == false) {
					cameraIndex = i;
					break;
				}
				if (cameraIndex < 0) {
					cameraIndex = i;
				}
			}

			if (cameraIndex >= 0) {
				DeviceName = WebCamTexture.devices [cameraIndex].name;
				//webCamDevice = WebCamTexture.devices [cameraIndex];
			}
		}

        protected override bool ProcessTexture(WebCamTexture input, ref Texture2D output)
        {
            var texture = new Texture2D(input.width, input.height);
            texture.SetPixels(input.GetPixels());
            var img = Unity.TextureToMat(texture, Unity.TextureConversionParams.Default);
            ProcessFrame(img, img.Cols, img.Rows);
            output = Unity.MatToTexture(img, output);
            Destroy(texture);
            return true;
        }

        private void ProcessFrame (Mat mat, int width, int height) {
			List<int> markerIds = markerDetector.Detect (mat, width, height);

			int count = 0;
			foreach (MarkerObject markerObject in markers) {
				List<int> foundedMarkers = new List<int>();
				for (int i=0; i<markerIds.Count; i++) {
					if (markerIds[i] == markerObject.markerId) {
						foundedMarkers.Add(i);
						count++;
					}
				}

				ProcessMarkesWithSameId(markerObject, gameObjects[markerObject.markerId], foundedMarkers);
			}
		}

		private void ProcessMarkesWithSameId(MarkerObject markerObject, List<MarkerOnScene> gameObjects, List<int> foundedMarkers) {
			int index = 0;
		
			index = gameObjects.Count - 1;
			while (index >= 0) {
				MarkerOnScene markerOnScene = gameObjects[index];
				markerOnScene.bestMatchIndex = -1;
				if (markerOnScene.destroyAt > 0 && markerOnScene.destroyAt < Time.fixedTime) {
					Destroy(markerOnScene.gameObject);
					gameObjects.RemoveAt(index);
				}
				--index;
			}

			index = foundedMarkers.Count - 1;

			// Match markers with existing gameObjects
			while (index >= 0) {
				int markerIndex = foundedMarkers[index];
				Matrix4x4 transforMatrix = markerDetector.TransfromMatrixForIndex(markerIndex);
				Vector3 position = MatrixHelper.GetPosition(transforMatrix);

				float minDistance = float.MaxValue;
				int bestMatch = -1;
				for (int i=0; i<gameObjects.Count; i++) {
					MarkerOnScene markerOnScene = gameObjects [i];
					if (markerOnScene.bestMatchIndex >= 0) {
						continue;
					}
					float distance = Vector3.Distance(markerOnScene.gameObject.transform.position, position);
					if (distance<minDistance) {
						bestMatch = i;
					}
				}

				if (bestMatch >=0) {
					gameObjects[bestMatch].bestMatchIndex = markerIndex;
					foundedMarkers.RemoveAt(index);
				} 
				--index;
			}

			//Destroy excessive objects
			index = gameObjects.Count - 1;
			while (index >= 0) {
				MarkerOnScene markerOnScene = gameObjects[index];
				if (markerOnScene.bestMatchIndex < 0) {
					if (markerOnScene.destroyAt < 0) {
						markerOnScene.destroyAt = Time.fixedTime + 0.2f;
					}
				} else {
					markerOnScene.destroyAt = -1f;
					int markerIndex = markerOnScene.bestMatchIndex;
					Matrix4x4 transforMatrix = markerDetector.TransfromMatrixForIndex(markerIndex);
					PositionObject(markerOnScene.gameObject, transforMatrix);
				}
				index--;
			}

			//Create objects for markers not matched with any game object
			foreach (int markerIndex in foundedMarkers) {                
				objeto = Instantiate(markerObject.markerPrefab);
				objeto.transform.parent = cameraPlayer.transform;
				objeto.transform.position = Vector3.zero;
				MarkerOnScene markerOnScene = new MarkerOnScene() {
					gameObject = objeto
				};
				gameObjects.Add(markerOnScene);

				Matrix4x4 transforMatrix = markerDetector.TransfromMatrixForIndex(markerIndex);
				PositionObject(markerOnScene.gameObject, transforMatrix);
			}
		}

		private void PositionObject(GameObject gameObject, Matrix4x4 transformMatrix) {
			Matrix4x4 matrixY = Matrix4x4.TRS (Vector3.zero, Quaternion.identity, new Vector3 (1, -1, 1));
			Matrix4x4 matrixZ = Matrix4x4.TRS (Vector3.zero, Quaternion.identity, new Vector3 (1, 1, -1));
			Matrix4x4 matrix = matrixY * transformMatrix * matrixZ;

            //gameObject.transform.localPosition = MatrixHelper.GetPosition (matrix);
            //gameObject.transform.localRotation = MatrixHelper.GetQuaternion (matrix);
            //gameObject.transform.localScale = MatrixHelper.GetScale (matrix);
            
            //Mita a camra
            gameObject.transform.LookAt(cameraPlayer.transform);
            //Rotamos para que este recto del todo
            //gameObject.transform.Rotate(-gameObject.transform.rotation.x + 90f, 0f, 0f);
            gameObject.transform.Rotate(-MatrixHelper.GetQuaternion(matrix).eulerAngles.x,MatrixHelper.GetQuaternion (matrix).eulerAngles.y,MatrixHelper.GetQuaternion (matrix).eulerAngles.z);
			gameObject.transform.Rotate(90f, 0f, 0f);
            //Tenemos en cuenta las coordenadas dadas en el trackeo y le sumamos la de un objeto invisible enfrente de la camara para que se mueva con la camara y no se quede en el origen
            //gameObject.transform.position = new Vector3((posObject.transform.position.x + MatrixHelper.GetPosition(matrix).x) - 0.055f, (posObject.transform.position.y + MatrixHelper.GetPosition(matrix).y) - 0.4f, posObject.transform.position.z); //+ MatrixHelper.GetPosition (matrix);
			gameObject.transform.localPosition = MatrixHelper.GetPosition (matrix);
            gameObject.transform.Translate(new Vector3 (0f,dist,0f));
		}

        public void destroyObject()
        {
            Destroy(objeto);

        }
	}
}
