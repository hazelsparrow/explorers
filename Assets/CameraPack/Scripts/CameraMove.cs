// // // //  ** ** ** ** ** ** ** // // // //  ** ** ** ** ** ** ** // // // // ** ** ** ** ** ** *
// * Copyright 2017  All Rights Reserved.
// *
// * Please direct any bugs or questions to vadakuma@gmail.com
// * version 1.3.0
// * author vadakuma 
// // // //  ** ** ** ** ** ** ** // // // //  ** ** ** ** ** ** ** // // // // ** ** ** ** ** ** **
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System;

namespace CameraPack
{
	[RequireComponent(typeof(Camera))]
	public class CameraMove : MonoBehaviour {
		
		public enum CameraModeNames {
			None,
			RTS_Camera,		// real time strategy
			RPG_Camera,		// role play game
			MTP_Camera		// move to point
		};
		// current camera mode
		[Tooltip("Set current camera mode")]
		private		CameraModeNames 	mode = CameraModeNames.RTS_Camera;
		// ref on Camera.main
		private 	Camera 				mainCamera;
		
		/** 
		 * RPG CAMERA
		*/
		#region RPG Public properties
		[Tooltip("The camera following of the object")]
		public		bool 				followMode;
		[Tooltip("The camera rotates around that object")]
		public		Transform 			target;
		[Range (0.0f,120f),Tooltip("Camera speed on X axis")]
		public		float 				xSpeed = 90.0f;
		[Range (0.0f,180f),Tooltip("Camera speed on Yaxis")]
		public		float 				ySpeed = 30.0f;
		[Tooltip("Camera limits on Y axis")]
		public 		float				yMaxLimit = 80f;
		[Tooltip("Camera limits on Y axis")]
		public 		float 				yMinLimit = 0.0f;
		[Tooltip("Max zoom distance")]
		public		float 				maxDistance = 20.0f;
		[Tooltip("Min zoom distance")]
		public		float 				minDistance = 10.0f;
		[Tooltip("Use Stable offset for camera position")]
		public 		bool 				isCamOffset = false;
		[Tooltip("Stable offset for camera position")]
		public 		Vector3				vectCamOffset = Vector3.zero;
		[Range (0.0f,1f),Tooltip("Orbit rotation smoothness")]
		public 		float 				rotationSmoothness = 0.1f;
		#endregion

		#region RPG Protected properties
		// for avoid fake touches
		private 	float 				touchThreshold      = 30.0f;
		private	 const float 			SPEED_CORRECTION 	= 0.02f;
		private	 const int 				MAX_ANGLE = 360;
		// last coordinates finger position
		private 	Vector3 			lastFingerPos  = Vector3.zero;
		private 	Vector3 			deltaFingerPos = Vector3.zero; //lastFingerPos - newFingerPos
		// parameters for tracking finger passed by.
		private 	bool 				isResetFingerMoveDist    = false;
		private 	float 				resetFingerMoveDistTimer = 0.2f;
		private 	float 				resetFingerMoveDistTime  = 0.2f;
		//Camera rotation amount, depends on the displacement
		private 	float 				xDelta = 0.0f;
		private 	float 				yDelta = 0.0f;
		// Current camera offset (depend on vectCamOffset)
		private 	Vector3				currVectCamOffset = Vector3.zero;
		// new camera position on next tick 
		private 	Vector3				rpgCamPos = Vector3.zero;
		// for keyboard events
		private 	bool 				KeyCodeW = false;
		private 	bool 				KeyCodeA = false;
		private 	bool 				KeyCodeS = false;
		private 	bool 				KeyCodeD = false;
		private 	bool 				KeyCodeQ = false;
		private 	bool 				KeyCodeE = false;
		// the distance to the object 
		private		float 				Distance = 2.0f;
		// new distance to the object
		private		float 				newDist = 0.0f;	
		/** path dist finger */
		private 	float 				fingerPathDist = 0.0f;
		#endregion

		/** 
		 * RTS CAMERA 
		 */
		#region RTS Public properties
		[Tooltip("Invert moving side")]
		public  	bool 				inversionX = true;
		[Tooltip("Invert moving side")]
		public  	bool 				inversionY = true;
		[Tooltip("WASD for moving and QE for zooming")]
		public  	bool 				useKeyBoardControl = true;
		[Tooltip("Activate touch event methods by touching special object with tag Touch.")]
		public  	bool 				canTouchObjects = true;
		[SerializeField] [Tooltip("When pointer near the edge of screen camera is moving")]
		public  	bool 				mouseScreenEdgeMovement = false;
		[SerializeField] [Tooltip("Orbit camera rotation around click point")]
		public 		bool 				useOrbitRotation = false;
		[Tooltip("Use Alt Axes Rotation")]
		public  	bool 				useAltAxesRotation = false;
		[SerializeField, Tooltip("Corner rotation speed")]
		public 		float 				rotationSpeed = 1.0f;
		[SerializeField] [Tooltip("User can move the camera by default way")]
		public  	bool 				canMoveByClick = true;
		[SerializeField] [Range (0.0f,10000), Tooltip("moveBorderWidth")]
		public  	float 				moveBorderWidth = 100;
		[SerializeField] [Tooltip("Set Camera Speed")]
		public 		float 				cameraSpeed = 1.0f;
		[Tooltip("use camera speed correction depends on camera altitude")]
		public 		bool 				useCamSpdAltCorr = false;
		[SerializeField] [Range (0.0f,1.0f),Tooltip("how fast camera is stopped")]
		public 		float 				speedDampingSmoothness = 0.35f;
		[Tooltip("Use lock perimeter for camera moving")]
		public 		bool 				useLockPerimeter    = true;
		[Tooltip("Lock perimeter for camera moving to the left side")]
		public 		float 				leftBorder    = -100f;
		[Tooltip("Lock perimeter for camera moving")]
		public 		float 				rightBorder   = 100f;
		[Tooltip("Lock perimeter for camera moving")]
		public 		float 				forwardBorder = 100f;
		[Tooltip("Lock perimeter for camera moving")]
		public 		float 				backBorder    = -100f;
		[Tooltip("Camera can RollOut out of perimeter")]
		public 		bool 				useRollOutEffect = false;
		[Tooltip("How far camera can roll out outside from lock perimeter")]
		public 		float 				rollOutValue = 10.0f;
		[Tooltip("roll in back in lock repimeter smoothness")]
		public 		float 				rollOutValueSmoothness = 0.8f;
		[Tooltip("Camera can move to selected object")]
		public 		bool 				useMoveToObjectEffect = false;
		[Tooltip("How fast camera should move to selected object")]
		public 		float 				moveToObjectSmoothness = 0.8f;
		[Tooltip("Change FOV in zooming")]
		public 		bool 				useComboZoom    = true;
		[Tooltip("Camera zoom settings. No zoom if Zoom Sensitivity = 0")]
		public 		float 				maxZoom       = 65f;
		[Tooltip("Camera zoom settings. No zoom if Zoom Sensitivity = 0")]
		public 		float 				minZoom       = 25f;
		[Range (0.0f,10f), Tooltip("Camera lateral inclination strength. 0 - without lateral inclinations")]
		public 		float 				camRotInten       = 0.1f;
		[Tooltip("For RTS and RPG. No zoom if Zoom Sensitivity = 0")]
		public		float 				zoomSensitivity   = 8f; 
		[Range (0.0f,70f), Tooltip("Camera tilt  in zooming")]
		public 		float 				zoomRotAmount     = 30.0f;
		[Range (0.0f,10f), Tooltip("changes FOV in zooming")]
		public 		float 				FOVZoomMultyplier = 0.0f;
		[Tooltip("Camera speed WASD moving")]
		public 		float 				keyCamSpeed = 10.0f;
		[Range (0.0f,1f),Tooltip("rotation smoothness")]
		public 		float 				Smoothness = 0.5f;
		#endregion

