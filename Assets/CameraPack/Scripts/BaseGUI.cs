using UnityEngine;
using UnityEngine.UI;
using System.Collections;
//using UnityEditor;

// FOR DEMO
namespace CameraPack
{
	
	//uGUI staff
	[System.Serializable]
	public class UIContainer {
		public 	CanvasGroup			rTSSettingsPanel;
		public 	CanvasGroup			mTPSettingsPanel;
		public 	CanvasGroup			rPGSettingsPanel;

		public 	GameObject 			backbtn;
		public 	GameObject 			leftbtn;
		public 	GameObject 			rightbtn;
		public 	RTSUIElements 		rTSUIElements = new RTSUIElements();
		public 	RPGUIElements 		rPGUIElements = new RPGUIElements();
		public 	MTPUIElements 		mTPUIElements = new MTPUIElements();

		public void InitializeUI(CameraMove cm) {
			if(cm) {
				switch(cm.Mode)
				{
				case CameraMove.CameraModeNames.MTP_Camera:
					mTPUIElements.InitializeUI(cm);

					if(cm.CameraReachedPoint) {
						if(backbtn && !backbtn.activeSelf)
							backbtn.SetActive(true);
					} else {
						if(backbtn && backbtn.activeSelf)
							backbtn.SetActive(false);
					}
					break;
				case CameraMove.CameraModeNames.RPG_Camera:
					rPGUIElements.InitializeUI(cm);
					break;
				case CameraMove.CameraModeNames.RTS_Camera:
					rTSUIElements.InitializeUI(cm);
					break;
				}
			}
				
		}


		[System.Serializable]
		public class RTSUIElements {
			public Toggle InvertX;
			public Toggle InvertY;
			public Toggle useKeyBoardControl;
			public Toggle canTouchObjects;
			public Slider cameraSpeed;
			public Slider speedDampingSmoothness;
			public Slider keyCamSpeed;
			public Slider maxZoom;
			public Slider minZoom;
			public Slider zoomSensitivity;
			public Slider camRotInten;
			public Toggle useLockPerimeter;
			public Slider leftBorder;
			public Slider rightBorder;
			public Slider forwardBorder;
			public Slider backBorder;
			public Toggle useRollOutEffect;
			public Slider rollOutValue;
			public Slider rollOutValueSmoothness;
			public Toggle useMoveToObjectEffect;
			public Slider moveToObjectSmoothness;
			public Toggle useComboZoom;
			public Slider zoomRotAmount;
			public Slider FOVZoomMultyplier;
			public Toggle useOrbitRotation;
			public Toggle useAltAxesRotation;
			public Slider rotationSpeed;

