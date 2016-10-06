using UnityEngine;
using System.Collections;

public class KeyboardDrums : MonoBehaviour {

	public KeyCode keyCode;
	public bool useColor = false;
	public bool smoothColor = false;
	public Color sourceColor = Color.red;
	public Color targetColor = Color.gray;
	public bool rotation = false;
	public float rotateMultiplier = 1;
	public bool scale = false;
	public bool smoothScale = false;
	public float minimumScale = 1;
	public float scaleMultiplier = 1;
	public bool useAnimation = false;
	public string[] triggerNames; 			//set animation triggers is WIP
	public bool useSound = false;
	public bool hide = false;

	private Color colorHandle = Color.green;
	private MeshRenderer meshRend;
	private AudioSource audioSource;
	private float scaleAmount;
	private float rotateAmount;
	private Animator anim;
	private int counter = 0;

	// Use this for initialization
	void Start () {
		meshRend = gameObject.GetComponent<MeshRenderer> ();
		audioSource = gameObject.GetComponent<AudioSource> ();
		anim = GetComponent<Animator>();
		colorHandle = sourceColor;
		counter = 0;
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown (keyCode)) {			//key is pressed

			//play audio
			if (useSound) {
				audioSource.Play ();
			}
				
			//trigger animation
			if (anim && useAnimation) {

				//trigger animation
				anim.SetTrigger (triggerNames[counter]);

				//set up system to cycle though animation events
				if (counter < (triggerNames.Length - 1)) {
					counter++;
				} else {
					counter = 0;
				}
			}
		}
			
		if (Input.GetKey (keyCode)) {			//key is down
			
			//adjust transforms
			if (rotation) {
				rotateAmount = rotateMultiplier * 10;
				transform.Rotate (new Vector3 (0, rotateAmount, 0));
			}
			if (scale) {
				scaleAmount = scaleMultiplier * 0.01f;
				transform.localScale += new Vector3 (scaleAmount, scaleAmount, scaleAmount);
			}
				
			//change to target color
			if (useColor) {
				colorHandle = targetColor;
				if (smoothColor) {
					meshRend.material.color = Color.Lerp (meshRend.material.color, colorHandle, Time.deltaTime * scaleMultiplier);
				} else {
					meshRend.material.color = colorHandle;
				}
			}

			//make visibile
			meshRend.enabled = true;

		} else {				//key is not down

			//smoothScale is checked and scale is above minimumScale
			if (smoothScale && transform.localScale.x > minimumScale) {

				//smoothly decrease scale
				transform.localScale -= new Vector3 (scaleAmount, scaleAmount, scaleAmount);

				//change to source color
				if (useColor) {
					colorHandle = sourceColor;

					if (smoothColor) {
						meshRend.material.color = Color.Lerp (meshRend.material.color, colorHandle, Time.deltaTime * scaleMultiplier);
					} else {
						meshRend.material.color = colorHandle;
					}
				}

			} else { //smoothScale is not checked and/or scale is at minimumScale

				//set scale back to minimum scale
				transform.localScale = new Vector3 (minimumScale, minimumScale, minimumScale);

				//change color to source color
				colorHandle = sourceColor;
				if (smoothColor) {
					meshRend.material.color = Color.Lerp (meshRend.material.color, colorHandle, Time.deltaTime * scaleMultiplier);
				} else {
					meshRend.material.color = colorHandle;
				}

				//hide if checked
				if (!hide) {
					meshRend.enabled = true;
				} else {
					meshRend.enabled = false;
				}
			}
		}
	}
}