		#region RTS Protected properties
		private 	int[] 				moveBorderSide = new int[]{0/*left */,0/*right*/,0/*back*/,0/*forward*/};
		private	  readonly float 		DEFAULT_LERP_SMOOTHNESS = 0.1f;
		// for inversion status control
		private  	int 				axisX = 1;
		private  	int 				axisY = 1;
		// indicator - camera moving(true) or not(false)
		private 	bool 				startMove   	   = false;
		private 	bool 				leftMouseBtnDown   = false;
		//  coordinates of the click
		private 	Vector3 			firstClick = Vector3.zero;
		// camera damping
		private 	float 				cameraViscosity    = 0.0f;
		private 	float 				maxCameraViscosity = 1.0f;
		private 	float 				speedDamping 	   = 0.0f;
		// camera damping speed
		private		float 				cameraViscositySpeed = 0.1f;
		[Tooltip("Camera speed correction depends on camera altitude")]
		private 	float 				cameraSpeedAltitudeCorrector = 1.0f;
		//Camera displacement amount, depends on mouse or keyboard 
		private  	Vector3 			displAmount   = Vector3.zero;
		[Range (0.0f,1f)]
		private  	float 				displAmountSmoothness   = 0.25f;
		private 	Vector3 			lastMousePos 			= Vector3.zero;
		// for ComboZoom, change size forward inclination 
		private 	float 				zoomRotation = 0.0f;
		[Range (0.0f,1f)]
		private 	float 				zoomRotationSmoothness = 0.25f;
		//
		private   	float 				currRollOutValue = 0.0f;
		// object position where the camera move on
		private  	Vector3 			selectedObjectPosition   = Vector3.zero;
		// special trigger, when camera moving to selected object
		private 	bool 				movingToObject = false;
		// initial camera Pos and Rot on Start
		private 	Quaternion 			initialCamRot;
		private 	float 				initialCamFOV;
		private 	Vector3 			initialCamLoc = Vector3.zero;
		private 	float 				deltaSpeedX = 0.0f;
		private 	float 				deltaSpeedY = 0.0f;
		private 	float 				deltaSpeedZ = 0.0f;
		// deltaAltitude adding to camAltitude
		private 	float 				deltaAltitude = 0.0f;
		private 	float 				maxDeltaAltitude = 10.0f;
		// add for Camera altitude
		private 	float 				altitudeControl = 0.0f;
		// for zoom
		private 	float 				oldTouchDeltaDist;
		//
		private 	Vector3 			lastPosition = Vector3.zero;
		// for corner rotation
		private 	Vector3 			lockPosition 		= Vector3.zero;
		private 	Vector3 			lockEulerRotation 	= Vector3.zero;
		private 	float 				cornerRotationSpeed = 0.0f;
		[Range (0.0f,1f)]
		private 	float 				cornerRotationSmoothness = 0.1f;
		private 	float 				lockDistance 		= 0.0f;

		private		float 				amountX = 1.0f;	// for edge moving speed multipliers by X axe
		private		float 				amountY = 1.0f;	// for edge moving speed multipliers by Y axe
		#endregion

		/** 
		 * MTP CAMERA 
		 */
		#region MTP Public properties
		[Tooltip("Point the camera from being start")]
		public 		Vector3 			startCamPoint = new Vector3(0,0,0);
		[Tooltip("Use own start camera point")]
		public		bool 				useDefaultCamPoint = false;
		[Range (0.0f,1f),Tooltip("Camera smoothing")]
		public		float 				cameraInOutSpeed = 0.02f;
		[Range (0.0f,100f),Tooltip("Offset Distance")]
		public		float 				closeOffsetDistance = 10.0f;
		[Tooltip("Activate rotation and zoom from rpg camera mode at the selected point. Settings get from RPG Camera.")]
		public		bool 				useMTPOrbitModeOnPOI = false;
		#endregion

		#region MTP Protected properties
		// to move to new point, or from the
		private		bool 				toPoint 	= false;
		private		bool 				cameraReachedPoint = true;
		// move to this point 
		private		Vector3 			thisPointLoc = Vector3.zero;
		private		Vector3 			realPointLoc = Vector3.zero;
		// camera rotation to this Quaternion
		//private 	Quaternion 			thisPointRot;
		// camera's FOV in new point
		private		float 				pointFOV = 60.0f;
		private		float 				currFOV  = 60.0f;
		// distance from initial pos to new pos
		private		float 				maxDistanceToPoint = 0.0f;
		private		float				maxDistanceDivider = 10.0f; // we want get event earlier then almost reach maxDistanceToPoint 
		private		Transform			lastHitObj;

		// for camera modes control
		protected 	delegate void 		CameraMode();
		protected 	CameraMode 			ActiveCameraMode = new CameraMode(EmptyMode);
		protected 	CameraMode 			MTPCameraMode 	 = new CameraMode(EmptyMode);
		#endregion