			public void InitializeUI(CameraMove cm)
			{
				if(cm) {
					InvertX.isOn  				 = cm.inversionX;
					InvertY.isOn 				 = cm.inversionY;
					useKeyBoardControl.isOn  	 = cm.useKeyBoardControl;
					canTouchObjects.isOn     	 = cm.canTouchObjects;
					cameraSpeed.value			 = cm.cameraSpeed;
					speedDampingSmoothness.value = cm.speedDampingSmoothness;
					keyCamSpeed.maxValue		 = cm.keyCamSpeed;

					minZoom.maxValue			 = cm.maxZoom;
					minZoom.value 				 = cm.minZoom;
					maxZoom.minValue			 = cm.minZoom;
					maxZoom.value	  			 = cm.maxZoom;

					zoomSensitivity.value	   	 = cm.zoomSensitivity;
					camRotInten.value 			 = cm.camRotInten;

					useLockPerimeter.isOn  		 = cm.useLockPerimeter;
					leftBorder.maxValue 		 = cm.rightBorder;
					leftBorder.value 			 = cm.leftBorder;
					rightBorder.minValue	  	 = cm.leftBorder;
					rightBorder.value	  		 = cm.rightBorder;
					forwardBorder.minValue	  	 = cm.backBorder;
					forwardBorder.value     	 = cm.forwardBorder;
					backBorder.maxValue 		 = cm.forwardBorder;
					backBorder.value    		 = cm.backBorder;

					useRollOutEffect.isOn 		 = cm.useRollOutEffect;
					rollOutValue.value 			 = cm.rollOutValue;
					rollOutValueSmoothness.value = cm.rollOutValueSmoothness;
					useMoveToObjectEffect.isOn   = cm.useMoveToObjectEffect;
					moveToObjectSmoothness.value = cm.moveToObjectSmoothness;
					useComboZoom.isOn  			 = cm.useComboZoom;
					zoomRotAmount.value 		 = cm.zoomRotAmount;
					FOVZoomMultyplier.value 	 = cm.FOVZoomMultyplier;
					useOrbitRotation.isOn  		 = cm.useOrbitRotation;
					useAltAxesRotation.isOn  	 = cm.useAltAxesRotation;
					rotationSpeed.value 		 = cm.rotationSpeed;
				}
			}
		}
		[System.Serializable]
		public class RPGUIElements {
			public Toggle followMode;
			public Slider xSpeed;
			public Slider ySpeed;
			public Slider yMaxLimit;
			public Slider yMinLimit;
			public Slider zoomSensitivity;
			public Slider maxDistance;
			public Slider minDistance;
			public Slider Smoothness;
			public Toggle isCamOffset;
			public Slider effectCamOffsetx;
			public Slider effectCamOffsety;
			public Slider effectCamOffsetz;

			public void InitializeUI(CameraMove cm) {
				if(cm) {
					followMode.isOn  = cm.followMode;
					xSpeed.value	 = cm.xSpeed;
					ySpeed.value	 = cm.ySpeed;
					yMinLimit.maxValue = cm.yMaxLimit;
					yMinLimit.value	   = cm.yMinLimit;
					yMaxLimit.minValue = cm.yMinLimit;
					yMaxLimit.value	   = cm.yMaxLimit;
					zoomSensitivity.value = cm.zoomSensitivity;
					maxDistance.minValue  = cm.minDistance;
					maxDistance.value	  = cm.maxDistance;
					minDistance.value     = cm.minDistance;
					Smoothness.value = cm.Smoothness;
					isCamOffset.isOn = cm.isCamOffset;
					effectCamOffsetx.value = cm.vectCamOffset.x;
					effectCamOffsety.value = cm.vectCamOffset.y;
					effectCamOffsetz.value = cm.vectCamOffset.z;
				}
			}
		}

		[System.Serializable]
		public class MTPUIElements {
			public Slider cameraInOutSpeed;
			public Slider closeOffsetDistance;
			public Toggle useDefaultCamPoint;
			public Slider startCamPointx;
			public Slider startCamPointy;
			public Slider startCamPointz;
			public Toggle useMTPOrbitModeOnPOI;

			public void InitializeUI(CameraMove cm) {
				if(cm) {
					cameraInOutSpeed.value 	  = cm.cameraInOutSpeed;
					closeOffsetDistance.value = cm.closeOffsetDistance;
					useDefaultCamPoint.isOn   = cm.useDefaultCamPoint;
					startCamPointx.value 	  = cm.startCamPoint.x;
					startCamPointy.value 	  = cm.startCamPoint.y;
					startCamPointz.value 	  = cm.startCamPoint.z;
					useMTPOrbitModeOnPOI.isOn = cm.useMTPOrbitModeOnPOI;
				}
			}
		}
	}

	// For DEMO
	public class BaseGUI : MonoBehaviour {

		[SerializeField]
		protected 	UIContainer 		uiDemo = new UIContainer();
		[SerializeField]
		protected 	GameObject[]		camPrefab;
		[SerializeField]
		protected 	GameObject			mainCharacter;
		// other staff
		private 	GameObject 			cam;
		private 	CameraMove			camScript;
		private 	int 				cameraTypeIndex = 0;


