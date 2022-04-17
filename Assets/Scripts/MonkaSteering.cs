using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace b0rdi.monkaSteering
{
	public class MonkaSteering : MonoBehaviour
	{
		public static MonkaSteering Instance;

		public Transform steeringWheel;
		public Image monkaImg;
		public Sprite monka;
		public Sprite monkaBlinking;

		public float shiftDelay;

		[SerializeField]
		private GameObject rightHand;

		public Action OnGearChanged;

		private uint gear;
		private uint Gear
		{
			get { return gear; }
			set { if (value != gear) { gear = value; OnGearChanged.Invoke(); } }
		}

		private Vector3 originPosition;
		private Quaternion originRotation;

		private void Awake()
		{
			Instance = this;
			Application.targetFrameRate = 60;
			OnGearChanged += DoShiftGear;
		}

		private void Start()
		{
			DoBlinking();
			originPosition = transform.position;
			originRotation = steeringWheel.rotation;
		}

		private void Update()
		{
			if (ForzaConnection.Instance.data.IsRaceOn)
			{
				if (ForzaConnection.Instance.data.VelocityZ > 0)
				{
					transform.position = Vector3.Lerp(transform.position, originPosition + Random.insideUnitSphere * 3 * ForzaConnection.Instance.data.VelocityZ, Time.deltaTime);
				}
				steeringWheel.eulerAngles = new Vector3(0, 0, ForzaConnection.Instance.data.Steer);
				Gear = ForzaConnection.Instance.data.Gear;
			}
			else
			{
				transform.position = originPosition;
				steeringWheel.rotation = originRotation;
			}
		}

		private void DoBlinking()
		{
			StartCoroutine(Blinking());
		}

		private void DoShiftGear()
		{
			StartCoroutine(ShiftGear());
		}

		private IEnumerator Blinking()
		{
			float blinkingTime = Random.Range(0.01f, 0.3f);
			monkaImg.sprite = monkaBlinking;
			yield return new WaitForSeconds(blinkingTime);
			monkaImg.sprite = monka;
			float waitForBlinking = Random.Range(2.5f, 5f);
			yield return new WaitForSeconds(waitForBlinking);

			DoBlinking();
		}

		private IEnumerator ShiftGear()
		{
			rightHand.gameObject.SetActive(true);
			yield return new WaitForSeconds(0.1f);
			rightHand.gameObject.SetActive(false);
			yield return new WaitForSeconds(shiftDelay);
			rightHand.gameObject.SetActive(true);
		}
	}
}