		// Use this for initialization
		void Start()
		{
			if(Camera.main) {
				mainCamera = Camera.main;
			} else {
				Debug.LogWarning("Start error. No Camera on this object. Try to create a Camera on this object!");
				mainCamera = CreateCamera();
			}

			if(mainCamera != null) {
				// All good. Set initial params
				initialCamRot = mainCamera.transform.rotation;
				initialCamFOV = mainCamera.fieldOfView;
				initialCamLoc = mainCamera.transform.position;
				if(!useDefaultCamPoint)
					startCamPoint = initialCamLoc;
				// set default camera
				Mode = mode;

				// warching on axis inversion for rts camera
				InversionX = inversionX;
				InversionY = inversionY;
			} else {
				Debug.LogWarning("Start error. Set this script on mainCamera!");
				Mode = CameraModeNames.None;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="CameraPack.CameraMove"/> cam on new position.
		/// </summary>
		/// <value><c>true</c> if cam on new position; otherwise, <c>false</c>.</value>
		public bool CameraReachedPoint {
			get { return cameraReachedPoint; }
			//set { cameraReachedPoint = value; }
		}

		/// <summary>
		/// @Depricated: use CameraReachedPoint.
		/// Gets or sets a value indicating whether this <see cref="CameraPack.CameraMove"/> cam on new position.
		/// </summary>
		/// <value><c>true</c> if cam on new position; otherwise, <c>false</c>.</value>
		public bool CamOnNewPos {
			get { return cameraReachedPoint; }
			//set { cameraReachedPoint = value; }
		}

		/// <summary>
		/// @Depricated: use CameraReachedPoint.
		/// Gets or sets a value indicating whether this <see cref="CameraPack.CameraMove"/> send camera back.
		/// </summary>
		/// <value><c>true</c> if send camera back; otherwise, <c>false</c>.</value>
		public bool SendCameraBack {
			get; set;
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="CameraPack.CameraMove"/> mtp to point move.
		/// </summary>
		/// <value><c>true</c> if camera move to point ; otherwise, <c>false</c>.</value>
		public bool MtpToPointMove {
			get { return toPoint; }
			set { 
				toPoint = value;
				if(toPoint) { // to selected point move
					MTPCameraMode = new CameraMode(MTP_ToPointState);
				} else { // to base point move
					MTPCameraMode = new CameraMode(MTP_ToBasePointState);
				}
			}
		}
			
		/** */
		protected bool InversionX {
			get { return inversionX; }
			set { 
				inversionX = value;
				if(inversionX)
					axisX = 1;
				else
					axisX = -1;
			}
		}

		/** */
		protected bool InversionY {
			get { return inversionY; }
			set { 
				inversionY = value;
				if(inversionY)
					axisY = -1;
				else
					axisY = 1;
			}
		}

		/// <summary>
		/// Gets or sets the set target.
		/// </summary>
		/// <value>Set target.</value>
		public Transform SetTarget {
			get { return target; }
			set { target = value;}
		}

		/// <summary>
		/// @DEPRICATED. Use Mode. Gets or sets the set camera mode.
		/// </summary>
		/// <value>The set mode.</value>
		public CameraModeNames SetMode {
			get { return mode; }
			set { Mode = value;}
		}

		/** Setting up and prepering camera modes*/
		public CameraModeNames Mode {
			get { return mode; }
			set { 
				mode = value;
				switch(mode)
				{
					//None
					case CameraModeNames.None:
						ActiveCameraMode = new CameraMode(EmptyMode);
					break;
					//RTS_Camera
					case CameraModeNames.RTS_Camera:
						ActiveCameraMode = new CameraMode(RTSCamera);
						// Initial params RTS_Camera
						if(mainCamera)
						{
							mainCamera.transform.position = initialCamLoc;
							mainCamera.transform.rotation = initialCamRot;
						}
					break;
					//RPG_Camera
					case CameraModeNames.RPG_Camera:
						if(target)
						{
							ActiveCameraMode = new CameraMode(RPGCamera);
							// Initial params RPG_Camera
							Distance = minDistance + maxDistance/4;
							newDist  = Distance;
							xDelta = transform.eulerAngles.y;
							yDelta = transform.eulerAngles.x;
						}
						else
						{
							Debug.LogWarning("Set Target object for using RPG Camera!");
							//Mode = CameraModeNames.None;
							ActiveCameraMode = new CameraMode(WaitMode);
						}
					break;
					//MTP_Camera
					case CameraModeNames.MTP_Camera:
						ActiveCameraMode = new CameraMode(MTPCamera);
						// Initial params MTP_Camera
						if(useDefaultCamPoint) {
							if(mainCamera)
								mainCamera.transform.position = startCamPoint;
						} else {
							if(!useDefaultCamPoint)
								startCamPoint = initialCamLoc;
							if(mainCamera)
								mainCamera.transform.position = startCamPoint;
						}
						if(mainCamera) {
							mainCamera.transform.rotation = initialCamRot;
							mainCamera.fieldOfView		  = currFOV;
						}
						cameraReachedPoint = false;
						// reset moving
						MtpToPointMove = false;
					break;
				}
			}
		}

		/** MTP Camera. Backing to the base point*/
		public void BackToBasePoint_MTP()
		{
			MtpToPointMove = false;
			lastHitObj 	   = null;
		}

		/** for ui button  down event(RTS mode)
		* rotDirection= -1 - to the left
		* rotDirection=  1 - to the right
		  */
		public void CornerRotationDownEvent(int rotDirection)
		{
			if(useOrbitRotation) {
				if(Mode == CameraModeNames.RTS_Camera) {
					deltaSpeedZ  = rotDirection;
					if(!useAltAxesRotation)
						lockPosition = RTSCornerRotationModePostion();
					else
						lockPosition = RTSCornerRotationModePostionAlt();
					lockDistance = Vector3.Distance(lockPosition , mainCamera.transform.position);
					lockEulerRotation = mainCamera.transform.rotation.eulerAngles;
				}
			}
		} 

		/** for ui button  up event. Orbit rotation off(RTS mode)  */
		public	void CornerRotationUpEvent()
		{
			if(useOrbitRotation) {
				if(Mode == CameraModeNames.RTS_Camera) {
					deltaSpeedZ  = 0;
					lastPosition = mainCamera.transform.position;
					movingToObject = false;
				}
			}
		}

		/** */
		void OnValidate()
		{
			// watching for the Mode param and reset if
			Mode 	   = mode;
			// warching on axis inversion for rts camera
			InversionX = inversionX;
			InversionY = inversionY;
		}

		/** Update is called once per frame */
		void Update () {
			//	Call current camera's mode, witch setup in SetMode  
			ActiveCameraMode();
		}

		/** */
		void OnGUI () {	}
		
	/**********************************************************************************************************************************************************************
	 *  CAMERA STATES
	 * *********************************************************************************************************************************************************************/

		/** null mode, no move */
		protected static void EmptyMode(){ }			

		/** when target is null, this mode is activated*/
		private void WaitMode ()
		{
			if(target != null) {
				Mode = CameraModeNames.RPG_Camera;
			}
		}
		/*******************************************************************************************
		 * RTSCamera STATE
		 * *****************************************************************************************/

		/** RTS_Camera  */
		private void RTSCamera()
		{
			InputMethod_RTS();	// for Mouse, WASD and Arrows control

			if(startMove)
			{
				RTSCameraMovingPrecalculate();
			}
			else // stoping camera
			{
				cameraViscosity  = Mathf.Lerp(cameraViscosity, 0.0f, cameraViscositySpeed);
				speedDamping 	 = Mathf.Lerp(speedDamping, 0.0f, speedDampingSmoothness);
				currRollOutValue = Mathf.Lerp(currRollOutValue, 0.0f, rollOutValueSmoothness);
				deltaSpeedX 	 = Mathf.Lerp(deltaSpeedX, 0.0f, DEFAULT_LERP_SMOOTHNESS);
				deltaSpeedY 	 = Mathf.Lerp(deltaSpeedY, 0.0f, DEFAULT_LERP_SMOOTHNESS);
			}

			// last calculated pos
			Vector3 newPos = Vector3.zero;
			if(useMoveToObjectEffect)
			{
				if(movingToObject)
					newPos = RTSMoveToObjectModePostion();
				else
					newPos = RTSDefaultModePostion();
			}
			else
			{
				newPos = RTSDefaultModePostion();
			}
			//camera altitude control
			RTSCameraPrepareAltitudeValue();

			// Calculate final POSITION
			mainCamera.transform.position = RTSCameraFinalPosition(newPos);

			// save last mouse|finger position
			lastMousePos = Input.mousePosition;

			// ZOOM calculate deltaAltitude for zoom effect
			RTSCameraZOOM();

			// FOV control
			mainCamera.fieldOfView = RTSCameraFieldOfView();

			// ROTATION control
			mainCamera.transform.rotation = RTSCameraRotation();
		}

		/** */
		private void RTSCameraMovingPrecalculate()
		{
			cameraViscosity  = Mathf.Lerp(cameraViscosity, maxCameraViscosity, cameraViscositySpeed);
			currRollOutValue = Mathf.Lerp(currRollOutValue, rollOutValue, rollOutValueSmoothness);
			if(Input.touchCount == 0 && leftMouseBtnDown)	// if mouse down
			{
				displAmount = Vector3.Lerp(displAmount, Input.mousePosition - lastMousePos, displAmountSmoothness);
			}
			else
			{
				if(Input.touchCount == 1)
					displAmount = Vector3.Lerp(displAmount, Input.GetTouch(0).deltaPosition, displAmountSmoothness);
			}
			
			// fake finger|mouse path, but little faster
			lastFingerPos.x += Mathf.Abs(displAmount.x);
			lastFingerPos.y += Mathf.Abs(displAmount.y);
			// smoothing turn the camera
			deltaSpeedX = Mathf.Lerp(deltaSpeedX, displAmount.x, DEFAULT_LERP_SMOOTHNESS);
			deltaSpeedY = Mathf.Lerp(deltaSpeedY, displAmount.y, DEFAULT_LERP_SMOOTHNESS);
			
			speedDamping = 1.0f;
			lastPosition = mainCamera.transform.position;
		}


		/**camera altitude control */
		private void RTSCameraPrepareAltitudeValue()
		{
			// camera altitude control
			float currcamAltitude = mainCamera.transform.position.y;
			// clamping delta altitude
			if(deltaAltitude > 0)
				deltaAltitude = Mathf.Clamp(deltaAltitude, 0, maxDeltaAltitude);
			if(deltaAltitude < 0)
				deltaAltitude = Mathf.Clamp(deltaAltitude, -maxDeltaAltitude, 0);
			// check camera altitude
			if( (currcamAltitude + deltaAltitude) < maxZoom &&
			   (currcamAltitude + deltaAltitude) > minZoom )
				altitudeControl = Mathf.Lerp(altitudeControl, deltaAltitude, DEFAULT_LERP_SMOOTHNESS);
			else // camera on edge maxZoom and moving to more when maxZoom 
				altitudeControl = Mathf.Lerp(altitudeControl, 0.0f, DEFAULT_LERP_SMOOTHNESS);
		}

		/** */
		private Vector3 RTSCameraFinalPosition(Vector3 oldPos)
		{
			// set offset
			Vector3		totalOffset = new Vector3( 0 , 2.0f * altitudeControl , -altitudeControl);
			totalOffset 			=  Quaternion.Euler(0, 0, 0) * totalOffset;
			
			// calculate new position
			Vector3 finalPos = (oldPos + totalOffset);
			// check camera altitude
			finalPos.y = Mathf.Clamp(finalPos.y, minZoom, maxZoom);
			
			if(useOrbitRotation)
			{
				if(deltaSpeedZ == 0)
				{
					cornerRotationSpeed = Mathf.Lerp(cornerRotationSpeed, 0.0f, cornerRotationSmoothness);
					return Vector3.Lerp(mainCamera.transform.position, finalPos, cornerRotationSmoothness);
				}
				else
				{
					lockEulerRotation.y = Mathf.Lerp(lockEulerRotation.y, lockEulerRotation.y - deltaSpeedZ * cornerRotationSpeed, Smoothness);
					Quaternion rotation = Quaternion.Euler(lockEulerRotation.x, lockEulerRotation.y, 0);
					totalOffset = new Vector3(0 , 0, -lockDistance);
					Vector3 position = lockPosition + rotation * totalOffset;
					
					finalPos = position;
					finalPos.y = mainCamera.transform.position.y;
					cornerRotationSpeed = Mathf.Lerp(cornerRotationSpeed, rotationSpeed, cornerRotationSmoothness);
					// apply new position
					return finalPos;
				}
			}
			else
			{
				return Vector3.Lerp(mainCamera.transform.position, finalPos, DEFAULT_LERP_SMOOTHNESS);
			}
		}

		/** calculate deltaAltitude for zoom effect*/
		private void RTSCameraZOOM()
		{
			if(Input.touchCount > 1 ) // for windows touch monitors/tablets. multytouch zoom. support in windows 7|vista|8|8.1|10
			{
				startMove 	 = false;
				Touch touch1 = Input.GetTouch(1);
				Touch touch0 = Input.GetTouch(0);
				float fingersDistance = Vector3.Distance(touch0.position, touch1.position);
				
				if(touch1.phase == TouchPhase.Began)
				{
					oldTouchDeltaDist = fingersDistance;
				}
				
				float touchDeltaDist = (oldTouchDeltaDist - fingersDistance);
				if(fingersDistance != oldTouchDeltaDist)
				{
					oldTouchDeltaDist = fingersDistance;
					deltaAltitude	  = Mathf.Lerp(deltaAltitude, zoomSensitivity * touchDeltaDist, DEFAULT_LERP_SMOOTHNESS);
				}
				else
				{
					deltaAltitude = Mathf.Lerp(deltaAltitude, 0.0f, DEFAULT_LERP_SMOOTHNESS);
				}
			}
			else
			{
				// pc zooming with Mouse ScrollWheel
				if(Input.GetAxis("Mouse ScrollWheel") != 0)
				{
					float _dist = Input.GetAxis("Mouse ScrollWheel");
					if(_dist < 0)
						deltaAltitude = zoomSensitivity;
					else
						deltaAltitude = -zoomSensitivity;
				}
				else
				{
					deltaAltitude = Mathf.Lerp(deltaAltitude, 0.0f, DEFAULT_LERP_SMOOTHNESS/2.0f);
				}
			}
		}

		/** */
		private float RTSCameraFieldOfView()
		{
			// changing cameras FOV depending on altitude
			if(useComboZoom) {
				float yPosCam = mainCamera.transform.position.y;
				zoomRotation = Mathf.Lerp(zoomRotation,(1 - yPosCam/( minZoom + (maxZoom - minZoom)/2)) * zoomRotAmount, zoomRotationSmoothness);
			} else {
				zoomRotation = Mathf.Lerp(zoomRotation,0, zoomRotationSmoothness);
			}
			return initialCamFOV + zoomRotation * FOVZoomMultyplier;
		}

		/** */
		private Quaternion RTSCameraRotation()
		{
			if(!useOrbitRotation || (useOrbitRotation && deltaSpeedZ == 0))
			{
				// tilt the camera while driving (free Y axis rotation)
				return Quaternion.Euler(initialCamRot.eulerAngles.x - deltaSpeedY * cameraViscosity * camRotInten - zoomRotation,
				                        mainCamera.transform.rotation.eulerAngles.y,
				                        initialCamRot.eulerAngles.z + deltaSpeedX * cameraViscosity * camRotInten);
			} else {
				return Quaternion.Euler(lockEulerRotation.x,
				                         lockEulerRotation.y,
				                         mainCamera.transform.rotation.eulerAngles.z);
			}
		}


		/** User controlling mode */
		private Vector3 RTSDefaultModePostion()
		{
			cameraSpeedAltitudeCorrector = SpeedAltitudeCorrector();

			if(startMove) {
				return RTSCameraActiveCheckLockPerimeter();
			} else	{
				return RTSCameraPassiveCheckLockPerimeter();
			}
		}

		/** If camera altitude > middle altitude return >1 multiplier
		 * If camera altitude < middle altitude return <1 multiplier
		 * + Multiplier correction depending on minZoom|maxZoom sing
		*/
		private float SpeedAltitudeCorrector()
		{
			if(minZoom > 0) {
				return (useCamSpdAltCorr) ? ( 2 * mainCamera.transform.position.y / (minZoom + maxZoom) ): 1.0f;
			} else if(minZoom == 0) {
				return (useCamSpdAltCorr) ? ( 2 * (mainCamera.transform.position.y + 1) / (minZoom + maxZoom + 2) ): 1.0f;
			} else  {
				return (useCamSpdAltCorr) ? ( 2 * (mainCamera.transform.position.y + Mathf.Abs(minZoom) + 1) / (minZoom + maxZoom + 2 * Mathf.Abs(minZoom) + 1) ): 1.0f;
			}
		}

		/** */
		private Vector3 RTSCameraPassiveCheckLockPerimeter()
		{
			Vector3 correctionPos = mainCamera.transform.position;
			// stop damping effect
			Vector3 dampDir = (lastPosition - mainCamera.transform.position).normalized * cameraSpeed * cameraSpeedAltitudeCorrector * speedDamping * Vector3.Distance(lastPosition, mainCamera.transform.position);
			correctionPos -= dampDir;

			// roll out effect when camera got edges of perimeter
			if(useLockPerimeter && useRollOutEffect)
			{
				if(mainCamera.transform.position.x > rightBorder)
					correctionPos.x = Mathf.Lerp(mainCamera.transform.position.x, rightBorder, rollOutValueSmoothness);
				if(mainCamera.transform.position.x < leftBorder)
					correctionPos.x = Mathf.Lerp(mainCamera.transform.position.x, leftBorder, rollOutValueSmoothness);
				if(mainCamera.transform.position.z < backBorder)
					correctionPos.z = Mathf.Lerp(mainCamera.transform.position.z, backBorder, rollOutValueSmoothness);
				if(mainCamera.transform.position.z > forwardBorder)
					correctionPos.z = Mathf.Lerp(mainCamera.transform.position.z, forwardBorder, rollOutValueSmoothness);
			}
			return correctionPos;
		}

		/** */
		private Vector3 RTSCameraActiveCheckLockPerimeter()
		{
			Vector3 correctionPos;

			if(useLockPerimeter)
			{
				float currLeftBoard    = Mathf.Clamp(leftBorder, float.MinValue, rightBorder);
				float currRightBoard   = Mathf.Clamp(rightBorder, leftBorder, float.MaxValue);
				float currBackBoard    = Mathf.Clamp(backBorder, float.MinValue, forwardBorder);
				float currForwardBoard = Mathf.Clamp(forwardBorder, backBorder, float.MaxValue);
				
				if(useRollOutEffect)
				{
					currLeftBoard    = leftBorder + ((leftBorder > 0) ? currRollOutValue : -currRollOutValue);
					currRightBoard   = rightBorder + ((rightBorder > 0)? currRollOutValue : -currRollOutValue);
					currForwardBoard = forwardBorder + ((forwardBorder > 0) ? currRollOutValue : -currRollOutValue);
					currBackBoard    = backBorder + ((backBorder > 0) ? currRollOutValue : -currRollOutValue);
				}
				correctionPos 	 = mainCamera.transform.forward * displAmount.y + mainCamera.transform.right * displAmount.x;
				correctionPos 	*= (cameraViscosity * cameraSpeed * cameraSpeedAltitudeCorrector);
				correctionPos   = new Vector3(Mathf.Clamp(mainCamera.transform.position.x + correctionPos.x * axisX, currLeftBoard, currRightBoard), // left right moves
				                       mainCamera.transform.position.y,
				                              Mathf.Clamp(mainCamera.transform.position.z + correctionPos.z * axisY, currBackBoard, currForwardBoard) // back and forward moves
				                       );
			} else {
				correctionPos = new Vector3(mainCamera.transform.position.x + displAmount.x * cameraViscosity * cameraSpeed * cameraSpeedAltitudeCorrector * axisX,
				                     mainCamera.transform.position.y,
				                            mainCamera.transform.position.z + displAmount.y * cameraViscosity * cameraSpeed * cameraSpeedAltitudeCorrector * axisY
				                     );

				// stop damping effect
				Vector3 dampDir = (lastPosition - mainCamera.transform.position).normalized * cameraSpeed * speedDamping * Vector3.Distance(lastPosition,mainCamera.transform.position);
				correctionPos -= dampDir;
				// roll out effect when camera got edges of perimeter
				if(useLockPerimeter && useRollOutEffect)
				{
					if(mainCamera.transform.position.x > rightBorder)
						correctionPos.x = Mathf.Lerp(mainCamera.transform.position.x, rightBorder, rollOutValueSmoothness);
					if(mainCamera.transform.position.x < leftBorder)
						correctionPos.x = Mathf.Lerp(mainCamera.transform.position.x, leftBorder, rollOutValueSmoothness);
					if(mainCamera.transform.position.z < backBorder)
						correctionPos.z = Mathf.Lerp(mainCamera.transform.position.z, backBorder, rollOutValueSmoothness);
					if(mainCamera.transform.position.z > forwardBorder)
						correctionPos.z = Mathf.Lerp(mainCamera.transform.position.z, forwardBorder, rollOutValueSmoothness);
				}
			}

			return correctionPos;
		}

		/** Same as MTP camera but in RTS camera. Auto moving to the point*/
		private Vector3 RTSMoveToObjectModePostion()
		{
			Quaternion 	rotation    = mainCamera.transform.rotation;
			Vector3		totalOffset = new Vector3(0.0f, 0.0f, -mainCamera.transform.position.y) + Vector3.zero * Distance;
			Vector3 	finalPos    = selectedObjectPosition + rotation * totalOffset;
			finalPos.y = mainCamera.transform.position.y;
			Vector3 	newPos		= Vector3.Lerp(mainCamera.transform.position, finalPos, moveToObjectSmoothness);

			if(Vector3.Distance(mainCamera.transform.position, finalPos) < 0.1f)
				movingToObject = false;

			return newPos;
		}

		/** */
		private Vector3 RTSCornerRotationModePostion()
		{
			Vector3 screenPoint = new Vector3(Screen.width/2, Screen.height/2, 1.0f); // center of screen
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay (screenPoint);
			if (Physics.Raycast (ray.origin, ray.direction * 100, out hit)) {
				target = hit.collider.gameObject.transform;
			}

			return hit.point;
		}

		/** */
		private Vector3 RTSCornerRotationModePostionAlt()
		{
			return mainCamera.transform.position;
		}


		/*******************************************************************************************
		 * RPG Camera STATE
		 * *****************************************************************************************/
	
		/**  
		 * camera rotation around the object (RPG_Camera)
		 */
		private void RPGCamera()
		{ 
			if(target == null) {
				Mode = CameraModeNames.RPG_Camera;
				return;
			} else {
				rpgCamPos = Vector3.Lerp(rpgCamPos, target.position, Smoothness);
			}
			if(Input.GetMouseButton(0))
			{
				if(!EventSystem.current.IsPointerOverGameObject())
				{
					Vector3 newDeltaFingerPos  = CheckTouchDistance(deltaFingerPos, Input.mousePosition, lastFingerPos, touchThreshold);
					deltaFingerPos = Vector3.Lerp(deltaFingerPos, newDeltaFingerPos, rotationSmoothness);

					//deltaFingerPos  = CheckTouchDistance(deltaFingerPos, Input.mousePosition, lastFingerPos, touchThreshold);
					fingerPathDist += Vector3.Distance(deltaFingerPos, Vector3.zero);
					isResetFingerMoveDist = false;
				}
			} else {
				deltaFingerPos = Vector3.Lerp(deltaFingerPos, Vector3.zero, DEFAULT_LERP_SMOOTHNESS);
				// we need dump FingerMoveDist, but with delay. This is for check total finger dist|length of a way
				//big length of a way - that touching to a 3d world point isn't activated. See CheckTouchDistance()
				//small length of a way - is considered that it is simple a click - touch the point will execute the action.
				// Without this we saw a very faster rotation camera if make many screen touches. This need only for "touch windows"
				// With mouse no problem 
				if(!isResetFingerMoveDist)
					ResetFingerMoveDist();
			}

			// FOR DEMO
			if(Input.GetMouseButtonUp(0))
			{
				CheckClickPoint_RTS(Input.mousePosition);
			}

			if(minDistance > maxDistance) // from Update
				minDistance = maxDistance;

			// multytouch zoom. support in windows 7|vista|8|8.1(thanks unity)
			newDist -= Input.GetAxis("Mouse ScrollWheel") * zoomSensitivity;
			newDist  = Mathf.Clamp(newDist, minDistance, maxDistance);
			Distance = Mathf.Lerp(Distance, newDist, DEFAULT_LERP_SMOOTHNESS);

			// add offset in camera position
			if(isCamOffset && Mode == CameraModeNames.RPG_Camera) {
				currVectCamOffset = Vector3.Lerp(currVectCamOffset, vectCamOffset, DEFAULT_LERP_SMOOTHNESS);
			} else {
				currVectCamOffset = Vector3.Lerp(currVectCamOffset, Vector3.zero, DEFAULT_LERP_SMOOTHNESS);
			}

			if(!followMode)
			{
				RPGCamera_DefaultMode();
			}
			else // Follow Mode On
			{
				RPGCamera_FollowMode();
			}

			// update finger|mouse position
			lastFingerPos = Input.mousePosition;

			// reset values fingerMoveDist
			if(resetFingerMoveDistTimer > 0.0f)
			{
				resetFingerMoveDistTimer -= Time.deltaTime;
			}
			else if(isResetFingerMoveDist)
			{
				fingerPathDist = 0.0f;
				isResetFingerMoveDist = false;
			}
		}
	
		/** rotation around object*/
		private void RPGCamera_DefaultMode()
		{
			//new displacement of the finger|mouse
			xDelta = Mathf.Lerp(xDelta, xDelta + (deltaFingerPos.x) * xSpeed * SPEED_CORRECTION, Smoothness);
			yDelta = Mathf.Lerp(yDelta, yDelta - (deltaFingerPos.y) * ySpeed * SPEED_CORRECTION, Smoothness);

			yDelta = ClampAngle(yDelta, yMinLimit, yMaxLimit);

			Quaternion 	rotation    = Quaternion.Euler(yDelta, xDelta, 0);
			//Quaternion 	rotation    = Quaternion.Lerp(mainCamera.transform.rotation, Quaternion.Euler(new Vector3(yDelta, xDelta, 0)), DEFAULT_LERP_SMOOTHNESS);
			Vector3		totalOffset = new Vector3(0.0f, 0.0f, -Distance) + currVectCamOffset;// * Distance;
			Vector3 	position    = rpgCamPos +  rotation * totalOffset;
			// set new params
			mainCamera.transform.rotation = rotation;
			mainCamera.transform.position = position;

		}

		/** Camera following to the object and rotation to th object rotation*/
		private void RPGCamera_FollowMode()
		{
			Vector3 	vect = rpgCamPos + currVectCamOffset;// * Distance;
			Quaternion 	rot = Quaternion.LookRotation(vect - mainCamera.transform.position);
			Vector3		totOffset = new Vector3(0.0f, 0.0f, -Distance);

			mainCamera.transform.position = vect + rot * totOffset;
			mainCamera.transform.rotation = Quaternion.Slerp(mainCamera.transform.rotation, rot, Smoothness);
		}

		/*******************************************************************************************
		 * MTPCamera STATE
		 * *****************************************************************************************/

		/** 
		 * driving camera to selected point (MTP_Camera)
		 */
		private void MTPCamera()
		{
			InputMethod_MTP();

			// mtp camera states
			MTPCameraMode();
		}

		/** Moving to base point State*/
		private void MTP_ToBasePointState()
		{
			mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, startCamPoint, cameraInOutSpeed);
			mainCamera.transform.rotation = Quaternion.Slerp(mainCamera.transform.rotation, initialCamRot, cameraInOutSpeed);
			mainCamera.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, initialCamFOV, cameraInOutSpeed);
			// the camera came nearer close to initial point
			if(cameraReachedPoint && Vector3.Distance(mainCamera.transform.position, startCamPoint) < maxDistanceToPoint - 1.0f)
			{
				cameraReachedPoint = false;
			}

			// start moving to the point by click
			InputClick_MTP();
		}

		/** Now smooth moving to selected point state*/
		private void MTP_ToPointState()
		{
			Vector3 newVec = mainCamera.transform.position;
			newVec.x = Mathf.Lerp(newVec.x,thisPointLoc.x, cameraInOutSpeed * 2f); // moving faster by X axe
			newVec.y = Mathf.Lerp(newVec.y,thisPointLoc.y, cameraInOutSpeed);
			newVec.z = Mathf.Lerp(newVec.z,thisPointLoc.z, cameraInOutSpeed);

			mainCamera.transform.position = newVec;
			mainCamera.transform.rotation = Quaternion.Slerp(mainCamera.transform.rotation, Quaternion.LookRotation(realPointLoc - mainCamera.transform.position), cameraInOutSpeed);
			mainCamera.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, pointFOV, cameraInOutSpeed);
			//the camera came nearer close to a point
			if(!cameraReachedPoint && Vector3.Distance(mainCamera.transform.position,thisPointLoc) < maxDistanceToPoint/maxDistanceDivider) {
				cameraReachedPoint = true;
				// activating orbit mode(rpg camera mode) at the point
				if(useMTPOrbitModeOnPOI)
				{
					MTPCameraMode = new CameraMode(MTP_PrepareToOrbitModeState);
				}
			}

			// start moving to the point by click
			InputClick_MTP();
		}