		void Start () {
			camScript = Camera.main.GetComponent<CameraMove>();
			SetMode_RTS();
		}

		void Update() {
			// for exit
			if(Input.GetKey(KeyCode.Escape)) {
				Application.Quit();
			}

			if(camScript) {
				// update inspector chenging in the UI. Don't use this in the real project!
				uiDemo.InitializeUI(camScript);
			}
		}


		public void RestartLevel() {
			Application.LoadLevel ("Demo");
		}
		public void SetMode_RTS() {
			cameraTypeIndex = 0;
			SwitchSettingsPanel(cameraTypeIndex);
			if(camScript.useOrbitRotation)
				SwitchOnOffOrbitBtns(true);
			if(uiDemo.backbtn && uiDemo.backbtn.activeSelf)
				uiDemo.backbtn.SetActive(false);
			camScript.Mode = CameraMove.CameraModeNames.RTS_Camera;
		}
		public void SetMode_RPG() {
			cameraTypeIndex = 1;
			SwitchSettingsPanel(cameraTypeIndex);
			if(mainCharacter)
				camScript.SetTarget = mainCharacter.transform; // setup chracter
			// ui staff
			SwitchOnOffOrbitBtns(false);
			if(uiDemo.backbtn && uiDemo.backbtn.activeSelf)
				uiDemo.backbtn.SetActive(false);
			// switching camera mode
			camScript.Mode = CameraMove.CameraModeNames.RPG_Camera; 
		}
		public void SetMode_MTP() {
			cameraTypeIndex = 2;
			SwitchSettingsPanel(cameraTypeIndex);
			SwitchOnOffOrbitBtns(false);
			camScript.Mode = CameraMove.CameraModeNames.MTP_Camera; 
		}