		/** */
		private void MTP_PrepareToOrbitModeState()
		{
			Vector3 newVec = mainCamera.transform.position;
			newVec.x = Mathf.Lerp(newVec.x,thisPointLoc.x, cameraInOutSpeed * 2f); // moving faster by X axe
			newVec.y = Mathf.Lerp(newVec.y,thisPointLoc.y, cameraInOutSpeed);
			newVec.z = Mathf.Lerp(newVec.z,thisPointLoc.z, cameraInOutSpeed);

			mainCamera.transform.position = newVec;
			mainCamera.transform.rotation = Quaternion.Slerp(mainCamera.transform.rotation,
				Quaternion.LookRotation(realPointLoc - mainCamera.transform.position), cameraInOutSpeed * 4); // we should rotate faster now!
			mainCamera.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, pointFOV, cameraInOutSpeed);

			if(Vector3.Distance(mainCamera.transform.position, thisPointLoc) < 0.05f) // almost reached the point checking
			{
				// Initial params RPG_Camera
				Distance  =  Vector3.Distance(mainCamera.transform.position, target.position);
				newDist   = Distance;
				xDelta 	  = transform.eulerAngles.y;
				yDelta 	  = transform.eulerAngles.x;
				rpgCamPos = target.position;
				Debug.Log("MTP_OrbitModeState");
				MTPCameraMode = new CameraMode(MTP_OrbitModeState);
			}
		}


		/** orbit mode state*/
		private void MTP_OrbitModeState()
		{
			// using rpg mode for that
			RPGCamera();

			InputClick_MTP();
		}

	/**********************************************************************************************************************************************************************
	 *  Other helper methods
	 * *********************************************************************************************************************************************************************/


		private void InputClick_MTP()
		{
			// start movint to the point by click
			if( Input.GetMouseButtonUp(0) ) {

				if(!EventSystem.current.IsPointerOverGameObject())
				{
					firstClick = Input.mousePosition;
					CheckClickPoint_MTP(firstClick);
				}
			}
		}

		private void InputMethod_MTP()
		{
			// move camera back on right moise button
			if(Input.GetMouseButtonUp(1) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Backspace))
			{
				if(Vector3.Distance(mainCamera.transform.position, thisPointLoc) < maxDistanceToPoint/maxDistanceDivider)
					MtpToPointMove = false;
			}
		}
		
		/** WASD moving (call from RTSCamera)*/
		private void InputMethod_RTS()
		{
			if( Input.GetMouseButtonDown(0) )
			{
				if(!EventSystem.current.IsPointerOverGameObject())
				{
					StartMoving();
					leftMouseBtnDown = true;
					movingToObject   = false;
				}
			}
			
			if( Input.GetMouseButtonUp(0) )
			{
				StopMoving();
				leftMouseBtnDown = false;
			}

			// Movement Of screen Edge by mouse control
			if(mouseScreenEdgeMovement && (!leftMouseBtnDown && canMoveByClick || !canMoveByClick))
			{
				amountX = 1.0f;
				amountY = 1.0f;
				moveBorderWidth = Mathf.Clamp(moveBorderWidth, 1.0f, Screen.width/2);
				Vector2 currMousePos = Input.mousePosition;
				//Left
				if(currMousePos.x < moveBorderWidth)
				{
					amountX = (moveBorderWidth - currMousePos.x)/moveBorderWidth;
					if(moveBorderSide[0] == 0)
					{
						StartMoving();
						moveBorderSide[0] = 1;
					}
				}
				else if(moveBorderSide[0] == 1)
				{
					moveBorderSide[0] = 0;
				}
				// Right
				if(currMousePos.x > Screen.width - moveBorderWidth)
				{ 
					amountX = (currMousePos.x - (Screen.width - moveBorderWidth))/moveBorderWidth;
					if(moveBorderSide[1] == 0)
					{
						StartMoving();
						moveBorderSide[1] = 1;
					}
				}
				else if(moveBorderSide[1] == 1)
				{
					moveBorderSide[1] = 0;
				}
				// Back
				if(currMousePos.y < moveBorderWidth)
				{
					amountY = (moveBorderWidth - currMousePos.y)/moveBorderWidth;
					if(moveBorderSide[2] == 0)
					{
						StartMoving();
						moveBorderSide[2] = 1;
					}
				}
				else if(moveBorderSide[2] == 1)
				{
					moveBorderSide[2] = 0;
				}
				// Forward
				if(currMousePos.y > Screen.height - moveBorderWidth)
				{
					amountY = (currMousePos.y - (Screen.height - moveBorderWidth))/moveBorderWidth;
					if(moveBorderSide[3] == 0)
					{
						StartMoving();
						moveBorderSide[3] = 1;
					}
				}
				else if(moveBorderSide[3] == 1)
				{
					moveBorderSide[3] = 0;
				}
				int sum = moveBorderSide[0] + moveBorderSide[1] + moveBorderSide[2] + moveBorderSide[3];
		
				if(sum == 0 && startMove || deltaSpeedZ != 0)
				{
					if(!KeyCodeD && !KeyCodeS && !KeyCodeA && !KeyCodeW)
						StopMoving();
				}
				else
				{
					if(sum > 0)
					{
						Vector2 normVect = (new Vector2(Screen.width/2, Screen.height/2) - currMousePos).normalized;
						normVect.x *= amountX;
						normVect.y *= amountY;
						displAmount = normVect * keyCamSpeed;
					}
				}
				//
				if(sum < 1)
					deltaSpeedZ = 0;
			}

			if(!useKeyBoardControl)
				return;

			// for Q
			if(Input.GetKeyDown(KeyCode.Q)) {
				KeyCodeQ = true;
			}
			if(Input.GetKeyUp(KeyCode.Q)) {
				KeyCodeQ = false;
			}

			// for E
			if(Input.GetKeyDown(KeyCode.E)) {
				KeyCodeE = true;
			}
			if(Input.GetKeyUp(KeyCode.E)) {
				KeyCodeE = false;
			}

			if(KeyCodeQ)
				deltaAltitude = zoomSensitivity;
			if(KeyCodeE)
				deltaAltitude = -zoomSensitivity;


			if(!leftMouseBtnDown) {
				if(KeyCodeW)
					displAmount.y = -keyCamSpeed;
				if(KeyCodeD)
					displAmount.x = -keyCamSpeed;
				if(KeyCodeS)
					displAmount.y = keyCamSpeed;
				if(KeyCodeA)
					displAmount.x = keyCamSpeed;

				// for W
				if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)){
					KeyCodeW = true;
					StartMoving();
				}
				if(Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow)) {
					KeyCodeW = false;
					if(!KeyCodeD && !KeyCodeS && !KeyCodeA)
						StopMoving();
				}
				// for D
				if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) {
					KeyCodeD = true;
					StartMoving();
				}
				if(Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow)) {
					KeyCodeD = false;
					if(!KeyCodeW && !KeyCodeS && !KeyCodeA)
						StopMoving();
				}
				// for A
				if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) {
					KeyCodeA = true;
					StartMoving();
				}
				if(Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow)) {
					KeyCodeA = false;
					if(!KeyCodeD && !KeyCodeS && !KeyCodeW)
						StopMoving();
				}
				// for S
				if(Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) {
					KeyCodeS = true;
					StartMoving();
				}
				if(Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow)) {
					KeyCodeS = false;
					if(!KeyCodeD && !KeyCodeW && !KeyCodeA)
						StopMoving();
				}
			}
		}

			
		/** event on click or keyboard down. RTS*/
		private void StartMoving()
		{
			startMove 		 = true;
			lastMousePos 	 = Input.mousePosition;
			firstClick 		 = Input.mousePosition;
			displAmount		 = Vector3.zero;
			lastFingerPos	 = Vector2.zero;
		}

		/** event on clickUP or keyboard UP. RTS*/
		private void StopMoving()
		{
			startMove  = false;
			firstClick = Vector3.zero;
			if(canTouchObjects && Vector2.Distance(lastFingerPos, Vector2.zero) < touchThreshold)
				CheckClickPoint_RTS(Input.mousePosition);

			if(useOrbitRotation) {
				if(Mode == CameraModeNames.RTS_Camera) {
					deltaSpeedZ  = 0;
					lastPosition = mainCamera.transform.position;
				}
			}
		}
		/** */
		private static Vector3 CheckTouchDistance(Vector3 _olddelta, Vector3 _newpos, Vector3 _lastpos, float touchthreshold)
		{
			if(Vector3.Distance(_newpos, _lastpos) > touchthreshold)
			{
				return _olddelta;
			} else {
				return (_newpos - _lastpos);
			}
		}
		
		/** */
		private static float ClampAngle (float angle, float min, float max ) {
			if (angle < -MAX_ANGLE)
				angle += MAX_ANGLE;
			if (angle > MAX_ANGLE)
				angle -= MAX_ANGLE;
			return Mathf.Clamp (angle, min, max);
		}
		
		/** */
		private void ResetFingerMoveDist()
		{
			isResetFingerMoveDist    = true;
			resetFingerMoveDistTimer = resetFingerMoveDistTime;
		}

		/**FOR DEMO  check click hit in 3d world || For MTP Camera*/
		protected  void CheckClickPoint_MTP(Vector3 pos)
		{
			Ray ray = Camera.main.ScreenPointToRay(pos);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit)) {
				GameObject 	hitObj = hit.collider.gameObject;
				try
				{
					if(hitObj.tag == "Touch" && (lastHitObj != hitObj.transform))
					{
						lastHitObj = hitObj.transform;
						if(useMTPOrbitModeOnPOI) // for activating orbit camera at the point
						{
							target = hitObj.transform;
						}
						if(!useDefaultCamPoint)
							startCamPoint = initialCamLoc;

						realPointLoc 		= hitObj.transform.position;
						Vector3 vec 		= Quaternion.LookRotation(hitObj.transform.position - startCamPoint)* Vector3.forward;
						closeOffsetDistance = Mathf.Clamp(closeOffsetDistance, 0, Vector3.Distance(startCamPoint, realPointLoc));

						thisPointLoc 		= hitObj.transform.position - closeOffsetDistance * vec;
						currFOV  			= initialCamFOV;
						MtpToPointMove 		= true;
						cameraReachedPoint 	= false;
						maxDistanceToPoint  = Vector3.Distance(mainCamera.transform.position, thisPointLoc);
					}
					else if(hitObj.tag == "TouchPerson")
					{
						if(hitObj.GetComponent<Animation>().IsPlaying("idle"))
							hitObj.GetComponent<Animation>().Play("jump_pose");
						else
							hitObj.GetComponent<Animation>().Play("idle");
					}
				}
				catch
				{
					Debug.LogWarning("Object is " + hitObj + ". Null? Remeber, Object tag should be Touch.");
				}
			}
		}
		
		/**FOR DEMO check click hit in 3d world || For RTS Camera*/
		protected void CheckClickPoint_RTS(Vector3 pos)
		{
			Ray ray = Camera.main.ScreenPointToRay(pos);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit)) {
				GameObject 	hitObj = hit.collider.gameObject;
				try{
					if(hitObj.tag == "Touch")
					{
						Color col = hitObj.GetComponent<Renderer>().material.GetColor("_Color");
						if(col != (new Color(1, 0, 0, 1)))
							hitObj.GetComponent<Renderer>().material.SetColor("_Color", new Color(1, 0, 0, 1));
						else
							hitObj.GetComponent<Renderer>().material.SetColor("_Color", new Color(1.0f, 1.0f, 1.0f, 1));

						if(useMoveToObjectEffect)
						{
							selectedObjectPosition = hitObj.transform.position;
							movingToObject = true;
						}
					}
					else if(hitObj.tag == "TouchPerson")
					{
						if(hitObj.GetComponent<Animation>().IsPlaying("idle"))
							hitObj.GetComponent<Animation>().Play("jump_pose");
						else
							hitObj.GetComponent<Animation>().Play("idle");

						if(useMoveToObjectEffect)
						{
							selectedObjectPosition = hitObj.transform.position;
							movingToObject = true;
						}
					}
				} catch {
					Debug.LogWarning("Object is " + hitObj + ". Null? Object Tag should be Touch or TouchPerson.");
				}
			}
		}

		/** check existence of the camera */
		protected Camera CreateCamera()
		{
			try {
				Camera cam = gameObject.GetComponent<Camera>();
				if(cam == null)
				{
					gameObject.AddComponent<Camera>();
					cam = gameObject.GetComponent<Camera>();
					cam.tag = "MainCamera";
				}
				return cam;
			} catch {
				Debug.LogWarning("No Camera on this object. Try to create Camera fail!");
				return null;
			}
		}
	}
}