		/** PUBLIC RTS METHODS  	*/
		public void RTS_SpawnPreset(int value) {
			if(camScript) {
				cam = camScript.gameObject;
				Destroy(cam);
			}
			switch(value) {
			case 0:
				cam = Instantiate(camPrefab[0]) as GameObject;
				break;
			case 1:
				cam = Instantiate(camPrefab[1]) as GameObject;
				break;
			case 2:
				cam = Instantiate(camPrefab[2]) as GameObject;
				break;
			}
			cam.name = "MainCamera " + value.ToString();
			camScript = cam.GetComponent<CameraMove>();
			cameraTypeIndex = 0;
		}
		public void RTS_InvertX() {
			//camScript.inversionX = !camScript.inversionX;
			camScript.inversionX = uiDemo.rTSUIElements.InvertX.isOn;
		}
		public void RTS_InvertY() {
			//camScript.inversionY = !camScript.inversionY;
			camScript.inversionX = uiDemo.rTSUIElements.InvertY.isOn;
		}
		public void RTS_UseKeyboard() {
			//camScript.useKeyBoardControl = !camScript.useKeyBoardControl;
			camScript.useKeyBoardControl = uiDemo.rTSUIElements.useKeyBoardControl.isOn;
		}
		public void RTS_CanTouchObjects() {
			//camScript.canTouchObjects = !camScript.canTouchObjects;
			camScript.canTouchObjects = uiDemo.rTSUIElements.canTouchObjects.isOn;
		}
		public void RTS_CameraSpeed(float value) {
			camScript.cameraSpeed = value;
		}
		public void RTS_SpeedDampingSmoothness(float value) {
			camScript.speedDampingSmoothness = value;
		}
		public void RTS_KeyBoardCameraSpeed(float value) {
			camScript.keyCamSpeed = value;
		}
		public void RTS_MaxZoom(float value) {
			camScript.maxZoom = value;
		}
		public void RTS_MinZoom(float value) {
			camScript.minZoom = value;
		}
		public void RTS_ZoomSensitivit(float value) {
			camScript.zoomSensitivity = value;
		}
		public void RTS_CameraTiltStrength(float value) {
			camScript.camRotInten = value;
		}
		public void RTS_UseLockPerimeter() {
			camScript.useLockPerimeter = uiDemo.rTSUIElements.useLockPerimeter.isOn;
			//camScript.useLockPerimeter = !camScript.useLockPerimeter;
		}
		public void RTS_LeftBorder(float value) {
			camScript.leftBorder = value;
		}
		public void RTS_RightBorder(float value) {
			camScript.rightBorder = value;
		}
		public void RTS_ForwardBorder(float value) {
			camScript.forwardBorder = value;
		}
		public void RTS_BackBorder(float value) {
			camScript.backBorder = value;
		}
		public void RTS_UseRollOutEffect() {
			//camScript.useRollOutEffect = !camScript.useRollOutEffect;
			camScript.useRollOutEffect = uiDemo.rTSUIElements.useRollOutEffect.isOn;
		}
		public void RTS_RollOutValue(float value) {
			camScript.rollOutValue = value;
		}
		public void RTS_RollOutSmoothness(float value) {
			camScript.rollOutValueSmoothness = value;
		}
		public void RTS_UseMoveToObject() {
			//camScript.useMoveToObjectEffect = !camScript.useMoveToObjectEffect;
			camScript.useMoveToObjectEffect = uiDemo.rTSUIElements.useMoveToObjectEffect.isOn;
		}
		public void RTS_MoveToObjectSmoothness(float value) {
			camScript.moveToObjectSmoothness = value;
		}
		public void RTS_UseComboZoom() {
			camScript.useComboZoom = uiDemo.rTSUIElements.useComboZoom.isOn;
			//camScript.useComboZoom = !camScript.useComboZoom;
		}
		public void RTS_ComboZoomRotAmount(float value) {
			camScript.zoomRotAmount = value;
		}
		public void RTS_ComboZoomFov(float value) {
			camScript.FOVZoomMultyplier = value;
		}
		public void RTS_RotatioPOI() {
			//camScript.useOrbitRotation = !camScript.useOrbitRotation;
			camScript.useOrbitRotation = uiDemo.rTSUIElements.useOrbitRotation.isOn;
			if(camScript.useOrbitRotation) {
				SwitchOnOffOrbitBtns(true);
			} else {
				SwitchOnOffOrbitBtns(false);
			}
		}
		public void RTS_UseAltAxesRotation() {
			//camScript.useOrbitRotation = !camScript.useOrbitRotation;
			camScript.useAltAxesRotation = uiDemo.rTSUIElements.useAltAxesRotation.isOn;
		}
		public void RTS_POIRotationSpeed(float value) {
			camScript.rotationSpeed = value;
		}

		public void RTS_CornerRotationDownEvent(int value) {
			camScript.CornerRotationDownEvent(value);
		}
		public void RTS_CornerRotationUpEvent() {
			camScript.CornerRotationUpEvent();
		}

		/** PUBLIC MTP METHODS */
		public void MTP_OrbitMode() {
			//camScript.useMTPOrbitModeOnPOI = !camScript.useMTPOrbitModeOnPOI;
			camScript.useMTPOrbitModeOnPOI = uiDemo.mTPUIElements.useMTPOrbitModeOnPOI.isOn;
		}
		public void MTP_CameraInOutSpeed(float value) {
			camScript.cameraInOutSpeed = value;
		}
		public void MTP_OffsetDistance(float value) {
			camScript.closeOffsetDistance = value;
		}
		public void MTP_UseDefaultCamPoint() {
			//camScript.useDefaultCamPoint = !camScript.useDefaultCamPoint;
			camScript.useDefaultCamPoint = uiDemo.mTPUIElements.useDefaultCamPoint.isOn;
		}
		public void MTP_StartCamPointX(float value) {
			camScript.startCamPoint.x = value;
		}
		public void MTP_StartCamPointY(float value) {
			camScript.startCamPoint.y = value;
		}
		public void MTP_StartCamPointZ(float value) {
			camScript.startCamPoint.z = value;
		}
		public void MTP_BackToBasePoint() {
			camScript.BackToBasePoint_MTP();
			//camScript.MtpToPointMove = false;
		}

		/** PUBLIC RPG METHODS */
		public void RPG_FollowMode() {
			//camScript.followMode = !camScript.followMode;
			camScript.followMode = uiDemo.rPGUIElements.followMode.isOn;
		}
		public void RPG_xSpeed(float value) {
			camScript.xSpeed = value;
		}
		public void RPG_ySpeed(float value) {
			camScript.ySpeed = value;
		}
		public void RPG_yMaxLimit(float value) {
			camScript.yMaxLimit = value;
		}
		public void RPG_yMinLimit(float value) {
			camScript.yMinLimit = value;
		}
		public void RPG_ZoomSensitivity(float value) {
			camScript.zoomSensitivity = value;
		}
		public void RPG_MaxDistance(float value) {
			camScript.maxDistance = value;
		}
		public void RPG_MinDistance(float value) {
			camScript.minDistance = value;
		}
		public void RPG_Smoothness(float value) {
			camScript.Smoothness = value;
		}
		public void RPG_CamOffset() {
			//camScript.isCamOffset = !camScript.isCamOffset;
			camScript.isCamOffset = uiDemo.rPGUIElements.isCamOffset.isOn;
		}
		public void RPG_vectCamOffsetX(float value) {
			camScript.vectCamOffset.x = value;
		}
		public void RPG_vectCamOffsetY(float value) {
			camScript.vectCamOffset.y = value;
		}
		public void RPG_vectCamOffsetZ(float value) {
			camScript.vectCamOffset.z = value;
		}


		/** */
		private void SwitchOnOffOrbitBtns(bool active)
		{
			if(uiDemo.leftbtn)
				uiDemo.leftbtn.SetActive(active);
			if(uiDemo.rightbtn)
				uiDemo.rightbtn.SetActive(active);
		}

		/** */
		private void SwitchCanvasGroup(CanvasGroup cg, bool hide)
		{
			if(cg) {
				if(hide) {
					cg.alpha = 0.0f;
					cg.blocksRaycasts = false;
					cg.interactable   = false;
				} else { 
					cg.alpha = 1.0f;
					cg.blocksRaycasts = true;
					cg.interactable   = true;
				}
			}
		}

		/** */
		private void SwitchSettingsPanel(int index)
		{
			switch(index) {
			case 0:
				SwitchCanvasGroup(uiDemo.rTSSettingsPanel, false);
				SwitchCanvasGroup(uiDemo.rPGSettingsPanel, true);
				SwitchCanvasGroup(uiDemo.mTPSettingsPanel, true);
				break;
			case 1:
				SwitchCanvasGroup(uiDemo.rTSSettingsPanel, true);
				SwitchCanvasGroup(uiDemo.rPGSettingsPanel, false);
				SwitchCanvasGroup(uiDemo.mTPSettingsPanel, true);
				break;
			case 2:
				SwitchCanvasGroup(uiDemo.rTSSettingsPanel, true);
				SwitchCanvasGroup(uiDemo.rPGSettingsPanel, true);
				SwitchCanvasGroup(uiDemo.mTPSettingsPanel, false);
				break;
			}
		}

		/** */
		public void ShowHidePanel () {
			switch(cameraTypeIndex) {
			case 0:
				SwitchCanvasGroup(uiDemo.rTSSettingsPanel, (uiDemo.rTSSettingsPanel.interactable) ? true : false);
				break;
			case 1:
				SwitchCanvasGroup(uiDemo.rPGSettingsPanel, (uiDemo.rPGSettingsPanel.interactable) ? true : false);
				break;
			case 2:
				SwitchCanvasGroup(uiDemo.mTPSettingsPanel, (uiDemo.mTPSettingsPanel.interactable) ? true : false);
				break;
			}
		}

		/** */
		void OnGUI()	{		}
	}
}